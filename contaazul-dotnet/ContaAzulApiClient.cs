using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using contaazul_dotnet.Http;
using contaazul_dotnet.Models;

namespace contaazul_dotnet
{
    public class ContaAzulApiClient : HttpClientBase
    {
        private const string AuthBaseUrl = "https://auth.contaazul.com";
        private const string ApiBaseUrl = "https://api-v2.contaazul.com";

        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _accessToken;
        private string _refreshToken;

        public string AccessToken => _accessToken;
        public string RefreshToken => _refreshToken;

        public PessoaApi Pessoas { get; }
        public VendaApi Vendas { get; }

        public ContaAzulApiClient(string clientId, string clientSecret, string baseUrl = ApiBaseUrl, HttpClient httpClient = null) : base(baseUrl, httpClient)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            _clientId = clientId;
            _clientSecret = clientSecret;

            Pessoas = new PessoaApi(this);
            Vendas = new VendaApi(this);
        }

        public async Task<TokenResponse> AuthorizeAsync(string code, string redirectUri, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(AuthBaseUrl);
                
                var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri)
                });

                var response = await httpClient.PostAsync("/oauth2/token", formData, cancellationToken).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Content: {content}");
                }

                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(content);

                _accessToken = tokenResponse.AccessToken;
                _refreshToken = tokenResponse.RefreshToken;

                SetAuthorizationHeader(_accessToken);

                return tokenResponse;
            }
        }

        public async Task<TokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_refreshToken))
            {
                throw new InvalidOperationException("Refresh token is not available. Please authorize first.");
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(AuthBaseUrl);
                
                var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", _refreshToken)
                });

                var response = await httpClient.PostAsync("/oauth2/token", formData, cancellationToken).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Content: {content}");
                }

                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(content);

                _accessToken = tokenResponse.AccessToken;
                _refreshToken = tokenResponse.RefreshToken;

                SetAuthorizationHeader(_accessToken);

                return tokenResponse;
            }
        }

        public void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            SetAuthorizationHeader(_accessToken);
        }

        public void SetRefreshToken(string refreshToken)
        {
            _refreshToken = refreshToken;
        }

        public new async Task<TResponse> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.GetAsync<TResponse>(endpoint, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public new async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.PostAsync<TRequest, TResponse>(endpoint, data, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public new async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.PutAsync<TRequest, TResponse>(endpoint, data, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public new async Task<TResponse> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.DeleteAsync<TResponse>(endpoint, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        public new async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            await ExecuteWithRetryAsync(
                async () =>
                {
                    await base.DeleteAsync(endpoint, cancellationToken).ConfigureAwait(false);
                    return true;
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        private async Task<TResponse> ExecuteWithRetryAsync<TResponse>(Func<Task<TResponse>> operation, CancellationToken cancellationToken)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (HttpRequestException ex) when (IsUnauthorizedError(ex) && CanRefreshToken())
            {
                await RefreshTokenAsync(cancellationToken).ConfigureAwait(false);
                return await operation().ConfigureAwait(false);
            }
        }

        private bool IsUnauthorizedError(HttpRequestException ex)
        {
            return ex.Message.Contains("Unauthorized") || 
                   ex.Message.Contains("401");
        }

        private bool CanRefreshToken()
        {
            return !string.IsNullOrWhiteSpace(_refreshToken);
        }
    }
}
