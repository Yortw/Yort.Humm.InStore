using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Represents a request to send an SMS to a mobile phone number inviting someone to sign up to Humm.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.RequestBase" />
	/// <seealso cref="HummClient.InviteAsync(InviteRequest)"/>
	/// <seealso cref="InviteResponse"/>
	public sealed class InviteRequest : RequestBase
	{
		/// <summary>
		/// Required. Gets or sets the mobile number to send the invitation to.
		/// </summary>
		/// <value>
		/// The mobile phone number.
		/// </value>
		[JsonProperty("x_mobile")]
		public string? MobileNumber { get; set; }

		/// <summary>
		/// Gets or sets the amount of a initial purchase amount to generate a pre-approval code for after sign-up is complete.
		/// </summary>
		/// <value>
		/// The purchase amount as a dollar value (i.e $20 would be 20.00).
		/// </value>
		[JsonProperty("x_purchase_amount")]
		public decimal PurchaseAmount { get; set; }
	}
}
