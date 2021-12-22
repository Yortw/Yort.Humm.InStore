using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
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
		/// <remarks>
		/// <para>Maximum length of 10 digits.</para>
		/// </remarks>
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

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Ensures that <see cref="MobileNumber" /> is not null, empty string or only whitespace.
		/// Also ensure no property is larger than it's maximum allowed length (see individual property notes for details).
		/// Also ensures all base properties are valid, see <see cref="RequestBase.Validate"/>.
		/// </para>
		/// </remarks>
		public override void Validate()
		{
			MobileNumber.GuardNullOrWhiteSpace("request", nameof(MobileNumber));
			MobileNumber.GuardLength("request", nameof(MobileNumber), 0, 11);

			base.Validate();
		}
	}
}
