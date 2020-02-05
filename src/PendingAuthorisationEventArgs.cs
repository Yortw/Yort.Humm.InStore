using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Arguments for the <see cref="HummClient.PendingAuthorisation"/> event.
	/// </summary>
	/// <seealso cref="System.EventArgs" />
	public class PendingAuthorisationEventArgs : EventArgs
	{
		private readonly string? _ClientReference;
		private readonly int _RetryDuration;
		private readonly Dictionary<string, string>? _TrackingData;

		/// <summary>
		/// Initializes a new instance of the <see cref="PendingAuthorisationEventArgs"/> class.
		/// </summary>
		/// <param name="clientReference">The client reference of the request associated with this event.</param>
		/// <param name="retryDuration">The minimum amount of time, in seconds, to wait before re-checking the request status.</param>
		/// <param name="trackingData">The tracking data associated with the original request, if any (may be null).</param>
		public PendingAuthorisationEventArgs(string? clientReference, int retryDuration, Dictionary<string, string>? trackingData)
		{
			_ClientReference = clientReference;
			_RetryDuration = retryDuration;
			_TrackingData = trackingData;
		}

		/// <summary>
		/// Returns the client reference of the request that caused this event.
		/// </summary>
		/// <value>
		/// The client reference of the <see cref="ProcessAuthorisationRequest"/> that is pending.
		/// </value>
		public string? ClientReference { get { return _ClientReference; } }
		/// <summary>
		/// Returns the minimum number of seconds to wait until re-checking the request status.
		/// </summary>
		/// <value>
		/// The duration of the retry interval (in seconds).
		/// </value>
		public int RetryDuration { get { return _RetryDuration; } }
		/// <summary>
		/// Returns the tracking data associated with the original request, if any (may be null).
		/// </summary>
		/// <value>
		/// The tracking data from the original <see cref="ProcessAuthorisationRequest"/>.
		/// </value>
		public Dictionary<string, string>? TrackingData { get { return _TrackingData; } }
	}
}
