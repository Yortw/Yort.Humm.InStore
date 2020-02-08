using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Humm.InStore.Tests
{
	[TestClass]
	public class HummClient_MockedTests
	{

		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_Sets_UserAgent_From_Config()
		{
			string sentUserAgent = null;

			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					sentUserAgent = clientRequest.Headers.UserAgent.ToString();
					return GetSuccessfulInviteResponse();
				}
			);

			var client = CreateMockedClient(handler);

			var request = new InviteRequest() { MobileNumber = "0400000", PurchaseAmount = 10.01M, OperatorId = "Automated" };
			var response = await client.InviteAsync(request);

			Assert.AreEqual("TestPOS/1.5 (Yort.Humm.Instore/1.0.0.0)", sentUserAgent);
		}

		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_Sets_Default_UserAgent()
		{
			string sentUserAgent = null;

			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					sentUserAgent = clientRequest.Headers.UserAgent.ToString();
					return GetSuccessfulInviteResponse();
				}
			);

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = new Uri("https://integration-pos.shophumm.com.au/webapi/v1/Test/"),
				MerchantId = "30299999",
				DeviceId = "d555",
				PosVersion = "1.0",
				DeviceKey = "1234567890",
				HttpClient = new HttpClient(handler)
			};

			var client = new HummClient(config);
			
			var request = new InviteRequest() { MobileNumber = "0400000", PurchaseAmount = 10.01M, OperatorId = "Automated" };
			var response = await client.InviteAsync(request);

			Assert.AreEqual("Yort.Humm.Instore/1.0.0.0", sentUserAgent);
		}



		[ExpectedException(typeof(HttpRequestException))]
		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_Throws_On_NonSuccess_Response()
		{
			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
				}
			);

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = new Uri("https://integration-pos.shophumm.com.au/webapi/v1/Test/"),
				MerchantId = "30299999",
				DeviceId = "d555",
				PosVersion = "1.0",
				DeviceKey = "1234567890",
				HttpClient = new HttpClient(handler)
			};

			var client = new HummClient(config);

			var request = new InviteRequest() { MobileNumber = "0400000", PurchaseAmount = 10.01M, OperatorId = "Automated" };
			_ = await client.InviteAsync(request);
		}

		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_Exception_On_NonSuccess_Includes_Response_Content()
		{
			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { Content = new StringContent("Unrecognised fields in request") });
				}
			);

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = new Uri("https://integration-pos.shophumm.com.au/webapi/v1/Test/"),
				MerchantId = "30299999",
				DeviceId = "d555",
				PosVersion = "1.0",
				DeviceKey = "1234567890",
				HttpClient = new HttpClient(handler)
			};

			var client = new HummClient(config);

			string responseContent = null;
			var request = new InviteRequest() { MobileNumber = "0400000", PurchaseAmount = 10.01M, OperatorId = "Automated" };
			try
			{
				_ = await client.InviteAsync(request);
			}
			catch (HttpRequestException ex)
			{
				responseContent = (string)ex.Data["ResponseContent"];
			}

			Assert.AreEqual("Unrecognised fields in request", responseContent);
		}


		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_CreateKey_SetsDeviceKey_On_Success()
		{
			int requestCount = 0;
			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					requestCount++;
					if (requestCount <= 1)
						return GetSuccessfulCreateKeyResponse();

					return GetSuccessfulInviteResponse();
				}
			);

			var client = CreateMockedClient(handler);

			var response = await client.CreateKeyAsync(new CreateKeyRequest() { DeviceToken = "ABC123", PosVendor = "Yort", OperatorId = "Yort", AutoUpdateClientToken = true });

			//Confirm key changed by making another call
			//that needs the returned key to gen valid sig
			var request = new InviteRequest() { MobileNumber = "0400000", PurchaseAmount = 10.01M, OperatorId = "Automated" };
			var response2 = await client.InviteAsync(request);
			Assert.AreEqual(RequestStates.Success, response.Status);
		}


		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_ProcessAuthorisation_Retries_On_Pending_When_AutoHandlePendingResponse_True()
		{
			int requestCount = 0;
			var requestContent = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = System.Guid.NewGuid().ToString(),
				PurchaseAmount = 100,
				FinanceAmount = 500,
				PreapprovalCode = "01000000",
				OperatorId = "Yort",
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" },
				BuyerConfirms = true,
				AutoHandlePendingResponse = true
			};

			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					requestCount++;
					if (requestCount >= 4)
						return GetSuccessfulProcessAuthorisationResponse();

					return GetPendingProcessAuthorisationResponse(requestContent);
				}
			);

			var client = CreateMockedClient(handler);

			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var response = await client.ProcessAuthorisationAsync(requestContent);
			sw.Stop();

			Assert.AreEqual(4, requestCount);
			Assert.IsTrue(sw.Elapsed.TotalSeconds >= 15);
			Assert.AreEqual(RequestStates.Success, response.Status);
		}

		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_ProcessAuthorisation_Returns_Pending_When_AutoHandlePendingResponse_False()
		{
			int requestCount = 0;

			var requestContent = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = System.Guid.NewGuid().ToString(),
				PurchaseAmount = 100,
				FinanceAmount = 500,
				PreapprovalCode = "01000000",
				OperatorId = "Yort",
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" },
				BuyerConfirms = true,
				AutoHandlePendingResponse = false
			};

			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					requestCount++;
					if (requestCount >= 4)
						return GetSuccessfulProcessAuthorisationResponse();

					return GetPendingProcessAuthorisationResponse(requestContent);
				}
			);

			var client = CreateMockedClient(handler);

			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var response = await client.ProcessAuthorisationAsync(requestContent);
			sw.Stop();

			Assert.AreEqual(1, requestCount);
			Assert.IsTrue(sw.Elapsed.TotalSeconds <= 1);
			Assert.AreEqual(RequestStates.Pending, response.Status);
		}

		[TestCategory("Unit.Mocked")]
		[TestMethod]
		public async Task HummClient_ProcessAuthorisation_RaisesEvent_When_AutoHandlePendingResponse_True()
		{
			int requestCount = 0;

			var request = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = System.Guid.NewGuid().ToString(),
				PurchaseAmount = 100,
				FinanceAmount = 500,
				PreapprovalCode = "01000000",
				OperatorId = "Yort",
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" },
				BuyerConfirms = true,
				AutoHandlePendingResponse = true,
				TrackingData = new Dictionary<string, string>() { { "Key1", "Value1" } }
			};

			var handler = new MockHttpHandler
			(
				(clientRequest) =>
				{
					requestCount++;
					if (requestCount >= 4)
						return GetSuccessfulProcessAuthorisationResponse();

					return GetPendingProcessAuthorisationResponse(request);
				}
			);
			var eventRaised = false;

			var client = CreateMockedClient(handler);
			client.PendingAuthorisation += (sender, e) =>
			{
				eventRaised = true;
				Assert.AreEqual(request.ClientTransactionReference, e.ClientReference);
				Assert.AreEqual(5, e.RetryDuration);
				Assert.AreEqual(request.TrackingData.Count, e.TrackingData.Count);
			};

			var sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			var response = await client.ProcessAuthorisationAsync(request);
			sw.Stop();

			Assert.AreEqual(4, requestCount);
			Assert.IsTrue(sw.Elapsed.TotalSeconds >= 15);
			Assert.IsTrue(eventRaised);
		}



		private Task<HttpResponseMessage> GetSuccessfulCreateKeyResponse()
		{
			var retVal = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new StringContent
				(
					Newtonsoft.Json.JsonConvert.SerializeObject
					(
						new CreateKeyResponse()
						{
							Code = "SCRK01",
							Status = RequestStates.Success,
							Message = "Success",
							Key = "1234567890",
							Signature = "00dab4e4b16dc3020f724e134caea9ab71eb35e4bd788f4aee0d3c7a6b1ae821"
						}
					)
				)
			};

			return Task.FromResult(retVal);
		}

		private Task<HttpResponseMessage> GetSuccessfulProcessAuthorisationResponse()
		{
			var retVal = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new StringContent
				(
					Newtonsoft.Json.JsonConvert.SerializeObject
					(
						new ProcessAuthorisationResponse()
						{
							Code = "SPRA01",
							Status = RequestStates.Success,
							Message = "Approved",
							RetryDuration = 5,
							Signature = "ba1a93e6137d363fa3bc3f54406fa8945b4fe75e919ef2f2db8806bb37e59b06"
						}
					)
				)
			};

			return Task.FromResult(retVal);
		}

		private Task<HttpResponseMessage> GetPendingProcessAuthorisationResponse(ProcessAuthorisationRequest request)
		{
			var responseContent = new ProcessAuthorisationResponse()
			{
				Code = "SPRA01",
				Status = RequestStates.Pending,
				Message = "Pending",
				RetryDuration = 5,
				Signature = "bb5bb8418bff30eacaa841e7b7d0fba94a5b40bb1ce9283c1b26511ade34f719"
			};
			if (request.TrackingData != null)
			{
				responseContent.TrackingData = new Dictionary<string, string>();
				CopyTrackingData(request.TrackingData, responseContent.TrackingData);
			}

			var retVal = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(responseContent)) 
			};

			return Task.FromResult(retVal);
		}

		private void CopyTrackingData(Dictionary<string, string> source, Dictionary<string, string> destination)
		{
			if (source == null) return;

			foreach (var kvp in source)
			{
				destination.Add(kvp.Key, kvp.Value);
			}
		}

		private static HummClient CreateMockedClient(MockHttpHandler handler)
		{
			var config = new HummClientConfiguration()
			{
				BaseApiUrl = new Uri("https://integration-pos.shophumm.com.au/webapi/v1/Test/"),
				UserAgentProductName = "TestPOS",
				UserAgentProductVersion = "1.5",
				MerchantId = "30299999",
				DeviceId = "d555",
				PosVersion = "1.0",
				DeviceKey = "1234567890",
				HttpClient = new HttpClient(handler)
			};

			var client = new HummClient(config);
			return client;
		}

		private Task<HttpResponseMessage> GetSuccessfulInviteResponse()
		{
			var retVal = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
			{
				Content = new StringContent
				(
					Newtonsoft.Json.JsonConvert.SerializeObject
					(
						new InviteResponse()
						{
							Code = "SINV01",
							Status = RequestStates.Success,
							Message = "Success",
							Signature = "be3f90d02bf10c3c404851fe3885ea5a44d4e6111900af2a7ea4828b2e2a4649"
						}
					)
				)
			};

			return Task.FromResult(retVal);
		}

		internal class MockHttpHandler : System.Net.Http.DelegatingHandler
		{
			private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _Func;

			public MockHttpHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> func)
			{
				_Func = func;
			}

			public MockHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
			{
			}

			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				return _Func(request);
			}
		}
	}
}
