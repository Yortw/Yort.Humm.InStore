using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore.Tests
{
	[TestCategory("Unit")]
	[TestClass]
	public class Hmac256SignatureGeneratorTests
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

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_Invite_Request()
		{
			//The original signature for this request as shown in the API documentation is 9acbc32115e19c3135738bbef891c864ef734e2f66c3c3d3aeb5e0b1982db5f4 
			//however that is not the signature generated by the algorithm and key provided. Even using the sample C# code from the Humm API documentation
			//doesn't generate that signature, but does generate the same signature as the one actually tested here. I suspect the samples were generated 
			//with a different key than the sample docs generate. I wasn't able to get Humm to understand/answer or change the docs. For now, testing using the 
			//key that I expect is the right one, since it was confirmed with the Humm sample code and actual calls to the API seem to work.

			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var values = new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("x_merchant_id", "30299999"),
				new KeyValuePair<string, object>("x_device_id", "d555"),
				new KeyValuePair<string, object>("x_operator_id", "test_operator"),
				new KeyValuePair<string, object>("x_firmware_version", "123"),
				new KeyValuePair<string, object>("x_mobile", "0400000000"),
				new KeyValuePair<string, object>("x_purchase_amount", 100M),
				new KeyValuePair<string, object>("signature", "e8045e8fdd521d9da2b4cd0c00f816680e9ec4d85bddab95c839d91f170f9deb")
			};

			var signature = generator.GenerateSignature(values);
			Assert.AreEqual("e8045e8fdd521d9da2b4cd0c00f816680e9ec4d85bddab95c839d91f170f9deb", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_Invite_Response()
		{
			//The original signature for this request as shown in the API documentation is 2e7ddc54f5457b22fcdd7382ce4e9651abd527051dbc069a3171b4201d8e359c 
			//however that is not the signature generated by the algorithm and key provided. Even using the sample C# code from the Humm API documentation
			//doesn't generate that signature, but does generate the same signature as the one actually tested here. I suspect the samples were generated 
			//with a different key than the sample docs generate. I wasn't able to get Humm to understand/answer or change the docs. For now, testing using the 
			//key that I expect is the right one, since it was confirmed with the Humm sample code and actual calls to the API seem to work.

			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Success"),
					new KeyValuePair<string, object>("x_code", "SINV01"),
					new KeyValuePair<string, object>("x_message", "Success"),
					new KeyValuePair<string, object>("signature", "07a431469cb23ebcf024f7933e75dfb10fe76c44e0295de72e6912b12af91157"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("07a431469cb23ebcf024f7933e75dfb10fe76c44e0295de72e6912b12af91157", signature);
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

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustment_Request()
		{
			//The original signature for this request as shown in the API documentation is ce20e2f1a9fe0d92b3d021ba7f1b372b006778cfab5fc4c09efa60a6d910c471 
			//however that is not the signature generated by the algorithm and key provided. Even using the sample C# code from the Humm API documentation
			//doesn't generate that signature, but does generate the same signature as the one actually tested here. I suspect the samples were generated 
			//with a different key than the sample docs generate. I wasn't able to get Humm to understand/answer or change the docs. For now, testing using the 
			//key that I expect is the right one, since it was confirmed with the Humm sample code and actual calls to the API seem to work.

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
					new KeyValuePair<string, object>("signature", "77d67d8dbae3aa2b403adc65c4e957af6dd918e62276a1cae0fadac743433f3b")
				}
			);

			Assert.AreEqual("77d67d8dbae3aa2b403adc65c4e957af6dd918e62276a1cae0fadac743433f3b", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustment_Response()
		{
			//The original signature for this request as shown in the API documentation is b490b0565a93b31bdf93037ede44b3619f08a1e0e5f68332db52f02176a86bb2 
			//however that is not the signature generated by the algorithm and key provided. Even using the sample C# code from the Humm API documentation
			//doesn't generate that signature, but does generate the same signature as the one actually tested here. I suspect the samples were generated 
			//with a different key than the sample docs generate. I wasn't able to get Humm to understand/answer or change the docs. For now, testing using the 
			//key that I expect is the right one, since it was confirmed with the Humm sample code and actual calls to the API seem to work.

			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Failed"),
					new KeyValuePair<string, object>("x_code", "FPSA05"),
					new KeyValuePair<string, object>("x_message", "Unable to process a sales adjustment for this contract. Please contact Merchant Services on [CollectionsPhone] during business hours for further information"),
					new KeyValuePair<string, object>("signature", "0c796d7e5f6720f5b56ed021e1bba8681d095fd764735f3c62c2d84b9b843af4"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("0c796d7e5f6720f5b56ed021e1bba8681d095fd764735f3c62c2d84b9b843af4", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustmentReversal_Request()
		{
			//The original signature for this request as shown in the API documentation is 1949a14cfdd8e6062a54f28ab3a607637f081afb7b8f4cffa3fb413fadab963b 
			//however that is not the signature generated by the algorithm and key provided. Even using the sample C# code from the Humm API documentation
			//doesn't generate that signature, but does generate the same signature as the one actually tested here. I suspect the samples were generated 
			//with a different key than the sample docs generate. I wasn't able to get Humm to understand/answer or change the docs. For now, testing using the 
			//key that I expect is the right one, since it was confirmed with the Humm sample code and actual calls to the API seem to work.

			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var values = new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("x_pos_transaction_ref", "tnx-rev1"),
				new KeyValuePair<string, object>("x_adjustment_signature", "ce20e2f1a9fe0d92b3d021ba7f1b372b006778cfab5fc4c09efa60a6d910c471"),
				new KeyValuePair<string, object>("x_merchant_id", "30299999"),
				new KeyValuePair<string, object>("x_device_id", "d555"),
				new KeyValuePair<string, object>("x_operator_id", "test_operator"),
				new KeyValuePair<string, object>("x_firmware_version", "123"),
				new KeyValuePair<string, object>("signature", "83a8f5b53cbd205474cd8c993f911b465d25a381d3fc09c781fefb9bef18859d")
			};

			var signature = generator.GenerateSignature(values);

			Assert.AreEqual("83a8f5b53cbd205474cd8c993f911b465d25a381d3fc09c781fefb9bef18859d", signature);
		}

		[TestMethod]
		public void Creates_Expected_Signature_For_Sample_ProcessSalesAdjustmentReversal_Response()
		{
			//The original signature for this request as shown in the API documentation is 1949a14cfdd8e6062a54f28ab3a607637f081afb7b8f4cffa3fb413fadab963b 
			//however that is not the signature generated by the algorithm and key provided. Even using the sample C# code from the Humm API documentation
			//doesn't generate that signature, but does generate the same signature as the one actually tested here. I suspect the samples were generated 
			//with a different key than the sample docs generate. I wasn't able to get Humm to understand/answer or change the docs. For now, testing using the 
			//key that I expect is the right one, since it was confirmed with the Humm sample code and actual calls to the API seem to work.

			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var signature = generator.GenerateSignature
			(
				new KeyValuePair<string, object>[]
				{
					new KeyValuePair<string, object>("x_status", "Failed"),
					new KeyValuePair<string, object>("x_code", "FPSA05"),
					new KeyValuePair<string, object>("x_message", "Unable to process a sales adjustment reversal for this contract. Please contact Merchant Services on [CollectionsPhone] during business hours for further information"),
					new KeyValuePair<string, object>("signature", "5397070a731b81064e69ebbff7814cb9458a885af5e5087f87cc554b20c36b24"),
					new KeyValuePair<string, object>("tracking_data", null)
				}
			);

			Assert.AreEqual("5397070a731b81064e69ebbff7814cb9458a885af5e5087f87cc554b20c36b24", signature);
		}

		[TestMethod]
		public void Disposes_Properly()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			generator.Dispose();
		}

		[TestMethod]
		public void Disposes_Multiple_Times_Without_Error()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");

			generator.Dispose();
			generator.Dispose();
		}

		[ExpectedException(typeof(System.ObjectDisposedException))]
		[TestMethod]
		public void Throws_If_Called_When_Disposed()
		{
			var generator = new Hmac256SignatureGenerator("nb4i6ldxuVQC");
			generator.Dispose();

			_ = generator.GenerateSignature
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
		}

		[ExpectedException(typeof(System.ArgumentNullException))]
		[TestMethod]
		public void Constructor_Throws_If_ApiKey_Null()
		{
			var generator = new Hmac256SignatureGenerator(null);
			generator.Dispose();
		}

		[ExpectedException(typeof(System.ArgumentException))]
		[TestMethod]
		public void Constructor_Throws_If_ApiKey_Empty()
		{
			var generator = new Hmac256SignatureGenerator(string.Empty);
			generator.Dispose();
		}

		[ExpectedException(typeof(System.ArgumentNullException))]
		[TestMethod]
		public void GenerateSignature_Throws_If_Properties_Null()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");
			try
			{
				generator.GenerateSignature(null);
			}
			finally
			{
				generator.Dispose();
			}
		}

		[TestMethod]
		public void GenerateSignature_Returns_Empty_If_No_Properties()
		{
			var generator = new Hmac256SignatureGenerator("dy33vQhksVsv");
			var sig = generator.GenerateSignature(new Dictionary<string, object>(0));
			Assert.AreEqual(string.Empty, sig);
		}

		//This is the Humm provided sample code for generating a signature. It is somewhat simpler, if potentially allocation heavy and relies on 
		//the caller to format non-string parameters appropriately.
		//It's provided here as a reference when testing an unexpected signature is correct (several documented signatures seem to be incorrect).
		//Note that decimal values passed to this must be multiplied by 100 and converted to integer strings, you cannot pass exactly the same 
		//values to this as you would the implementation used by this library.
		/*
		private static string GenerateHMAC(string apiKey, IEnumerable<KeyValuePair<string, object>> payloadValues)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(apiKey)))
			{
				string payloadSignature =
					payloadValues
						.Where(kvp => kvp.Key.StartsWith("x_"))
						.OrderBy(kvp => kvp.Key)
						.Select(kvp => $"{kvp.Key}{kvp.Value}")
						.Aggregate((current, next) => $"{current}{next}");

				var rawHmac = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payloadSignature));

				return System.BitConverter.ToString(rawHmac).Replace("-", string.Empty).ToLower();
			}
		}
		*/

	}
}
