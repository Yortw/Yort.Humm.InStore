using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore.Infrastructure
{
	/// <summary>
	/// Provides properties common to all Humm responses. 
	/// </summary>
	public abstract class ResponseBase
	{

		/// <summary>
		/// Gets or sets the status of the request, used to determine if the request was successful, declined, cancelled or resulted in an error.
		/// </summary>
		/// <value>
		/// A string containig the status of the request.
		/// </value>
		/// <seealso cref="RequestStates" />
		/// <seealso cref="Code"/>
		/// <seealso cref="Message"/>
		[JsonProperty("x_status")]
		public string? Status { get; set; }
		/// <summary>
		/// A code providing more details about the <see cref="Status"/>, suitable for using user code to take programmtic action on or log as a language neutral description of the outcome.
		/// </summary>
		/// <value>
		/// The code returned by Humm.
		/// </value>
		/// <remarks>
		/// See <a href="https://docs.shophumm.com.au/pos/api_information/status_codes/" /> for a list of codes returned by Humm.
		/// </remarks>
		/// <seealso cref="Status"/>
		/// <seealso cref="Message"/>
		[JsonProperty("x_code")]
		public string? Code { get; set; }
		/// <summary>
		/// Gets or sets a human readable message providing more details about the <see cref="Status"/>.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		[JsonProperty("x_message")]
		public string? Message { get; set; }
		/// <summary>
		/// Gets the tracking data that was sent with the request that generated this response.
		/// </summary>
		/// <value>
		/// A dictionary of strings containing the tracking data.
		/// </value>
		/// <seealso cref="RequestBase.TrackingData"/>
		[JsonProperty("tracking_data")]
		public Dictionary<string, string>? TrackingData { get; private set; }
		/// <summary>
		/// Gets or sets the signature of this response, used to verify the response is authentic.
		/// </summary>
		/// <value>
		/// The signature as a string.
		/// </value>
		/// <seealso cref="VerifySignature(ISignatureGenerator)"/>
		[JsonProperty("signature")]
		public string? Signature { get; set; }

		/// <summary>
		/// Verifies the signature of this response matches the expected signature using the <see cref="ISignatureGenerator"/> provided. 
		/// </summary>
		/// <remarks>
		/// <para>The <see cref="HummClient"/> automatically calls this method when a reponse is received, application code does not need to call this method explicitly unless performing it's own calls without using a <see cref="HummClient"/> instance.</para>
		/// <para>The <paramref name="signatureGenerator"/> must have been created/initialised with the same device key as the request that generated this response.</para>
		/// <para>If the <see cref="Signature"/> property is null or empty, this call is a no-op and will return without throwing an exception.</para>
		/// </remarks>
		/// <param name="signatureGenerator">The signature generator to use to confirm the response signature.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="signatureGenerator"/> is null.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the signature of the response does not match the expected signature.</exception>
		public void VerifySignature(ISignatureGenerator signatureGenerator)
		{
			if (String.IsNullOrEmpty(Signature)) return;

			signatureGenerator.GuardNull(nameof(signatureGenerator));

			var calcSig = signatureGenerator.GenerateSignature(HummModelPropertyReader.GetPropertyValues(this));
			if (calcSig != this.Signature) throw new HummResponseSignatureException();
		}
	}
}