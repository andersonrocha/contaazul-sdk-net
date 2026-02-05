using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using contaazul_dotnet;
using contaazul_dotnet.Models;
using Moq;
using Moq.Protected;

namespace contaazul_dotnet_tests;

[TestFixture]
public class TokenRefreshTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    [Test]
    public void WhenSetRefreshTokenThenRefreshTokenIsSet()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetRefreshToken("test-refresh-token");

        Assert.That(client.RefreshToken, Is.EqualTo("test-refresh-token"));
    }

    [Test]
    public void WhenSetAccessTokenAndRefreshTokenThenBothAreSet()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetAccessToken("test-access-token");
        client.SetRefreshToken("test-refresh-token");

        Assert.Multiple(() =>
        {
            Assert.That(client.AccessToken, Is.EqualTo("test-access-token"));
            Assert.That(client.RefreshToken, Is.EqualTo("test-refresh-token"));
        });
    }

    [Test]
    public void WhenSetRefreshTokenWithNullThenSetsNull()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        client.SetRefreshToken("initial-token");

        client.SetRefreshToken(null);

        Assert.That(client.RefreshToken, Is.Null);
    }

    [Test]
    public void WhenSetRefreshTokenWithEmptyStringThenSetsEmptyString()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        client.SetRefreshToken(string.Empty);

        Assert.That(client.RefreshToken, Is.EqualTo(string.Empty));
    }
}
