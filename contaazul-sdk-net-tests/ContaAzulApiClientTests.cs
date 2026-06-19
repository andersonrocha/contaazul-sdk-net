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
            options: new ContaAzulApiClientOptions { BaseUrl = customBaseUrl });

        Assert.Multiple(() =>
        {
            Assert.That(client, Is.Not.Null);
            Assert.That(client.AccessToken, Is.EqualTo(accessToken));
            Assert.That(client.RefreshToken, Is.EqualTo(refreshToken));
        });
    }

    #region BuildAuthorizationUrl Tests

    [Test]
    public void WhenBuildAuthorizationUrlWithValidParametersThenReturnsCorrectUrl()
    {
        var clientId = "3d0hgi6c523vj9u4t47h029asg";
        var redirectUri = "https://www.contaazul.com/";
        var state = "546512316845316541";
        var scope = "openid profile aws.cognito.signin.user.admin";

        var result = ContaAzulApiClient.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.StartWith("https://auth.contaazul.com/oauth2/authorize?"));
            Assert.That(result, Does.Contain("response_type=code"));
            Assert.That(result, Does.Contain($"client_id={Uri.EscapeDataString(clientId)}"));
            Assert.That(result, Does.Contain($"redirect_uri={redirectUri}"));
            Assert.That(result, Does.Contain($"state={Uri.EscapeDataString(state)}"));
            Assert.That(result, Does.Contain("scope=openid%20profile%20aws.cognito.signin.user.admin"));
        });
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithScopeContainingSpacesThenEncodesWithPercentTwenty()
    {
        var clientId = "test-client-id";
        var redirectUri = "https://example.com/callback";
        var state = "random-state";
        var scope = "openid profile email";

        var result = ContaAzulApiClient.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

        Assert.That(result, Does.Contain("scope=openid%20profile%20email"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithSpecialCharactersInClientIdThenEncodesCorrectly()
    {
        var clientId = "client+id&special=chars";
        var redirectUri = "https://example.com/callback";
        var state = "state-123";
        var scope = "openid";

        var result = ContaAzulApiClient.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

        Assert.That(result, Does.Contain($"client_id={Uri.EscapeDataString(clientId)}"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlThenRedirectUriIsLiteralWithoutPercentEncode()
    {
        // O ContaAzul espera o redirect_uri literal (sem %3A/%2F). ":" e "/" são válidos no query.
        var clientId = "test-client-id";
        var redirectUri = "https://x509lbwb-90.brs.devtunnels.ms/api/integracoes/contaazul/callback";
        var state = "state-123";
        var scope = "openid";

        var result = ContaAzulApiClient.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain($"redirect_uri={redirectUri}"));
            Assert.That(result, Does.Not.Contain("%2F"));
            Assert.That(result, Does.Not.Contain("%3A"));
        });
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithSpecialCharactersInStateThenEncodesCorrectly()
    {
        var clientId = "test-client-id";
        var redirectUri = "https://example.com/callback";
        var state = "state with spaces & special=chars";
        var scope = "openid";

        var result = ContaAzulApiClient.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

        Assert.That(result, Does.Contain($"state={Uri.EscapeDataString(state)}"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullClientIdThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                null!,
                "https://example.com/callback",
                "state-123",
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("clientId"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithEmptyClientIdThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                string.Empty,
                "https://example.com/callback",
                "state-123",
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("clientId"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithWhitespaceClientIdThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "   ",
                "https://example.com/callback",
                "state-123",
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("clientId"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullRedirectUriThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                null!,
                "state-123",
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("redirectUri"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithEmptyRedirectUriThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                string.Empty,
                "state-123",
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("redirectUri"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithWhitespaceRedirectUriThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "   ",
                "state-123",
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("redirectUri"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithInvalidRedirectUriThenThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "not-a-valid-url",
                "state-123",
                "openid"));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.ParamName, Is.EqualTo("redirectUri"));
            Assert.That(exception.Message, Does.Contain("valid absolute URL"));
        });
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithRelativeRedirectUriThenThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "/callback",
                "state-123",
                "openid"));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.ParamName, Is.EqualTo("redirectUri"));
            Assert.That(exception.Message, Does.Contain("valid absolute URL"));
        });
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullStateThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "https://example.com/callback",
                null!,
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("state"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithEmptyStateThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "https://example.com/callback",
                string.Empty,
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("state"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithWhitespaceStateThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "https://example.com/callback",
                "   ",
                "openid"));

        Assert.That(exception!.ParamName, Is.EqualTo("state"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullScopeThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "https://example.com/callback",
                "state-123",
                null!));

        Assert.That(exception!.ParamName, Is.EqualTo("scope"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithEmptyScopeThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "https://example.com/callback",
                "state-123",
                string.Empty));

        Assert.That(exception!.ParamName, Is.EqualTo("scope"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithWhitespaceScopeThenThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulApiClient.BuildAuthorizationUrl(
                "test-client-id",
                "https://example.com/callback",
                "state-123",
                "   "));

        Assert.That(exception!.ParamName, Is.EqualTo("scope"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithHttpsRedirectUriThenSucceeds()
    {
        var result = ContaAzulApiClient.BuildAuthorizationUrl(
            "test-client-id",
            "https://example.com/callback",
            "state-123",
            "openid");

        Assert.That(result, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithHttpRedirectUriThenSucceeds()
    {
        var result = ContaAzulApiClient.BuildAuthorizationUrl(
            "test-client-id",
            "http://localhost:3000/callback",
            "state-123",
            "openid");

        Assert.That(result, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithCustomPortInRedirectUriThenSucceeds()
    {
        var redirectUri = "https://example.com:8443/callback";

        var result = ContaAzulApiClient.BuildAuthorizationUrl(
            "test-client-id",
            redirectUri,
            "state-123",
            "openid");

        // redirect_uri é literal (sem percent-encode), inclusive a porta.
        Assert.That(result, Does.Contain($"redirect_uri={redirectUri}"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithMultipleScopesThenFormatsCorrectly()
    {
        var result = ContaAzulApiClient.BuildAuthorizationUrl(
            "test-client-id",
            "https://example.com/callback",
            "state-123",
            "openid profile email address phone");

        Assert.That(result, Does.Contain("scope=openid%20profile%20email%20address%20phone"));
    }

    #endregion
}
