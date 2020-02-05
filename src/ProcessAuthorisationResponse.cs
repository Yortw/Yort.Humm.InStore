using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Contains details of a response to a <see cref="ProcessAuthorisationRequest"/>.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.ResponseBase" />
	/// <seealso cref="HummClient.ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/>
	/// <seealso cref="ProcessAuthorisationRequest"/>
	public sealed class ProcessAuthorisationResponse : ResponseBase
	{
		/// <summary>
		/// Gets or sets the Humm generated purchase number.
		/// </summary>
		/// <value>
		/// The purchase number assigned to this transaction by Humm.
		/// </value>
		[JsonProperty("x_purchase_number")]
		public string? PurchaseNumber { get; set; }

		/// <summary>
		/// Gets or sets the interval to wait before retrying the request, if the <see cref="ResponseBase.Status"/> is <see cref="RequestStates.Pending"/>.
		/// </summary>
		/// <value>
		/// The duration of the retry delay, in seconds.
		/// </value>
		[JsonProperty("retry_duration")]
		public int RetryDuration { get; set; }
	}
}
