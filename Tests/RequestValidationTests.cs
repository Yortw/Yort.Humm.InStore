using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Humm.InStore.Tests
{
	[TestCategory("Unit")]
	[TestClass]
	public class RequestValidationTests
	{

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task InviteAsync_Validates_MobileNumber_NotNull()
		{
			var client = CreateTestClient();

			var request = new InviteRequest()
			{
				MobileNumber = null,
				PurchaseAmount = 0
			};
			_ = await client.InviteAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task InviteAsync_Validates_MobileNumber_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new InviteRequest()
			{
				MobileNumber = String.Empty,
				PurchaseAmount = 0
			};
			_ = await client.InviteAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task InviteAsync_Validates_MobileNumber_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new InviteRequest()
			{
				MobileNumber = "01234567890",
				PurchaseAmount = 0
			};
			_ = await client.InviteAsync(request);
		}

		[TestMethod]
		public void InviteRequest_Validates_Ok_When_Valid()
		{
			var request = new InviteRequest()
			{
				OperatorId = "Yort",
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				MobileNumber = "0123456789",
				PurchaseAmount = 10
			};
			request.Validate();
		}


		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task CreateKeyAsync_Validates_DeviceToken_NotNull()
		{
			var client = CreateTestClient();
			
			var request = new CreateKeyRequest()
			{
				DeviceToken = null,
				PosVendor = "Test Vendor"
			};
			_ = await client.CreateKeyAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task CreateKeyAsync_Validates_DeviceToken_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new CreateKeyRequest()
			{
				DeviceToken = String.Empty,
				PosVendor = "Test Vendor"
			};
			_ = await client.CreateKeyAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task CreateKeyAsync_Validates_DeviceToken_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new CreateKeyRequest()
			{
				DeviceToken = "0123456789012345678901234567890123456789012345678901234567890123456789",
				PosVendor = "Test Vendor"
			};
			_ = await client.CreateKeyAsync(request);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task CreateKeyAsync_Validates_PosVendor_NotNull()
		{
			var client = CreateTestClient();

			var request = new CreateKeyRequest()
			{
				DeviceToken = "UyWlQcseYNCC",
				PosVendor = null
			};
			_ = await client.CreateKeyAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task CreateKeyAsync_Validates_PosVendor_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new CreateKeyRequest()
			{
				DeviceToken = "UyWlQcseYNCC",
				PosVendor = String.Empty
			};
			_ = await client.CreateKeyAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task CreateKeyAsync_Validates_PosVendor_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new CreateKeyRequest()
			{
				DeviceToken = "UyWlQcseYNCC",
				PosVendor = "0123456789001234567890012345678900123456789001234567890012345678900123456789001234567890012345678900123456789001234567890"
			};
			_ = await client.CreateKeyAsync(request);
		}

		[TestMethod]
		public void CreateKeyRequest_Validates_Ok_When_Valid()
		{
			var request = new CreateKeyRequest()
			{
				OperatorId = "Yort",
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				DeviceToken = "UyWlQcseYNCC",
				PosVendor = "Test Vendor"
			}; 
			request.Validate();
		}



		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task ProcessAuthorisationAsync_Validates_ClientTransactionReference_NotNull()
		{
			var client = CreateTestClient();

			var request = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = null,
				PreapprovalCode = "1234567890",
				PurchaseAmount = 10,
				FinanceAmount = 10,
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessAuthorisationAsync_Validates_ClientTransactionReference_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = String.Empty,
				PreapprovalCode = "1234567890",
				PurchaseAmount = 10,
				FinanceAmount = 10,
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessAuthorisationAsync_Validates_ClientTransactionReference_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = "1234567890123456789012345678901234567890123456789012345678901234567890",
				PreapprovalCode = "1234567890",
				PurchaseAmount = 10,
				FinanceAmount = 10,
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task ProcessAuthorisationAsync_Validates_PreapprovalCode_NotNull()
		{
			var client = CreateTestClient();

			var request = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = "1234567890",
				PreapprovalCode = null,
				PurchaseAmount = 10,
				FinanceAmount = 10,
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessAuthorisationAsync_Validates_PreapprovalCode_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = "1234567890",
				PreapprovalCode = String.Empty,
				PurchaseAmount = 10,
				FinanceAmount = 10,
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessAuthorisationAsync_Validates_PreapprovalCode_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new ProcessAuthorisationRequest()
			{
				ClientTransactionReference = "1234567890123456789012345678901234567890123456789012345678901234567890",
				PreapprovalCode = "1234567890",
				PurchaseAmount = 10,
				FinanceAmount = 10,
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
			};
			_ = await client.ProcessAuthorisationAsync(request);
		}

		[TestMethod]
		public void ProcessAuthorisationRequest_Validates_Ok_When_Valid()
		{
			var request = new ProcessAuthorisationRequest()
			{
				OperatorId = "Yort",
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				ClientTransactionReference = "12345678901234567890",
				PreapprovalCode = "1234567890",
				PurchaseAmount = 10,
				FinanceAmount = 10,
				PurchaseItems = new PurchaseItemsCollection() { "Item1", "Item2" }
			};
			request.Validate();
		}



		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Validates_ClientTransactionReference_NotNull()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentRequest()
			{
				ClientTransactionReference = null,
				PurchaseReference = "0123456789",
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Validates_ClientTransactionReference_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentRequest()
			{
				ClientTransactionReference = String.Empty,
				PurchaseReference = "0123456789",
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Validates_ClientTransactionReference_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentRequest()
			{
				ClientTransactionReference = "0123456789012345678901234567890123456789012345678901234567890123456789",
				PurchaseReference = "0123456789",
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Validates_PurchaseReference_NotNull()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentRequest()
			{
				ClientTransactionReference = "0123456789",
				PurchaseReference = null,
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Validates_PurchaseReference_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentRequest()
			{
				ClientTransactionReference = "0123456789",
				PurchaseReference = String.Empty,
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentAsync_Validates_PurchaseReference_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentRequest()
			{
				ClientTransactionReference = "0123456789",
				PurchaseReference = "0123456789012345678901234567890123456789012345678901234567890123456789",
				Amount = 10
			};
			_ = await client.ProcessSalesAdjustmentAsync(request);
		}

		[TestMethod]
		public void ProcessSalesAdjustmentRequest_Validates_Ok_When_Valid()
		{
			var request = new ProcessSalesAdjustmentRequest()
			{
				OperatorId = "Yort",
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				ClientTransactionReference = "0123456789",
				PurchaseReference = "012345678901234567890",
				Amount = 10
			};
			request.Validate();
		}



		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Validates_ClientTransactionReference_NotNull()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				ClientTransactionReference = null,
				AdjustmentSignature = "0123456789"
			};
			_ = await client.ProcessSalesAdjustmentReversalAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Validates_ClientTransactionReference_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				ClientTransactionReference = String.Empty,
				AdjustmentSignature = "0123456789"
			};
			_ = await client.ProcessSalesAdjustmentReversalAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Validates_ClientTransactionReference_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				ClientTransactionReference = "0123456789012345678901234567890123456789012345678901234567890123456789",
				AdjustmentSignature = "0123456789"
			};
			_ = await client.ProcessSalesAdjustmentReversalAsync(request);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Validates_AdjustmentSignature_NotNull()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				ClientTransactionReference = "0123456789",
				AdjustmentSignature = null
			};
			_ = await client.ProcessSalesAdjustmentReversalAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Validates_AdjustmentSignature_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				ClientTransactionReference = "0123456789",
				AdjustmentSignature = String.Empty
			};
			_ = await client.ProcessSalesAdjustmentReversalAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task ProcessSalesAdjustmentReversal_Validates_AdjustmentSignature_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				ClientTransactionReference = "0123456789",
				AdjustmentSignature = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
			};
			_ = await client.ProcessSalesAdjustmentReversalAsync(request);
		}

		[TestMethod]
		public void ProcessSalesAdjustmentReversalRequest_Validates_Ok_When_Valid()
		{
			var request = new ProcessSalesAdjustmentReversalRequest()
			{
				OperatorId = "Yort",
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				ClientTransactionReference = "0123456789",
				AdjustmentSignature = "012345678901234567890"
			};
			request.Validate();
		}



		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task SendReceiptAsync_Validates_ClientTransactionReference_NotNull()
		{
			var client = CreateTestClient();

			var request = new SendReceiptRequest()
			{
				ClientTransactionReference = null,
				ReceiptNumber = "0123456789"
			};
			_ = await client.SendReceiptAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task SendReceiptAsync_Validates_ClientTransactionReference_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new SendReceiptRequest()
			{
				ClientTransactionReference = String.Empty,
				ReceiptNumber = "0123456789"
			};
			_ = await client.SendReceiptAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task SendReceiptAsync_Validates_ClientTransactionReference_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new SendReceiptRequest()
			{
				ClientTransactionReference = "0123456789012345678901234567890123456789012345678901234567890123456789",
				ReceiptNumber = "0123456789"
			};
			_ = await client.SendReceiptAsync(request);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public async Task SendReceiptAsync_Validates_ReceiptNumber_NotNull()
		{
			var client = CreateTestClient();

			var request = new SendReceiptRequest()
			{
				ClientTransactionReference = "0123456789",
				ReceiptNumber = null
			};
			_ = await client.SendReceiptAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task SendReceiptAsync_Validates_ReceiptNumber_NotEmpty()
		{
			var client = CreateTestClient();

			var request = new SendReceiptRequest()
			{
				ClientTransactionReference = "0123456789",
				ReceiptNumber = String.Empty
			};
			_ = await client.SendReceiptAsync(request);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public async Task SendReceiptAsync_Validates_ReceiptNumber_NotOverLength()
		{
			var client = CreateTestClient();

			var request = new SendReceiptRequest()
			{
				ClientTransactionReference = "0123456789",
				ReceiptNumber = "0123456789012345678901234567890123456789012345678901234567890123456789"
			};
			_ = await client.SendReceiptAsync(request);
		}

		[TestMethod]
		public void SendReceiptRequest_Validates_Ok_When_Valid()
		{
			var request = new SendReceiptRequest()
			{
				OperatorId = "Yort",
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				ClientTransactionReference = "0123456789",
				ReceiptNumber = "01234567890A"
			};
			request.Validate();
		}

		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		[TestMethod]
		public void RequestBase_Validates_Max_TrackingItems()
		{
			var request = new InviteRequest()
			{
				DeviceId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_DeviceId"),
				MerchantId = Environment.GetEnvironmentVariable("Humm_Test_Sandbox_MerchantId"),
				PosVersion = "1.0",
				MobileNumber = "04000000",
				PurchaseAmount = 10,
				OperatorId = "Yort",
				TrackingData = new Dictionary<string, string>()
			};
			
			for (int cnt = 0; cnt < 1000010; cnt++)
			{
				request.TrackingData.Add("Key" + cnt.ToString(), cnt.ToString());
			}

			request.Validate();
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
