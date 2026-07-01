using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;

namespace ContaAzul.Sdk.Net.Tests;

/// <summary>
/// Handler HTTP compartilhado que captura a requisição (método, URI e corpo) e devolve uma
/// resposta fixa. Reutilizado pelos testes de contrato das áreas que não têm helper próprio.
/// </summary>
internal sealed class CapturingHttpHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _status;
    private readonly string _body;

    public HttpMethod LastMethod { get; private set; }
    public Uri LastUri { get; private set; }
    public string LastBody { get; private set; }

    public CapturingHttpHandler(HttpStatusCode status = HttpStatusCode.OK, string body = "")
    {
        _status = status;
        _body = body;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastMethod = request.Method;
        LastUri = request.RequestUri;
        LastBody = request.Content == null ? null : await request.Content.ReadAsStringAsync().ConfigureAwait(false);

        return new HttpResponseMessage
        {
            StatusCode = _status,
            Content = new StringContent(_body ?? string.Empty, Encoding.UTF8, "application/json")
        };
    }
}

internal static class TestClientFactory
{
    private const string BaseUrl = "https://api-v2.contaazul.com";

    public static ContaAzulApiClient Build(CapturingHttpHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) };
        return new ContaAzulApiClient(
            "test-client-id", "test-client-secret", "token", null,
            new ContaAzulApiClientOptions { BaseUrl = BaseUrl, HttpClient = httpClient });
    }
}
