using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Thrown if the signature on a <see cref="Infrastructure.ResponseBase"/> did not verify as correct.
	/// </summary>
	/// <seealso cref="System.Exception" />
	[Serializable]
	public class HummResponseSignatureException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HummResponseSignatureException"/> class.
		/// </summary>
		public HummResponseSignatureException() : this(ErrorMessages.Response_SignatureMismatch) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="HummResponseSignatureException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		[ExcludeFromCodeCoverage]
		public HummResponseSignatureException(string message) : base(message) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="HummResponseSignatureException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner exception.</param>
		[ExcludeFromCodeCoverage]
		public HummResponseSignatureException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="HummResponseSignatureException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		[ExcludeFromCodeCoverage]
		protected HummResponseSignatureException(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
