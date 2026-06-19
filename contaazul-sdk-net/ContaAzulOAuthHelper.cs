using System;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Provides static helpers for the ContaAzul OAuth 2.0 authorization flow.
    /// <para>
    /// Use this class when you need to build authorization URLs independently of
    /// <see cref="IContaAzulApiClient"/> — for example, in code programmed against the
    /// interface that cannot access the static members of the concrete
    /// <see cref="ContaAzulApiClient"/> class.
    /// </para>
    /// </summary>
    public static class ContaAzulOAuthHelper
    {
        private const string AuthBaseUrl = "https://auth.contaazul.com";

        /// <summary>
        /// Builds the ContaAzul OAuth 2.0 authorization URL with the specified parameters.
        /// </summary>
        /// <param name="clientId">The OAuth 2.0 client ID.</param>
        /// <param name="redirectUri">
        /// The redirect URI registered for the application. Must be a valid absolute URL.
        /// </param>
        /// <param name="state">
        /// An opaque value used to maintain state between the request and the callback.
        /// Use a cryptographically random string to prevent CSRF attacks.
        /// </param>
        /// <param name="scope">
        /// Space-separated scope(s) to request, e.g.
        /// <c>"openid profile aws.cognito.signin.user.admin"</c>.
        /// </param>
        /// <returns>The fully-formed authorization URL ready to redirect the user's browser to.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="clientId"/>, <paramref name="redirectUri"/>,
        /// <paramref name="state"/> or <paramref name="scope"/> is <see langword="null"/> or whitespace.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="redirectUri"/> is not a valid absolute URL.
        /// </exception>
        public static string BuildAuthorizationUrl(string clientId, string redirectUri, string state, string scope)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(redirectUri)) throw new ArgumentNullException(nameof(redirectUri));
            if (string.IsNullOrWhiteSpace(state)) throw new ArgumentNullException(nameof(state));
            if (string.IsNullOrWhiteSpace(scope)) throw new ArgumentNullException(nameof(scope));

            if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out _))
            {
                throw new ArgumentException("redirectUri must be a valid absolute URL.", nameof(redirectUri));
            }

            // O redirect_uri é enviado SEM percent-encode. Embora o encode (ex.: "%3A%2F%2F")
            // seja o padrão OAuth2, o servidor de autorização do ContaAzul não o interpreta
            // corretamente; ele espera o redirect_uri literal. Os caracteres ":" e "/" são
            // permitidos no componente de query (RFC 3986), então a URL continua válida.
            // (Não passe um redirect_uri que contenha "&", "#" ou query string própria.)
            // Os espaços do scope continuam codificados como %20, pois espaços não são válidos
            // em uma URL e o Cognito (auth do ContaAzul) os aceita assim.
            var query = $"response_type=code"
                      + $"&client_id={Uri.EscapeDataString(clientId)}"
                      + $"&redirect_uri={redirectUri}"
                      + $"&state={Uri.EscapeDataString(state)}"
                      + $"&scope={Uri.EscapeDataString(scope)}";

            return $"{AuthBaseUrl}/oauth2/authorize?{query}";
        }
    }
}
