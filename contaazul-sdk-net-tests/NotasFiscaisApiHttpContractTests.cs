using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models.NotasFiscais;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class NotasFiscaisApiHttpContractTests
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

        public CapturingHandler(HttpStatusCode status, string responseBody = "")
        {
            _status = status;
            _responseBody = responseBody;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
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

    // --- ObterNotasFiscaisAsync (NF-e) ---

    [Test]
    public async Task WhenObterNotasFiscaisThenSendsGetToNotasFiscaisEndpointWithFilters()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"itens\":[{\"chave_acesso\":\"42250\",\"numero_nota\":123,\"status\":\"EMITIDA\"}]," +
            "\"paginacao\":{\"pagina_atual\":1,\"total_paginas\":1,\"tamanho_pagina\":10,\"total_itens\":1}}");
        using var client = BuildClient(handler);

        var resposta = await client.NotasFiscais.ObterNotasFiscaisAsync(new NotaFiscalFiltro
        {
            DataInicial = "2024-01-01",
            DataFinal = "2024-01-15",
            NumeroNota = "123"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/notas-fiscais"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_inicial=2024-01-01"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_final=2024-01-15"));
            Assert.That(handler.LastUri.Query, Does.Contain("numero_nota=123"));
            Assert.That(resposta.Itens, Has.Count.EqualTo(1));
            Assert.That(resposta.Itens[0].ChaveAcesso, Is.EqualTo("42250"));
            Assert.That(resposta.Paginacao.TotalItens, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task WhenPaginacaoTemInteirosComoDecimalEntaoDesserializaSemErro()
    {
        // Casos reais da API de NF-e: inteiros como decimal (ex.: 1.0) e tamanho_pagina como
        // sentinela Int64.MaxValue (sem limite). Ambos devem desserializar sem erro.
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"itens\":[],\"paginacao\":{\"pagina_atual\":1.0,\"total_paginas\":0,\"tamanho_pagina\":9223372036854775807,\"total_itens\":15.0}}");
        using var client = BuildClient(handler);

        var resposta = await client.NotasFiscais.ObterNotasFiscaisAsync(new NotaFiscalFiltro
        {
            DataInicial = "2024-01-01",
            DataFinal = "2024-01-15"
        });

        Assert.Multiple(() =>
        {
            Assert.That(resposta.Paginacao.PaginaAtual, Is.EqualTo(1));
            Assert.That(resposta.Paginacao.TotalPaginas, Is.EqualTo(0));
            Assert.That(resposta.Paginacao.TamanhoPagina, Is.EqualTo(long.MaxValue));
            Assert.That(resposta.Paginacao.TotalItens, Is.EqualTo(15));
        });
    }

    [Test]
    public void WhenObterNotasFiscaisWithNullFiltroThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.NotasFiscais.ObterNotasFiscaisAsync(null!));
    }

    [Test]
    public void WhenObterNotasFiscaisWithoutDataInicialThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.NotasFiscais.ObterNotasFiscaisAsync(new NotaFiscalFiltro { DataFinal = "2024-01-15" }));
    }

    [Test]
    public void WhenObterNotasFiscaisWithoutDataFinalThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.NotasFiscais.ObterNotasFiscaisAsync(new NotaFiscalFiltro { DataInicial = "2024-01-01" }));
    }

    // --- ObterNotasFiscaisServicoAsync (NFS-e) ---

    [Test]
    public async Task WhenObterNotasFiscaisServicoThenSendsGetToServicoEndpointWithFilters()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"itens\":[{\"id\":\"nfse-1\",\"numero_nfse\":7718,\"status\":\"EMITIDA\"}]," +
            "\"paginacao\":{\"pagina_atual\":1,\"total_paginas\":1,\"tamanho_pagina\":10,\"total_itens\":1}}");
        using var client = BuildClient(handler);

        var resposta = await client.NotasFiscais.ObterNotasFiscaisServicoAsync(new NotaFiscalServicoFiltro
        {
            DataCompetenciaDe = "2024-01-01",
            DataCompetenciaAte = "2024-01-15",
            NumeroVenda = 1001,
            Status = "EMITIDA"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/notas-fiscais-servico"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_competencia_de=2024-01-01"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_competencia_ate=2024-01-15"));
            Assert.That(handler.LastUri.Query, Does.Contain("numero_venda=1001"));
            Assert.That(handler.LastUri.Query, Does.Contain("status=EMITIDA"));
            Assert.That(resposta.Itens, Has.Count.EqualTo(1));
            Assert.That(resposta.Itens[0].NumeroNfse, Is.EqualTo(7718));
        });
    }

    [Test]
    public void WhenObterNotasFiscaisServicoWithNullFiltroThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.NotasFiscais.ObterNotasFiscaisServicoAsync(null!));
    }

    [Test]
    public void WhenObterNotasFiscaisServicoWithoutDataCompetenciaThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.NotasFiscais.ObterNotasFiscaisServicoAsync(
                new NotaFiscalServicoFiltro { DataCompetenciaAte = "2024-01-15" }));
    }

    // --- VincularNotaFiscalMdfeAsync ---

    [Test]
    public async Task WhenVincularNotaFiscalMdfeThenPostsBodyToVinculoEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        await client.NotasFiscais.VincularNotaFiscalMdfeAsync(new LinkNotaFiscalMdfe
        {
            ChavesAcesso = new List<string> { "chave1", "chave2" },
            Identificador = "345345",
            Status = "ENCERRADO"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/notas-fiscais/vinculo-mdfe"));
            Assert.That(handler.LastBody, Does.Contain("\"chaves_acesso\""));
            Assert.That(handler.LastBody, Does.Contain("\"identificador\":\"345345\""));
            Assert.That(handler.LastBody, Does.Contain("\"status\":\"ENCERRADO\""));
        });
    }

    [Test]
    public void WhenVincularNotaFiscalMdfeWithNullThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.NotasFiscais.VincularNotaFiscalMdfeAsync(null!));
    }

    [Test]
    public void WhenVincularNotaFiscalMdfeWithoutChavesThenThrowsArgumentException()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentException>(
            async () => await client.NotasFiscais.VincularNotaFiscalMdfeAsync(
                new LinkNotaFiscalMdfe { Identificador = "1" }));
    }

    [Test]
    public void WhenVincularNotaFiscalMdfeWithoutIdentificadorThenThrowsArgumentException()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentException>(
            async () => await client.NotasFiscais.VincularNotaFiscalMdfeAsync(
                new LinkNotaFiscalMdfe { ChavesAcesso = new List<string> { "chave1" } }));
    }

    // --- ObterNotaFiscalPorChaveAsync ---

    [Test]
    public async Task WhenObterNotaFiscalPorChaveThenSendsGetAndReturnsXml()
    {
        const string xml = "<?xml version=\"1.0\"?><nfeProc></nfeProc>";
        var handler = new CapturingHandler(HttpStatusCode.OK, xml);
        using var client = BuildClient(handler);

        var resultado = await client.NotasFiscais.ObterNotaFiscalPorChaveAsync("42250323643586000108550010000001151606401726");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/notas-fiscais/42250323643586000108550010000001151606401726"));
            Assert.That(resultado, Is.EqualTo(xml));
        });
    }

    [Test]
    public void WhenObterNotaFiscalPorChaveWithEmptyChaveThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.NotasFiscais.ObterNotaFiscalPorChaveAsync("  "));
    }
}
