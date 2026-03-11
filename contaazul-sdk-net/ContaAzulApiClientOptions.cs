using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Configuration options for <see cref="ContaAzulApiClient"/>.
    /// Group all optional settings here instead of passing them as individual constructor parameters.
    /// </summary>
    public class ContaAzulApiClientOptions
    {
        /// <summary>
        /// The base URL for API requests. Defaults to the ContaAzul production API.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Optional custom <see cref="HttpClient"/> for API requests. When provided, the caller
        /// is responsible for its lifetime. When omitted, a dedicated instance is created and
        /// disposed with the client.
        /// </summary>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Optional custom <see cref="HttpClient"/> for authentication requests. When provided,
        /// the caller is responsible for its lifetime. When omitted, a dedicated instance is
        /// created and disposed with the client.
        /// </summary>
        public HttpClient AuthHttpClient { get; set; }

        /// <summary>
        /// Optional logger for structured logging of token lifecycle and HTTP retry events.
        /// When omitted, logging is disabled.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Optional HTTP timeout configuration applied to API and token-endpoint requests.
        /// When omitted, the <see cref="HttpClient"/> default timeout (100 seconds) is used.
        /// </summary>
        public HttpOptions HttpOptions { get; set; }

        /// <summary>
        /// The UTC expiry date of a previously issued access token. Set this when restoring a
        /// session from storage so that proactive token refresh works correctly.
        /// </summary>
        public DateTime TokenExpiresAt { get; set; }

        /// <summary>
        /// Controls the retry behaviour for transient errors.
        /// Defaults to 3 retries with binary exponential backoff starting at 1 second.
        /// </summary>
        public RetryOptions RetryOptions { get; set; } = new RetryOptions();

        /// <summary>
        /// Controls the client-side rate limiting.
        /// Defaults to 10 requests per second (sliding-window algorithm).
        /// </summary>
        public RateLimitOptions RateLimitOptions { get; set; } = new RateLimitOptions();
    }
}
