using System;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Controls the retry behavior of <see cref="ContaAzulApiClient"/> for transient errors.
    /// <para>
    /// Retries are applied to server errors (5xx), rate-limit responses (429) and network failures.
    /// Client errors (4xx) and authentication failures (401) are never retried by this policy.
    /// </para>
    /// </summary>
    public class RetryOptions
    {
        /// <summary>
        /// Maximum number of retry attempts after the initial try.
        /// A value of 0 disables retries entirely. Default: 3.
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Delay before the first retry attempt. Subsequent delays grow by <see cref="BackoffMultiplier"/>.
        /// Default: 1 second.
        /// </summary>
        public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Factor by which the delay is multiplied on each successive attempt.
        /// Default: 2.0 (binary exponential backoff).
        /// <para>Delay schedule with defaults: 1s → 2s → 4s</para>
        /// </summary>
        public double BackoffMultiplier { get; set; } = 2.0;

        /// <summary>
        /// Upper bound on the computed delay, regardless of how many retries have occurred.
        /// Default: 30 seconds.
        /// </summary>
        public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Returns a new <see cref="RetryOptions"/> instance with all retries disabled
        /// (<see cref="MaxRetries"/> = 0).
        /// <para>
        /// A fresh instance is returned on each access so callers can freely adjust other properties
        /// (e.g. <see cref="InitialDelay"/>) without affecting other consumers.
        /// This is intentional: <see cref="RetryOptions"/> is mutable, and a shared singleton
        /// would allow one caller's mutations to silently affect all other consumers.
        /// </para>
        /// </summary>
        public static RetryOptions None => new RetryOptions { MaxRetries = 0 };
    }
}
