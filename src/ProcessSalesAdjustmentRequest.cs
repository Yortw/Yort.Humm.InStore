using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Represents a request to full or partially refund a previously successful <see cref="HummClient.ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/> request.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.RequestBase" />
	/// <seealso cref="ProcessSalesAdjustmentResponse"/>
	/// <seealso cref="HummClient.ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest)"/>
	public sealed class ProcessSalesAdjustmentRequest : RequestBase
	{
		/// <summary>
		/// Required. Gets or sets the client transaction reference for this adjustment. 
		/// </summary>
		/// <value>
		/// This is the transaction reference of the sales adjustment.
		/// </value>
		/// <remarks>
		/// <para>Be sure to read the notes on idempotency and retry logic at https://docs.shophumm.com.au/pos/api_information/retries_and_idempotency/ </para>
		/// </remarks>
		[JsonProperty("x_pos_transaction_ref")]
		public string? ClientTransactionReference { get; set; }

		/// <summary>
		/// Required. Gets or sets the purchase reference of the original transaction to be refunded.
		/// </summary>
		/// <value>A tranaction reference for the original authorisation to be refunded.</value>
		/// <remarks>
		/// <para>
		/// The original transaction reference. It can either be the <see cref="ProcessAuthorisationRequest.ClientTransactionReference"/> that was passed through as part of the <see cref="HummClient.ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/> 
		/// request (or the <see cref="HummClient.SendReceiptAsync(SendReceiptRequest)"/> request), or the humm purchase number that was returned from the call to ProcessAuthorisationAsync. 
		/// In the case of the former, the <see cref="ProcessAuthorisationRequest.ClientTransactionReference"/> must be unique among all sellers in a chain of sellers (unique among all devices sharing a merchant id).
		/// In the case of the latter, the POS software would be required to store the <see cref="ProcessAuthorisationResponse.PurchaseNumber"/> retured by <see cref="HummClient.ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/>.	
		/// </para>
		/// <para>Maximum length of 64 characters.</para>
		/// </remarks>
		[JsonProperty("x_purchase_ref")]
		public string? PurchaseReference { get; set; }

		/// <summary>
		/// Gets or sets the amount of the refund.
		/// </summary>
		/// <value>
		/// The refund amount as a dollar value in decimal format.
		/// </value>
		/// <remarks>
		/// <para>Although the Humm API requires this value to be sent as a number of cents, this library uses a dollar value and will perform the conversion to cents for you.
		/// For example, to send $119.50 set this value to 119.5.</para>
		/// <para>Maximum length of 64 characters.</para>
		/// </remarks>
		[JsonProperty("x_amount")]
		public decimal Amount { get; set; }

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <remarks>
		/// <para>Ensures <seealso cref="ClientTransactionReference"/> and <see cref="PurchaseReference"/> are not null, empty strings or contain only whitespace. Also ensures they are not longer than allowed.</para>
		/// <para>Also ensures all base properties are valid, see <see cref="RequestBase.Validate"/>.</para>
		/// </remarks>
		public override void Validate()
		{
			ClientTransactionReference.GuardNullOrWhiteSpace(nameof(ClientTransactionReference));
			ClientTransactionReference.GuardLength(nameof(ClientTransactionReference), 64);

			PurchaseReference.GuardNullOrWhiteSpace(nameof(PurchaseReference));
			PurchaseReference.GuardLength(nameof(PurchaseReference), 64);

			base.Validate();
		}
	}
}
