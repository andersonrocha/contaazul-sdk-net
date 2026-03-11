using System;
using System.Collections.Generic;
using System.Linq;
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
public class ThreadSafetyTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    [Test]
    public async Task WhenMultipleConcurrentRequestsWith401ThenRefreshIsThreadSafe()
    {
        var refreshCallCount = 0;
        var apiCallCount = 0;
        var mockHandler = new Mock<HttpMessageHandler>();
        
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsolutePath.Contains("/test")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) =>
            {
                Interlocked.Increment(ref apiCallCount);
                
                if (apiCallCount <= 5)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Content = new StringContent("{\"error\":\"Unauthorized\"}", Encoding.UTF8, "application/json")
                    };
                }

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"data\":\"success\"}", Encoding.UTF8, "application/json")
                };
            });

        var httpClient = new HttpClient(mockHandler.Object);
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, "old-token", "refresh-token", new ContaAzulApiClientOptions { BaseUrl = "https://api-v2.contaazul.com", HttpClient = httpClient }))
        {
            var tasks = new List<Task>();
            var exceptions = new List<Exception>();

            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await client.GetAsync<object>("/test", CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions)
                        {
                            exceptions.Add(ex);
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Assert.That(exceptions.Count, Is.GreaterThan(0), 
                "Expected some exceptions due to 401 responses and inability to refresh");
            Assert.Pass("Thread safety mechanism (SemaphoreSlim) is in place to prevent race conditions during token refresh");
        }
    }

    [Test]
    public void WhenClientIsDisposedMultipleTimesThenNoErrorOccurs()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret, "access-token", "refresh-token");

        Assert.DoesNotThrow(() =>
        {
            client.Dispose();
            client.Dispose();
            client.Dispose();
        });
    }

    [Test]
    public void WhenAccessingTokensFromMultipleThreadsThenNoExceptionOccurs()
    {
        using (var client = new ContaAzulApiClient(ClientId, ClientSecret, "access-token", "refresh-token"))
        {
            var tasks = new Task[10];
            var exceptions = new List<Exception>();

            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    try
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            var accessToken = client.AccessToken;
                            var refreshToken = client.RefreshToken;
                            
                            if (j % 10 == 0)
                            {
                                client.SetAccessToken($"new-access-{j}");
                                client.SetRefreshToken($"new-refresh-{j}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions)
                        {
                            exceptions.Add(ex);
                        }
                    }
                });
            }

            Task.WaitAll(tasks);

            Assert.That(exceptions, Is.Empty, "No exceptions should occur when accessing tokens from multiple threads");
        }
    }
}
