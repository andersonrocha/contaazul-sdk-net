using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Exceptions;
using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Http
{
    public class HttpClientBase : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _disposeClient;

        public HttpClientBase(string baseUrl, HttpClient httpClient = null, TimeSpan? timeout = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (httpClient == null)
            {
                _httpClient = new HttpClient();
                _disposeClient = true;
            }
            else
            {
                _httpClient = httpClient;
                _disposeClient = false;
            }

            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (timeout.HasValue)
            {
                _httpClient.Timeout = timeout.Value;
            }
        }

        protected void SetAuthorizationHeader(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected async Task<TResponse> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var content = CreateJsonContent(data);
            var response = await _httpClient.PostAsync(endpoint, content, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        protected async Task<TResponse> PostFormUrlEncodedAsync<TResponse>(string endpoint, HttpContent content, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsync(endpoint, content, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        public async Task<TResponse> PostFormAsync<TResponse>(string endpoint, HttpContent content, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsync(endpoint, content, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var content = CreateJsonContent(data);
            var response = await _httpClient.PutAsync(endpoint, content, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        protected async Task<TResponse> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync(endpoint, cancellationToken).ConfigureAwait(false);
            return await ProcessResponseAsync<TResponse>(response).ConfigureAwait(false);
        }

        protected async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync(endpoint, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ThrowApiException(response.StatusCode, content);
            }
        }

        private HttpContent CreateJsonContent<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data);
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

            return JsonConvert.DeserializeObject<TResponse>(content);
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
            if (_disposeClient)
            {
                _httpClient?.Dispose();
            }
        }
    }
}
