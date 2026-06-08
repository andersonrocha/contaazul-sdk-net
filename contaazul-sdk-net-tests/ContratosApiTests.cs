using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class ContratosApiTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";
    private const string ApiBaseUrl = "https://api-v2.contaazul.com";

    private sealed class CapturingHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _status;
        private readonly string _responseBody;

        public HttpMethod LastMethod { get; private set; }
        public Uri LastUri { get; private set; }
        public string LastBody { get; private set; }
        public int CallCount { get; private set; }

        public CapturingHandler(HttpStatusCode status, string responseBody = "")
        {
            _status = status;
            _responseBody = responseBody;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            LastMethod = request.Method;
            LastUri = request.RequestUri;
            LastBody = request.Content == null
                ? null
                : await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new HttpResponseMessage
            {
                StatusCode = _status,
                Content = new StringContent(_responseBody ?? string.Empty, Encoding.UTF8, "application/json")
            };
        }
    }

    private static ContaAzulApiClient BuildClient(CapturingHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(ApiBaseUrl) };
        return new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            new ContaAzulApiClientOptions { BaseUrl = ApiBaseUrl, HttpClient = httpClient });
    }

    [Test]
    public void WhenConstructorThenCreatesInstanceWithContratosApi()
    {
        using var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.That(client.Contratos, Is.Not.Null);
    }

    // --- ListarContratosAsync ---

    [Test]
    public async Task WhenListarContratosThenSendsGetToContratosEndpointWithFilters()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"itens\":[{\"id\":\"c1\",\"numero\":1014}],\"itens_totais\":1}");
        using var client = BuildClient(handler);

        var resposta = await client.Contratos.ListarContratosAsync(new ContratoFiltro
        {
            DataInicio = "2026-08-15",
            DataFim = "2027-08-15",
            Status = "ATIVO"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/contratos"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_inicio=2026-08-15"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_fim=2027-08-15"));
            Assert.That(handler.LastUri.Query, Does.Contain("status=ATIVO"));
            Assert.That(handler.LastUri.Query, Does.Contain("pagina=1"));
            Assert.That(resposta.ItensTotais, Is.EqualTo(1));
            Assert.That(resposta.Itens, Has.Count.EqualTo(1));
            Assert.That(resposta.Itens[0].Id, Is.EqualTo("c1"));
            Assert.That(resposta.Itens[0].Numero, Is.EqualTo(1014));
        });
    }

    [Test]
    public void WhenListarContratosWithNullFiltroThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Contratos.ListarContratosAsync(null!));
    }

    [Test]
    public void WhenListarContratosWithoutDataInicioThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Contratos.ListarContratosAsync(new ContratoFiltro { DataFim = "2027-08-15" }));
    }

    [Test]
    public void WhenListarContratosWithoutDataFimThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Contratos.ListarContratosAsync(new ContratoFiltro { DataInicio = "2026-08-15" }));
    }

    // --- ObterContratoPorIdAsync ---

    [Test]
    public async Task WhenObterContratoPorIdThenSendsGetToContratoByIdEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"id\":\"abc-123\",\"status\":\"ATIVO\",\"cliente\":{\"id\":\"cli1\",\"nome\":\"João\"}}");
        using var client = BuildClient(handler);

        var contrato = await client.Contratos.ObterContratoPorIdAsync("abc-123");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/contratos/abc-123"));
            Assert.That(contrato.Id, Is.EqualTo("abc-123"));
            Assert.That(contrato.Status, Is.EqualTo("ATIVO"));
            Assert.That(contrato.Cliente!.Nome, Is.EqualTo("João"));
        });
    }

    [Test]
    public void WhenObterContratoPorIdWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Contratos.ObterContratoPorIdAsync("  "));
    }

    // --- ObterProximoNumeroAsync ---

    [Test]
    public async Task WhenObterProximoNumeroThenSendsGetAndReturnsInteger()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "7");
        using var client = BuildClient(handler);

        var proximo = await client.Contratos.ObterProximoNumeroAsync();

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/contratos/proximo-numero"));
            Assert.That(proximo, Is.EqualTo(7));
        });
    }

    // --- CriarContratoAsync ---

    [Test]
    public async Task WhenCriarContratoThenPostsBodyAndReturnsResumo()
    {
        var handler = new CapturingHandler(HttpStatusCode.Created,
            "{\"id\":\"new-id\",\"id_legado\":12345,\"id_venda\":\"venda-1\"}");
        using var client = BuildClient(handler);

        var resumo = await client.Contratos.CriarContratoAsync(new CriarContrato
        {
            IdCliente = "cli1",
            CondicaoPagamento = new CriarCondicaoPagamentoContrato
            {
                DiaVencimento = 10,
                PrimeiraDataVencimento = "2025-01-10",
                TipoPagamento = "BOLETO_BANCARIO"
            },
            Itens = new System.Collections.Generic.List<CriarItemVendaContrato>
            {
                new CriarItemVendaContrato { Id = "prod1", Quantidade = 2, Valor = 100.5m }
            },
            Termos = new CriarTermosContrato
            {
                DataInicio = "2025-01-01",
                DataFim = "2025-12-31",
                DiaEmissaoVenda = 5,
                IntervaloFrequencia = 1,
                Numero = 12,
                TipoExpiracao = "DATA",
                TipoFrequencia = "MENSAL"
            }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/contratos"));
            Assert.That(handler.LastBody, Does.Contain("\"id_cliente\":\"cli1\""));
            Assert.That(handler.LastBody, Does.Contain("\"condicao_pagamento\""));
            // Optional fields left null must be omitted from the payload.
            Assert.That(handler.LastBody, Does.Not.Contain("id_categoria"));
            Assert.That(handler.LastBody, Does.Not.Contain("observacoes"));
            Assert.That(resumo.Id, Is.EqualTo("new-id"));
            Assert.That(resumo.IdLegado, Is.EqualTo(12345));
            Assert.That(resumo.IdVenda, Is.EqualTo("venda-1"));
        });
    }

    [Test]
    public void WhenCriarContratoWithNullThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.Created, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Contratos.CriarContratoAsync(null!));
    }

    // --- RemoverContratoAsync ---

    [Test]
    public async Task WhenRemoverContratoThenSendsDeleteToContratoByIdEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        await client.Contratos.RemoverContratoAsync("abc-123");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Delete));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/contratos/abc-123"));
        });
    }

    [Test]
    public void WhenRemoverContratoWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Contratos.RemoverContratoAsync(""));
    }

    // --- EncerrarContratoAsync ---

    [Test]
    public async Task WhenEncerrarContratoThenSendsPostWithoutBodyToEncerrarEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        await client.Contratos.EncerrarContratoAsync("abc-123");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/contratos/abc-123/encerrar"));
            Assert.That(handler.LastBody, Is.Null);
        });
    }

    [Test]
    public void WhenEncerrarContratoWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Contratos.EncerrarContratoAsync(null!));
    }
}
