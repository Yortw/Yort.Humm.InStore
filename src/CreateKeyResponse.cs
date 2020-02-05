using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Contains details of a response to a <see cref="CreateKeyRequest"/>.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.ResponseBase" />
	/// <seealso cref="HummClient.CreateKeyAsync(CreateKeyRequest)"/>
	/// <seealso cref="CreateKeyRequest"/>
	public sealed class CreateKeyResponse : ResponseBase
	{
		/// <summary>
		/// The device key to use for all future API requests using the merchant id and device id associated with the request that generated this response.
		/// </summary>
		/// <value>
		/// The key as a string.
		/// </value>
		/// <remarks>
		/// <para>This key is required to generate signatures for all future requests to the Humm API that use the same merchant id and device id as the request that generated this response.
		/// User code should ensure this value is persisted to disk and re-loaded as required, and ensure any relevant <see cref="HummClient"/> instance is initialised with this key.
		/// </para>
		/// </remarks>
		[JsonProperty("x_key")]
		public string Key { get; set; } = String.Empty;
	}
}
