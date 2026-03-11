using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class LoggingTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";
    private const string BaseUrl = "https://api-v2.contaazul.com";
    private const string AuthBaseUrl = "https://auth.contaazul.com";

    /// <summary>
    /// Logger simples que captura todas as entradas em memória para verificação nos testes.
    /// </summary>
    private sealed class CapturingLogger : ILogger<ContaAzulApiClient>
    {
        public List<(LogLevel Level, string Message)> Entries { get; } = new List<(LogLevel, string)>();

        public bool HasEntry(LogLevel level, string containsText) =>
            Entries.Exists(e => e.Level == level && e.Message.Contains(containsText));

        IDisposable ILogger.BeginScope<TState>(TState state) => NullScope.Instance;

        bool ILogger.IsEnabled(LogLevel logLevel) => true;

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            Entries.Add((logLevel, formatter(state, exception)));
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new NullScope();
            public void Dispose() { }
        }
    }

    private static string TokenResponseBody(string accessToken = "new-access-token", int expiresIn = 3600) =>
        $"{{\"access_token\":\"{accessToken}\",\"refresh_token\":\"new-refresh-token\",\"expires_in\":{expiresIn},\"token_type\":\"Bearer\"}}";

    private static Mock<HttpMessageHandler> BuildHandlerReturning(HttpStatusCode status, string body = "{}")
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = status,
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            });
        return handler;
    }

    // --- Construtor sem logger ---

    [Test]
    public void WhenNullLoggerIsPassedThenClientCreatesSuccessfully()
    {
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret))
        {
            Assert.That(client, Is.Not.Null);
        }
    }

    // --- AuthorizeAsync ---

    [Test]
    public async Task WhenAuthorizationSucceedsThenLogsDebugBeforeAndInformationAfter()
    {
        var authHandler = BuildHandlerReturning(HttpStatusCode.OK, TokenResponseBody());
        var logger = new CapturingLogger();

        using (var authClient = new HttpClient(authHandler.Object) { BaseAddress = new Uri(AuthBaseUrl) })
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            new ContaAzulApiClientOptions { AuthHttpClient = authClient, Logger = logger }))
        {
            await client.AuthorizeAsync("auth-code", "https://app.example.com/callback");
        }

        Assert.That(logger.HasEntry(LogLevel.Debug, "authorization code"), Is.True);
        Assert.That(logger.HasEntry(LogLevel.Information, "Token obtained"), Is.True);
    }

    [Test]
    public void WhenTokenEndpointFailsThenLogsError()
    {
        var authHandler = BuildHandlerReturning(HttpStatusCode.BadRequest, "{\"error\":\"invalid_grant\"}");
        var logger = new CapturingLogger();

        using (var authClient = new HttpClient(authHandler.Object) { BaseAddress = new Uri(AuthBaseUrl) })
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            new ContaAzulApiClientOptions { AuthHttpClient = authClient, Logger = logger }))
        {
            Assert.ThrowsAsync<ContaAzulApiException>(async () =>
                await client.AuthorizeAsync("bad-code", "https://app.example.com/callback"));
        }

        Assert.That(logger.HasEntry(LogLevel.Error, "Token endpoint request failed"), Is.True);
    }

    // --- RefreshTokenAsync ---

    [Test]
    public async Task WhenTokenRefreshSucceedsThenLogsDebugAndInformation()
    {
        var authHandler = BuildHandlerReturning(HttpStatusCode.OK, TokenResponseBody());
        var logger = new CapturingLogger();

        using (var authClient = new HttpClient(authHandler.Object) { BaseAddress = new Uri(AuthBaseUrl) })
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            null, "refresh-token", new ContaAzulApiClientOptions { BaseUrl = BaseUrl, AuthHttpClient = authClient, Logger = logger }))
        {
            await client.RefreshTokenAsync();
        }

        Assert.That(logger.HasEntry(LogLevel.Debug, "Refreshing access token"), Is.True);
        Assert.That(logger.HasEntry(LogLevel.Information, "Token obtained"), Is.True);
    }

    // --- ExecuteWithRetryAsync: 401 ---

    [Test]
    public async Task When401OccursThenLogsWarningBeforeRefresh()
    {
        var callCount = 0;
        var apiHandler = new Mock<HttpMessageHandler>();
        apiHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1
                    ? new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent("{}", Encoding.UTF8, "application/json") }
                    : new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("{}", Encoding.UTF8, "application/json") };
            });

        var authHandler = BuildHandlerReturning(HttpStatusCode.OK, TokenResponseBody());
        var logger = new CapturingLogger();

        using (var authClient = new HttpClient(authHandler.Object) { BaseAddress = new Uri(AuthBaseUrl) })
        using (var apiClient = new HttpClient(apiHandler.Object))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            "token", "refresh-token", new ContaAzulApiClientOptions { BaseUrl = BaseUrl, HttpClient = apiClient, AuthHttpClient = authClient, Logger = logger }))
        {
            client.RetryOptions = RetryOptions.None;
            await client.GetAsync<object>("/test");
        }

        Assert.That(logger.HasEntry(LogLevel.Warning, "401 Unauthorized"), Is.True);
    }

    // --- ExecuteWithRetryAsync: erro transitório ---

    [Test]
    public async Task WhenTransientErrorOccursThenLogsWarningWithAttemptNumber()
    {
        var callCount = 0;
        var apiHandler = new Mock<HttpMessageHandler>();
        apiHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1
                    ? new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("{}", Encoding.UTF8, "application/json") }
                    : new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("{}", Encoding.UTF8, "application/json") };
            });

        var logger = new CapturingLogger();

        using (var httpClient = new HttpClient(apiHandler.Object))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            "token", null, new ContaAzulApiClientOptions { BaseUrl = BaseUrl, HttpClient = httpClient, Logger = logger }))
        {
            client.RetryOptions = new RetryOptions { MaxRetries = 1, InitialDelay = TimeSpan.Zero };
            await client.GetAsync<object>("/test");
        }

        Assert.That(logger.HasEntry(LogLevel.Warning, "Transient error"), Is.True);
        Assert.That(logger.HasEntry(LogLevel.Warning, "attempt 1 of 1"), Is.True);
    }

    // --- EnsureValidTokenAsync: refresh proativo ---

    [Test]
    public async Task WhenTokenIsExpiredThenLogsDebugBeforeProactiveRefresh()
    {
        var authHandler = BuildHandlerReturning(HttpStatusCode.OK, TokenResponseBody());
        var apiHandler = BuildHandlerReturning(HttpStatusCode.OK, "{}");
        var logger = new CapturingLogger();

        var expiredAt = DateTime.UtcNow.AddSeconds(-1);

        using (var authClient = new HttpClient(authHandler.Object) { BaseAddress = new Uri(AuthBaseUrl) })
        using (var apiClient = new HttpClient(apiHandler.Object))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            "expired-token", "refresh-token", new ContaAzulApiClientOptions { BaseUrl = BaseUrl, HttpClient = apiClient, TokenExpiresAt = expiredAt, AuthHttpClient = authClient, Logger = logger }))
        {
            await client.GetAsync<object>("/test");
        }

        Assert.That(logger.HasEntry(LogLevel.Debug, "expiring"), Is.True);
    }

    // --- EnforceRateLimitAsync ---

    [Test]
    public async Task WhenRateLimitIsHitThenLogsDebug()
    {
        var apiHandler = BuildHandlerReturning(HttpStatusCode.OK, "{}");
        var logger = new CapturingLogger();

        using (var httpClient = new HttpClient(apiHandler.Object))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            "token", null, new ContaAzulApiClientOptions { BaseUrl = BaseUrl, HttpClient = httpClient, Logger = logger }))
        {
            client.RateLimitOptions = new RateLimitOptions { RequestsPerSecond = 1 };
            client.RetryOptions = RetryOptions.None;

            await client.GetAsync<object>("/test");
            await client.GetAsync<object>("/test");
        }

        Assert.That(logger.HasEntry(LogLevel.Debug, "Rate limit"), Is.True);
    }
}
