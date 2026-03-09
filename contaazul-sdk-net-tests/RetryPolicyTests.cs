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
public class RetryPolicyTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";
    private const string BaseUrl = "https://api-v2.contaazul.com";

    private static ContaAzulApiClient BuildClient(Mock<HttpMessageHandler> handler, int maxRetries = 3)
    {
        var httpClient = new HttpClient(handler.Object);
        var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "access-token", "refresh-token",
            BaseUrl, httpClient);

        client.RetryOptions = new RetryOptions
        {
            MaxRetries = maxRetries,
            InitialDelay = TimeSpan.Zero,  // no real waiting in tests
            BackoffMultiplier = 2.0
        };

        return client;
    }

    private static Mock<HttpMessageHandler> BuildSequenceHandler(params HttpStatusCode[] statuses)
    {
        var callIndex = 0;
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                var status = statuses[Math.Min(callIndex, statuses.Length - 1)];
                callIndex++;
                return new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent("{}", Encoding.UTF8, "application/json")
                };
            });
        return handler;
    }

    // --- Transient errors are retried ---

    [Test]
    public async Task WhenServerReturns500ThenRetriesAndSucceeds()
    {
        var handler = BuildSequenceHandler(
            HttpStatusCode.InternalServerError,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.OK);

        using (var client = BuildClient(handler))
        {
            var result = await client.GetAsync<object>("/test");
            Assert.That(result, Is.Not.Null);
        }

        handler.Protected().Verify("SendAsync", Times.Exactly(3),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task WhenServerReturns503ThenRetriesAndSucceeds()
    {
        var handler = BuildSequenceHandler(
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.OK);

        using (var client = BuildClient(handler))
        {
            await client.GetAsync<object>("/test");
        }

        handler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task WhenServerReturns429ThenRetriesAndSucceeds()
    {
        var handler = BuildSequenceHandler(
            (HttpStatusCode)429,
            HttpStatusCode.OK);

        using (var client = BuildClient(handler))
        {
            await client.GetAsync<object>("/test");
        }

        handler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task WhenServerReturns502ThenRetriesAndSucceeds()
    {
        var handler = BuildSequenceHandler(HttpStatusCode.BadGateway, HttpStatusCode.OK);

        using (var client = BuildClient(handler))
        {
            await client.GetAsync<object>("/test");
        }

        handler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public async Task WhenServerReturns504ThenRetriesAndSucceeds()
    {
        var handler = BuildSequenceHandler(HttpStatusCode.GatewayTimeout, HttpStatusCode.OK);

        using (var client = BuildClient(handler))
        {
            await client.GetAsync<object>("/test");
        }

        handler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    // --- Non-transient errors are NOT retried ---

    [Test]
    public void WhenServerReturns404ThenDoesNotRetry()
    {
        var handler = BuildSequenceHandler(
            HttpStatusCode.NotFound,
            HttpStatusCode.OK);  // should never be reached

        using (var client = BuildClient(handler))
        {
            Assert.ThrowsAsync<ContaAzulApiException>(
                async () => await client.GetAsync<object>("/test"));
        }

        handler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public void WhenServerReturns400ThenDoesNotRetry()
    {
        var handler = BuildSequenceHandler(HttpStatusCode.BadRequest, HttpStatusCode.OK);

        using (var client = BuildClient(handler))
        {
            Assert.ThrowsAsync<ContaAzulApiException>(
                async () => await client.GetAsync<object>("/test"));
        }

        handler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    // --- Max retries exhausted ---

    [Test]
    public void WhenMaxRetriesExhaustedThenThrowsLastException()
    {
        // Always returns 500
        var handler = BuildSequenceHandler(
            HttpStatusCode.InternalServerError,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.InternalServerError);  // 1 initial + 3 retries

        using (var client = BuildClient(handler, maxRetries: 3))
        {
            var ex = Assert.ThrowsAsync<ContaAzulApiException>(
                async () => await client.GetAsync<object>("/test"));

            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        handler.Protected().Verify("SendAsync", Times.Exactly(4),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [Test]
    public void WhenRetryOptionsNoneThenNoRetry()
    {
        var handler = BuildSequenceHandler(HttpStatusCode.InternalServerError, HttpStatusCode.OK);

        var httpClient = new HttpClient(handler.Object);
        using (var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null, BaseUrl, httpClient))
        {
            client.RetryOptions = RetryOptions.None;

            Assert.ThrowsAsync<ContaAzulApiException>(
                async () => await client.GetAsync<object>("/test"));
        }

        handler.Protected().Verify("SendAsync", Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    // --- Cancellation ---

    [Test]
    public void WhenCancelledDuringRetryThenThrowsOperationCanceledException()
    {
        using (var cts = new CancellationTokenSource())
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
                    if (callCount == 2) cts.Cancel();
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent("{}", Encoding.UTF8, "application/json")
                    };
                });

            var httpClient = new HttpClient(handler.Object);
            using (var client = new ContaAzulApiClient(
                ClientId, ClientSecret, "token", null, BaseUrl, httpClient))
            {
                client.RetryOptions = new RetryOptions
                {
                    MaxRetries = 5,
                    InitialDelay = TimeSpan.Zero
                };

                Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await client.GetAsync<object>("/test", cts.Token));
            }
        }
    }

    // --- Delay calculation ---

    [TestCase(0, 1000)]
    [TestCase(1, 2000)]
    [TestCase(2, 4000)]
    public void WhenDelayCalculatedThenExponentialBackoff(int attempt, double expectedMs)
    {
        var options = new RetryOptions
        {
            InitialDelay = TimeSpan.FromSeconds(1),
            BackoffMultiplier = 2.0,
            MaxDelay = TimeSpan.FromMinutes(1)
        };

        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        client.RetryOptions = options;

        // Use reflection to access the private CalculateDelay method
        var method = typeof(ContaAzulApiClient).GetMethod(
            "CalculateDelay",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var result = (TimeSpan)method.Invoke(client, new object[] { attempt });

        Assert.That(result.TotalMilliseconds, Is.EqualTo(expectedMs));
    }

    [Test]
    public void WhenDelayExceedsMaxDelayThenCappedAtMaxDelay()
    {
        var options = new RetryOptions
        {
            InitialDelay = TimeSpan.FromSeconds(10),
            BackoffMultiplier = 10.0,
            MaxDelay = TimeSpan.FromSeconds(5)
        };

        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        client.RetryOptions = options;

        var method = typeof(ContaAzulApiClient).GetMethod(
            "CalculateDelay",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var result = (TimeSpan)method.Invoke(client, new object[] { 0 });

        Assert.That(result, Is.EqualTo(TimeSpan.FromSeconds(5)));
    }
}
