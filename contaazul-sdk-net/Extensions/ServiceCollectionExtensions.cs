using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContaAzul.Sdk.Net.Extensions
{
    /// <summary>
    /// Extension methods for registering <see cref="ContaAzulApiClient"/> in the
    /// <see cref="IServiceCollection"/> dependency-injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="ContaAzulApiClient"/> as a singleton <see cref="IContaAzulApiClient"/>
        /// in the DI container.
        /// </summary>
        /// <param name="services">The service collection to add the client to.</param>
        /// <param name="clientId">The OAuth2 client ID.</param>
        /// <param name="clientSecret">The OAuth2 client secret.</param>
        /// <param name="options">
        /// Optional configuration options. When omitted, defaults are applied.
        /// The <see cref="ContaAzulApiClientOptions.Logger"/> is resolved from the container
        /// automatically when not explicitly set.
        /// </param>
        /// <returns>The original <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddContaAzulApiClient(
            this IServiceCollection services,
            string clientId,
            string clientSecret,
            ContaAzulApiClientOptions options = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            services.AddSingleton<IContaAzulApiClient>(sp =>
            {
                // D6: always build a fresh copy so the caller's options object is never mutated
                // (e.g. Logger would otherwise be silently injected into the caller's instance).
                var resolvedOptions = new ContaAzulApiClientOptions
                {
                    BaseUrl          = options?.BaseUrl,
                    HttpClient       = options?.HttpClient,
                    AuthHttpClient   = options?.AuthHttpClient,
                    HttpOptions      = options?.HttpOptions,
                    TokenExpiresAt   = options?.TokenExpiresAt ?? default,
                    RetryOptions     = options?.RetryOptions,
                    RateLimitOptions = options?.RateLimitOptions,
                    Logger           = options?.Logger ?? sp.GetService<ILogger<ContaAzulApiClient>>()
                };

                return new ContaAzulApiClient(clientId, clientSecret, resolvedOptions);
            });

            return services;
        }
    }
}
