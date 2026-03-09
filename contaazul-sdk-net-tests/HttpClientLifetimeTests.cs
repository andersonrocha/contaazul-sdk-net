using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;
using Moq;
using Moq.Protected;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class HttpClientLifetimeTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    private static HttpClient BuildAuthHttpClient(Mock<HttpMessageHandler> handler)
    {
        return new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://auth.contaazul.com")
        };
    }

    private static Mock<HttpMessageHandler> BuildAuthHandlerReturning(TokenResponse tokenResponse)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(tokenResponse),
                    Encoding.UTF8, "application/json")
            });
        return handler;
    }

    [Test]
    public async Task WhenAuthorizeAsyncThenUsesInjectedAuthHttpClient()
    {
        var tokenResponse = new TokenResponse
        {
            AccessToken = "new-access-token",
            RefreshToken = "new-refresh-token",
            ExpiresIn = ContaAzulApiClient.AccessTokenLifetimeSeconds
        };

        var authHandler = BuildAuthHandlerReturning(tokenResponse);
        using (var authHttpClient = BuildAuthHttpClient(authHandler))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, accessToken: null, refreshToken: null,
            authHttpClient: authHttpClient))
        {
            var result = await client.AuthorizeAsync("auth-code", "https://app.com/callback");

            Assert.That(result.AccessToken, Is.EqualTo("new-access-token"));
            Assert.That(client.AccessToken, Is.EqualTo("new-access-token"));
            Assert.That(client.RefreshToken, Is.EqualTo("new-refresh-token"));
        }

        authHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task WhenRefreshTokenAsyncThenUsesInjectedAuthHttpClient()
    {
        var tokenResponse = new TokenResponse
        {
            AccessToken = "refreshed-access-token",
            RefreshToken = "rotated-refresh-token",
            ExpiresIn = ContaAzulApiClient.AccessTokenLifetimeSeconds
        };

        var authHandler = BuildAuthHandlerReturning(tokenResponse);
        using (var authHttpClient = BuildAuthHttpClient(authHandler))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "old-token", "old-refresh",
            authHttpClient: authHttpClient))
        {
            var result = await client.RefreshTokenAsync();

            Assert.That(result.AccessToken, Is.EqualTo("refreshed-access-token"));
            Assert.That(client.AccessToken, Is.EqualTo("refreshed-access-token"));
            // Token rotation: refresh token must be updated on every renewal
            Assert.That(client.RefreshToken, Is.EqualTo("rotated-refresh-token"));
        }
    }

    [Test]
    public async Task WhenAuthHttpClientIsInjectedThenNotDisposedWithClient()
    {
        var tokenResponse = new TokenResponse
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresIn = ContaAzulApiClient.AccessTokenLifetimeSeconds
        };

        var authHandler = BuildAuthHandlerReturning(tokenResponse);
        var authHttpClient = BuildAuthHttpClient(authHandler);

        var client = new ContaAzulApiClient(
            ClientId, ClientSecret, accessToken: null, refreshToken: null,
            authHttpClient: authHttpClient);

        client.Dispose();

        // authHttpClient was injected externally: still usable after the client is disposed
        Assert.DoesNotThrowAsync(async () =>
            await authHttpClient.GetAsync("https://auth.contaazul.com/health"));

        authHttpClient.Dispose();
    }

    [Test]
    public void WhenAuthorizationHeaderThenContainsBasicCredentials()
    {
        var callCount = 0;
        string capturedAuthHeader = null;

        var authHandler = new Mock<HttpMessageHandler>();
        authHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) =>
            {
                callCount++;
                capturedAuthHeader = req.Headers.Authorization?.ToString();
            })
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(new TokenResponse
                    {
                        AccessToken = "token",
                        RefreshToken = "refresh"
                    }),
                    Encoding.UTF8, "application/json")
            });

        using (var authHttpClient = BuildAuthHttpClient(authHandler))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, accessToken: null, refreshToken: null,
            authHttpClient: authHttpClient))
        {
            client.AuthorizeAsync("code", "https://app.com/callback").GetAwaiter().GetResult();
        }

        var expectedCredentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));

        Assert.That(capturedAuthHeader, Is.EqualTo($"Basic {expectedCredentials}"));
    }

    [Test]
    public async Task WhenMultipleRefreshCallsThenReusesSameAuthHttpClient()
    {
        var callCount = 0;
        var tokenResponse = new TokenResponse
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresIn = ContaAzulApiClient.AccessTokenLifetimeSeconds
        };

        var authHandler = new Mock<HttpMessageHandler>();
        authHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                callCount++;
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        Newtonsoft.Json.JsonConvert.SerializeObject(tokenResponse),
                        Encoding.UTF8, "application/json")
                };
            });

        using (var authHttpClient = BuildAuthHttpClient(authHandler))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "old-token", "old-refresh",
            authHttpClient: authHttpClient))
        {
            await client.RefreshTokenAsync();
            await client.RefreshTokenAsync();
            await client.RefreshTokenAsync();
        }

        // All three calls went through the same injected HttpClient (handler was called 3 times)
        Assert.That(callCount, Is.EqualTo(3));
    }
}
