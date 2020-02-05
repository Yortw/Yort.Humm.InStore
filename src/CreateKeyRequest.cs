using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Represents a request to swap a device (initialisation) token for a device key used to sign future requests.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.RequestBase" />
	/// <seealso cref="HummClient.CreateKeyAsync(CreateKeyRequest)"/>
	/// <seealso cref="CreateKeyResponse"/>
	public sealed class CreateKeyRequest : RequestBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateKeyRequest"/> class.
		/// </summary>
		public CreateKeyRequest()
		{
			AutoUpdateClientToken = true;
		}

		/// <summary>
		/// Required. Gets or sets the one-time use device token generated in the Humm Seller/merchant portal.
		/// </summary>
		/// <remarks>
		/// Maximum legth of 64 characters.
		/// </remarks>
		/// <value>
		/// The device token generated in the Humm Seller/merchant portal.
		/// </value>
		[JsonProperty("x_device_token")]
		public string? DeviceToken { get; set; }

		/// <summary>
		/// Required. Gets or sets the name of the company that suppliers the POS hardware or software.
		/// </summary>
		/// <remarks>
		/// <para>Maximum length of 100 characters.</para>
		/// </remarks>
		/// <value>
		/// The company name of the POS supplier.
		/// </value>
		[JsonProperty("x_pos_vendor")]
		public string? PosVendor { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the client automatically initialises itself with the returned device token (if the request is successful) so it is used on all future requests.
		/// </summary>
		/// <value>
		/// <para>If true the <see cref="HummClient"/> performing this request will automatically update itself to use the returned token from a succesful request as well as returning the response to the caller so the token can be persisted for future calls.</para>
		/// <para>If false the the response is returned to the caller, and <see cref="HummClient.SetDeviceKey(string)"/> must be manually called on whichever client instance should use the new token.</para>
		/// </value>
		/// <remarks>
		/// <para>Even when <see cref="AutoUpdateClientToken"/> is true, it is still up to the calling application to persist the device key returned in the response so it can be used in the event of a process/machine restart etc.</para>
		/// </remarks>
		[JsonIgnore]
		public bool AutoUpdateClientToken { get; set; }

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <remarks>
		/// <para>Ensures <seealso cref="DeviceToken"/> and <see cref="PosVendor"/> are not null, empty strings or contain only whitespace. Also ensures they are not longer than allowed.</para>
		/// <para>Also ensures all base properties are valid, see <see cref="RequestBase.Validate"/>.</para>
		/// </remarks>
		public override void Validate()
		{
			DeviceToken.GuardNullOrWhiteSpace("request", nameof(DeviceToken));
			DeviceToken.GuardLength("request", nameof(DeviceToken), 64);

			PosVendor.GuardNullOrWhiteSpace("request", nameof(PosVendor));
			PosVendor.GuardLength("request", nameof(PosVendor), 100);


			base.Validate();
		}

	}
}
