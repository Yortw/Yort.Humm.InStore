using System;
using System.Collections.Generic;
using System.Text;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Provides information to a <see cref="HummClient"/> about how to operate and default values to use for requests.
	/// </summary>
	public sealed class HummClientConfiguration
	{
		/// <summary>
		/// Gets or sets the default merchant identifier for requests.
		/// </summary>
		/// <value>
		/// The merchant identifier.
		/// </value>
		/// <remarks>
		/// <para>Any request sent with a null or empty merchant id will have this value used instead.</para>
		/// </remarks>
		/// <seealso cref="Infrastructure.RequestBase.MerchantId"/>
		public string? MerchantId { get; set; }
		/// <summary>
		/// Gets or sets the unique device identifier for the POS the <see cref="HummClient"/> represents.
		/// </summary>
		/// <value>
		/// The device identifier.
		/// </value>
		/// <remarks>
		/// <para>Any request sent with a null or empty device id will have this value used instead.</para>
		/// </remarks>
		/// <seealso cref="Infrastructure.RequestBase.DeviceId"/>
		public string? DeviceId { get; set; }
		/// <summary>
		/// Gets or sets the POS version.
		/// </summary>
		/// <value>
		/// The POS version.
		/// </value>
		/// <remarks>
		/// <para>Any request sent with a null or empty POS version will have this value used instead.</para>
		/// </remarks>
		/// <seealso cref="Infrastructure.RequestBase.PosVersion"/>
		public string? PosVersion { get; set; }

		/// <summary>
		/// Gets or sets a <see cref="System.Uri"/> which is the base uri of the Humm API to call.
		/// </summary>
		/// <value>
		/// A <see cref="System.Uri"/> containing the base address to use.
		/// </value>
		/// <remarks>
		/// <para>The API address must be provided so the system knows whether to use a test or live environment, and for which country/region. Use the <see cref="HummApiUrlSelector"/> to decide on a URL if you'd rather not hard code the addreses yourself.</para>
		/// <para>Changing this property will have no effect on <see cref="HummClient"/> instances that have already been constructed.</para>
		/// </remarks>
		/// <seealso cref="HummApiUrlSelector"/>
		public Uri? BaseApiUrl { get; set; }

		/// <summary>
		/// Gets or sets the device key issued by Humm for creating digital signatures. 
		/// </summary>
		/// <value>
		/// The device key.
		/// </value>
		/// <remarks>
		/// <para>This value is must match the one returned by a prior <see cref="CreateKeyRequest"/> for the same <see cref="MerchantId"/> and <see cref="DeviceId"/>.</para>
		/// <para>If you have not yet initialised the POS device and do not have a device key, use null as the device key property here and then initialise the client instance using a <see cref="HummClient.CreateKeyAsync(CreateKeyRequest)"/> call.</para>
		/// <para>Changing this property will have no effect on <see cref="HummClient"/> instances that have already been constructed.</para>
		/// </remarks>
		/// <seealso cref="HummClient.CreateKeyAsync(CreateKeyRequest)"/>
		/// <seealso cref="CreateKeyRequest"/>
		/// <seealso cref="CreateKeyResponse"/>
		public string? DeviceKey { get; set; }

		/// <summary>
		/// Optional. Gets or sets the name of the 'product' to use in the user agent header on HTTP calls to Humm.
		/// </summary>
		/// <value>
		/// The name of the user agent product.
		/// </value>
		/// <remarks>
		/// <para>If not supplied the Yort.Humm.InStore library name is used instead.</para>
		/// <para>Changing this property will have no effect on <see cref="HummClient"/> instances that have already been constructed.</para>
		/// </remarks>
		public string? UserAgentProductName { get; set; }
		/// <summary>
		/// Optional. Gets or sets the version of the 'product' to use in the user agent header on HTTP calls to Humm.
		/// </summary>
		/// <value>
		/// The user agent product version.
		/// </value>
		/// <remarks>
		/// <para>If not supplied the Yort.Humm.InStore library version is used instead.</para>
		/// <para>Changing this property will have no effect on <see cref="HummClient"/> instances that have already been constructed.</para>
		/// </remarks>
		public string? UserAgentProductVersion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not <see cref="HummClient"/> instances automatically perform client side validation of requests before sending them.
		/// </summary>
		/// <remarks>
		/// <para>The default and recommended value is true, ensuring the simple validation such as required fields and minimum/maximum lengths of values are checked 
		/// before making a request to Humm. This property exists to disable this feature in the future should Humm change the validation rules and a new, updated 
		/// version of this library not yet be available.</para>
		/// </remarks>
		/// <value>
		///   <c>true</c> to have <see cref="RequestBase.Validate"/> called automatically before sending a request.<c>false</c>.
		/// </value>
		public bool AutoValidateRequests { get; set; } = true;
	}
}