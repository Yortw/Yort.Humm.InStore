using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Humm.InStore
{ 
	/// <summary>
	/// Determines the correct base URI for calling Humm based on a country and API enviroment.
	/// </summary>
	public sealed class HummApiUrlSelector : IHummApiUrlSelector
	{
		/// <summary>
		/// Gets or sets the country the system is operating in.
		/// </summary>
		/// <value>
		/// A value from the <see cref="HummCountry"/> enum.
		/// </value>
		public HummCountry Country { get; set; }

		/// <summary>
		/// Gets or sets the API environment to use.
		/// </summary>
		/// <value>
		/// A value from the <see cref="HummEnvironment"/> specifying a test or live environment to use.
		/// </value>
		public HummEnvironment Environment { get; set; } = HummEnvironment.Sandbox;

		/// <summary>
		/// Gets or sets the version of the API to call.
		/// </summary>
		/// <value>
		/// The API version.
		/// </value>
		/// <remarks>
		/// <para>Currently the only supported version is 1.</para>
		/// </remarks>
		public decimal ApiVersion { get; set; } = 1.0M;

		/// <summary>
		/// Gets the base URL to use for Humm API calls.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Uri" /> representing the base API address to be used.
		/// </returns>
		/// <exception cref="InvalidOperationException">Thrown if a unknown <see cref="Country"/>, <see cref="Environment"/> or <see cref="ApiVersion"/> value is specified.</exception>
		/// <seealso cref="Environment"/>
		/// <seealso cref="Country"/>
		/// <seealso cref="ApiVersion"/>
		public Uri GetUrl()
		{
			if (ApiVersion != 1) throw new InvalidOperationException(ErrorMessages.UnknownApiVersion);

			return Country switch
			{
				HummCountry.Australia => GetAUUri(),
				HummCountry.NewZealand => GetNZUri(),
				_ => throw new InvalidOperationException(ErrorMessages.UnknownCountry),
			};
		}

		private Uri GetAUUri()
		{
			return Environment switch
			{
				HummEnvironment.Dummy => new Uri("https://integration-pos.shophumm.com.au/webapi/v1/Test/"),
				HummEnvironment.Sandbox => new Uri("https://integration-pos.shophumm.com.au/webapi/v1/"),
				HummEnvironment.Production => new Uri("https://pos.shophumm.com.au/webapi/v1/"),
				_ => throw new InvalidOperationException(ErrorMessages.UnknownApiEnvironment)
			};
		}

		private Uri GetNZUri()
		{
			return Environment switch
			{
				HummEnvironment.Dummy => new Uri("https://integration-pos.shophumm.co.nz/webapi/v1/Test/"),
				HummEnvironment.Sandbox => new Uri("https://integration-pos.shophumm.co.nz/webapi/v1/"),
				HummEnvironment.Production => new Uri("https://pos.shophumm.co.nz/webapi/v1/"),
				_ => throw new InvalidOperationException(ErrorMessages.UnknownApiEnvironment)
			};
		}
	}

	/// <summary>
	/// Provides a list of countries in which Humm/Oxipay API's are available.
	/// </summary>
	/// <remarks>
	/// <para>You must call the API for the country your Humm merchant was registered in (NZ merchants must use NZ endpoints, AU merchants must use AU endpoints etc).</para>
	/// </remarks>
	public enum HummCountry
	{
		/// <summary>
		/// The Australian API's.
		/// </summary>
		Australia,
		/// <summary>
		/// The New Zealand API's.
		/// </summary>
		NewZealand
	}

	/// <summary>
	/// Specifies which Humm API endpoints should be used.
	/// </summary>
	public enum HummEnvironment
	{
		/// <summary>
		/// Default. The dummy API environment is suitable for automated testing, with fixed responses sent based on specific request inputs.
		/// </summary>
		/// <remarks>
		/// <para>Not typically used by application code, but the default value for the enum to avoid accidental usage of the production environment.</para>
		/// </remarks>
		Dummy,
		/// <summary>
		/// The sandbox API environment is suitable for manual testing, user-acceptance testing and demo systems where real financial transactions are not desired but human interactive and realistic worflows are.
		/// </summary>
		Sandbox,
		/// <summary>
		/// The production API environment is the live API used for conducting real financial transactions.
		/// </summary>
		Production
	}
}
