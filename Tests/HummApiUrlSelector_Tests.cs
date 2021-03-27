using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yort.Humm.InStore.Tests
{
	[TestCategory("Unit")]
	[TestClass]
	public class HummApiUrlSelector_Tests
	{
		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public void HummApiUrlSelector_Throws_On_Unknown_Api_Version()
		{
			var selector = new HummApiUrlSelector();
			selector.ApiVersion = 1000;
			selector.GetUrl();
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public void HummApiUrlSelector_Throws_On_Unknown_AU_Api_Environment()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.Australia;
			selector.Environment = (HummEnvironment)1000;
			selector.GetUrl();
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public void HummApiUrlSelector_Throws_On_Unknown_NZ_Api_Environment()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.NewZealand;
			selector.Environment = (HummEnvironment)1000;
			selector.GetUrl();
		}

		[ExpectedException(typeof(InvalidOperationException))]
		[TestMethod]
		public void HummApiUrlSelector_Throws_On_Unknown_Country()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = (HummCountry)1000;
			selector.GetUrl();
		}

		[TestMethod]
		public void HummApiUrlSelector_Australia_V1_Dummy_Resolves_Correctly()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.Australia;
			selector.ApiVersion = 1;
			selector.Environment = HummEnvironment.Dummy;

			var result = selector.GetUrl();
			Assert.AreEqual(new Uri("https://integration-pos.shophumm.com.au/webapi/v1/Test/"), result);
		}

		[TestMethod]
		public void HummApiUrlSelector_Australia_V1_Sandbox_Resolves_Correctly()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.Australia;
			selector.ApiVersion = 1;
			selector.Environment = HummEnvironment.Sandbox;

			var result = selector.GetUrl();
			Assert.AreEqual(new Uri("https://integration-pos.shophumm.com.au/webapi/v1/"), result);
		}

		[TestMethod]
		public void HummApiUrlSelector_Australia_V1_Production_Resolves_Correctly()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.Australia;
			selector.ApiVersion = 1;
			selector.Environment = HummEnvironment.Production;

			var result = selector.GetUrl();
			Assert.AreEqual(new Uri("https://pos.shophumm.com.au/webapi/v1/"), result);
		}

		[TestMethod]
		public void HummApiUrlSelector_NewZealand_V1_Dummy_Resolves_Correctly()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.NewZealand;
			selector.ApiVersion = 1;
			selector.Environment = HummEnvironment.Dummy;

			var result = selector.GetUrl();
			Assert.AreEqual(new Uri("https://integration-pos.shophumm.co.nz/webapi/v1/Test/"), result);
		}

		[TestMethod]
		public void HummApiUrlSelector_NewZealand_V1_Sandbox_Resolves_Correctly()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.NewZealand;
			selector.ApiVersion = 1;
			selector.Environment = HummEnvironment.Sandbox;

			var result = selector.GetUrl();
			Assert.AreEqual(new Uri("https://integration-pos.shophumm.co.nz/webapi/v1/"), result);
		}

		[TestMethod]
		public void HummApiUrlSelector_NewZealand_V1_Production_Resolves_Correctly()
		{
			var selector = new HummApiUrlSelector();
			selector.Country = HummCountry.NewZealand;
			selector.ApiVersion = 1;
			selector.Environment = HummEnvironment.Production;

			var result = selector.GetUrl();
			Assert.AreEqual(new Uri("https://pos.shophumm.co.nz/webapi/v1/"), result);
		}

		[TestMethod]
		public void HummApiUrlSelector_Constructor_Defaults_Country_Australia()
		{
			var selector = new HummApiUrlSelector();
			Assert.AreEqual(HummCountry.Australia, selector.Country);
		}

		[TestMethod]
		public void HummApiUrlSelector_Constructor_Defaults_ApiVersion_1()
		{
			var selector = new HummApiUrlSelector();
			Assert.AreEqual(1M, selector.ApiVersion);
		}

		[TestMethod]
		public void HummApiUrlSelector_Constructor_Defaults_Environment_Sandbox()
		{
			var selector = new HummApiUrlSelector();
			Assert.AreEqual(HummEnvironment.Sandbox, selector.Environment);
		}

	}
}
