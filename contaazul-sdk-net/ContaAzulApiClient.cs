using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Apis;
using ContaAzul.Sdk.Net.Http;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Client for interacting with the ContaAzul API, providing authentication and access to various API endpoints.
    /// </summary>
    public class ContaAzulApiClient : HttpClientBase
    {
        private const string AuthBaseUrl = "https://auth.contaazul.com";
        private const string ApiBaseUrl = "https://api-v2.contaazul.com";

        /// <summary>Access token lifetime issued by ContaAzul: 3600 seconds (1 hour).</summary>
        public const int AccessTokenLifetimeSeconds = 3600;

        /// <summary>Refresh token lifetime issued by ContaAzul: 5 years, rotated on every use.</summary>
        public const int RefreshTokenLifetimeDays = 1825;

        // Proactive buffer: refresh 5 minutes before expiry (~8% of the 3600s lifetime).
        private const int TokenExpirationBufferSeconds = 300;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly SemaphoreSlim _refreshLock;
        private readonly HttpClient _authHttpClient;
        private readonly bool _disposeAuthClient;
        private string _accessToken;
        private string _refreshToken;
        private DateTime _tokenExpiresAt;

        public string AccessToken => _accessToken;
        public string RefreshToken => _refreshToken;

        /// <summary>
        /// Gets the UTC date and time when the current access token expires.
        /// Returns <see cref="DateTime.MinValue"/> if no expiration has been set.
        /// </summary>
        public DateTime TokenExpiresAt => _tokenExpiresAt;

        public PessoasApi Pessoas { get; }
        public VendasApi Vendas { get; }
        public NotasFiscaisApi NotasFiscais { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContaAzulApiClient"/> class with stored tokens.
        /// Use this constructor when restoring a previously authenticated session from storage.
        /// The client will automatically use the provided tokens and refresh them when necessary.
        /// <para>
        /// <b>Token rotation:</b> ContaAzul rotates the refresh token on every renewal.
        /// Always persist the updated <see cref="AccessToken"/>, <see cref="RefreshToken"/> and
        /// <see cref="TokenExpiresAt"/> after each API call cycle.
        /// </para>
        /// </summary>
        /// <param name="clientId">The client ID for OAuth authentication.</param>
        /// <param name="clientSecret">The client secret for OAuth authentication.</param>
        /// <param name="accessToken">The previously stored access token.</param>
        /// <param name="refreshToken">The previously stored refresh token. Valid for 5 years or until next renewal.</param>
        /// <param name="baseUrl">The base URL for the API. Defaults to the production API URL.</param>
        /// <param name="httpClient">Optional custom HttpClient instance. If not provided, a new instance will be created.</param>
        /// <param name="authHttpClient">
        /// Optional. Custom <see cref="HttpClient"/> for authentication requests. When provided, the caller is
        /// responsible for its lifetime. When omitted, a dedicated instance is created and disposed with this client.
        /// </param>
        public ContaAzulApiClient(string clientId, string clientSecret, string accessToken, string refreshToken, string baseUrl = ApiBaseUrl, HttpClient httpClient = null, DateTime tokenExpiresAt = default, HttpClient authHttpClient = null) 
            : base(baseUrl, httpClient)
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
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _tokenExpiresAt = tokenExpiresAt;
            _refreshLock = new SemaphoreSlim(1, 1);

            if (authHttpClient == null)
            {
                _authHttpClient = new HttpClient();
                _authHttpClient.BaseAddress = new Uri(AuthBaseUrl);
                _disposeAuthClient = true;
            }
            else
            {
                _authHttpClient = authHttpClient;
                _disposeAuthClient = false;
            }

            var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            _authHttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                SetAuthorizationHeader(_accessToken);
            }

            Pessoas = new PessoasApi(this);
            Vendas = new VendasApi(this);
            NotasFiscais = new NotasFiscaisApi(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContaAzulApiClient"/> class without tokens.
        /// Use this constructor for the first time authentication flow.
        /// After authorization, store the AccessToken and RefreshToken for future use.
        /// </summary>
        /// <param name="clientId">The client ID for OAuth authentication.</param>
        /// <param name="clientSecret">The client secret for OAuth authentication.</param>
        /// <param name="baseUrl">The base URL for the API. Defaults to the production API URL.</param>
        /// <param name="httpClient">Optional custom HttpClient instance. If not provided, a new instance will be created.</param>
        public ContaAzulApiClient(string clientId, string clientSecret, string baseUrl = ApiBaseUrl, HttpClient httpClient = null) 
            : this(clientId, clientSecret, null, null, baseUrl, httpClient)
        {
        }
       
       /// <summary>
       /// Builds the ContaAzul OAuth2 authorization URL with the specified parameters.
       /// </summary>
       /// <param name="clientId">The client ID for OAuth authentication.</param>
       /// <param name="redirectUri">The redirect URI to be used in the OAuth flow.</param>
       /// <param name="state">A unique state string to prevent CSRF attacks.</param>
       /// <param name="scope">The scope(s) to request, separated by spaces (e.g., "openid profile aws.cognito.signin.user.admin").</param>
       /// <returns>The formatted authorization URL.</returns>
       public static string BuildAuthorizationUrl(string clientId, string redirectUri, string state, string scope)
       {
           if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
           if (string.IsNullOrWhiteSpace(redirectUri)) throw new ArgumentNullException(nameof(redirectUri));
           if (string.IsNullOrWhiteSpace(state)) throw new ArgumentNullException(nameof(state));
           if (string.IsNullOrWhiteSpace(scope)) throw new ArgumentNullException(nameof(scope));

           // Validate redirectUri is a valid absolute URL
           if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uriResult))
           {
               throw new ArgumentException("redirectUri must be a valid absolute URL.", nameof(redirectUri));
           }
           var query = $"response_type=code&client_id={Uri.EscapeDataString(clientId)}&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={Uri.EscapeDataString(state)}&scope={Uri.EscapeDataString(scope.Replace(" ", "+"))}";
           return $"{AuthBaseUrl}/oauth2/authorize?{query}";
       }

        /// <summary>
        /// Authorizes the client using the provided authorization code and redirect URI.
        /// This method exchanges the authorization code for access and refresh tokens.
        /// </summary>
        /// <param name="code">The authorization code received from the OAuth flow.</param>
        /// <param name="redirectUri">The redirect URI used in the OAuth flow.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TokenResponse"/> containing the access and refresh tokens.</returns>
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

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
            });

            return await PostTokenEndpointAsync(formData, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Refreshes the access token using the current refresh token.
        /// ContaAzul issues a new access token valid for <see cref="AccessTokenLifetimeSeconds"/> seconds.
        /// <para>
        /// <b>Token rotation:</b> each call invalidates the current refresh token and returns a new one.
        /// Always persist <see cref="AccessToken"/>, <see cref="RefreshToken"/> and <see cref="TokenExpiresAt"/>
        /// after this method completes.
        /// </para>
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TokenResponse"/> containing the new access and refresh tokens.</returns>
        public async Task<TokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_refreshToken))
            {
                throw new InvalidOperationException("Refresh token is not available. Please authorize first.");
            }

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _refreshToken)
            });

            return await PostTokenEndpointAsync(formData, cancellationToken).ConfigureAwait(false);
        }

        private async Task<TokenResponse> PostTokenEndpointAsync(FormUrlEncodedContent formData, CancellationToken cancellationToken)
        {
            using (formData)
            {
                var response = await _authHttpClient.PostAsync("/oauth2/token", formData, cancellationToken).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Content: {content}");
                }

                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(content);

                UpdateTokens(tokenResponse);

                return tokenResponse;
            }
        }

        /// <summary>
        /// Sets the access token to be used for API requests and updates the authorization header.
        /// </summary>
        /// <param name="accessToken">The access token to set.</param>
        /// <param name="expiresIn">
        /// Optional. Token lifetime in seconds as returned by the authorization server
        /// (ContaAzul issues <see cref="AccessTokenLifetimeSeconds"/> seconds).
        /// When provided, enables proactive token refresh before expiry.
        /// </param>
        public void SetAccessToken(string accessToken, int expiresIn = 0)
        {
            _accessToken = accessToken;

            if (expiresIn > 0)
            {
                _tokenExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn);
            }

            SetAuthorizationHeader(_accessToken);
        }

        /// <summary>
        /// Sets the refresh token to be used for obtaining new access tokens.
        /// </summary>
        /// <param name="refreshToken">The refresh token to set.</param>
        public void SetRefreshToken(string refreshToken)
        {
            _refreshToken = refreshToken;
        }

        /// <summary>
        /// Sends a GET request to the specified API endpoint and deserializes the response to the specified type.
        /// Automatically retries the request if the access token is expired and can be refreshed.
        /// </summary>
        /// <typeparam name="TResponse">The type to which the response will be deserialized.</typeparam>
        /// <param name="endpoint">The API endpoint to send the GET request to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The deserialized response from the API.</returns>
        public new async Task<TResponse> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.GetAsync<TResponse>(endpoint, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a POST request to the specified API endpoint with the provided data and deserializes the response to the specified type.
        /// Automatically retries the request if the access token is expired and can be refreshed.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request data to be sent.</typeparam>
        /// <typeparam name="TResponse">The type to which the response will be deserialized.</typeparam>
        /// <param name="endpoint">The API endpoint to send the POST request to.</param>
        /// <param name="data">The data to send in the POST request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The deserialized response from the API.</returns>
        public new async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.PostAsync<TRequest, TResponse>(endpoint, data, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a PUT request to the specified API endpoint with the provided data and deserializes the response to the specified type.
        /// Automatically retries the request if the access token is expired and can be refreshed.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request data to be sent.</typeparam>
        /// <typeparam name="TResponse">The type to which the response will be deserialized.</typeparam>
        /// <param name="endpoint">The API endpoint to send the PUT request to.</param>
        /// <param name="data">The data to send in the PUT request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The deserialized response from the API.</returns>
        public new async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.PutAsync<TRequest, TResponse>(endpoint, data, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a DELETE request to the specified API endpoint and deserializes the response to the specified type.
        /// Automatically retries the request if the access token is expired and can be refreshed.
        /// </summary>
        /// <typeparam name="TResponse">The type to which the response will be deserialized.</typeparam>
        /// <param name="endpoint">The API endpoint to send the DELETE request to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The deserialized response from the API.</returns>
        public new async Task<TResponse> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithRetryAsync(
                async () => await base.DeleteAsync<TResponse>(endpoint, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a DELETE request to the specified API endpoint.
        /// Automatically retries the request if the access token is expired and can be refreshed.
        /// </summary>
        /// <param name="endpoint">The API endpoint to send the DELETE request to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
            await EnsureValidTokenAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (HttpRequestException ex) when (IsUnauthorizedError(ex) && CanRefreshToken())
            {
                await _refreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
                try
                {
                    await RefreshTokenAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    _refreshLock.Release();
                }
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

        /// <summary>
        /// Returns true when the access token is expired or will expire within the buffer window.
        /// Returns false if no expiration has been recorded (e.g. token was set via SetAccessToken without expiresIn).
        /// </summary>
        public bool IsTokenExpired()
        {
            if (_tokenExpiresAt == DateTime.MinValue)
            {
                return false;
            }
            return DateTime.UtcNow >= _tokenExpiresAt.AddSeconds(-TokenExpirationBufferSeconds);
        }

        private void UpdateTokens(TokenResponse tokenResponse)
        {
            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken;

            if (tokenResponse.ExpiresIn > 0)
            {
                _tokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
            }

            SetAuthorizationHeader(_accessToken);
        }

        private async Task EnsureValidTokenAsync(CancellationToken cancellationToken)
        {
            if (!IsTokenExpired() || !CanRefreshToken())
            {
                return;
            }

            await _refreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                // Re-check after acquiring the lock to avoid double refresh
                if (IsTokenExpired() && CanRefreshToken())
                {
                    await RefreshTokenAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        public new void Dispose()
        {
            _refreshLock?.Dispose();

            if (_disposeAuthClient)
            {
                _authHttpClient?.Dispose();
            }

            base.Dispose();
        }
    }
}
