using System;
using System.Collections.Generic;
using System.Diagnostics;
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
public class RateLimitingTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";
    private const string BaseUrl = "https://api-v2.contaazul.com";

    private static ContaAzulApiClient BuildClient(Mock<HttpMessageHandler> handler, int requestsPerSecond)
    {
        var httpClient = new HttpClient(handler.Object);
        var client = new ContaAzulApiClient(
            ClientId, ClientSecret, "access-token", "refresh-token",
            BaseUrl, httpClient);

        client.RateLimitOptions = new RateLimitOptions { RequestsPerSecond = requestsPerSecond };
        client.RetryOptions = RetryOptions.None;

        return client;
    }

    private static Mock<HttpMessageHandler> BuildOkHandler()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            });
        return handler;
    }

    // --- RateLimitOptions ---

    [Test]
    public void WhenRateLimitOptionsDefaultThenRequestsPerSecondIsTen()
    {
        var options = new RateLimitOptions();
        Assert.That(options.RequestsPerSecond, Is.EqualTo(10));
    }

    [Test]
    public void WhenRateLimitOptionsNoneThenRequestsPerSecondIsZero()
    {
        Assert.That(RateLimitOptions.None.RequestsPerSecond, Is.EqualTo(0));
    }

    // --- No throttling ---

    [Test]
    public async Task WhenRateLimitDisabledThenManyRequestsCompleteWithoutDelay()
    {
        var handler = BuildOkHandler();
        using (var client = BuildClient(handler, 0))
        {
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 10; i++)
            {
                await client.GetAsync<object>("/test");
            }
            stopwatch.Stop();

            Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromSeconds(1)));
        }
    }

    [Test]
    public async Task WhenRequestsWithinLimitThenNoThrottlingOccurs()
    {
        var handler = BuildOkHandler();
        using (var client = BuildClient(handler, 10))
        {
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < 5; i++)
            {
                await client.GetAsync<object>("/test");
            }
            stopwatch.Stop();

            // 5 requests within a limit of 10/s should not trigger any delay.
            Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromSeconds(1)));
        }
    }

    // --- Throttling ---

    [Test]
    public async Task WhenRequestsExceedRateLimitThenThrottlingIntroducesDelay()
    {
        var handler = BuildOkHandler();
        using (var client = BuildClient(handler, 2))
        {
            var stopwatch = Stopwatch.StartNew();

            // 4 requests with limit = 2/s:
            //   requests 1-2 dispatch immediately,
            //   request  3 must wait ~1 s for a slot to open,
            //   request  4 may wait another slot if the window is still full.
            for (var i = 0; i < 4; i++)
            {
                await client.GetAsync<object>("/test");
            }

            stopwatch.Stop();

            Assert.That(stopwatch.Elapsed, Is.GreaterThanOrEqualTo(TimeSpan.FromMilliseconds(800)));
        }
    }

    [Test]
    public async Task WhenConcurrentRequestsExceedRateLimitThenAllCompleteEventually()
    {
        var handler = BuildOkHandler();
        using (var client = BuildClient(handler, 2))
        {
            var tasks = new List<Task>();
            for (var i = 0; i < 4; i++)
            {
                tasks.Add(client.GetAsync<object>("/test"));
            }

            // All tasks should complete without error, just with added latency.
            Assert.DoesNotThrowAsync(async () => await Task.WhenAll(tasks));
        }
    }

    // --- Cancellation ---

    [Test]
    public void WhenCancelledDuringRateLimitWaitThenThrowsOperationCanceledException()
    {
        var handler = BuildOkHandler();
        using (var client = BuildClient(handler, 1))
        {
            // Exhaust the one-per-second slot.
            client.GetAsync<object>("/test").GetAwaiter().GetResult();

            using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50)))
                {
                    Assert.ThrowsAsync<TaskCanceledException>(
                        async () => await client.GetAsync<object>("/test", cts.Token));
                }
        }
    }
}
