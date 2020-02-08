using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Humm.InStore.Tests
{
	[TestCategory("Integration")]
	[TestClass]
	public class HummClient_DummyApiTests
	{

		[TestMethod]
		public async Task CreateKey_Success()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.Australia,
				Environment = HummEnvironment.Dummy
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = "d555",
				MerchantId = "30299999",
				PosVersion = "1.0"
			};
			var client = new HummClient(config);

			var response = await client.CreateKeyAsync(new CreateKeyRequest() { DeviceToken = "01000000", PosVendor = "Test", OperatorId="Automated" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
			Assert.IsNotNull(response.Key);
		}

		[TestMethod]
		public async Task CreateKey_Failure()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.Australia,
				Environment = HummEnvironment.Dummy
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = "d555",
				MerchantId = "30299999",
				PosVersion = "1.0"
			};
			var client = new HummClient(config);

			var response = await client.CreateKeyAsync(new CreateKeyRequest() { DeviceToken = "10000000", PosVendor = "Test", OperatorId = "Automated" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Failed, response.Status);
			Assert.AreEqual("FCRK01", response.Code);
		}

		[TestMethod]
		public async Task CreateKey_Error()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.Australia,
				Environment = HummEnvironment.Dummy
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = "d555",
				MerchantId = "30299999",
				PosVersion = "1.0"
			};
			var client = new HummClient(config);

			var response = await client.CreateKeyAsync(new CreateKeyRequest() { DeviceToken = "30000000", PosVendor = "Test", OperatorId = "Automated" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Error, response.Status);
			Assert.AreEqual("EVAL01", response.Code);
		}



		[TestMethod]
		public async Task Invite_Success()
		{
			var client = CreateTestClient();

			var response = await client.InviteAsync(new InviteRequest() { MobileNumber="0400000", PurchaseAmount = 10.01M, OperatorId = "Automated" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
			Assert.AreEqual("SINV01", response.Code);
		}

		[TestMethod]
		public async Task Invite_Error()
		{
			var client = CreateTestClient();

			var response = await client.InviteAsync(new InviteRequest() { MobileNumber = "0400000", PurchaseAmount = 10.30M, OperatorId = "Automated" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Error, response.Status);
			Assert.AreEqual("EVAL01", response.Code);
		}



		[TestMethod]
		public async Task ProcessAuthorisation_Success()
		{
			var client = CreateTestClient();

			var response = await client.ProcessAuthorisationAsync
			(
				new ProcessAuthorisationRequest()
				{
					ClientTransactionReference = System.Guid.NewGuid().ToString(),
					PurchaseAmount = 100,
					FinanceAmount = 500,
					PreapprovalCode = "01000000",
					OperatorId = "Yort",
					PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
				}
			);

			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
		}

		[Ignore("Can't get pendng status returned as expected from dummy API")]
		[TestMethod]
		public async Task ProcessAuthorisation_Repeats_Request_When_Pending_And_AutoHandlePendingResponse_True()
		{
			var client = CreateTestClient();

			var response = await client.ProcessAuthorisationAsync
			(
				new ProcessAuthorisationRequest()
				{
					ClientTransactionReference = System.Guid.NewGuid().ToString(),
					PurchaseAmount = 5000,
					FinanceAmount = 5000,
					PreapprovalCode = "Pending",
					OperatorId = "Yort",
					PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" },
					BuyerConfirms = true,
					AutoHandlePendingResponse = true
				}
			);

			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Pending, response.Status);
		}

		[Ignore("Can't get pendng status returned as expected from dummy API")]
		[TestMethod]
		public async Task ProcessAuthorisation_Returns_Pending_When_AutoHandlePendingResponse_False()
		{
			var client = CreateTestClient();

			var response = await client.ProcessAuthorisationAsync
			(
				new ProcessAuthorisationRequest()
				{
					ClientTransactionReference = System.Guid.NewGuid().ToString(),
					PurchaseAmount = 100,
					FinanceAmount = 500,
					PreapprovalCode = "Pending",
					OperatorId = "Yort",
					PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" },
					BuyerConfirms = false,
					AutoHandlePendingResponse = false
				}
			);

			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Pending, response.Status);
		}


		[TestMethod]
		public async Task SendReceipt_Success()
		{
			var client = CreateTestClient();

			var response = await client.SendReceiptAsync(new SendReceiptRequest() { ClientTransactionReference = "0123456789", ReceiptNumber = "01000000", OperatorId = "Yort" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
			Assert.AreEqual("SSER01", response.Code);
		}

		[TestMethod]
		public async Task SendReceipt_Failed()
		{
			var client = CreateTestClient();

			var response = await client.SendReceiptAsync(new SendReceiptRequest() { ClientTransactionReference = "0123456789", ReceiptNumber = "10000000", OperatorId = "Yort" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Failed, response.Status);
			Assert.AreEqual("FSER01", response.Code);
		}

		[TestMethod]
		public async Task SendReceipt_Error()
		{
			var client = CreateTestClient();

			var response = await client.SendReceiptAsync(new SendReceiptRequest() { ClientTransactionReference = "0123456789", ReceiptNumber = "30000000", OperatorId = "Yort" });
			Assert.IsNotNull(response);
			Assert.AreEqual("Error", response.Status);
			Assert.AreEqual("EVAL01", response.Code);
		}



		[TestMethod]
		public async Task ProcessSalesAdjustment_Success()
		{
			var client = CreateTestClient();
			
			var response = await client.ProcessSalesAdjustmentAsync
			(
				new ProcessSalesAdjustmentRequest() 
				{ 
					ClientTransactionReference = "adj-1",
					Amount = 1.01M,
					PurchaseReference = "0123456789",
					OperatorId = "Yort"
				}
			);
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
			Assert.AreEqual("SPSA01", response.Code);
		}

		[TestMethod]
		public async Task ProcessSalesAdjustment_Failed()
		{
			var client = CreateTestClient();

			var response = await client.ProcessSalesAdjustmentAsync
			(
				new ProcessSalesAdjustmentRequest()
				{
					ClientTransactionReference = "adj-1",
					Amount = 1.10M,
					PurchaseReference = "0123456789",
					OperatorId = "Yort"
				}
			);
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Failed, response.Status);
			Assert.AreEqual("FPSA01", response.Code);
		}

		[TestMethod]
		public async Task ProcessSalesAdjustment_Error()
		{
			var client = CreateTestClient();

			var response = await client.ProcessSalesAdjustmentAsync
			(
				new ProcessSalesAdjustmentRequest()
				{
					ClientTransactionReference = "adj-1",
					Amount = 1.30M,
					PurchaseReference = "0123456789",
					OperatorId = "Yort"
				}
			);
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Error, response.Status);
			Assert.AreEqual("EVAL01", response.Code);
		}



		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Success()
		{
			var client = CreateTestClient();

			var response = await client.ProcessSalesAdjustmentReversalAsync
			(
				new ProcessSalesAdjustmentReversalRequest()
				{
					ClientTransactionReference = "adj-1",
					AdjustmentSignature = "0100",
					OperatorId = "Yort"
				}
			);
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
			Assert.AreEqual("SPAR01", response.Code);
		}

		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Failed()
		{
			var client = CreateTestClient();

			var response = await client.ProcessSalesAdjustmentReversalAsync
			(
				new ProcessSalesAdjustmentReversalRequest()
				{
					ClientTransactionReference = "adj-1",
					AdjustmentSignature = "1000",
					OperatorId = "Yort"
				}
			);
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Failed, response.Status);
			Assert.AreEqual("FPAR01", response.Code);
		}

		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Error()
		{
			var client = CreateTestClient();

			var response = await client.ProcessSalesAdjustmentReversalAsync
			(
				new ProcessSalesAdjustmentReversalRequest()
				{
					ClientTransactionReference = "adj-1",
					AdjustmentSignature = "3000",
					OperatorId = "Yort"
				}
			);

			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Error, response.Status);
			Assert.AreEqual("EVAL01", response.Code);
		}



		[TestCategory("Integration")]
		[TestMethod]
		public async Task Client_Does_Not_Validate_When_Config_Disables_Validation()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.Australia,
				Environment = HummEnvironment.Dummy
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = "d555",
				MerchantId = "30299999",
				PosVersion = "1.0",
				DeviceKey = "1234567890",
				AutoValidateRequests = false
			};
			var client = new HummClient(config);

			var request = new SendReceiptRequest()
			{
				OperatorId = "Yort",
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				ClientTransactionReference = null,
				ReceiptNumber = null
			};
			var response = await client.SendReceiptAsync(request);
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Error, response.Status);
		}



		private static HummClient CreateTestClient()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.Australia,
				Environment = HummEnvironment.Dummy
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = "d555",
				MerchantId = "30299999",
				PosVersion = "1.0",
				DeviceKey = "1234567890"
			};
			var client = new HummClient(config);
			return client;
		}
	}
}