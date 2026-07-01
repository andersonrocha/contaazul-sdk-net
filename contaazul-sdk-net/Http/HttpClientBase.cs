using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Http
{
    public class HttpClientBase : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _disposeClient;

        internal HttpClientBase(string baseUrl, HttpClient httpClient = null, TimeSpan? timeout = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (httpClient == null)
            {
                // Internally-created client: we own its full lifetime and configuration.
                _httpClient = new HttpClient();
                _disposeClient = true;
                _httpClient.BaseAddress = new Uri(baseUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            else
            {
                // M1: Externally-provided client — respect the caller's existing configuration.
                // Overwriting a BaseAddress that is already set (and may have dispatched requests)
                // would throw InvalidOperationException and silently break the caller's setup.
                _httpClient = httpClient;
                _disposeClient = false;

                if (_httpClient.BaseAddress == null)
                {
                    _httpClient.BaseAddress = new Uri(baseUrl);
                }

                // Add Accept: application/json only if the caller has not already expressed a preference.
                if (_httpClient.DefaultRequestHeaders.Accept.All(h => h.MediaType != "application/json"))
                {
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
            }

            if (timeout.HasValue)
            {
                _httpClient.Timeout = timeout.Value;
            }
        }

        private Func<string> _tokenProvider;

        /// <summary>
        /// Registers a callback that returns the current Bearer token at the moment each request
        /// is dispatched. The token is injected as a per-request <c>Authorization</c> header so
        /// token updates are reflected immediately without mutating shared <c>DefaultRequestHeaders</c>,
        /// which is not thread-safe for concurrent writes.
        /// </summary>
        protected void SetTokenProvider(Func<string> tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Creates an <see cref="HttpRequestMessage"/> with the given method, endpoint, and optional
        /// content, injecting the current Bearer token as a per-request <c>Authorization</c> header.
        /// Per-request headers are local to the message and safe for concurrent use.
        /// </summary>
        private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (content != null)
            {
                request.Content = content;
            }

            var token = _tokenProvider?.Invoke();
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return request;
        }

        protected async Task<TResponse> CoreGetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Get, endpoint);
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a GET request and returns the raw response body as a byte array.
        /// Used for binary endpoints such as PDF generation.
        /// </summary>
        protected async Task<byte[]> CoreGetBytesAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Get, endpoint);
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ThrowApiException(response.StatusCode, content);
            }

            return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        protected async Task<TResponse> CorePostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Post, endpoint, CreateJsonContent(data));
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        protected async Task<TResponse> CorePutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Put, endpoint, CreateJsonContent(data));
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a PATCH request. Uses <c>new HttpMethod("PATCH")</c> for .NET Standard 2.0 compatibility
        /// (<c>HttpMethod.Patch</c> was added in .NET 5).
        /// </summary>
        protected async Task<TResponse> CorePatchAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(new HttpMethod("PATCH"), endpoint, CreateJsonContent(data));
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a PATCH request with a request body but without deserializing the response.
        /// Used for endpoints that take a payload and return <c>204 No Content</c>.
        /// </summary>
        protected async Task CorePatchAsync<TRequest>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(new HttpMethod("PATCH"), endpoint, CreateJsonContent(data));
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ThrowApiException(response.StatusCode, content);
            }
        }

        protected async Task<TResponse> CoreDeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Delete, endpoint);
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        protected async Task CoreDeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Delete, endpoint);
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ThrowApiException(response.StatusCode, content);
            }
        }

        /// <summary>
        /// Sends a DELETE request with a request body but without deserializing the response.
        /// Used for batch-delete endpoints that take a payload and return <c>204 No Content</c>.
        /// </summary>
        protected async Task CoreDeleteAsync<TRequest>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Delete, endpoint, CreateJsonContent(data));
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ThrowApiException(response.StatusCode, content);
            }
        }

        /// <summary>
        /// Sends a POST request without a request body and without deserializing the response.
        /// Used for action endpoints that take no payload and return <c>204 No Content</c>.
        /// </summary>
        protected async Task CorePostAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Post, endpoint);
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ThrowApiException(response.StatusCode, content);
            }
        }

        /// <summary>
        /// Sends a POST request with a request body but without deserializing the response.
        /// Used for endpoints that take a payload and return <c>204 No Content</c>.
        /// </summary>
        protected async Task CorePostAsync<TRequest>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Post, endpoint, CreateJsonContent(data));
            var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ThrowApiException(response.StatusCode, content);
            }
        }

        protected static readonly JsonSerializerOptions DefaultJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            // Optional fields left null on request bodies must be omitted, not serialized as
            // "field": null, so the API applies its own defaults (e.g. on contract creation).
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            // A API às vezes devolve inteiros como número decimal (ex.: 10.0) ou string;
            // estes conversores tornam a leitura tolerante para evitar JsonException.
            Converters =
            {
                new ContaAzul.Sdk.Net.Json.FlexibleInt32Converter(),
                new ContaAzul.Sdk.Net.Json.FlexibleNullableInt32Converter(),
                new ContaAzul.Sdk.Net.Json.FlexibleInt64Converter(),
                new ContaAzul.Sdk.Net.Json.FlexibleNullableInt64Converter(),
                new ContaAzul.Sdk.Net.Json.FlexibleNullableDateTimeConverter()
            }
        };

        private HttpContent CreateJsonContent<T>(T data)
        {
            var json = JsonSerializer.Serialize(data, DefaultJsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<TResponse> ProcessResponseAsync<TResponse>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                ThrowApiException(response.StatusCode, content);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return default(TResponse);
            }

            return JsonSerializer.Deserialize<TResponse>(content, DefaultJsonOptions);
        }

        /// <summary>
        /// Throws the most specific <see cref="ContaAzulException"/> subtype for the given HTTP status code.
        /// </summary>
        protected static void ThrowApiException(HttpStatusCode statusCode, string responseContent)
        {
            switch ((int)statusCode)
            {
                case 401:
                    throw new ContaAzulAuthenticationException(responseContent);
                case 429:
                    throw new ContaAzulRateLimitException(responseContent);
                default:
                    throw new ContaAzulApiException(statusCode, responseContent);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _disposeClient)
            {
                _httpClient?.Dispose();
            }
        }
    }
}
