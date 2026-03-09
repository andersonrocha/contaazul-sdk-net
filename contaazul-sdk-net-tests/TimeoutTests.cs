using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using Moq;
using Moq.Protected;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class TimeoutTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";
    private const string BaseUrl = "https://api-v2.contaazul.com";
    private const string AuthBaseUrl = "https://auth.contaazul.com";

    private static Mock<HttpMessageHandler> BuildOkHandler(string body = "{}") 
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            });
        return handler;
    }

    private static Mock<HttpMessageHandler> BuildSlowHandler(TimeSpan delay)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>(async (req, ct) =>
            {
                await Task.Delay(delay, ct);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}", Encoding.UTF8, "application/json")
                };
            });
        return handler;
    }

    // --- HttpOptions padrão ---

    [Test]
    public void WhenHttpOptionsDefaultThenTimeoutIs30Seconds()
    {
        Assert.That(new HttpOptions().DefaultTimeout, Is.EqualTo(TimeSpan.FromSeconds(30)));
    }

    [Test]
    public void WhenClientCreatedWithoutHttpOptionsThenNoException()
    {
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret))
        {
            Assert.That(client, Is.Not.Null);
        }
    }

    [Test]
    public void WhenClientCreatedWithHttpOptionsThenNoException()
    {
        var httpOptions = new HttpOptions { DefaultTimeout = TimeSpan.FromSeconds(15) };
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, httpOptions: httpOptions))
        {
            Assert.That(client, Is.Not.Null);
        }
    }

    // --- Timeout aplicado ao HttpClient da API ---

    [Test]
    public void WhenHttpOptionsProvidedThenTimeoutAppliedToApiClient()
    {
        var httpOptions = new HttpOptions { DefaultTimeout = TimeSpan.FromSeconds(5) };
        var apiClient = new HttpClient(BuildOkHandler().Object);

        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            "token", null, BaseUrl, apiClient, default, null, null, httpOptions))
        {
            Assert.That(apiClient.Timeout, Is.EqualTo(TimeSpan.FromSeconds(5)));
        }
    }

    [Test]
    public void WhenHttpOptionsNullThenApiClientTimeoutIsNotChanged()
    {
        var apiClient = new HttpClient(BuildOkHandler().Object)
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            "token", null, BaseUrl, apiClient))
        {
            Assert.That(apiClient.Timeout, Is.EqualTo(TimeSpan.FromSeconds(60)));
        }
    }

    // --- Timeout aplicado ao HttpClient de autenticação ---

    [Test]
    public void WhenHttpOptionsProvidedThenTimeoutAppliedToAuthClient()
    {
        var httpOptions = new HttpOptions { DefaultTimeout = TimeSpan.FromSeconds(10) };
        var authClient = new HttpClient(BuildOkHandler().Object)
        {
            BaseAddress = new Uri(AuthBaseUrl)
        };

        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            authHttpClient: authClient, httpOptions: httpOptions))
        {
            Assert.That(authClient.Timeout, Is.EqualTo(TimeSpan.FromSeconds(10)));
        }
    }

    [Test]
    public void WhenHttpOptionsNullThenAuthClientTimeoutIsNotChanged()
    {
        var authClient = new HttpClient(BuildOkHandler().Object)
        {
            BaseAddress = new Uri(AuthBaseUrl),
            Timeout = TimeSpan.FromSeconds(45)
        };

        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            authHttpClient: authClient))
        {
            Assert.That(authClient.Timeout, Is.EqualTo(TimeSpan.FromSeconds(45)));
        }
    }

    // --- Comportamento funcional: timeout dispara em requisição lenta ---

    [Test]
    public void WhenApiRequestExceedsTimeoutThenThrowsTaskCanceledException()
    {
        var slowHandler = BuildSlowHandler(TimeSpan.FromSeconds(10));
        var httpOptions = new HttpOptions { DefaultTimeout = TimeSpan.FromMilliseconds(100) };

        using (var apiClient = new HttpClient(slowHandler.Object))
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            "token", null, BaseUrl, apiClient, default, null, null, httpOptions))
        {
            client.RetryOptions = RetryOptions.None;
            Assert.ThrowsAsync<TaskCanceledException>(
                async () => await client.GetAsync<object>("/test"));
        }
    }

    [Test]
    public void WhenAuthRequestExceedsTimeoutThenThrowsTaskCanceledException()
    {
        var slowAuthHandler = BuildSlowHandler(TimeSpan.FromSeconds(10));
        var httpOptions = new HttpOptions { DefaultTimeout = TimeSpan.FromMilliseconds(100) };

        using (var authClient = new HttpClient(slowAuthHandler.Object) { BaseAddress = new Uri(AuthBaseUrl) })
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret,
            authHttpClient: authClient, httpOptions: httpOptions))
        {
            Assert.ThrowsAsync<TaskCanceledException>(
                async () => await client.AuthorizeAsync("code", "https://app.example.com/callback"));
        }
    }
}
