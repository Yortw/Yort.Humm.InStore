using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
using Newtonsoft.Json;

namespace Yort.Humm.InStore.Infrastructure
{
	/// <summary>
	/// Contains properties common to all requests.
	/// </summary>
	/// <remarks>
	/// <para>Many of these properties have an equivalent property on <see cref="HummClientConfiguration"/>. If the property on a request 
	/// is null or emptry string, the value from the configuration instance passed to the <see cref="HummClient"/> will be copied into the 
	/// request prior to sending the request. This minimises the number of properties that have to be explicitly set for every request.
	/// </para>
	/// </remarks>
	public abstract class RequestBase
	{
		/// <summary>
		/// Required. Gets or sets the unique merchant identifier assigned by Humm to the retailer/organisation making requests.
		/// </summary>
		/// <value>
		/// The merchant identifier assigned by Humm.
		/// </value>
		/// <remarks>
		/// <para>Maximum length of 10 characters.</para>
		/// </remarks>
		[JsonProperty("x_merchant_id")]
		public string? MerchantId { get; set; }
		/// <summary>
		/// Required. Gets or sets the device identifier for the POS making the request, must match the device id used when the <see cref="HummClientConfiguration.DeviceKey"/> in use was requested.
		/// </summary>
		/// <value>
		/// The device identifier.
		/// </value>
		/// <remarks>
		/// <para>Maximum length of 64 characters.</para>
		/// </remarks>
		[JsonProperty("x_device_id")]
		public string? DeviceId { get; set; }
		/// <summary>
		/// Required. Gets or sets the POS version.
		/// </summary>
		/// <value>
		/// The POS version.
		/// </value>
		/// <remarks>
		/// <para>Maximum length of 64 characters.</para>
		/// </remarks>
		[JsonProperty("x_firmware_version")]
		public string? PosVersion { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier of the POS operator.
		/// </summary>
		/// <value>
		/// The operator identifier.
		/// </value>
		/// <remarks>
		/// <para>Maximum length of 64 characters.</para>
		/// </remarks>
		[JsonProperty("x_operator_id")]
		public string? OperatorId { get; set; }

		/// <summary>
		/// Optional. Gets or sets custom information to return in the response.
		/// </summary>
		/// <value>
		/// A dictionary of strings containing the tracking data.
		/// </value>
		/// <remarks>
		/// <para>Tracking data can be used by the POS to provide additional information to return with the response, which can be 
		/// useful for mapping responses back to specific clients/payments or managing other state.</para>
		/// <para>Can be set to null (and should be if there is no tracking data). Null is the default value unless explicitly initialised to a new dictionary instance.</para>
		/// <para>Limit of 1000000 items.</para>
		/// </remarks>
		[JsonProperty("tracking_data")]
#pragma warning disable CA2227 // Collection properties should be read only
		public Dictionary<string, string>? TrackingData { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

		internal void FillFromConfig(HummClientConfiguration config)
		{
			MerchantId = Coalesce(MerchantId, config.MerchantId);
			DeviceId = Coalesce(DeviceId, config.DeviceId);
			PosVersion = Coalesce(PosVersion, config.PosVersion);
		}

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <remarks>
		/// <para>Ensures that <see cref="MerchantId"/>, <see cref="DeviceId"/>, <see cref="PosVersion"/> and <see cref="OperatorId"/> are not null, empty string or only whitespace.
		/// Also ensure no property is larger than it's maximum allowed length (see individual property notes for details).
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">Thrown if <see cref="MerchantId"/>, <see cref="DeviceId"/>, <see cref="PosVersion"/>, or <see cref="OperatorId"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown if <see cref="MerchantId"/>, <see cref="DeviceId"/>, <see cref="PosVersion"/>, or <see cref="OperatorId"/> is empty or only whitespace, or longer than their maximum allowed lengths.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="TrackingData"/> contains more than 1000000 items.</exception>
		public virtual void Validate()
		{
			MerchantId.GuardNullOrWhiteSpace("request", nameof(MerchantId));
			MerchantId.GuardLength("request", nameof(MerchantId), 10);

			DeviceId.GuardNullOrWhiteSpace("request", nameof(DeviceId));
			DeviceId.GuardLength("request", nameof(DeviceId), 64);

			PosVersion.GuardNullOrWhiteSpace("request", nameof(PosVersion));
			PosVersion.GuardLength("request", nameof(PosVersion), 64);

			OperatorId.GuardNullOrWhiteSpace("request", nameof(OperatorId));
			OperatorId.GuardLength("request", nameof(OperatorId), 64);

			var td = this.TrackingData;
			if (td != null)
				td.Count.GuardRange(nameof(TrackingData), 0, 1000000);
		}

		private static string? Coalesce(string? firstValue, string? secondValue)
		{
			if (String.IsNullOrEmpty(firstValue)) return secondValue;

			return firstValue;
		}
	}
}