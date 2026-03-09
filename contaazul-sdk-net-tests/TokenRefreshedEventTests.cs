using System;
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
public class TokenRefreshedEventTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    private static HttpClient BuildAuthHttpClient(Mock<HttpMessageHandler> handler) =>
        new HttpClient(handler.Object) { BaseAddress = new Uri("https://auth.contaazul.com") };

    private static Mock<HttpMessageHandler> BuildHandlerReturningToken(
        string accessToken = "new-access",
        string refreshToken = "new-refresh",
        int expiresIn = ContaAzulApiClient.AccessTokenLifetimeSeconds)
    {
        var tokenResponse = new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = expiresIn
        };

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
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
    public async Task WhenAuthorizeAsyncThenTokenRefreshedEventFires()
    {
        TokenRefreshedEventArgs received = null;

        using (var authHttpClient = BuildAuthHttpClient(BuildHandlerReturningToken("at", "rt")))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, authHttpClient: authHttpClient))
        {
            client.TokenRefreshed += (_, args) => received = args;

            await client.AuthorizeAsync("auth-code", "https://app.com/callback");
        }

        Assert.That(received, Is.Not.Null);
        Assert.That(received.AccessToken, Is.EqualTo("at"));
        Assert.That(received.RefreshToken, Is.EqualTo("rt"));
    }

    [Test]
    public async Task WhenRefreshTokenAsyncThenTokenRefreshedEventFires()
    {
        TokenRefreshedEventArgs received = null;

        using (var authHttpClient = BuildAuthHttpClient(BuildHandlerReturningToken("refreshed-at", "rotated-rt")))
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "old-token", "old-refresh",
            authHttpClient: authHttpClient))
        {
            client.TokenRefreshed += (_, args) => received = args;

            await client.RefreshTokenAsync();
        }

        Assert.That(received, Is.Not.Null);
        Assert.That(received.AccessToken, Is.EqualTo("refreshed-at"));
        Assert.That(received.RefreshToken, Is.EqualTo("rotated-rt"));
    }

    [Test]
    public async Task WhenTokenRefreshedThenEventArgsContainsCorrectExpiresAt()
    {
        var before = DateTime.UtcNow.AddSeconds(ContaAzulApiClient.AccessTokenLifetimeSeconds);
        TokenRefreshedEventArgs received = null;

        using (var authHttpClient = BuildAuthHttpClient(BuildHandlerReturningToken()))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, authHttpClient: authHttpClient))
        {
            client.TokenRefreshed += (_, args) => received = args;

            await client.AuthorizeAsync("code", "https://app.com/callback");
        }

        var after = DateTime.UtcNow.AddSeconds(ContaAzulApiClient.AccessTokenLifetimeSeconds);

        Assert.That(received, Is.Not.Null);
        Assert.That(received.ExpiresIn, Is.EqualTo(ContaAzulApiClient.AccessTokenLifetimeSeconds));
        Assert.That(received.TokenExpiresAt, Is.InRange(before, after));
    }

    [Test]
    public async Task WhenNoHandlerSubscribedThenNoExceptionOnTokenRefresh()
    {
        using (var authHttpClient = BuildAuthHttpClient(BuildHandlerReturningToken()))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, authHttpClient: authHttpClient))
        {
            // No subscriber — event must not throw
            Assert.DoesNotThrowAsync(async () =>
                await client.AuthorizeAsync("code", "https://app.com/callback"));
        }
    }

    [Test]
    public async Task WhenMultipleHandlersSubscribedThenAllAreInvoked()
    {
        var callCount = 0;

        using (var authHttpClient = BuildAuthHttpClient(BuildHandlerReturningToken()))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, authHttpClient: authHttpClient))
        {
            client.TokenRefreshed += (_, _) => callCount++;
            client.TokenRefreshed += (_, _) => callCount++;
            client.TokenRefreshed += (_, _) => callCount++;

            await client.AuthorizeAsync("code", "https://app.com/callback");
        }

        Assert.That(callCount, Is.EqualTo(3));
    }

    [Test]
    public async Task WhenTokenRefreshedThenClientTokensMatchEventArgs()
    {
        TokenRefreshedEventArgs received = null;

        using (var authHttpClient = BuildAuthHttpClient(BuildHandlerReturningToken("final-at", "final-rt")))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, authHttpClient: authHttpClient))
        {
            client.TokenRefreshed += (_, args) => received = args;

            await client.AuthorizeAsync("code", "https://app.com/callback");

            // Event args must match the client's live state at the time of invocation
            Assert.That(received.AccessToken, Is.EqualTo(client.AccessToken));
            Assert.That(received.RefreshToken, Is.EqualTo(client.RefreshToken));
            Assert.That(received.TokenExpiresAt, Is.EqualTo(client.TokenExpiresAt));
        }
    }
}
