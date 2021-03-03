using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Provides constants for known Humm status codes (returned in <see cref="Infrastructure.ResponseBase.Code"/>).
	/// </summary>
	public static class HummStatusCodes
	{
		/// <summary>
		/// Success.
		/// </summary>
		public const string CreateKeySuccess = "SCRK01";

		/// <summary>
		/// Success.
		/// </summary>
		public const string InviteSuccess = "SINV01";

		/// <summary>
		/// Success.
		/// </summary>
		public const string ProcessAuthorisationSuccess = "SPRA01";

		/// <summary>
		/// Success.
		/// </summary>
		public const string ProcessSalesAdjustmentSuccess = "SPSA01";

		/// <summary>
		/// Success.
		/// </summary>
		public const string ProcessSalesAdjustmentReversalSuccess = "SPAR01";

		/// <summary>
		/// Success.
		/// </summary>
		public const string SendReceiptSuccess = "SSER01";

		/// <summary>
		/// Authorisation is pending (big things flow only).
		/// </summary>
		public const string AuthorisationPending = "SPND01";

		/// <summary>
		/// The payment request was cancelled (big things flow only).
		/// </summary>
		public const string Cancelled = "FCNL01";

		/// <summary>
		/// Declined due to internal risk assessment against the customer.
		/// </summary>
		public const string DeclinedDueToCreditRisk = "FPRA01";

		/// <summary>
		/// Declined due to insufficient funds for the deposit.
		/// </summary>
		public const string DeclinedDueToInsufficientFunds = "FPRA02";

		/// <summary>
		/// Declined due to bank unavailable (comms issue)
		/// </summary>
		public const string DeclinedDueToBankUnavailable = "FPRA03";

		/// <summary>
		/// Declined due to credit limit exceeded.
		/// </summary>
		public const string DeclinedDueToCreditLimit = "FPRA04";

		/// <summary>
		/// Declined due to payment history.
		/// </summary>
		public const string DeclinedDueToPaymentHistory = "FPRA05";

		/// <summary>
		/// Declined due to credit card for deposit being expired.
		/// </summary>
		public const string DeclinedDueToCardExpired = "FPRA06";

		/// <summary>
		/// Declined because supplied posTransactionRef has already been processed.
		/// </summary>
		public const string DuplicatePosTransactionReference = "FPRA07";

		/// <summary>
		/// Declined because the instalment amount was below the minimum threshold.
		/// </summary>
		public const string DeclinedDueToInstallmentAmountBelowMinimum = "FPRA08";

		/// <summary>
		/// Declined because purchase amount exceeded pre-approved amount.
		/// </summary>
		public const string DeclinedDueToInstallmentAmountExceededPreapprovedAmount = "FPRA08";

		/// <summary>
		/// The Barcode was not found.
		/// </summary>
		public const string BarcodeNotFound = "FPRA21";

		/// <summary>
		/// The Barcode has already been used.
		/// </summary>
		public const string BarcodeAlreadyUsed = "FPRA22";

		/// <summary>
		/// The Barcode has expired.
		/// </summary>
		public const string BarcodeHasExpired = "FPRA23";

		/// <summary>
		/// The Barcode has been cancelled.
		/// </summary>
		public const string BarcodeCancelled = "FPRA24";

		/// <summary>
		/// Declined.
		/// </summary>
		public const string Declined = "FPRA99";

		/// <summary>
		/// Unable to find the specified POS transaction reference
		/// </summary>
		public const string PosTransactionReferenceNotFoundForSalesAdjustment = "FPSA01";

		/// <summary>
		/// This contract has already been completed.
		/// </summary>
		public const string ContractAlreadyCompletedForSalesAdjustment = "FPSA02";

		/// <summary>
		/// This humm contract has previously been cancelled and all payments collected have been refunded to the customer.
		/// </summary>
		public const string ContractCancelledAndRefundedForSalesAdjustment = "FPSA03";

		/// <summary>
		/// Sales adjustment cannot be processed for this amount.
		/// </summary>
		public const string InvalidAmountForSalesAdjustment = "FPSA04";

		/// <summary>
		/// Unable to process a sales adjustment for this contract. Please contact Merchant Services on [CollectionsPhone] during business hours for further information.
		/// </summary>
		public const string UnableToProcessSalesAdjustmentForContract = "FPSA05";

		/// <summary>
		/// Sales adjustment cannot be processed. Please call humm Collections on [CollectionsPhone].
		/// </summary>
		public const string UnableToProcessSalesAdjustment = "FPSA06";

		/// <summary>
		/// Sales adjustment cannot be processed at this store.
		/// </summary>
		public const string SalesAdjustmentCannotBeProcessedAtThisStore = "FPSA07";

		/// <summary>
		/// Sales adjustment cannot be processed for this transaction. Duplicate receipt number found.
		/// </summary>
		public const string SalesAdjustmentCannotBeProcessedDuplicateReceipt = "FPSA08";

		/// <summary>
		/// Sales adjustment cannot be processed for this transaction. Duplicate receipt number found.
		/// </summary>
		public const string SalesAdjustmentAmountMustBeGreaterThanZero = "FPSA09";

		/// <summary>
		/// Unable to find the specified POS signature.
		/// </summary>
		public const string ProcessSalesAdjustmentReversal = "FPAR01";

		/// <summary>
		/// Contract has already been completed.
		/// </summary>
		public const string ContractAlreadyCompletedForProcessSalesAdjustmentReversal = "FPAR02";

		/// <summary>
		/// This humm contract has previously been cancelled and all payments collected have been refunded to the customer.
		/// </summary>
		public const string ContractCancelledAndRefundedForProcessSalesAdjustmentReversal = "FPAR03";

		/// <summary>
		/// Unable to process a sales adjustment for this contract. Please contact Merchant Services on [CollectionsPhone] during business hours for further information.
		/// </summary>
		public const string UnableToProcessSalesAdjustmentReversalForContract = "FPAR05";

		/// <summary>
		/// Sales adjustment adjustment cannot be processed. Please call humm Collections on [CollectionsPhone].
		/// </summary>
		public const string UnableToProcessSalesAdjustmentReversal = "FPAR06";

		/// <summary>
		/// Sales adjustment adjustment cannot be processed at this store.
		/// </summary>
		public const string UnableToProcessSalesAdjustmentReversalAtThisStore = "FPAR07";

		/// <summary>
		/// Sales adjustment adjustment cannot be processed for this transaction. Duplicate signature found.
		/// </summary>
		public const string UnableToProcessSalesAdjustmentReversalDuplicateSignature = "FPAR08";

		/// <summary>
		/// Unable to find the specified POS transaction reference.
		/// </summary>
		public const string SendReceiptUnableToFindPosTransactionReference = "FSER01";

		/// <summary>
		/// The specified POS transaction reference is already in use.
		/// </summary>
		public const string SendReceiptDuplicateTransactionReference = "FSER01";

		/// <summary>
		/// The specified POS transaction reference is already in use.
		/// </summary>
		public const string CreateKeyDeviceTokenNotFound = "FCRK01";

		/// <summary>
		/// The specified POS transaction reference is already in use.
		/// </summary>
		public const string CreateKeyDeviceTokenAlreadyUsed = "FCRK02";

		/// <summary>
		/// 	Validation error.
		/// </summary>
		public const string ValidationError = "EVAL01";
		/// <summary>
		/// Minimum finance amount is $1.
		/// </summary>
		public const string MinimumFinanceAmount = "EVAL02";
		/// <summary>
		/// Authentication error.
		/// </summary>
		public const string AuthenticationError = "EAUT01";
		/// <summary>
		/// Signature mismatch error.
		/// </summary>
		public const string SignatureMismatchError = "ESIG01";
		/// <summary>
		/// Internal server error.
		/// </summary>
		public const string InternalServerError = "EISE01";

	}
}
