using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Humm.InStore.Tests
{
	[TestClass]
	public class HummClient_DummyApiTests
	{

		[Ignore("Waiting on dummy api access to be granted")]
		[TestMethod]
		public async Task CreateKey_Success()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.NewZealand,
				Environment = HummEnvironment.Sandbox
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0"
			};
			var client = new HummClient(config);

			var response = await client.CreateKeyAsync(new CreateKeyRequest() { DeviceToken = "A6tQLvTnfG9z", PosVendor = "Test", OperatorId="Automated" });
			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
			Assert.IsNotNull(response.Key);
		}

		[Ignore("Waiting on dummy api access to be granted")]
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
					PreapprovalCode = "843624",
					OperatorId = "Yort",
					PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
				}
			);

			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
		}

		private static HummClient CreateTestClient()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.NewZealand,
				Environment = HummEnvironment.Sandbox
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				DeviceKey = "1234567890"
			};
			var client = new HummClient(config);
			return client;
		}
	}
}