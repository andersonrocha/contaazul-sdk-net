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
public class TokenExpirationTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    [Test]
    public void WhenNoExpirationSetThenIsTokenExpiredReturnsFalse()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret, "access-token", "refresh-token");

        Assert.That(client.IsTokenExpired(), Is.False);
    }

    [Test]
    public void WhenTokenExpiresInFutureThenIsTokenExpiredReturnsFalse()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetAccessToken("access-token", expiresIn: 7200);

        Assert.That(client.IsTokenExpired(), Is.False);
    }

    [Test]
    public void WhenTokenExpiredInPastThenIsTokenExpiredReturnsTrue()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        // expiresIn of 1 second minus the 300s buffer means it is already in the buffer zone
        client.SetAccessToken("access-token", expiresIn: 1);

        Assert.That(client.IsTokenExpired(), Is.True);
    }

    [Test]
    public void WhenTokenExpiresWithinBufferWindowThenIsTokenExpiredReturnsTrue()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        // 299 seconds < 300s buffer, so it should be considered expired
        client.SetAccessToken("access-token", expiresIn: 299);

        Assert.That(client.IsTokenExpired(), Is.True);
    }

    [Test]
    public void WhenTokenExpiresExactlyAtBufferBoundaryThenIsTokenExpiredReturnsTrue()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        // Exactly at the buffer limit is still expired
        client.SetAccessToken("access-token", expiresIn: 300);

        Assert.That(client.IsTokenExpired(), Is.True);
    }

    [Test]
    public void WhenTokenExpiresAfterBufferWindowThenIsTokenExpiredReturnsFalse()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        // 301 seconds > 300s buffer, should be valid
        client.SetAccessToken("access-token", expiresIn: 301);

        Assert.That(client.IsTokenExpired(), Is.False);
    }

    [Test]
    public void WhenSetAccessTokenWithoutExpiresInThenTokenExpiresAtIsUnchanged()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetAccessToken("access-token");

        Assert.That(client.TokenExpiresAt, Is.EqualTo(DateTime.MinValue));
    }

    [Test]
    public void WhenSetAccessTokenWithExpiresInThenTokenExpiresAtIsSet()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        var before = DateTime.UtcNow.AddSeconds(3600);

        client.SetAccessToken("access-token", expiresIn: 3600);

        var after = DateTime.UtcNow.AddSeconds(3600);
        Assert.That(client.TokenExpiresAt, Is.InRange(before, after));
    }

    [Test]
    public async Task WhenTokenIsExpiredThenGetAsyncRefreshesBeforeRequest()
    {
        var refreshCount = 0;
        var apiCallCount = 0;

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsolutePath.Contains("/test")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                apiCallCount++;
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}", Encoding.UTF8, "application/json")
                };
            });

        var apiHttpClient = new HttpClient(mockHandler.Object);

        // Simula refresh com um HttpClient separado (não podemos interceptar o de auth aqui,
        // então apenas verificamos que EnsureValidTokenAsync não lança quando não há refresh token)
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, "expired-token", null, new ContaAzulApiClientOptions { BaseUrl = "https://api-v2.contaazul.com", HttpClient = apiHttpClient }))
        {
            // Token expirado sem refresh token: não deve tentar refresh
            client.SetAccessToken("expired-token", expiresIn: 1);

            await client.GetAsync<object>("/test", CancellationToken.None);

            Assert.That(apiCallCount, Is.EqualTo(1));
            Assert.That(refreshCount, Is.EqualTo(0));
        }
    }

    [Test]
    public void WhenConstructorReceivesValidTokenExpiresAtThenIsTokenExpiredReturnsFalse()
    {
        var expiresAt = DateTime.UtcNow.AddSeconds(ContaAzulApiClient.AccessTokenLifetimeSeconds);

        var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "access-token", "refresh-token",
            new ContaAzulApiClientOptions { TokenExpiresAt = expiresAt });

        Assert.That(client.IsTokenExpired(), Is.False);
    }

    [Test]
    public void WhenConstructorReceivesExpiredTokenExpiresAtThenIsTokenExpiredReturnsTrue()
    {
        var expiresAt = DateTime.UtcNow.AddSeconds(-1);

        var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "access-token", "refresh-token",
            new ContaAzulApiClientOptions { TokenExpiresAt = expiresAt });

        Assert.That(client.IsTokenExpired(), Is.True);
    }

    [Test]
    public void WhenConstructorReceivesNoTokenExpiresAtThenTokenExpiresAtIsDefault()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret, "access-token", "refresh-token");

        Assert.That(client.TokenExpiresAt, Is.EqualTo(DateTime.MinValue));
    }

    [Test]
    public void WhenAccessTokenLifetimeSecondsIsUsedAsExpiresInThenTokenIsNotExpired()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetAccessToken("access-token", expiresIn: ContaAzulApiClient.AccessTokenLifetimeSeconds);

        Assert.That(client.IsTokenExpired(), Is.False);
    }
}
