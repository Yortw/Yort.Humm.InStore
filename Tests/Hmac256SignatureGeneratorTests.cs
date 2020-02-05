using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore.Tests
{
	[TestClass]
	public class Test_Hmac256SignatureGenerator
	{
		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_CreateKey_Request()
		{
			var generator = new Hmac256SignatureGenerator("nb4i6ldxuVQC");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_merchant_id", "30299999"),
					new KeyValuePair<string, object>("x_device_id", "d555"),
					new KeyValuePair<string, object>("x_operator_id", "test_operator"),
					new KeyValuePair<string, object>("x_firmware_version", "123"),
					new KeyValuePair<string, object>("x_device_token", "nb4i6ldxuVQC"),
					new KeyValuePair<string, object>("x_pos_vendor", "Pos Provider"),
				}
			);

			Assert.AreEqual("cf4f4a19662c7617ea83a47456934123f530c82cdfdd66f5b706dd2263806848", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_CreateKey_Response()
		{
			var generator = new Hmac256SignatureGenerator("nb4i6ldxuVQC");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_key", "dy33vQhksVsv"),
					new KeyValuePair<string, object>("x_status", "Success"),
					new KeyValuePair<string, object>("x_code", "SCRK01"),
					new KeyValuePair<string, object>("x_message", "Success"),
					new KeyValuePair<string, object>("signature", "c8340d19b24f8ec2fb915202ea7fb08d81fc711681fbfea297106d219a5c30a6"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("c8340d19b24f8ec2fb915202ea7fb08d81fc711681fbfea297106d219a5c30a6", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessAuthorisation_Request()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(	
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_merchant_id", "30299999"),
					new KeyValuePair<string, object>("x_device_id", "d555"),
					new KeyValuePair<string, object>("x_operator_id", "test_operator"),
					new KeyValuePair<string, object>("x_firmware_version", "123"),
					new KeyValuePair<string, object>("x_pos_transaction_ref", "tnx-ref1"),
					new KeyValuePair<string, object>("x_pre_approval_code", "6111NHQMAM4Z"),
					new KeyValuePair<string, object>("x_finance_amount", 100M),
					new KeyValuePair<string, object>("x_purchase_amount", 500M),
					new KeyValuePair<string, object>("purchase_items", "{ \"PurchaseItems\": [ \"Item1\", \"Item2\" ] }"),
					new KeyValuePair<string, object>("signature", "d6967970b99e585fc7d1a11702690f25b4be0c5fbfd10b39fbea3953ba48bad2")
				}
			);

			Assert.AreEqual("d6967970b99e585fc7d1a11702690f25b4be0c5fbfd10b39fbea3953ba48bad2", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessAuthorisation_Response()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Success"),
					new KeyValuePair<string, object>("x_code", "SPRA01"),
					new KeyValuePair<string, object>("x_message", "Approved"),
					new KeyValuePair<string, object>("signature", "07fa5c9b43520bb9417d7eadb8390a87bf7a42767a67bd5af95627b51c2d8bae"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("07fa5c9b43520bb9417d7eadb8390a87bf7a42767a67bd5af95627b51c2d8bae", signature);
		}

		[Ignore("Suspect sample sig is incorrect")]
		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_Invite_Request()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var values = new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("x_merchant_id", "30299999"),
				new KeyValuePair<string, object>("x_device_id", "d555"),
				new KeyValuePair<string, object>("x_operator_id", "test_operator"),
				new KeyValuePair<string, object>("x_firmware_version", "123"),
				new KeyValuePair<string, object>("x_mobile", "0400000000"),
				new KeyValuePair<string, object>("x_purchase_amount", 100M),
				new KeyValuePair<string, object>("signature", "9acbc32115e19c3135738bbef891c864ef734e2f66c3c3d3aeb5e0b1982db5f4")
			};

			var signature = generator.GenerateSignature(values);

			Assert.AreEqual("9acbc32115e19c3135738bbef891c864ef734e2f66c3c3d3aeb5e0b1982db5f4", signature);
		}

		[Ignore("Suspect sample sig is incorrect")]
		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_Invite_Response()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Success"),
					new KeyValuePair<string, object>("x_code", "SINV01"),
					new KeyValuePair<string, object>("x_message", "Success"),
					new KeyValuePair<string, object>("signature", "2e7ddc54f5457b22fcdd7382ce4e9651abd527051dbc069a3171b4201d8e359c"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("2e7ddc54f5457b22fcdd7382ce4e9651abd527051dbc069a3171b4201d8e359c", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_SendReceipt_Request()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_merchant_id", "30299999"),
					new KeyValuePair<string, object>("x_device_id", "d555"),
					new KeyValuePair<string, object>("x_operator_id", "test_operator"),
					new KeyValuePair<string, object>("x_firmware_version", "123"),
					new KeyValuePair<string, object>("x_pos_transaction_ref", "tnx-ref1"),
					new KeyValuePair<string, object>("x_receipt_number", "NewReceipt"),
					new KeyValuePair<string, object>("signature", "ac164e40585474838641bde1ad9bab08b6e6e0ab0c2dae814298e4fcef9c52fa")
				}
			);

			Assert.AreEqual("ac164e40585474838641bde1ad9bab08b6e6e0ab0c2dae814298e4fcef9c52fa", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_SendReceipt_Response()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Success"),
					new KeyValuePair<string, object>("x_code", "SSER01"),
					new KeyValuePair<string, object>("x_message", "Success"),
					new KeyValuePair<string, object>("signature", "b55c8cbe59305d3d8abc6ea8f0cdcf4702059d6d2fa935c83595e56e7001157b"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("b55c8cbe59305d3d8abc6ea8f0cdcf4702059d6d2fa935c83595e56e7001157b", signature);
		}

		[Ignore("Suspect sample sig is incorrect")]
		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustment_Request()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_pos_transaction_ref", "tnx-ref1"),
					new KeyValuePair<string, object>("x_purchase_ref", "123456789"),
					new KeyValuePair<string, object>("x_merchant_id", "30299999"),
					new KeyValuePair<string, object>("x_amount", 10M),
					new KeyValuePair<string, object>("x_device_id", "d555"),
					new KeyValuePair<string, object>("x_operator_id", "test_operator"),
					new KeyValuePair<string, object>("x_firmware_version", "123"),
					new KeyValuePair<string, object>("signature", "ce20e2f1a9fe0d92b3d021ba7f1b372b006778cfab5fc4c09efa60a6d910c471")
				}
			);

			Assert.AreEqual("ce20e2f1a9fe0d92b3d021ba7f1b372b006778cfab5fc4c09efa60a6d910c471", signature);
		}

		[Ignore("Suspect sample sig is incorrect")]
		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustment_Response()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Failed"),
					new KeyValuePair<string, object>("x_code", "FPSA05"),
					new KeyValuePair<string, object>("x_message", "Unable to process a sales adjustment for this contract. Please contact Merchant Services on [CollectionsPhone] during business hours for further information"),
					new KeyValuePair<string, object>("signature", "b490b0565a93b31bdf93037ede44b3619f08a1e0e5f68332db52f02176a86bb2"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("b55c8cbe59305d3d8abc6ea8f0cdcf4702059d6d2fa935c83595e56e7001157b", signature);
		}

		[Ignore("Suspect sample sig is incorrect")]
		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustmentReversal_Request()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var values = new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("x_pos_transaction_ref", "tnx-rev1"),
				new KeyValuePair<string, object>("x_adjustment_signature", "ce20e2f1a9fe0d92b3d021ba7f1b372b006778cfab5fc4c09efa60a6d910c471"),
				new KeyValuePair<string, object>("x_merchant_id", "30299999"),
				new KeyValuePair<string, object>("x_device_id", "d555"),
				new KeyValuePair<string, object>("x_operator_id", "test_operator"),
				new KeyValuePair<string, object>("x_firmware_version", "123"),
				new KeyValuePair<string, object>("signature", "1949a14cfdd8e6062a54f28ab3a607637f081afb7b8f4cffa3fb413fadab963b")
			};

			var signature = generator.GenerateSignature(values);

			Assert.AreEqual("1949a14cfdd8e6062a54f28ab3a607637f081afb7b8f4cffa3fb413fadab963b", signature);
		}

		[Ignore("Suspect sample sig is incorrect")]
		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustmentReversal_Response()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Failed"),
					new KeyValuePair<string, object>("x_code", "FPSA05"),
					new KeyValuePair<string, object>("x_message", "Unable to process a sales adjustment reversal for this contract. Please contact Merchant Services on [CollectionsPhone] during business hours for further information"),
					new KeyValuePair<string, object>("signature", "1949a14cfdd8e6062a54f28ab3a607637f081afb7b8f4cffa3fb413fadab963b"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("1949a14cfdd8e6062a54f28ab3a607637f081afb7b8f4cffa3fb413fadab963b", signature);
		}

	}
}
