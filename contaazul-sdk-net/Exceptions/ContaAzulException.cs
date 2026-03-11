using System;
using System.Net;
using System.Runtime.Serialization;

namespace ContaAzul.Sdk.Net.Exceptions
{
    /// <summary>
    /// Base exception for all errors raised by the ContaAzul SDK.
    /// </summary>
    [Serializable]
    public class ContaAzulException : Exception
    {
        /// <summary>Gets the HTTP status code returned by the server, if the error originated from an HTTP response.</summary>
        public HttpStatusCode? StatusCode { get; }

        /// <summary>Gets the raw response body returned by the server.</summary>
        public string ResponseContent { get; }

        /// <summary>Initializes a new instance with a message and optional HTTP context.</summary>
        public ContaAzulException(string message, HttpStatusCode? statusCode = null, string responseContent = null)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        /// <summary>Initializes a new instance wrapping an inner exception.</summary>
        public ContaAzulException(string message, Exception innerException, HttpStatusCode? statusCode = null, string responseContent = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

#pragma warning disable SYSLIB0051
        /// <summary>Initializes a new instance during deserialization.</summary>
        protected ContaAzulException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#pragma warning restore SYSLIB0051
    }

    /// <summary>
    /// Thrown when the API returns a non-success HTTP status code that does not map to a more specific exception.
    /// </summary>
    [Serializable]
    public class ContaAzulApiException : ContaAzulException
    {
        /// <summary>
        /// Initializes a new instance with the failing status code and response body.
        /// </summary>
        public ContaAzulApiException(HttpStatusCode statusCode, string responseContent)
            : base(
                $"ContaAzul API request failed with status {(int)statusCode} ({statusCode}). Content: {responseContent}",
                statusCode,
                responseContent)
        {
        }

#pragma warning disable SYSLIB0051
        /// <summary>Initializes a new instance during deserialization.</summary>
        protected ContaAzulApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#pragma warning restore SYSLIB0051
    }

    /// <summary>
    /// Thrown when the API returns HTTP 401 Unauthorized.
    /// This typically means the access token is invalid or has expired and could not be refreshed.
    /// </summary>
    [Serializable]
    public class ContaAzulAuthenticationException : ContaAzulException
    {
        /// <summary>Initializes a new instance with the response body from the server.</summary>
        public ContaAzulAuthenticationException(string responseContent)
            : base(
                "Authentication failed. The access token is invalid or has expired.",
                HttpStatusCode.Unauthorized,
                responseContent)
        {
        }

#pragma warning disable SYSLIB0051
        /// <summary>Initializes a new instance during deserialization.</summary>
        protected ContaAzulAuthenticationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#pragma warning restore SYSLIB0051
    }

    /// <summary>
    /// Thrown when the API returns HTTP 429 Too Many Requests.
    /// The caller should back off and retry the operation after the indicated period.
    /// </summary>
    [Serializable]
    public class ContaAzulRateLimitException : ContaAzulException
    {
        // HttpStatusCode.TooManyRequests (429) was added in .NET Standard 2.1 / .NET 5.
        // Casting from int ensures compatibility with .NET Standard 2.0.
        private const HttpStatusCode TooManyRequests = (HttpStatusCode)429;

        /// <summary>Initializes a new instance with the response body from the server.</summary>
        public ContaAzulRateLimitException(string responseContent)
            : base(
                "API rate limit exceeded. Please retry after some time.",
                TooManyRequests,
                responseContent)
        {
        }

#pragma warning disable SYSLIB0051
        /// <summary>Initializes a new instance during deserialization.</summary>
        protected ContaAzulRateLimitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#pragma warning restore SYSLIB0051
    }
}
