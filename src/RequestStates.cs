using System;
using System.Collections.Generic;
using System.Text;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Provides a set of constants for possible <see cref="ResponseBase.Status"/> values.
	/// </summary>
	/// <remarks>
	/// <para>See the Humm documentation at <a href="https://docs.shophumm.com.au/pos/api_information/status_codes/"/>.</para>
	/// <para>
	/// The most common statuses are:
	/// <list type="bullet">
	/// <item><description><b>Success</b> When the intent of the request is successful e.g. an Approval from the ProcessAuthorisation API.</description></item>
	/// <item><description><b>Failed</b> When the intent of the request is unsuccessful e.g. a Decline from the ProcessAuthorisation API.</description></item>
	/// <item><description><b>Error</b> When there is a problem with the request or an unexpected error.</description></item>
	/// </list>
	/// </para>
	/// </remarks>
	public static class RequestStates
	{
		/// <summary>
		/// The request was succesfully processed and approved.
		/// </summary>
		public const string Success = "Success";
		/// <summary>
		/// The request is pending and needs more time to be processed. Wait for the specified interval and retry the request.
		/// </summary>
		public const string Pending = "Pending";
		/// <summary>
		/// The request was cancelled.
		/// </summary>
		public const string Cancelled = "Cancelled";
		/// <summary>
		/// The request failed or was declined.
		/// </summary>
		public const string Failed = "Failed";
		/// <summary>
		/// The request failed due to an error.
		/// </summary>
		public const string Error = "Error";
	}
}
