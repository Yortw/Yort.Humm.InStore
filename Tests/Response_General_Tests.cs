using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore.Tests
{
	[TestCategory("Unit")]
	[TestClass]
	public class Response_General_Tests
	{
		[ExpectedException(typeof(Yort.Humm.InStore.HummResponseSignatureException))]
		[TestMethod]
		public void Response_VerifySignature_ThrowsOnMismatch()
		{
			var sigGen = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var response = new SendReceiptResponse()
			{
				Code = "123",
				Status = RequestStates.Success,
				Message = "Test response",
				Signature = System.Guid.NewGuid().ToString()
			};

			response.VerifySignature(sigGen);
		}

		[TestMethod]
		public void Response_VerifySignature_IgnoresNullSignature()
		{
			var sigGen = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var response = new SendReceiptResponse()
			{
				Code = "123",
				Status = RequestStates.Success,
				Message = "Test response",
				Signature = String.Empty
			};

			response.VerifySignature(sigGen);
		}

		[TestMethod]
		public void Response_VerifySignature_IgnoresEmptySignature()
		{
			var sigGen = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var response = new SendReceiptResponse()
			{
				Code = "123",
				Status = RequestStates.Success,
				Message = "Test response",
				Signature = String.Empty
			};

			response.VerifySignature(sigGen);
		}

		[TestMethod]
		public void Response_VerifySignature_ValidatesCorrectSignature()
		{
			var sigGen = new Hmac256SignatureGenerator("dy33vQhksVsv");

			var response = new SendReceiptResponse()
			{
				Code = "123",
				Status = RequestStates.Success,
				Message = "Test response",
				Signature = "8835d92906403bf01f155f9cab315929b5e893e9f3283eb6f38d28d0e4c53ae4"
			};

			response.VerifySignature(sigGen);
		}

	}
}
