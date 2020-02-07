using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ladon;
using Yort.Humm.InStore.Infrastructure;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// The primary object used to make requests to Humm.
	/// </summary>
	/// <remarks>
	/// <para>Each instance of this class represents a single POS device, if being used from a centralised service or a web server then you will need to create an instance for each device, either per-request or pooled as neccesary.</para>
	/// <para>Details about the device the instance represents are provided via the <see cref="HummClientConfiguration"/> instance passed into the constructor.</para>
	/// <para>Note that financial/dollar values should be expressed as <see cref="System.Decimal"/> in their dollar &amp; cents format, i.e $20.50 should be set as 20.50 on 
	/// a request object, the library will take care of converting this to a cents value for Humm.</para>
	/// <para>This object makes requests via the REST interface, SOAP is not currently supported by this library.</para>
	/// <para>See the Humm API documentation at; https://docs.shophumm.com.au/pos/getting-started/ </para>
	/// </remarks>
	/// <_Config cref="Yort.Humm.InStore.IHummClient" />
	/// <seealso cref="HummClientConfiguration"/>
	/// <seealso cref="System.IDisposable" />
	public sealed class HummClient : IHummClient, IDisposable
	{
		private readonly HummClientConfiguration _Config;
		private System.Net.Http.HttpClient _Client;
		private readonly Uri? _BaseUrl;
		private bool _IsDisposed;

		private ISignatureGenerator? _SignatureGenerator;
		private SignedRequestWriter? _RequestWriter;

		private readonly Newtonsoft.Json.JsonSerializer _Serialiser;

		/// <summary>
		/// Raised when a <see cref="ProcessAuthorisationResponse"/> is received with a status of <see cref="RequestStates.Pending"/> and the <see cref="ProcessAuthorisationRequest.AutoHandlePendingResponse"/> property of the request was true.
		/// </summary>
		/// <remarks>
		/// <para>While the <see cref="ProcessAuthorisationRequest.AutoHandlePendingResponse"/> option frees the client from having to handle the repeat API call logic to Humm in the case of a pending 'big things' purchase flow, 
		/// this event can be used by the client to update it's UI to indicate the pending status and/or when the next recheck will occur if it desires.</para>
		/// </remarks>
		public event EventHandler<PendingAuthorisationEventArgs> PendingAuthorisation;

		/// <summary>
		/// Constructs a new instance using the specified configuration.
		/// </summary>
		/// <param name="config">A <see cref="HummClientConfiguration"/> specifying details of how this instance is configured.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="config"/> is null, or <see cref="HummClientConfiguration.BaseApiUrl"/> is null.</exception>
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "The argument names should not be localised.")]
		public HummClient(HummClientConfiguration config)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
		{
			_Config = config.GuardNull(nameof(config));
			_BaseUrl = config.BaseApiUrl.GuardNull(nameof(config), nameof(config.BaseApiUrl));

			_Serialiser = new Newtonsoft.Json.JsonSerializer()
			{
				Formatting = Newtonsoft.Json.Formatting.None,
			};

			ConfigureServicePoint();

			CreateHttpClient(config);

			if (!String.IsNullOrEmpty(_Config.DeviceKey))
				SetDeviceKey(_Config.DeviceKey);
		}

		/// <summary>
		/// Sets the device key used for generating digital signatures.
		/// </summary>
		/// <param name="deviceKey">A string containing the device key previously returns by a <see cref="CreateKeyAsync(CreateKeyRequest)" /> request.</param>
		/// <remarks>
		/// <para>The device key is usually provided via the <see cref="HummClientConfiguration" /> constructor argument if known before client is created. This method is typically used when registering a new device via <see cref="CreateKeyAsync(CreateKeyRequest)" /> and not electing to have the token automatically applied.</para>
		/// <para>Null can be provided to clear the currently assigned device key (if any), prior to calling <see cref="CreateKeyAsync(CreateKeyRequest)" /> to generate a new device key.</para>
		/// </remarks>
		/// <seealso cref="CreateKeyAsync(CreateKeyRequest)" />
		/// <seealso cref="HummClientConfiguration" />
		public void SetDeviceKey(string? deviceKey)
		{
			var oldWriter = _RequestWriter;
			try
			{
				if (String.IsNullOrEmpty(deviceKey))
				{
					_SignatureGenerator = null;
					_RequestWriter = null;
				}
				else
				{
#pragma warning disable CS8604 // Possible null reference argument.
					_SignatureGenerator = new Hmac256SignatureGenerator(deviceKey);
#pragma warning restore CS8604 // Possible null reference argument.
					_RequestWriter = new SignedRequestWriter(_SignatureGenerator);
				}
			}
			finally
			{
				oldWriter?.Dispose();
			}
		}

		/// <summary>
		/// Requests a new device key from Humm using the (initialisation) device token created in the Humm Seller (merchant) portal.
		/// </summary>
		/// <param name="request">A <see cref="CreateKeyRequest" /> instance containing details of the request to make.</param>
		/// <returns>
		/// <para>A <see cref="CreateKeyResponse" /> instance containing the Humm response.</para>
		/// <para>See the Humm documentation at https://docs.shophumm.com.au/pos/api/create_key/ </para>
		/// </returns>
		/// <remarks>
		/// If the <see cref="CreateKeyRequest.AutoUpdateClientToken" /> argument is true and the request is successsful, the client will automatically call <see cref="SetDeviceKey(string)" /> with the value of <see cref="CreateKeyResponse.Key" /> for you, ensuring all future calls made with this client instance use the new token.
		/// </remarks>
		/// <seealso cref="CreateKeyRequest" />
		/// <seealso cref="CreateKeyResponse" />
		/// <seealso cref="SetDeviceKey(string)" />
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="request"/>, <see cref="CreateKeyRequest.DeviceToken"/> or <see cref="CreateKeyRequest.PosVendor"/> is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown if <see cref="CreateKeyRequest.DeviceToken"/> or <see cref="CreateKeyRequest.PosVendor"/> is an empty string or contain only whitespace characters.</exception>
		/// <exception cref="System.ObjectDisposedException">Thrown if this instance has been disposed.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the response signature cannot be validated.</exception>
		/// <exception cref="System.Net.Http.HttpRequestException">Thrown if an HTTP protocol level or <see cref="System.Net.Http.HttpClient"/> pipeline error occurs.</exception>
		public async Task<CreateKeyResponse> CreateKeyAsync(CreateKeyRequest request)
		{
			if (_IsDisposed) throw new ObjectDisposedException(nameof(HummClient));
			request.GuardNull(nameof(request));
			request.FillFromConfig(_Config);
			if (_Config.AutoValidateRequests)
				request.Validate();
			
#pragma warning disable CS8604 // Possible null reference argument.
			using (var sigGen = new Hmac256SignatureGenerator(request.DeviceToken))
#pragma warning restore CS8604 // Possible null reference argument.
			using (var writer = new SignedRequestWriter(sigGen))
			{
				var response = await SendRequest<CreateKeyRequest, CreateKeyResponse>("CreateKey", request, writer, sigGen).ConfigureAwait(false);

				if (request.AutoUpdateClientToken && !String.IsNullOrEmpty(response.Key) && response.Status == RequestStates.Success)
					SetDeviceKey(response.Key);

				return response;
			}
		}

		/// <summary>
		/// Sends an SMS to the customer's mobile phone inviting them to sign-up as a Humm customer.
		/// </summary>
		/// <param name="request">A <see cref="InviteRequest"/> instance.</param>
		/// <para>See the Humm documentation at https://docs.shophumm.com.au/pos/api/invite/ </para>
		/// <returns>A <see cref="InviteResponse"/>.</returns>
		/// <seealso cref="InviteRequest"/>
		/// <seealso cref="InviteResponse"/>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the response signature cannot be validated.</exception>
		/// <exception cref="System.ObjectDisposedException">Thrown if this instance has been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if this instance has not been initialised with a non-null device key (<see cref="SetDeviceKey(string)"/> and <see cref="HummClientConfiguration.DeviceKey"/>)..</exception>
		/// <exception cref="System.Net.Http.HttpRequestException">Thrown if an HTTP protocol level or <see cref="System.Net.Http.HttpClient"/> pipeline error occurs.</exception>
		public Task<InviteResponse> InviteAsync(InviteRequest request)
		{
			GuardClientState();

			return SendRequest<InviteRequest, InviteResponse>("Invite", request);
		}

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
		/// <para>See the Humm documentation at https://docs.shophumm.com.au/pos/api/process_authorisation/ and https://docs.shophumm.com.au/pos/api_information/retries_and_idempotency/ </para>
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
		/// <exception cref="System.Net.Http.HttpRequestException">Thrown if an HTTP protocol level or <see cref="System.Net.Http.HttpClient"/> pipeline error occurs.</exception>
		public async Task<ProcessAuthorisationResponse> ProcessAuthorisationAsync(ProcessAuthorisationRequest request)
		{
			GuardClientState();

			while (true)
			{
				var response = await SendRequest<ProcessAuthorisationRequest, ProcessAuthorisationResponse>("ProcessAuthorisation", request).ConfigureAwait(false);

#pragma warning disable CA1062 // Validate arguments of public methods
				if (request.AutoHandlePendingResponse && response.Status == RequestStates.Pending)
#pragma warning restore CA1062 // Validate arguments of public methods
				{
					OnPendingAuthorisation(response, request);
					await Task.Delay(response.RetryDuration * 1000).ConfigureAwait(false);
				}
				else
				{
					return response;
				}
			}
		}

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
		/// <seealso cref="ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the response signature cannot be validated.</exception>
		/// <exception cref="System.ObjectDisposedException">Thrown if this instance has been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if this instance has not been initialised with a non-null device key (<see cref="SetDeviceKey(string)"/> and <see cref="HummClientConfiguration.DeviceKey"/>)..</exception>
		/// <exception cref="System.Net.Http.HttpRequestException">Thrown if an HTTP protocol level or <see cref="System.Net.Http.HttpClient"/> pipeline error occurs.</exception>
		public Task<SendReceiptResponse> SendReceiptAsync(SendReceiptRequest request)
		{
			GuardClientState();

			return SendRequest<SendReceiptRequest, SendReceiptResponse>("SendReceipt", request);
		}

		/// <summary>
		/// A sales adjustment is used to refund money in the case of an item that is returned or fails to ship etc. 
		/// </summary>
		/// <remarks>
		/// <para>A sales adjustment can be used for full or partial refund of previously approved <see cref="ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/>.</para>
		/// <para>See the Humm documentation at https://docs.shophumm.com.au/pos/api/process_sales_adjustment/ and https://docs.shophumm.com.au/pos/api_information/retries_and_idempotency/ </para>
		/// </remarks>
		/// <param name="request">A <see cref="ProcessSalesAdjustmentRequest"/> instance.</param>
		/// <returns>A <see cref="ProcessSalesAdjustmentResponse"/> instance.</returns>
		/// <seealso cref="ProcessAuthorisationAsync(ProcessAuthorisationRequest)"/>
		/// <seealso cref="ProcessSalesAdjustmentReversalAsync(ProcessSalesAdjustmentReversalRequest)"/>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the response signature cannot be validated.</exception>
		/// <exception cref="System.ObjectDisposedException">Thrown if this instance has been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if this instance has not been initialised with a non-null device key (<see cref="SetDeviceKey(string)"/> and <see cref="HummClientConfiguration.DeviceKey"/>)..</exception>
		/// <exception cref="System.Net.Http.HttpRequestException">Thrown if an HTTP protocol level or <see cref="System.Net.Http.HttpClient"/> pipeline error occurs.</exception>
		public Task<ProcessSalesAdjustmentResponse> ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest request)
		{
			GuardClientState();

			return SendRequest<ProcessSalesAdjustmentRequest, ProcessSalesAdjustmentResponse>("ProcessSalesAdjustment", request);
		}

		/// <summary>
		/// This endpoint is used to process a reversal of a <see cref="ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest)"/> at the point-of-sale
		/// </summary>
		/// <remarks>
		/// <para>This is typically used to undo a refund in the case where it was made in error, or where the outcome was unknown (due to a network timeout/error) 
		/// and an idempotent retry is not desirable. See the Humm documentation at; https://docs.shophumm.com.au/pos/api_information/retries_and_idempotency/ </para>
		/// </remarks>
		/// <param name="request">A <see cref="ProcessSalesAdjustmentReversalRequest"/> instance.</param>
		/// <returns>A <see cref="ProcessSalesAdjustmentReversalResponse"/> instance.</returns>
		/// <seealso cref="ProcessSalesAdjustmentAsync(ProcessSalesAdjustmentRequest)"/>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
		/// <exception cref="HummResponseSignatureException">Thrown if the response signature cannot be validated.</exception>
		/// <exception cref="System.ObjectDisposedException">Thrown if this instance has been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">Thrown if this instance has not been initialised with a non-null device key (<see cref="SetDeviceKey(string)"/> and <see cref="HummClientConfiguration.DeviceKey"/>)..</exception>
		/// <exception cref="System.Net.Http.HttpRequestException">Thrown if an HTTP protocol level or <see cref="System.Net.Http.HttpClient"/> pipeline error occurs.</exception>
		public Task<ProcessSalesAdjustmentReversalResponse> ProcessSalesAdjustmentReversalAsync(ProcessSalesAdjustmentReversalRequest request)
		{
			GuardClientState();

			return SendRequest<ProcessSalesAdjustmentReversalRequest, ProcessSalesAdjustmentReversalResponse>("ProcessSalesAdjustmentReversal", request);
		}

		private void OnPendingAuthorisation(ProcessAuthorisationResponse response, ProcessAuthorisationRequest request)
		{
			PendingAuthorisation?.Invoke(this, new PendingAuthorisationEventArgs(request.ClientTransactionReference, response.RetryDuration, response.TrackingData));
		}

		private void CreateHttpClient(HummClientConfiguration config)
		{
			System.Net.Http.HttpClientHandler? handler = null;
			try
			{
				handler = new System.Net.Http.HttpClientHandler();
				if (handler.SupportsAutomaticDecompression)
					handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

				_Client = new System.Net.Http.HttpClient(handler);
				if (String.IsNullOrEmpty(config.UserAgentProductName) || String.IsNullOrEmpty(config.UserAgentProductVersion))
					_Client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Yort.Humm.Instore", typeof(HummClient).Assembly.GetName().Version.ToString()));
				else
				{
					_Client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(config.UserAgentProductName, config.UserAgentProductVersion));
					_Client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("(Yort.Humm.Instore/" + typeof(HummClient).Assembly.GetName().Version.ToString() + ")"));
				}
			}
			catch
			{
				handler?.Dispose();
				_Client?.Dispose();

				throw;
			}
		}

		private Task<TResponse> SendRequest<TRequest, TResponse>(string relativePath, TRequest request) where TRequest : RequestBase where TResponse : ResponseBase
		{
			return SendRequest<TRequest, TResponse>(relativePath, request, _RequestWriter, _SignatureGenerator);
		}

		private async Task<TResponse> SendRequest<TRequest, TResponse>(string relativePath, TRequest request, [ValidatedNotNull] SignedRequestWriter? requestWriter, [ValidatedNotNull] ISignatureGenerator? signatureGenerator) where TRequest : RequestBase where TResponse : ResponseBase
		{
			request.GuardNull(nameof(request));
			requestWriter.GuardNull(nameof(requestWriter));
			signatureGenerator.GuardNull(nameof(signatureGenerator));

			request.FillFromConfig(_Config);

			if (_Config.AutoValidateRequests)
				request.Validate();

			try
			{
#pragma warning disable CS8604 // Possible null reference argument.
				using (var content = GetRequestContent(request, requestWriter))
#pragma warning restore CS8604 // Possible null reference argument.
				using (var response = await _Client.PostAsync(new Uri(_BaseUrl, relativePath), content).ConfigureAwait(false))
				{
					await ThrowIfErrorStatus(response).ConfigureAwait(false);

					var retVal = await DeserialiseContentFromJson<TResponse>(response.Content).ConfigureAwait(false);
#pragma warning disable CS8604 // Possible null reference argument.
					retVal.VerifySignature(signatureGenerator);
#pragma warning restore CS8604 // Possible null reference argument.

					return retVal;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
				throw;
			}
		}

		private async Task<TResponse> DeserialiseContentFromJson<TResponse>(HttpContent content) where TResponse : ResponseBase
		{
			using (var stream = await content.ReadAsStreamAsync().ConfigureAwait(false))
			{
				using (var sr = new StreamReader(stream))
				{
					using (var reader = new Newtonsoft.Json.JsonTextReader(sr))
					{
						reader.ArrayPool = JsonArrayPool.Instance;

						return _Serialiser.Deserialize<TResponse>(reader);
					}
				}
			}
		}

		private static async Task ThrowIfErrorStatus(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				var exception = new HttpRequestException(response.ReasonPhrase);
				exception.Data["HttpStatusCode"] = response.StatusCode;
				if (response.Content != null)
				{
					exception.Data["ResponseContent"] = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				}
				throw exception;
			}
		}

		private void GuardClientState()
		{
			if (_IsDisposed) throw new ObjectDisposedException(nameof(HummClient));
			if (_RequestWriter == null) throw new InvalidOperationException(ErrorMessages.InvalidConfig_NoDeviceKey);
		}

		private System.Net.Http.StreamContent GetRequestContent<T>(T request, SignedRequestWriter writer) where T : class
		{
			System.IO.Stream? stream = null;
			try
			{
				stream = MemoryStreamFactory.CreateStream();
				writer.WriteRequest(request, stream);
				stream.Seek(0, System.IO.SeekOrigin.Begin);

				var retVal = new System.Net.Http.StreamContent(stream);
				retVal.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
				{
					CharSet = System.Text.UTF8Encoding.UTF8.HeaderName
				};

				return retVal;
			}
			catch
			{
				stream?.Dispose();

				throw;
			}
		}

		private void ConfigureServicePoint()
		{
			try
			{
				var servicePoint = System.Net.ServicePointManager.FindServicePoint(_BaseUrl);
				servicePoint.Expect100Continue = false; //Improves performance by reducing round trips
				servicePoint.UseNagleAlgorithm = true; //Improves latency/performance
			}
			//Ignore any exceptions that might be thrown from poorly/partially implemented Net Standard 2.0.
			catch (PlatformNotSupportedException) { }
			catch (NotImplementedException) { }
			catch (NotSupportedException) { }

			try
			{
				//TLS 1.2 is required.
				//.Net 4.0 doesn't contain the TLS enum value, but converting the expected numeric value (3072)
				//to the enum type works, so long as either a later .Net version is installed, or the machine 
				//has had registry edits & patches to enable that protocol. Either way, we need to ensure TLS 1.2 is turned on
				//in System.Net.ServicePointManager.SecurityProtocol.
				var tls12 = (System.Net.SecurityProtocolType)3072;
				if ((System.Net.ServicePointManager.SecurityProtocol & tls12) != tls12)
				{
					System.Net.ServicePointManager.SecurityProtocol = (System.Net.ServicePointManager.SecurityProtocol | tls12);
				}
			}
			//Ignore any exceptions that might be thrown from poorly/partially implemented Net Standard 2.0.
			catch (PlatformNotSupportedException) { }
			catch (NotImplementedException) { }
			catch (NotSupportedException) { }
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <remarks>
		/// <para>Once disposed, calls to most methods on this object with throw <see cref="ObjectDisposedException"/>.</para>
		/// <para>This method is safe to call multiple times in series. It is not guarateed to call in parallel, though no known issues exist.</para>
		/// </remarks>
		public void Dispose()
		{
			if (_IsDisposed) return;
		
			_IsDisposed = true;
			_Client?.Dispose();
			_RequestWriter?.Dispose();
			_SignatureGenerator?.Dispose();
		}
	}
}
