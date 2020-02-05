using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore.Tests
{
	[TestClass]
	public class SignedRequestWriterTests
	{
		[TestMethod]
		public void Writes_Expected_CreateKey_Request_With_Signature()
		{
			var sigGen = new Hmac256SignatureGenerator("nb4i6ldxuVQC");
			var writer = new SignedRequestWriter(sigGen);

			var request = new CreateKeyRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				DeviceToken = "nb4i6ldxuVQC",
				OperatorId = "test_operator",
				PosVersion = "123",
				PosVendor = "Pos Provider"
			};

			var result = writer.WriteRequest(request);
			Assert.AreEqual("{\"x_device_token\":\"nb4i6ldxuVQC\",\"x_pos_vendor\":\"Pos Provider\",\"x_merchant_id\":\"30299999\",\"x_device_id\":\"d555\",\"x_firmware_version\":\"123\",\"x_operator_id\":\"test_operator\",\"signature\":\"cf4f4a19662c7617ea83a47456934123f530c82cdfdd66f5b706dd2263806848\"}", result);
			Assert.IsTrue(result.Contains("cf4f4a19662c7617ea83a47456934123f530c82cdfdd66f5b706dd2263806848"));
		}

		[TestMethod]
		public void Writes_Expected_ProcessAuthorisation_Request_With_Signature()
		{
			var sigGen = new Hmac256SignatureGenerator("dy33vQhksVsv");
			var writer = new SignedRequestWriter(sigGen);

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

			var result = writer.WriteRequest(request);
			Assert.AreEqual("{\"x_pos_transaction_ref\":\"tnx-ref1\",\"x_pre_approval_code\":\"6111NHQMAM4Z\",\"x_finance_amount\":10000,\"x_purchase_amount\":50000,\"purchase_items\":\"{ \\\"PurchaseItems\\\":[\\\"Item1\\\",\\\"Item2\\\"]}\",\"buyer_confirms\":false,\"x_merchant_id\":\"30299999\",\"x_device_id\":\"d555\",\"x_firmware_version\":\"123\",\"x_operator_id\":\"test_operator\",\"signature\":\"d6967970b99e585fc7d1a11702690f25b4be0c5fbfd10b39fbea3953ba48bad2\"}", result);
			Assert.IsTrue(result.Contains("d6967970b99e585fc7d1a11702690f25b4be0c5fbfd10b39fbea3953ba48bad2"));
		}

		[TestMethod]
		public void Writes_Expected_Invite_Request_With_Signature()
		{
			var sigGen = new Hmac256SignatureGenerator("dy33vQhksVsv");
			var writer = new SignedRequestWriter(sigGen);

			var request = new InviteRequest()
			{
				MerchantId = "30299999",
				DeviceId = "d555",
				OperatorId = "test_operator",
				PosVersion = "123",
				MobileNumber = "0400000000",
				PurchaseAmount = 100M
			};

			var result = writer.WriteRequest(request);
			//Note: The signature expected here is different to the one shown in the Humm sample page (https://docs.shophumm.com.au/pos/api_information/http_examples/).
			//The sample C# code provided by Humm at (https://docs.shophumm.com.au/pos/security/signature_generation/) generates the same code used here
			//and this signature gen works with the API endpoints, so it would appear the sample was updated using a different key or something, and the
			//sample signature is now incorrect.
			Assert.AreEqual("{\"x_mobile\":\"0400000000\",\"x_purchase_amount\":10000,\"x_merchant_id\":\"30299999\",\"x_device_id\":\"d555\",\"x_firmware_version\":\"123\",\"x_operator_id\":\"test_operator\",\"signature\":\"e8045e8fdd521d9da2b4cd0c00f816680e9ec4d85bddab95c839d91f170f9deb\"}", result);
			Assert.IsTrue(result.Contains("e8045e8fdd521d9da2b4cd0c00f816680e9ec4d85bddab95c839d91f170f9deb"));
		}

	}

}
