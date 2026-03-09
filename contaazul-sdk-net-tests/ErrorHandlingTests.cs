using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Exceptions;
using ContaAzul.Sdk.Net.Models;
using Moq;
using Moq.Protected;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class ErrorHandlingTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    private static Mock<HttpMessageHandler> BuildApiHandlerReturning(HttpStatusCode status, string body = "{}")
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = status,
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            });
        return handler;
    }

    private static Mock<HttpMessageHandler> BuildApiHandlerWithSequence(
        HttpStatusCode firstStatus, string firstBody,
        HttpStatusCode secondStatus, string secondBody)
    {
        var callCount = 0;
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                callCount++;
                return callCount == 1
                    ? new HttpResponseMessage { StatusCode = firstStatus, Content = new StringContent(firstBody, Encoding.UTF8, "application/json") }
                    : new HttpResponseMessage { StatusCode = secondStatus, Content = new StringContent(secondBody, Encoding.UTF8, "application/json") };
            });
        return handler;
    }

    // --- ContaAzulAuthenticationException ---

    [Test]
    public void WhenApiReturns401ThenThrowsContaAzulAuthenticationException()
    {
        var handler = BuildApiHandlerReturning(HttpStatusCode.Unauthorized, "{\"error\":\"unauthorized\"}");
        using (var apiHttpClient = new HttpClient(handler.Object))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            "https://api-v2.contaazul.com", apiHttpClient))
        {
            var ex = Assert.ThrowsAsync<ContaAzulAuthenticationException>(
                async () => await client.GetAsync<object>("/test"));

            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(ex.ResponseContent, Does.Contain("unauthorized"));
        }
    }

    [Test]
    public void WhenAuthEndpointReturns401ThenThrowsContaAzulAuthenticationException()
    {
        var authHandler = BuildApiHandlerReturning(HttpStatusCode.Unauthorized, "{\"error\":\"invalid_client\"}");
        using (var authHttpClient = new HttpClient(authHandler.Object) { BaseAddress = new Uri("https://auth.contaazul.com") })
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, authHttpClient: authHttpClient))
        {
            var ex = Assert.ThrowsAsync<ContaAzulAuthenticationException>(
                async () => await client.AuthorizeAsync("bad-code", "https://app.com/callback"));

            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }

    // --- ContaAzulRateLimitException ---

    [Test]
    public void WhenApiReturns429ThenThrowsContaAzulRateLimitException()
    {
        var handler = BuildApiHandlerReturning((HttpStatusCode)429, "{\"error\":\"rate_limit_exceeded\"}");
        using (var apiHttpClient = new HttpClient(handler.Object))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            "https://api-v2.contaazul.com", apiHttpClient))
        {
            var ex = Assert.ThrowsAsync<ContaAzulRateLimitException>(
                async () => await client.GetAsync<object>("/test"));

            Assert.That((int)ex.StatusCode, Is.EqualTo(429));
        }
    }

    // --- ContaAzulApiException ---

    [Test]
    public void WhenApiReturns500ThenThrowsContaAzulApiException()
    {
        var handler = BuildApiHandlerReturning(HttpStatusCode.InternalServerError, "{\"error\":\"server_error\"}");
        using (var apiHttpClient = new HttpClient(handler.Object))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            "https://api-v2.contaazul.com", apiHttpClient))
        {
            var ex = Assert.ThrowsAsync<ContaAzulApiException>(
                async () => await client.GetAsync<object>("/test"));

            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
    }

    [Test]
    public void WhenApiReturns404ThenThrowsContaAzulApiException()
    {
        var handler = BuildApiHandlerReturning(HttpStatusCode.NotFound, "{\"error\":\"not_found\"}");
        using (var apiHttpClient = new HttpClient(handler.Object))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            "https://api-v2.contaazul.com", apiHttpClient))
        {
            var ex = Assert.ThrowsAsync<ContaAzulApiException>(
                async () => await client.GetAsync<object>("/test"));

            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.ResponseContent, Does.Contain("not_found"));
        }
    }

    // --- Exception hierarchy ---

    [Test]
    public void WhenAuthenticationExceptionThrownThenIsContaAzulException()
    {
        var handler = BuildApiHandlerReturning(HttpStatusCode.Unauthorized, "{}");
        using (var apiHttpClient = new HttpClient(handler.Object))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            "https://api-v2.contaazul.com", apiHttpClient))
        {
            var ex = Assert.ThrowsAsync<ContaAzulAuthenticationException>(
                async () => await client.GetAsync<object>("/test"));

            Assert.That(ex, Is.InstanceOf<ContaAzulException>());
        }
    }

    [Test]
    public void WhenRateLimitExceptionThrownThenIsContaAzulException()
    {
        var handler = BuildApiHandlerReturning((HttpStatusCode)429, "{}");
        using (var apiHttpClient = new HttpClient(handler.Object))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            "https://api-v2.contaazul.com", apiHttpClient))
        {
            var ex = Assert.ThrowsAsync<ContaAzulRateLimitException>(
                async () => await client.GetAsync<object>("/test"));

            Assert.That(ex, Is.InstanceOf<ContaAzulException>());
        }
    }

    // --- Retry on 401 ---

    [Test]
    public async Task WhenApiReturns401AndHasRefreshTokenThenRetriesAfterRefresh()
    {
        var tokenResponse = new TokenResponse
        {
            AccessToken = "new-access-token",
            RefreshToken = "new-refresh-token",
            ExpiresIn = ContaAzulApiClient.AccessTokenLifetimeSeconds
        };

        // API: first call returns 401, second returns 200
        var apiHandler = BuildApiHandlerWithSequence(
            HttpStatusCode.Unauthorized, "{\"error\":\"unauthorized\"}",
            HttpStatusCode.OK, "{\"id\":\"123\"}");

        // Auth endpoint: returns a fresh token on refresh
        var authHandler = BuildApiHandlerReturning(
            HttpStatusCode.OK,
            Newtonsoft.Json.JsonConvert.SerializeObject(tokenResponse));

        using (var apiHttpClient = new HttpClient(apiHandler.Object))
        using (var authHttpClient = new HttpClient(authHandler.Object) { BaseAddress = new Uri("https://auth.contaazul.com") })
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "old-token", "old-refresh",
            "https://api-v2.contaazul.com", apiHttpClient,
            authHttpClient: authHttpClient))
        {
            var result = await client.GetAsync<object>("/test");

            Assert.That(result, Is.Not.Null);
            Assert.That(client.AccessToken, Is.EqualTo("new-access-token"));
        }
    }

    [Test]
    public void WhenApiReturns401AndNoRefreshTokenThenThrowsContaAzulAuthenticationException()
    {
        var handler = BuildApiHandlerReturning(HttpStatusCode.Unauthorized, "{}");
        using (var apiHttpClient = new HttpClient(handler.Object))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            "https://api-v2.contaazul.com", apiHttpClient))
        {
            Assert.ThrowsAsync<ContaAzulAuthenticationException>(
                async () => await client.GetAsync<object>("/test"));
        }
    }
}
