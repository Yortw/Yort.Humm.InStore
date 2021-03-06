﻿using System;
using System.Threading.Tasks;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// An interface for the <see cref="HummClient"/> provided to allow mocking/stubbing etc.
	/// </summary>
	/// <seealso cref="HummClient"/>
	public interface IHummClient
	{
		/// <summary>
		/// Raised when a <see cref="ProcessAuthorisationResponse"/> is received with a status of <see cref="RequestStates.Pending"/> and the <see cref="ProcessAuthorisationRequest.AutoHandlePendingResponse"/> property of the request was true.
		/// </summary>
		/// <remarks>
		/// <para>While the <see cref="ProcessAuthorisationRequest.AutoHandlePendingResponse"/> option frees the client from having to handle the repeat API call logic to Humm in the case of a pending 'big things' purchase flow, 
		/// this event can be used by the client to update it's UI to indicate the pending status and/or when the next recheck will occur if it desires.</para>
		/// </remarks>
		event EventHandler<PendingAuthorisationEventArgs> PendingAuthorisation;

		/// <summary>
		/// Sets the device key used for generating digital signatures.
		/// </summary>
		/// <remarks>
		/// <para>The device key is usually provided via the <see cref="HummClientConfiguration"/> constructor argument if known before client is created. This method is typically used when registering a new device via <see cref="CreateKeyAsync(CreateKeyRequest)"/> and not electing to have the token automatically applied.</para>
		/// <para>Null can be provided to clear the currently assigned device key (if any), prior to calling <see cref="CreateKeyAsync(CreateKeyRequest)"/> to generate a new device key.</para>
		/// </remarks>
		/// <param name="deviceKey">A string containing the device key previously returns by a <see cref="CreateKeyAsync(CreateKeyRequest)"/> request.</param>
		/// <seealso cref="CreateKeyAsync(CreateKeyRequest)"/>
		/// <seealso cref="HummClientConfiguration"/>
		void SetDeviceKey(string deviceKey);

		/// <summary>
		///	Requests a new device key from Humm using the (initialisation) device token created in the Humm Seller (merchant) portal.
		/// </summary>
		/// <remarks>
		/// <para>If the <see cref="CreateKeyRequest.AutoUpdateClientToken"/> argument is true and the request is successsful, the client will automatically call <see cref="SetDeviceKey(string)"/> with the value of <see cref="CreateKeyResponse.Key"/> for you, ensuring all future calls made with this client instance use the new token.</para>
		/// </remarks>
		/// <param name="request">A <see cref="CreateKeyRequest"/> instance containing details of the request to make.</param>
		/// <returns>A <see cref="CreateKeyResponse"/> instance containing the Humm response.</returns>
		/// <seealso cref="CreateKeyRequest"/>
		/// <seealso cref="CreateKeyResponse"/>
		/// <seealso cref="SetDeviceKey(string)"/>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		Task<CreateKeyResponse> CreateKeyAsync(CreateKeyRequest request);

		/// <summary>
		/// Sends an SMS to the customer's mobile phone inviting them to sign-up as a Humm customer.
		/// </summary>
		/// <param name="request">A <see cref="InviteRequest"/> instance.</param>
		/// <returns>A <see cref="InviteResponse"/>.</returns>
		/// <seealso cref="InviteRequest"/>
		/// <seealso cref="InviteResponse"/>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the response signature cannot be validated.</exception>
		/// <exception cref="System.ObjectDisposedException">Thrown if this instance has been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if this instance has not been initialised with a non-null device key (<see cref="SetDeviceKey(string)"/> and <see cref="HummClientConfiguration.DeviceKey"/>)..</exception>
		Task<InviteResponse> InviteAsync(InviteRequest request);

		/// <summary>
		/// Requests an authorisation (for payment) be processed.
		/// </summary>
		/// <remarks>
		/// <para>This method is used to request a payment contract be setup for a specified amount using an approval code generated by the customer's device.
		/// It is the most common API call made to Humm in a 'happy path'.
		/// </para>
		/// <para>If the <see cref="ProcessAuthorisationRequest.AutoHandlePendingResponse"/> value is true and the response indicates a <see cref="RequestStates.Pending"/> status then
		/// this call will raise the <see cref="PendingAuthorisation"/> event, wait for the specified retry interval, and the re-make the request again. This will be repeated until a 
		/// non-pending status is returned, or an error is thrown from one of the handlers for the <see cref="PendingAuthorisation"/> event. In this case the response returned 
		/// to the caller will be the final status (success/failure/error).
		/// </para>
		/// <para>
		/// If <see cref="ProcessAuthorisationRequest.AutoHandlePendingResponse"/> is false then the first response will be returned to the caller and if the status is <see cref="RequestStates.Pending"/> 
		/// it is up to the caller to repeat the request until a final state is reached.
		/// </para>
		/// </remarks>
		/// <param name="request">A <see cref="ProcessAuthorisationRequest"/> instance.</param>
		/// <returns>A <see cref="ProcessAuthorisationResponse"/> instance.</returns>
		/// <seealso cref="PendingAuthorisation"/>
		/// <seealso cref="ProcessAuthorisationRequest"/>
		/// <seealso cref="ProcessAuthorisationResponse"/>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the response signature cannot be validated.</exception>
		/// <exception cref="System.ObjectDisposedException">Thrown if this instance has been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if this instance has not been initialised with a non-null device key (<see cref="SetDeviceKey(string)"/> and <see cref="HummClientConfiguration.DeviceKey"/>)..</exception>
		Task<ProcessAuthorisationResponse> ProcessAuthorisationAsync(ProcessAuthorisationRequest request);

		/// <summary>
		/// Registers a POS transaction number against a prior authorisation.
		/// </summary>
		/// <remarks>
		/// <para>This method is only used if the <see cref="ProcessAuthorisationRequest.ClientTransactionReference"/> was a temporary value and not a POS transaction number.
		/// In this case, when the POS finally assigns a permanent transaction number this method can be called to update the payment with the relevant reference.</para>
		/// <para>See the Humm documentation at https://docs.shophumm.com.au/pos/api/send_receipt/ and https://docs.shophumm.com.au/pos/api_information/retries_and_idempotency/ </para>
		/// </remarks>
		/// <param name="request">A <see cref="SendReceiptRequest"/> instance.</param>
		/// <returns>A <see cref="SendReceiptResponse"/> instance containing the result of the request.</returns>
		Task<SendReceiptResponse> SendReceiptAsync(SendReceiptRequest request);

		/// <summary>
		/// A sales adjustment is used to refund money in the case of an item that is returned or fails to ship etc. 
		/// </summary>
		/// <remarks>
		/// <para>A sales adjustment can be used for full or partial refund of previously approved <see cref="ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/>.</para>
		/// <para>See the Humm documentation at https://docs.shophumm.com.au/pos/api/process_sales_adjustment/ and https://docs.shophumm.com.au/pos/api_information/retries_and_idempotency/ </para>
		/// </remarks>
		/// <param name="request">A <see cref="ProcessSalesAdjustmentRequest"/> instance.</param>
		/// <returns>A <see cref="ProcessSalesAdjustmentResponse"/> instance.</returns>
		Task<ProcessSalesAdjustmentResponse> ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest request);

		/// <summary>
		/// This endpoint is used to process a reversal of a <see cref="ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest)"/> at the point-of-sale
		/// </summary>
		/// <remarks>
		/// <para>This is typically used to undo a refund in the case where it was made in error, or where the outcome was unknown (due to a network timeout/error) 
		/// and an idempotent retry is not desirable. See the Humm documentation at; https://docs.shophumm.com.au/pos/api_information/retries_and_idempotency/ </para>
		/// </remarks>
		/// <param name="request">A <see cref="ProcessSalesAdjustmentReversalRequest"/> instance.</param>
		/// <returns>A <see cref="ProcessSalesAdjustmentReversalResponse"/> instance.</returns>
		Task<ProcessSalesAdjustmentReversalResponse> ProcessSalesAdjustmentReversalAsync(ProcessSalesAdjustmentReversalRequest request);

	}
}