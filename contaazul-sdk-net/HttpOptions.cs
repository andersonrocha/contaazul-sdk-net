using System;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Controls HTTP timeout settings for <see cref="ContaAzulApiClient"/> requests.
    /// <para>
    /// When specified, <see cref="DefaultTimeout"/> is applied to both API requests and
    /// token-endpoint calls (authorize / refresh). The timeout is applied regardless of
    /// whether the <see cref="System.Net.Http.HttpClient"/> was created internally by the
    /// SDK or provided by the caller.
    /// </para>
    /// <para>
    /// Callers that need to manage timeouts themselves should omit <see cref="HttpOptions"/>
    /// from the constructor and configure their <see cref="System.Net.Http.HttpClient"/>
    /// instances directly.
    /// </para>
    /// </summary>
    public class HttpOptions
    {
        /// <summary>
        /// Timeout applied to every API request and to token-endpoint calls.
        /// Must be a positive <see cref="TimeSpan"/> or
        /// <see cref="System.Threading.Timeout.InfiniteTimeSpan"/> to disable the timeout.
        /// Default: 30 seconds.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(30);
    }
}
