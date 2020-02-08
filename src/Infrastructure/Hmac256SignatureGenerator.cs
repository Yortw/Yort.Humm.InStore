using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Ladon;
using System.Linq;

namespace Yort.Humm.InStore.Infrastructure
{
	/// <summary>
	/// Used to generate signatures using the <see cref="HMACSHA256"/> hash algorithm, currently the default and only supported algorithm by Humm.
	/// </summary>
	/// <seealso cref="Yort.Humm.InStore.Infrastructure.ISignatureGenerator" />
	public sealed class Hmac256SignatureGenerator : ISignatureGenerator
	{
		private HMACSHA256? _Hasher;

		/// <summary>
		/// Initializes a new instance of the <see cref="Hmac256SignatureGenerator"/> class.
		/// </summary>
		/// <param name="apiKey">The secret key to use when generating the signature. This should be a Humm Device Key (<see cref="HummClientConfiguration.DeviceKey"/>).</param>
		public Hmac256SignatureGenerator([ValidatedNotNull] string apiKey)
		{
			apiKey.GuardNullOrEmpty(nameof(apiKey));
			_Hasher = new HMACSHA256(Encoding.UTF8.GetBytes(apiKey));
		}

		/// <summary>
		/// Generates the signature and returns it as a string.
		/// </summary>
		/// <param name="properties">A dictionary of values to be considered for use in the signature generation. See remarks for more details.</param>
		/// <returns>
		/// A string containing the signature calculated from the properties provided.
		/// </returns>
		/// <exception cref="ObjectDisposedException">Hmac256SignatureGenerator</exception>
		/// <remarks>
		/// <para>Humm signatures are currently case-insensitive hexadecimal strings.</para>
		/// <para>Only values from the <paramref name="properties" /> argument where the key starts with "x_" will be used, so the caller may pass the full property set if desired.
		/// Values of type decimal are treated as dollar values and will be automatically multiplied by 100, so should be passed as their decimal dollar representation.
		/// Values that are null will be ignored (treated as a missing/undeclared property and excluded from the signature generation).
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="properties"/> is null.</exception>
		public string GenerateSignature(IEnumerable<KeyValuePair<string, object>> properties)
		{
			properties.GuardNull(nameof(properties));
			if (_Hasher == null) throw new ObjectDisposedException(nameof(Hmac256SignatureGenerator));

			var payload = GeneratePayload(properties);
			if (String.IsNullOrEmpty(payload)) return String.Empty;

			var hashBytes = _Hasher.ComputeHash(Encoding.UTF8.GetBytes(payload));
			return BytesToHexString(hashBytes);
		}

		private string BytesToHexString([ValidatedNotNull] byte[] hashBytes)
		{
			if ((hashBytes?.Length ?? 0) == 0) return String.Empty;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var sb = new StringBuilder(hashBytes.Length * 2);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			for (int cnt = 0; cnt < hashBytes.Length; cnt++)
			{
				sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0:x2}", hashBytes[cnt]);
			}
			return sb.ToString();
		}

		private string GeneratePayload(IEnumerable<KeyValuePair<string, object>> properties)
		{
			var sb = new StringBuilder(1024);

			var signatureProperties =
			(
				from kvp
				in properties
				where kvp.Key.StartsWith("x_", StringComparison.OrdinalIgnoreCase) 
				orderby kvp.Key
				select kvp
			);

			foreach (var kvp in signatureProperties)
			{
				sb.Append(kvp.Key);
				if (kvp.Value is decimal dv)
					sb.Append(Convert.ToInt32(dv * 100).ToString(System.Globalization.CultureInfo.InvariantCulture)); 
				else
					sb.Append(kvp.Value);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_Hasher?.Dispose();
			_Hasher = null;
		}

	}
}
