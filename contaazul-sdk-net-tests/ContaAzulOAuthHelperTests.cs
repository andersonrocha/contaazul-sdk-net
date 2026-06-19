using System;
using ContaAzul.Sdk.Net;

namespace ContaAzul.Sdk.Net.Tests;

/// <summary>
/// Tests for <see cref="ContaAzulOAuthHelper.BuildAuthorizationUrl"/>.
/// Mirrors the BuildAuthorizationUrl tests in ContaAzulApiClientTests to verify that
/// both entry points produce identical results, and that ContaAzulApiClient.BuildAuthorizationUrl
/// correctly delegates to ContaAzulOAuthHelper.
/// </summary>
[TestFixture]
public class ContaAzulOAuthHelperTests
{
    [Test]
    public void WhenBuildAuthorizationUrlWithValidParametersThenReturnsCorrectUrl()
    {
        var clientId = "3d0hgi6c523vj9u4t47h029asg";
        var redirectUri = "https://www.contaazul.com/";
        var state = "546512316845316541";
        var scope = "openid profile aws.cognito.signin.user.admin";

        var result = ContaAzulOAuthHelper.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.StartWith("https://auth.contaazul.com/oauth2/authorize?"));
            Assert.That(result, Does.Contain("response_type=code"));
            Assert.That(result, Does.Contain($"client_id={Uri.EscapeDataString(clientId)}"));
            Assert.That(result, Does.Contain($"redirect_uri={redirectUri}"), "redirect_uri deve ser literal (sem percent-encode).");
            Assert.That(result, Does.Contain($"state={Uri.EscapeDataString(state)}"));
            Assert.That(result, Does.Contain("scope=openid%20profile%20aws.cognito.signin.user.admin"));
        });
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithScopeSpacesThenEncodesWithPercentTwenty()
    {
        var result = ContaAzulOAuthHelper.BuildAuthorizationUrl(
            "test-client", "https://example.com/cb", "state", "openid profile email");

        Assert.That(result, Does.Contain("scope=openid%20profile%20email"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullClientIdThenThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulOAuthHelper.BuildAuthorizationUrl(null!, "https://ex.com/cb", "s", "openid"));

        Assert.That(ex!.ParamName, Is.EqualTo("clientId"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullRedirectUriThenThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulOAuthHelper.BuildAuthorizationUrl("id", null!, "s", "openid"));

        Assert.That(ex!.ParamName, Is.EqualTo("redirectUri"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithInvalidRedirectUriThenThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            ContaAzulOAuthHelper.BuildAuthorizationUrl("id", "not-a-url", "s", "openid"));

        Assert.Multiple(() =>
        {
            Assert.That(ex!.ParamName, Is.EqualTo("redirectUri"));
            Assert.That(ex.Message, Does.Contain("valid absolute URL"));
        });
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullStateThenThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulOAuthHelper.BuildAuthorizationUrl("id", "https://ex.com/cb", null!, "openid"));

        Assert.That(ex!.ParamName, Is.EqualTo("state"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithNullScopeThenThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
            ContaAzulOAuthHelper.BuildAuthorizationUrl("id", "https://ex.com/cb", "s", null!));

        Assert.That(ex!.ParamName, Is.EqualTo("scope"));
    }

    [Test]
    public void WhenBuildAuthorizationUrlViaContaAzulApiClientThenProducesSameResultAsHelper()
    {
        // M3: ContaAzulApiClient.BuildAuthorizationUrl must delegate to ContaAzulOAuthHelper —
        // both entry points must always produce byte-for-byte identical output.
        var clientId = "test-client-id";
        var redirectUri = "https://example.com/callback";
        var state = "random-state-123";
        var scope = "openid profile";

        var viaClient = ContaAzulApiClient.BuildAuthorizationUrl(clientId, redirectUri, state, scope);
        var viaHelper = ContaAzulOAuthHelper.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

        Assert.That(viaClient, Is.EqualTo(viaHelper));
    }

    [Test]
    public void WhenBuildAuthorizationUrlWithMultipleScopesThenFormatsCorrectly()
    {
        var result = ContaAzulOAuthHelper.BuildAuthorizationUrl(
            "client",
            "https://example.com/callback",
            "state",
            "openid profile email address phone");

        Assert.That(result, Does.Contain("scope=openid%20profile%20email%20address%20phone"));
    }
}
