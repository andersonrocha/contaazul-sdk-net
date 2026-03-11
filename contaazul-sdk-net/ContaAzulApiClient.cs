using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Apis;
using ContaAzul.Sdk.Net.Exceptions;
using ContaAzul.Sdk.Net.Http;
using ContaAzul.Sdk.Net.Models;
using Microsoft.Extensions.Logging;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Client for interacting with the ContaAzul API, providing authentication and access to various API endpoints.
    /// </summary>
    public class ContaAzulApiClient : HttpClientBase, IContaAzulApiClient
    {
        private const string AuthBaseUrl = "https://auth.contaazul.com";
        private const string ApiBaseUrl = "https://api-v2.contaazul.com";

        /// <summary>Access token lifetime issued by ContaAzul: 3600 seconds (1 hour).</summary>
        public const int AccessTokenLifetimeSeconds = 3600;

        // Proactive buffer: refresh 5 minutes before expiry (~8% of the 3600s lifetime).
        private const int TokenExpirationBufferSeconds = 300;

        private readonly SemaphoreSlim _refreshLock;
        private readonly SemaphoreSlim _rateLimiterLock;
        private readonly Queue<DateTime> _requestTimestamps;
        private readonly HttpClient _authHttpClient;
        private readonly bool _disposeAuthClient;
        private readonly ILogger _logger;
        private volatile string _accessToken;
        private volatile string _refreshToken;
        private DateTime _tokenExpiresAt;
        private volatile bool _disposed;

        public string AccessToken => _accessToken;
        public string RefreshToken => _refreshToken;

        /// <summary>
        /// Gets the UTC date and time when the current access token expires.
        /// Returns <see cref="DateTime.MinValue"/> if no expiration has been set.
        /// </summary>
        public DateTime TokenExpiresAt => _tokenExpiresAt;

        /// <summary>
        /// Controls how many times transient API failures are retried and how long the client waits between attempts.
        /// Defaults to 3 retries with binary exponential backoff starting at 1 second.
        /// Set to <see cref="ContaAzul.Sdk.Net.RetryOptions.None"/> to disable retries.
        /// </summary>
        public RetryOptions RetryOptions { get; set; } = new RetryOptions();

        /// <summary>
        /// Controls the maximum number of API requests dispatched per second (sliding-window algorithm).
        /// Defaults to 10 requests per second.
        /// Set to <see cref="ContaAzul.Sdk.Net.RateLimitOptions.None"/> to disable rate limiting.
        /// </summary>
        public RateLimitOptions RateLimitOptions { get; set; } = new RateLimitOptions();

        /// <summary>
        /// Raised after tokens are successfully updated — either via <see cref="AuthorizeAsync"/>
        /// or an automatic/manual <see cref="RefreshTokenAsync"/>.
        /// <para>
        /// Subscribe to this event to persist the new <see cref="TokenRefreshedEventArgs.AccessToken"/>,
        /// <see cref="TokenRefreshedEventArgs.RefreshToken"/> and
        /// <see cref="TokenRefreshedEventArgs.TokenExpiresAt"/> to storage.
        /// </para>
        /// </summary>
        public event EventHandler<TokenRefreshedEventArgs> TokenRefreshed;

        /// <summary>Raises the <see cref="TokenRefreshed"/> event.</summary>
        protected virtual void OnTokenRefreshed(TokenRefreshedEventArgs args)
        {
            TokenRefreshed?.Invoke(this, args);
        }

        public PessoasApi Pessoas { get; }
        public VendasApi Vendas { get; }
        public NotasFiscaisApi NotasFiscais { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContaAzulApiClient"/> class with stored tokens.
        /// Use this constructor when restoring a previously authenticated session from storage.
        /// The client will automatically use the provided tokens and refresh them when necessary.
        /// <para>
        /// <b>Token rotation:</b> ContaAzul rotates the refresh token on every renewal.
        /// Subscribe to <see cref="TokenRefreshed"/> to persist the updated
        /// <see cref="AccessToken"/>, <see cref="RefreshToken"/> and <see cref="TokenExpiresAt"/>
        /// automatically whenever tokens change.
        /// </para>
        /// </summary>
        /// <param name="clientId">The client ID for OAuth authentication.</param>
        /// <param name="clientSecret">The client secret for OAuth authentication.</param>
        /// <param name="accessToken">The previously stored access token.</param>
        /// <param name="refreshToken">The previously stored refresh token. Valid for 5 years or until next renewal.</param>
        /// <param name="options">
        /// Optional. Configuration options including base URL, HTTP clients, logger and timeout settings.
        /// When omitted, defaults are applied.
        /// </param>
        public ContaAzulApiClient(string clientId, string clientSecret, string accessToken, string refreshToken, ContaAzulApiClientOptions options = null) 
            : base(options?.BaseUrl ?? ApiBaseUrl, options?.HttpClient, options?.HttpOptions?.DefaultTimeout)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _tokenExpiresAt = options?.TokenExpiresAt ?? default;
            _refreshLock = new SemaphoreSlim(1, 1);
            _rateLimiterLock = new SemaphoreSlim(1, 1);
            _requestTimestamps = new Queue<DateTime>();
            _logger = options?.Logger;

            var authHttpClient = options?.AuthHttpClient;
            var httpOptions = options?.HttpOptions;

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

            if (httpOptions != null)
            {
                _authHttpClient.Timeout = httpOptions.DefaultTimeout;
            }

            // B2: register a per-request token provider; the lambda reads _accessToken at dispatch
            // time, avoiding any concurrent mutation of DefaultRequestHeaders.
            SetTokenProvider(() => _accessToken);

            RetryOptions = options?.RetryOptions ?? new RetryOptions();
            RateLimitOptions = options?.RateLimitOptions ?? new RateLimitOptions();

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
        /// <param name="options">
        /// Optional. Configuration options including base URL, HTTP clients, logger and timeout settings.
        /// When omitted, defaults are applied.
        /// </param>
        public ContaAzulApiClient(string clientId, string clientSecret, ContaAzulApiClientOptions options = null) 
            : this(clientId, clientSecret, null, null, options)
        {
        }
       
       /// <summary>
       /// Builds the ContaAzul OAuth2 authorization URL with the specified parameters.
       /// </summary>
       /// <remarks>
       /// This method delegates to <see cref="ContaAzulOAuthHelper.BuildAuthorizationUrl"/>.
       /// Code programmed against <see cref="IContaAzulApiClient"/> should call
       /// <see cref="ContaAzulOAuthHelper.BuildAuthorizationUrl"/> directly, as static
       /// members are not accessible through an interface reference.
       /// </remarks>
       /// <param name="clientId">The client ID for OAuth authentication.</param>
       /// <param name="redirectUri">The redirect URI to be used in the OAuth flow.</param>
       /// <param name="state">A unique state string to prevent CSRF attacks.</param>
       /// <param name="scope">The scope(s) to request, separated by spaces (e.g., "openid profile aws.cognito.signin.user.admin").</param>
       /// <returns>The formatted authorization URL.</returns>
       public static string BuildAuthorizationUrl(string clientId, string redirectUri, string state, string scope)
       {
           return ContaAzulOAuthHelper.BuildAuthorizationUrl(clientId, redirectUri, state, scope);
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
            ThrowIfDisposed();
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

            _logger?.LogDebug("Requesting access token via authorization code grant.");

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
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(_refreshToken))
            {
                throw new InvalidOperationException("Refresh token is not available. Please authorize first.");
            }

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _refreshToken)
            });

            _logger?.LogDebug("Refreshing access token using refresh token grant.");

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
                    _logger?.LogError("Token endpoint request failed with HTTP {StatusCode}.", (int)response.StatusCode);
                    ThrowApiException(response.StatusCode, content);
                }

                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, DefaultJsonOptions);

                if (tokenResponse == null)
                {
                    throw new ContaAzulApiException(
                        System.Net.HttpStatusCode.OK,
                        "The token endpoint returned an empty or unrecognizable response.");
                }

                _logger?.LogInformation("Token obtained successfully. Expires in {ExpiresIn}s.", tokenResponse.ExpiresIn);

                UpdateTokens(tokenResponse);

                return tokenResponse;
            }
        }

        /// <summary>
        /// Sets the access token used for subsequent API requests.
        /// The new value is picked up automatically by the per-request authorization header
        /// injected on each outgoing call — no manual header mutation required.
        /// </summary>
        /// <param name="accessToken">The access token to set.</param>
        /// <param name="expiresIn">
        /// Optional. Token lifetime in seconds as returned by the authorization server
        /// (ContaAzul issues <see cref="AccessTokenLifetimeSeconds"/> seconds).
        /// When provided, enables proactive token refresh before expiry.
        /// </param>
        public void SetAccessToken(string accessToken, int expiresIn = 0)
        {
            ThrowIfDisposed();
            _accessToken = accessToken;

            if (expiresIn > 0)
            {
                _tokenExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn);
            }
        }

        /// <summary>
        /// Sets the refresh token to be used for obtaining new access tokens.
        /// </summary>
        /// <param name="refreshToken">The refresh token to set.</param>
        public void SetRefreshToken(string refreshToken)
        {
            ThrowIfDisposed();
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
        public async Task<TResponse> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await ExecuteWithRetryAsync(
                async () => await CoreGetAsync<TResponse>(endpoint, cancellationToken).ConfigureAwait(false),
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
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await ExecuteWithRetryAsync(
                async () => await CorePostAsync<TRequest, TResponse>(endpoint, data, cancellationToken).ConfigureAwait(false),
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
        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await ExecuteWithRetryAsync(
                async () => await CorePutAsync<TRequest, TResponse>(endpoint, data, cancellationToken).ConfigureAwait(false),
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
        public async Task<TResponse> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await ExecuteWithRetryAsync(
                async () => await CoreDeleteAsync<TResponse>(endpoint, cancellationToken).ConfigureAwait(false),
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
        public async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            await ExecuteWithRetryAsync(
                async () =>
                {
                    await CoreDeleteAsync(endpoint, cancellationToken).ConfigureAwait(false);
                    return true;
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        private async Task<TResponse> ExecuteWithRetryAsync<TResponse>(Func<Task<TResponse>> operation, CancellationToken cancellationToken)
        {
            await EnsureValidTokenAsync(cancellationToken).ConfigureAwait(false);

            // Wrap every outgoing HTTP call with the rate limiter so that both
            // the initial attempt and any post-refresh retry are throttled.
            Func<Task<TResponse>> rateLimitedOperation = async () =>
            {
                await EnforceRateLimitAsync(cancellationToken).ConfigureAwait(false);
                return await operation().ConfigureAwait(false);
            };

            for (var attempt = 0; ; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    // Inner try handles the 401 ? token-refresh ? single retry path.
                    try
                    {
                        return await rateLimitedOperation().ConfigureAwait(false);
                    }
                    catch (ContaAzulAuthenticationException) when (CanRefreshToken())
                    {
                        _logger?.LogWarning("Received 401 Unauthorized. Refreshing access token and retrying.");
                        await _refreshLock.WaitAsync(cancellationToken).ConfigureAwait(false);
                        try
                        {
                            await RefreshTokenAsync(cancellationToken).ConfigureAwait(false);
                        }
                        finally
                        {
                            _refreshLock.Release();
                        }
                        return await rateLimitedOperation().ConfigureAwait(false);
                    }
                }
                catch (Exception ex) when (IsTransientError(ex) && attempt < RetryOptions.MaxRetries)
                {
                    // Outer catch handles transient errors with exponential backoff.
                    var delay = CalculateDelay(attempt);
                    _logger?.LogWarning(ex, "Transient error on attempt {Attempt} of {MaxRetries}. Retrying in {DelayMs}ms.", attempt + 1, RetryOptions.MaxRetries, (int)delay.TotalMilliseconds);
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
            }
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

            OnTokenRefreshed(new TokenRefreshedEventArgs(tokenResponse, _tokenExpiresAt));
        }

        private async Task EnsureValidTokenAsync(CancellationToken cancellationToken)
        {
            if (!IsTokenExpired() || !CanRefreshToken())
            {
                return;
            }

            _logger?.LogDebug("Access token is expiring soon. Initiating proactive refresh.");

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

        private static bool IsTransientError(Exception ex)
        {
            if (ex is ContaAzulRateLimitException)
            {
                return true;
            }

            if (ex is ContaAzulApiException apiEx)
            {
                var status = (int)apiEx.StatusCode.GetValueOrDefault();
                return status == 500 || status == 502 || status == 503 || status == 504;
            }

            return ex is HttpRequestException;
        }

        private TimeSpan CalculateDelay(int attempt)
        {
            var delayMs = RetryOptions.InitialDelay.TotalMilliseconds
                * Math.Pow(RetryOptions.BackoffMultiplier, attempt);

            var cappedMs = Math.Min(delayMs, RetryOptions.MaxDelay.TotalMilliseconds);
            return TimeSpan.FromMilliseconds(cappedMs);
        }

        /// <summary>
        /// Throttles the caller using a sliding-window algorithm so that at most
        /// <see cref="RateLimitOptions.RequestsPerSecond"/> API requests are dispatched per second.
        /// When the limit is reached the method awaits asynchronously until a slot becomes available.
        /// Does nothing when <see cref="RateLimitOptions.RequestsPerSecond"/> is &lt;= 0.
        /// </summary>
        private async Task EnforceRateLimitAsync(CancellationToken cancellationToken)
        {
            if (RateLimitOptions == null || RateLimitOptions.RequestsPerSecond <= 0)
            {
                return;
            }

            await _rateLimiterLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            var lockHeld = true;
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var now = DateTime.UtcNow;
                    var windowStart = now.AddSeconds(-1);

                    // Evict timestamps that have left the 1-second window.
                    while (_requestTimestamps.Count > 0 && _requestTimestamps.Peek() <= windowStart)
                    {
                        _requestTimestamps.Dequeue();
                    }

                    if (_requestTimestamps.Count < RateLimitOptions.RequestsPerSecond)
                    {
                        _requestTimestamps.Enqueue(now);
                        return;
                    }

                    // Compute how long until the oldest request leaves the window.
                    var delay = _requestTimestamps.Peek().AddSeconds(1) - DateTime.UtcNow;
                    if (delay <= TimeSpan.Zero)
                    {
                        continue;
                    }

                    // Release the lock while waiting to allow other callers to make progress.
                    _logger?.LogDebug("Rate limit reached ({RequestsPerSecond} req/s). Waiting {DelayMs}ms.", RateLimitOptions.RequestsPerSecond, (int)delay.TotalMilliseconds);
                    lockHeld = false;
                    _rateLimiterLock.Release();
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                    await _rateLimiterLock.WaitAsync(cancellationToken).ConfigureAwait(false);
                    lockHeld = true;
                }
            }
            finally
            {
                if (lockHeld)
                {
                    _rateLimiterLock.Release();
                }
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ContaAzulApiClient));
            }
        }

        /// <summary>
        /// Sends a PATCH request to the specified API endpoint with the provided data and deserializes the response.
        /// Automatically retries the request if the access token is expired and can be refreshed.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request data to be sent.</typeparam>
        /// <typeparam name="TResponse">The type to which the response will be deserialized.</typeparam>
        /// <param name="endpoint">The API endpoint to send the PATCH request to.</param>
        /// <param name="data">The data to send in the PATCH request body.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>The deserialized response from the API.</returns>
        public async Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            return await ExecuteWithRetryAsync(
                async () => await CorePatchAsync<TRequest, TResponse>(endpoint, data, cancellationToken).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                _refreshLock?.Dispose();
                _rateLimiterLock?.Dispose();

                if (_disposeAuthClient)
                {
                    _authHttpClient?.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
