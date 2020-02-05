using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Represents a request to associate a POS receipt number with a prior authorisation request.
	/// </summary>
	/// <seealso cref="HummClient.SendReceiptAsync(SendReceiptRequest)"/>
	/// <seealso cref="SendReceiptResponse"/>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.RequestBase" />
	public sealed class SendReceiptRequest : RequestBase
	{
		/// <summary>
		/// Required. Gets or sets the client transaction reference of the authorisation to update.
		/// </summary>
		/// <value>
		/// This must be the same reference used in the prior <see cref="HummClient.ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/> request to be udpated.
		/// </value>
		/// <remarks>
		/// <para>Maximum length of 64 characters.</para>
		/// </remarks>
		[JsonProperty("x_pos_transaction_ref")]
		public string? ClientTransactionReference { get; set; }

		/// <summary>
		/// Required. Gets or sets the receipt number.
		/// </summary>
		/// <value>
		/// The new receipt number to associate with the authorisation.
		/// </value>
		/// <remarks>
		/// <para>This must be the same reference (x_pos_transaction_ref) that would get passed through on future <see cref="HummClient.ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest)"/> requests.</para>
		/// <para>Maximum length of 64 characters.</para>
		/// </remarks>
		[JsonProperty("x_receipt_number")]
		public string? ReceiptNumber { get; set; }

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <remarks>
		/// <para>Ensures <seealso cref="ClientTransactionReference"/> and <see cref="ReceiptNumber"/> are not null, empty strings or contain only whitespace. Also ensures they are not longer than allowed.</para>
		/// <para>Also ensures all base properties are valid, see <see cref="RequestBase.Validate"/>.</para>
		/// </remarks>
		public override void Validate()
		{
			ClientTransactionReference.GuardNullOrWhiteSpace(nameof(ClientTransactionReference));
			ClientTransactionReference.GuardLength(nameof(ClientTransactionReference), 64);

			ReceiptNumber.GuardNullOrWhiteSpace(nameof(ReceiptNumber));
			ReceiptNumber.GuardLength(nameof(ReceiptNumber), 64);

			base.Validate();
		}
	}
}
