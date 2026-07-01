using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Apis;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net
{
    /// <summary>
    /// Defines the contract for the ContaAzul API client.
    /// Implement this interface or use it as a mock target in unit tests.
    /// </summary>
    public interface IContaAzulApiClient : IDisposable
    {
        /// <summary>Gets the current access token.</summary>
        string AccessToken { get; }

        /// <summary>Gets the current refresh token.</summary>
        string RefreshToken { get; }

        /// <summary>
        /// Gets the UTC date and time when the current access token expires.
        /// Returns <see cref="DateTime.MinValue"/> if no expiration has been set.
        /// </summary>
        DateTime TokenExpiresAt { get; }

        /// <summary>
        /// Controls how many times transient API failures are retried and how long the client waits between attempts.
        /// </summary>
        RetryOptions RetryOptions { get; set; }

        /// <summary>
        /// Controls the maximum number of API requests dispatched per second (sliding-window algorithm).
        /// </summary>
        RateLimitOptions RateLimitOptions { get; set; }

        /// <summary>Gets the Pessoas API.</summary>
        PessoasApi Pessoas { get; }

        /// <summary>Gets the Vendas API.</summary>
        VendasApi Vendas { get; }

        /// <summary>Gets the Notas Fiscais API.</summary>
        NotasFiscaisApi NotasFiscais { get; }

        /// <summary>Gets the Contratos (scheduled/recurring sales) API.</summary>
        ContratosApi Contratos { get; }

        /// <summary>Gets the Cobranças (charge) API.</summary>
        CobrancasApi Cobrancas { get; }

        /// <summary>Gets the Baixas (acquittance) API.</summary>
        BaixasApi Baixas { get; }

        /// <summary>Gets the Financeiro (financial) API.</summary>
        FinanceiroApi Financeiro { get; }

        /// <summary>Gets the Produtos (products/inventory) API.</summary>
        ProdutosApi Produtos { get; }

        /// <summary>Gets the Serviços (services) API.</summary>
        ServicosApi Servicos { get; }

        /// <summary>Gets the Protocolos (protocol tracking) API.</summary>
        ProtocolosApi Protocolos { get; }

        /// <summary>Gets the Orçamentos (quotes/proposals) API.</summary>
        OrcamentosApi Orcamentos { get; }

        /// <summary>
        /// Raised after tokens are successfully updated — either via <see cref="AuthorizeAsync"/>
        /// or an automatic/manual <see cref="RefreshTokenAsync"/>.
        /// </summary>
        event EventHandler<TokenRefreshedEventArgs> TokenRefreshed;

        /// <summary>
        /// Exchanges an authorization code for access and refresh tokens.
        /// </summary>
        /// <param name="code">The authorization code received from the OAuth flow.</param>
        /// <param name="redirectUri">The redirect URI used in the OAuth flow.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TokenResponse"/> containing the access and refresh tokens.</returns>
        Task<TokenResponse> AuthorizeAsync(string code, string redirectUri, CancellationToken cancellationToken = default);

        /// <summary>
        /// Refreshes the access token using the current refresh token.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A <see cref="TokenResponse"/> containing the new access and refresh tokens.</returns>
        Task<TokenResponse> RefreshTokenAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the access token to be used for API requests and updates the authorization header.
        /// </summary>
        /// <param name="accessToken">The access token to set.</param>
        /// <param name="expiresIn">Optional. Token lifetime in seconds.</param>
        void SetAccessToken(string accessToken, int expiresIn = 0);

        /// <summary>
        /// Sets the refresh token to be used for obtaining new access tokens.
        /// </summary>
        /// <param name="refreshToken">The refresh token to set.</param>
        void SetRefreshToken(string refreshToken);

        /// <summary>
        /// Returns true when the access token is expired or within the proactive refresh buffer window.
        /// </summary>
        bool IsTokenExpired();

        /// <summary>
        /// Sends a GET request to the specified endpoint and deserializes the response.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the response into.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task<TResponse> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a GET request to the specified endpoint and returns the raw response body as bytes.
        /// Intended for binary endpoints such as PDF generation.
        /// </summary>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task<byte[]> GetBytesAsync(string endpoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a POST request to the specified endpoint and deserializes the response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response into.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="data">The request body data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a POST request to the specified endpoint without a request or response body.
        /// Intended for action endpoints that return <c>204 No Content</c>.
        /// </summary>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task PostAsync(string endpoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a POST request with a request body to the specified endpoint without reading a response body.
        /// Intended for endpoints that take a payload and return <c>204 No Content</c>.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="data">The request body data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task PostAsync<TRequest>(string endpoint, TRequest data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a PUT request to the specified endpoint and deserializes the response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response into.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="data">The request body data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a PATCH request to the specified endpoint and deserializes the response.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <typeparam name="TResponse">The type to deserialize the response into.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="data">The request body data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a PATCH request with a request body to the specified endpoint without reading a response body.
        /// Intended for endpoints that take a payload and return <c>204 No Content</c>.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="data">The request body data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task PatchAsync<TRequest>(string endpoint, TRequest data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a DELETE request to the specified endpoint and deserializes the response.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the response into.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task<TResponse> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a DELETE request to the specified endpoint without a response body.
        /// </summary>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a DELETE request with a request body to the specified endpoint without reading a
        /// response body. Intended for batch-delete endpoints that return <c>204 No Content</c>.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <param name="endpoint">The API endpoint.</param>
        /// <param name="data">The request body data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        Task DeleteAsync<TRequest>(string endpoint, TRequest data, CancellationToken cancellationToken = default);
    }
}
