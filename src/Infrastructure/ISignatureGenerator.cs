using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Humm.InStore.Infrastructure
{
	/// <summary>
	/// A generic interface for components that can calculate a digital signature for values from a Humm request or response.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface ISignatureGenerator : IDisposable
	{
		/// <summary>
		/// Generates the signature and returns it as a string.
		/// </summary>
		/// <remarks>
		/// <para>Humm signatures are currently case-insensitive hexadecimal strings.</para>
		/// <para>Only values from the <paramref name="properties"/> argument where the key starts with "x_" will be used, so the caller may pass the full property set if desired.
		/// Values of type decimal are treated as dollar values and will be automatically multiplied by 100, so should be passed as their decimal dollar representation.
		/// Values that are null will be ignored (treated as a missing/undeclared property and excluded from the signature generation).
		/// </para>
		/// </remarks>
		/// <param name="properties">A dictionary of values to be considered for use in the signature generation. See remarks for more details.</param>
		/// <returns>A string containing the signature calculated from the properties provided.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="properties"/> is null.</exception>
		string GenerateSignature(IEnumerable<KeyValuePair<string, object>> properties);
	}
}
