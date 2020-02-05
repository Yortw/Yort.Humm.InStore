using System;
using System.Collections.Generic;
using System.Text;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// Contains details of a response to a <see cref="ProcessAuthorisationRequest"/>.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.ResponseBase" />
	/// <seealso cref="HummClient.SendReceiptAsync(SendReceiptRequest)"/>
	/// <seealso cref="SendReceiptRequest"/>
	public sealed class SendReceiptResponse : ResponseBase
	{
	}
}
