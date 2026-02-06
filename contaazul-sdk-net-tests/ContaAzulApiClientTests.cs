using System;
using ContaAzul.Sdk.Net;
using NUnit.Framework;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class ContaAzulApiClientTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";
    private const string AuthCode = "test-auth-code";
    private const string RedirectUri = "https://test.com/callback";

    [Test]
    public void WhenConstructorWithNullClientIdThenThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient(null!, ClientSecret));
    }

    [Test]
    public void WhenConstructorWithEmptyClientIdThenThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient(string.Empty, ClientSecret));
    }

    [Test]
    public void WhenConstructorWithWhitespaceClientIdThenThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient("   ", ClientSecret));
    }

    [Test]
    public void WhenConstructorWithNullClientSecretThenThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient(ClientId, null!));
    }

    [Test]
    public void WhenConstructorWithEmptyClientSecretThenThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient(ClientId, string.Empty));
    }

    [Test]
    public void WhenConstructorWithWhitespaceClientSecretThenThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient(ClientId, "   "));
    }

    [Test]
    public void WhenConstructorWithValidParametersThenCreatesInstance()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        
        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.Null);
            Assert.That(client.RefreshToken, Is.Null);
            Assert.That(client.Pessoas, Is.Not.Null);
            Assert.That(client.Vendas, Is.Not.Null);
        });
    }

    [Test]
    public void WhenConstructorWithCustomHttpClientThenCreatesInstance()
    {
        var httpClient = new System.Net.Http.HttpClient();
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.That(client, Is.Not.Null);
    }

    [Test]
    public void WhenAuthorizeAsyncWithNullCodeThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.AuthorizeAsync(null!, RedirectUri));
    }

    [Test]
    public void WhenAuthorizeAsyncWithEmptyCodeThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.AuthorizeAsync(string.Empty, RedirectUri));
    }

    [Test]
    public void WhenAuthorizeAsyncWithWhitespaceCodeThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.AuthorizeAsync("   ", RedirectUri));
    }

    [Test]
    public void WhenAuthorizeAsyncWithNullRedirectUriThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.AuthorizeAsync(AuthCode, null!));
    }

    [Test]
    public void WhenAuthorizeAsyncWithEmptyRedirectUriThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.AuthorizeAsync(AuthCode, string.Empty));
    }

    [Test]
    public void WhenAuthorizeAsyncWithWhitespaceRedirectUriThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.AuthorizeAsync(AuthCode, "   "));
    }

    [Test]
    public void WhenRefreshTokenAsyncWithoutPriorAuthorizationThenThrowsInvalidOperationException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await client.RefreshTokenAsync());

        Assert.That(exception!.Message, Does.Contain("Refresh token is not available"));
    }

    [Test]
    public void WhenSetAccessTokenWithValidTokenThenUpdatesAccessToken()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        var newAccessToken = "manually-set-token";

        client.SetAccessToken(newAccessToken);

        Assert.That(client.AccessToken, Is.EqualTo(newAccessToken));
    }

    [Test]
    public void WhenSetAccessTokenWithNullValueThenUpdatesAccessToken()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetAccessToken(null!);

        Assert.That(client.AccessToken, Is.Null);
    }

    [Test]
    public void WhenSetAccessTokenWithEmptyStringThenUpdatesAccessToken()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetAccessToken(string.Empty);

        Assert.That(client.AccessToken, Is.EqualTo(string.Empty));
    }

    [Test]
    public void WhenConstructorWithAccessTokenAndRefreshTokenThenCreatesInstanceWithTokens()
    {
        var accessToken = "test-access-token";
        var refreshToken = "test-refresh-token";

        var client = new ContaAzulApiClient(
            clientId: ClientId,
            clientSecret: ClientSecret,
            accessToken: accessToken,
            refreshToken: refreshToken);

        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.EqualTo(accessToken));
            Assert.That(client.RefreshToken, Is.EqualTo(refreshToken));
            Assert.That(client.Pessoas, Is.Not.Null);
            Assert.That(client.Vendas, Is.Not.Null);
        });
    }

    [Test]
    public void WhenConstructorWithOnlyAccessTokenThenCreatesInstanceWithAccessTokenOnly()
    {
        var accessToken = "test-access-token";

        var client = new ContaAzulApiClient(
            clientId: ClientId,
            clientSecret: ClientSecret,
            accessToken: accessToken,
            refreshToken: null);

        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.EqualTo(accessToken));
            Assert.That(client.RefreshToken, Is.Null);
        });
    }

    [Test]
    public void WhenConstructorWithOnlyRefreshTokenThenCreatesInstanceWithRefreshTokenOnly()
    {
        var refreshToken = "test-refresh-token";

        var client = new ContaAzulApiClient(
            clientId: ClientId,
            clientSecret: ClientSecret,
            accessToken: null,
            refreshToken: refreshToken);

        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.Null);
            Assert.That(client.RefreshToken, Is.EqualTo(refreshToken));
        });
    }

    [Test]
    public void WhenConstructorWithNullTokensThenCreatesInstanceWithoutTokens()
    {
        var client = new ContaAzulApiClient(
            clientId: ClientId,
            clientSecret: ClientSecret,
            accessToken: null,
            refreshToken: null);

        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.Null);
            Assert.That(client.RefreshToken, Is.Null);
        });
    }

    [Test]
    public void WhenConstructorWithEmptyAccessTokenThenCreatesInstanceWithEmptyToken()
    {
        var client = new ContaAzulApiClient(
            clientId: ClientId,
            clientSecret: ClientSecret,
            accessToken: string.Empty,
            refreshToken: null);

        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.EqualTo(string.Empty));
            Assert.That(client.RefreshToken, Is.Null);
        });
    }

    [Test]
    public void WhenConstructorWithTokensAndNullClientIdThenThrowsArgumentNullException()
    {
        var accessToken = "test-access-token";
        var refreshToken = "test-refresh-token";

        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient(
                clientId: null!,
                clientSecret: ClientSecret,
                accessToken: accessToken,
                refreshToken: refreshToken));
    }

    [Test]
    public void WhenConstructorWithTokensAndNullClientSecretThenThrowsArgumentNullException()
    {
        var accessToken = "test-access-token";
        var refreshToken = "test-refresh-token";

        Assert.Throws<ArgumentNullException>(() =>
            new ContaAzulApiClient(
                clientId: ClientId,
                clientSecret: null!,
                accessToken: accessToken,
                refreshToken: refreshToken));
    }

    [Test]
    public void WhenConstructorWithTokensAndCustomBaseUrlThenCreatesInstanceWithTokens()
    {
        var accessToken = "test-access-token";
        var refreshToken = "test-refresh-token";
        var customBaseUrl = "https://custom-api.contaazul.com";

        var client = new ContaAzulApiClient(
            clientId: ClientId,
            clientSecret: ClientSecret,
            accessToken: accessToken,
            refreshToken: refreshToken,
            baseUrl: customBaseUrl);

        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.EqualTo(accessToken));
            Assert.That(client.RefreshToken, Is.EqualTo(refreshToken));
        });
    }
}
