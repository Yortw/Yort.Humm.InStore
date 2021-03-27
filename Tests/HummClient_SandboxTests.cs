using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Humm.InStore.Tests
{
	[TestCategory("Integration")]
	[TestClass]
	public class HummClient_SandboxTests
	{

		[Ignore("Requires env-var Humm_Test_Sandbox_NewDeviceId to be set to an unused init token for each run")]
		[TestMethod]
		public async Task Test_CreateKey()
		{
			var apiSelector = new HummApiUrlSelector()
			{
				Country = HummCountry.NewZealand,
				Environment = HummEnvironment.Sandbox
			};

			var config = new HummClientConfiguration()
			{
				BaseApiUrl = apiSelector.GetUrl(),
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_NewDeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0"
			};
			var client = new HummClient(config);

			var response = await client.CreateKeyAsync(new CreateKeyRequest() { DeviceToken = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceRegistrationKey"), PosVendor = "Yort", OperatorId = "Yort" });
			Assert.IsNotNull(response);
			Assert.AreEqual(response.Status, RequestStates.Success);
			Assert.IsTrue(!String.IsNullOrEmpty(response.Key));
		}

		[Ignore("Currently works for AU but not NZ, need to contact Humm to find out why")]
		[TestMethod]
		public async Task Test_Invite()
		{
			var client = CreateRegisteredSandboxClient();

			var response = await client.InviteAsync
			(
				new InviteRequest()
				{
					MobileNumber = "0400000000",
					PurchaseAmount = 100,
					OperatorId = "Yort"
				}
			);

			Assert.IsNotNull(response);
			Assert.AreEqual(response.Status, RequestStates.Success);
		}

		[Ignore("Requires a current, unused pre-approval code to be set for each run")]
		[TestMethod]
		public async Task Test_ProcessAuthorisation()
		{
			var client = CreateRegisteredSandboxClient();

			var clientRef = System.Guid.NewGuid().ToString();
			var response = await client.ProcessAuthorisationAsync
			(
				new ProcessAuthorisationRequest() 
				{
					ClientTransactionReference = clientRef,
					FinanceAmount = 50,
					PurchaseAmount = 50,
					PreapprovalCode = "759481",
					OperatorId = "Yort",
					PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
				}
			);

			Assert.IsNotNull(response);
			Assert.AreEqual(RequestStates.Success, response.Status);
		}

		private static HummClient CreateRegisteredSandboxClient()
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
				DeviceKey = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceKey")
			};
			var client = new HummClient(config);
			return client;
		}

	}
}
