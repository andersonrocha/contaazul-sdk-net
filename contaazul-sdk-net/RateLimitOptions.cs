using System;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Controls the client-side rate limiting behaviour of <see cref="ContaAzulApiClient"/>.
    /// <para>
    /// The limiter uses a sliding-window algorithm: at most <see cref="RequestsPerSecond"/> HTTP
    /// requests will be dispatched within any rolling 1-second window.  Callers that would exceed
    /// the limit are throttled with an async delay instead of receiving an error.
    /// </para>
    /// <para>
    /// Rate limiting applies only to API requests, not to token-endpoint calls
    /// (<c>AuthorizeAsync</c> / <c>RefreshTokenAsync</c>).
    /// </para>
    /// </summary>
    public class RateLimitOptions
    {
        /// <summary>
        /// Maximum number of API requests allowed per second (sliding window).
        /// Set to 0 or a negative value to disable rate limiting entirely.
        /// Default: 10.
        /// </summary>
        public int RequestsPerSecond { get; set; } = 10;

        /// <summary>
        /// Returns a new <see cref="RateLimitOptions"/> instance with rate limiting disabled
        /// (<see cref="RequestsPerSecond"/> = 0).
        /// <para>
        /// A fresh instance is returned on each access so callers can freely adjust other properties
        /// without affecting other consumers.
        /// This is intentional: <see cref="RateLimitOptions"/> is mutable, and a shared singleton
        /// would allow one caller's mutations to silently affect all other consumers.
        /// </para>
        /// </summary>
        public static RateLimitOptions None => new RateLimitOptions { RequestsPerSecond = 0 };
    }
}
