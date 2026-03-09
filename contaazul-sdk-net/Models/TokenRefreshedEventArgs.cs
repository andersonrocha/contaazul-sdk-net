using System;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Provides data for the <see cref="ContaAzulApiClient.TokenRefreshed"/> event.
    /// Contains the new tokens issued by the authorization server.
    /// <para>
    /// Persist <see cref="AccessToken"/>, <see cref="RefreshToken"/> and <see cref="TokenExpiresAt"/>
    /// to storage whenever this event fires, so the next client instance can be restored with valid tokens.
    /// </para>
    /// </summary>
    public class TokenRefreshedEventArgs : EventArgs
    {
        /// <summary>Gets the new access token.</summary>
        public string AccessToken { get; }

        /// <summary>
        /// Gets the new refresh token.
        /// ContaAzul rotates the refresh token on every renewal — this value replaces the previous one.
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>Gets the access token lifetime in seconds as returned by the server.</summary>
        public int ExpiresIn { get; }

        /// <summary>
        /// Gets the UTC date and time at which the new access token expires.
        /// Use this value when restoring the client via
        /// <see cref="ContaAzulApiClient(string, string, string, string, string, System.Net.Http.HttpClient, DateTime, System.Net.Http.HttpClient)"/>.
        /// </summary>
        public DateTime TokenExpiresAt { get; }

        internal TokenRefreshedEventArgs(TokenResponse tokenResponse, DateTime tokenExpiresAt)
        {
            AccessToken = tokenResponse.AccessToken;
            RefreshToken = tokenResponse.RefreshToken;
            ExpiresIn = tokenResponse.ExpiresIn;
            TokenExpiresAt = tokenExpiresAt;
        }
    }
}
