using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Http
{
    public class HttpClientBase : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _disposeClient;

        public HttpClientBase(string baseUrl, HttpClient httpClient = null)
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
            response.EnsureSuccessStatusCode();
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
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Content: {content}");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return default(TResponse);
            }

            return JsonConvert.DeserializeObject<TResponse>(content);
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
