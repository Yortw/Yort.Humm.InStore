using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using Ladon;
using Newtonsoft.Json.Serialization;

namespace Yort.Humm.InStore.Infrastructure
{
	/// <summary>
	/// Used to write a Humm request as json including the relevant signature in the output.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public sealed class SignedRequestWriter : IDisposable
	{
		private ISignatureGenerator? _SignatureGenerator;
		private readonly System.Text.Encoding _Encoding;
		private readonly Newtonsoft.Json.JsonSerializer _Serialiser;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignedRequestWriter"/> class.
		/// </summary>
		/// <param name="signatureGenerator">The signature generator.</param>
		public SignedRequestWriter(ISignatureGenerator signatureGenerator)
		{
			_SignatureGenerator = signatureGenerator.GuardNull(nameof(signatureGenerator));
			_Encoding = new System.Text.UTF8Encoding(false);
			_Serialiser = new Newtonsoft.Json.JsonSerializer()
			{
				Formatting = Newtonsoft.Json.Formatting.None
			};
		}

		/// <summary>
		/// Writes the specified request as to a string and returns the result.
		/// </summary>
		/// <typeparam name="T">The type of request to be written, must be a .Net reference type.</typeparam>
		/// <param name="request">The request to write.</param>
		/// <returns>A string containing the request written as json and containing the required digital signature.</returns>
		/// <exception cref="ObjectDisposedException">Thrown if this instance is disposed.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		/// <seealso cref="WriteRequest{T}(T, System.IO.Stream)"/>
		/// <seealso cref="Dispose"/>
		public string WriteRequest<T>(T request) where T : class
		{
			using (var stream = new System.IO.MemoryStream())
			{
				WriteRequest(request, stream);

				return System.Text.UTF8Encoding.UTF8.GetString(stream.ToArray());
			}
		}

		/// <summary>
		/// Writes the specified request to the specified stream.
		/// </summary>
		/// <typeparam name="T">The type of request to be written, must be a .Net reference type.</typeparam>
		/// <param name="request">The request to write.</param>
		/// <param name="outputStream">The stream to write to.</param>
		/// <exception cref="ObjectDisposedException">Thrown if this instance is disposed.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> or <paramref name="outputStream"/> is null.</exception>
		/// <seealso cref="WriteRequest{T}(T)"/>
		/// <seealso cref="Dispose"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Analyzer appears confused. Argument is marked validated, there is a guard clause, and compiler says it is not null at point it is used. Believe this warning is in error.")]
		public void WriteRequest<T>(T request, [ValidatedNotNull] System.IO.Stream outputStream) where T : class 
		{
			request.GuardNull(nameof(request));
			outputStream.GuardNull(nameof(outputStream));

			if (_SignatureGenerator == null) throw new ObjectDisposedException(nameof(SignedRequestWriter));

			using (var textWriter = new System.IO.StreamWriter(outputStream, _Encoding, 1024, true))
			using (var writer = new Newtonsoft.Json.JsonTextWriter(textWriter))
			{
				writer.WriteStartObject();

				var values = HummModelPropertyReader.GetPropertyValues(request);

				foreach (var kvp in values)
				{
					if (kvp.Value == null) continue;

					writer.WritePropertyName(kvp.Key);
					//TODO: This should be more declarative/automatic so it handles other types
					//but haven't thought of a good solution at the moment and this is the only type that
					//matters for now.
					if (kvp.Value is PurchaseItemsCollection items && items != null)
					{
						//Weird, this is required to be a string containing json, not nested json.
						var tmpValue = GetPurchaseItemsJsonString(kvp.Value);
						writer.WriteValue(tmpValue);
					}
					else if (kvp.Value is decimal dv)
					{
						writer.WriteValue(Convert.ToInt32(dv * 100));
					}
					else if (kvp.Value is Dictionary<string, string> dict)
					{
						writer.WriteStartObject();
						foreach (var dictEntry in dict)
						{
							writer.WritePropertyName(dictEntry.Key);
							writer.WriteValue(dictEntry.Value);
						}
						writer.WriteEndObject();
					}
					else
						writer.WriteValue(kvp.Value);
				}

				string signature = _SignatureGenerator.GenerateSignature(values);
				writer.WritePropertyName("signature");
				writer.WriteValue(signature);

				writer.WriteEndObject();

				writer.Flush();
				textWriter.Flush();
				outputStream.Flush();
			}
		}

		private string GetPurchaseItemsJsonString(object value)
		{
			using (var outputStream = MemoryStreamFactory.CreateStream())
			using (var writer = new System.IO.StreamWriter(outputStream, _Encoding, 1024, true))
			using (var jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer))
			{
				jsonWriter.ArrayPool = JsonArrayPool.Instance;

				writer.Write("{\"PurchaseItems\":");
				_Serialiser.Serialize(jsonWriter, value);
				writer.Write("}");

				jsonWriter.Flush();
				writer.Flush();
				outputStream.Flush();

				outputStream.Seek(0, System.IO.SeekOrigin.Begin);
				using (var reader = new System.IO.StreamReader(outputStream, _Encoding,false, 1024, true))
				{
					return reader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <remarks>
		/// <para>Will dispose the <see cref="ISignatureGenerator"/> passed into the constructor when this instance was created.</para>
		/// </remarks>
		public void Dispose()
		{
			_SignatureGenerator?.Dispose();
			_SignatureGenerator = null;
		}
	}
}