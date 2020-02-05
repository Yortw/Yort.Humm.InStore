using System;
using System.Collections.Generic;
using System.Text;
using Ladon;
using Newtonsoft.Json;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Represents a prior <see cref="HummClient.ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest)"/> request be reversed/undone/cancelled.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.RequestBase" />
	/// <seealso cref="ProcessSalesAdjustmentReversalRequest"/>
	/// <seealso cref="HummClient.ProcessSalesAdjustmentReversalAsync(ProcessSalesAdjustmentReversalRequest)"/>
	public sealed class ProcessSalesAdjustmentReversalRequest : RequestBase
	{
		/// <summary>
		/// Required. Gets or sets the client transaction reference for this reversal.
		/// </summary>
		/// <value>
		/// This is the transaction reference of the sales adjustment reversal.
		/// </value>
		/// <remarks>
		/// <para>
		/// <para>Maximum length of 64 characters.</para>
		/// </para>
		/// </remarks>
		[JsonProperty("x_pos_transaction_ref")]
		public string? ClientTransactionReference { get; set; }

		/// <summary>
		/// Required. Gets or sets the adjustment signature.
		/// </summary>
		/// <value>
		/// The adjustment signature.
		/// </value>
		/// <remarks>
		/// <para>
		/// The original adjustment signature that we are trying to reverse. We are using the Sales Adjustment Signature as we can not rely on the 
		/// Sales Adjustment to return a result. (e.g.Network Issues). See the Humm documentation at https://docs.shophumm.com.au/pos/api/process_adjustment_reversal/
		/// </para>
		/// <para>Maximum length of 200 characters.</para>
		/// </remarks>
		[JsonProperty("x_adjustment_signature")]
		public string? AdjustmentSignature { get; set; }

		/// <summary>
		/// Validates this instance.
		/// </summary>
		/// <remarks>
		/// <para>Ensures <seealso cref="ClientTransactionReference"/> and <see cref="AdjustmentSignature"/> are not null, empty strings or contain only whitespace. Also ensures they are not longer than allowed.</para>
		/// <para>Also ensures all base properties are valid, see <see cref="RequestBase.Validate"/>.</para>
		/// </remarks>
		public override void Validate()
		{
			ClientTransactionReference.GuardNullOrWhiteSpace(nameof(ClientTransactionReference));
			ClientTransactionReference.GuardLength(nameof(ClientTransactionReference), 64);

			AdjustmentSignature.GuardNullOrWhiteSpace(nameof(AdjustmentSignature));
			AdjustmentSignature.GuardLength(nameof(AdjustmentSignature), 200);

			base.Validate();
		}
	}
}
