using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Humm.InStore.Tests
{
	[TestCategory("Unit")]
	[TestClass]
	public class HummClient_General
	{

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void Constructor_Throws_On_Null_Config()
		{
			_ = new HummClient(null);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void Constructor_Throws_On_Null_Config_BaseUrl()
		{
			var config = new HummClientConfiguration();
			_ = new HummClient(config);
		}

		[TestMethod]
		public void Constructor_Accepts_Null_DeviceKey()
		{
			var config = new HummClientConfiguration()
			{
				BaseApiUrl = new Uri("https://integration-pos.shophumm.com.au/webapi/v1/"),
				DeviceKey = null,
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0"
			};
			_ = new HummClient(config);
		}

		[ExpectedException(typeof(ObjectDisposedException))]
		[TestMethod]
		public async Task CreateKeyAsync_Throws_When_Disposed()
		{
			var client = CreateTestClient();
			client.Dispose();

			var request = new CreateKeyRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				DeviceToken = "0123456789",
				PosVendor = "Test Vendor"
			};
			_ = await client.CreateKeyAsync(request);
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public async Task InviteAsync_Throws_Without_DeviceKey()
		{
			var client = CreateTestClient();
			client.SetDeviceKey(null);

			var request = new InviteRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				MobileNumber = "0400000000",
				PurchaseAmount = 100M
			};
			_ = await client.InviteAsync(request);
		}

		[ExpectedException(typeof(ObjectDisposedException))]
		[TestMethod]
		public async Task InviteAsync_Throws_When_Disposed()
		{
			var client = CreateTestClient();
			client.Dispose();

			var request = new InviteRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				MobileNumber = "0400000000",
				PurchaseAmount = 100M
			};
			_ = await client.InviteAsync(request);
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public async Task ProcessAuthorisationAsync_Throws_Without_DeviceKey()
		{
			var client = CreateTestClient();
			client.SetDeviceKey(null);

			var request = new ProcessAuthorisationRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-ref1",
				PreapprovalCode = "6111NHQMAM4Z",
				FinanceAmount = 100,
				PurchaseAmount = 500,
				PurchaseItems = new PurchaseItemsCollection
				(
					new string[]
					{
						"Item1",
						"Item2"
					}
				)
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(ObjectDisposedException))]
		[TestMethod]
		public async Task ProcessAuthorisationRequest_Throws_When_Disposed()
		{
			var client = CreateTestClient();
			client.Dispose();

			var request = new ProcessAuthorisationRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-ref1",
				PreapprovalCode = "6111NHQMAM4Z",
				FinanceAmount = 100,
				PurchaseAmount = 500,
				PurchaseItems = new PurchaseItemsCollection
				(
					new string[]
					{
						"Item1",
						"Item2"
					}
				)
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(ObjectDisposedException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Throws_When_Disposed()
		{
			var client = CreateTestClient();
			client.Dispose();

			var request = new ProcessSalesAdjustmentRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-ref1",
				PurchaseReference = "123456789",
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Throws_Without_DeviceKey()
		{
			var client = CreateTestClient();
			client.SetDeviceKey(null);

			var request = new ProcessSalesAdjustmentRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-ref1",
				PurchaseReference = "123456789",
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[ExpectedException(typeof(ObjectDisposedException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversalAsync_Throws_When_Disposed()
		{
			var client = CreateTestClient();
			client.Dispose();

			var request = new ProcessAuthorisationRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-ref1",
				PreapprovalCode = "6111NHQMAM4Z",
				FinanceAmount = 100,
				PurchaseAmount = 500,
				PurchaseItems = new PurchaseItemsCollection
				(
					new string[]
					{
						"Item1",
						"Item2"
					}
				)
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversalAsync_Throws_Without_DeviceKey()
		{
			var client = CreateTestClient();
			client.SetDeviceKey(null);

			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-rev1",
				AdjustmentSignature = "ce20e2f1a9fe0d92b3d021ba7f1b372b006778cfab5fc4c09efa60a6d910c471"
			};

			_ = await client.ProcessSalesAdjustmentReversalAsync(request);
		}

		[ExpectedException(typeof(ObjectDisposedException))]
		[TestMethod]
		public async Task SendReceiptAsync_Throws_When_Disposed()
		{
			var client = CreateTestClient();
			client.Dispose();

			var request = new SendReceiptRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-ref1",
				ReceiptNumber = "NewReceipt"
			};

			_ = await client.SendReceiptAsync(request);
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public async Task SendReceiptAsync_Throws_Without_DeviceKey()
		{
			var client = CreateTestClient();
			client.SetDeviceKey(null);

			var request = new SendReceiptRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				ClientTransactionReference = "tnx-ref1",
				ReceiptNumber = "NewReceipt"
			};

			_ = await client.SendReceiptAsync(request);
		}

		[TestMethod]
		public void Client_Can_Dispose_Multiple_Times_Without_Error()
		{
			var client = CreateTestClient();
			client.Dispose();
			client.Dispose();
			client.Dispose();
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
