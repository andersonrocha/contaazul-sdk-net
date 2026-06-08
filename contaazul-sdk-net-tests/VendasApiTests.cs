using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class VendasApiTests
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

    [Test]
    public void WhenConstructorThenCreatesInstanceWithVendasApi()
    {
        using var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.That(client.Vendas, Is.Not.Null);
    }

    // --- GetVendedoresAsync ---

    [Test]
    public async Task WhenGetVendedoresThenSendsGetToVendedoresEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "[{\"id\":\"v1\",\"nome\":\"João\",\"id_legado\":123}]");
        using var client = BuildClient(handler);

        var vendedores = await client.Vendas.GetVendedoresAsync();

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/vendedores"));
            Assert.That(vendedores, Has.Count.EqualTo(1));
            Assert.That(vendedores[0].Nome, Is.EqualTo("João"));
            Assert.That(vendedores[0].IdLegado, Is.EqualTo(123));
        });
    }

    // --- GetVendasAsync (busca) ---

    [Test]
    public async Task WhenGetVendasThenSendsGetToBuscaEndpointWithFilters()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"itens\":[],\"total_itens\":0}");
        using var client = BuildClient(handler);

        await client.Vendas.GetVendasAsync(new VendaFiltro
        {
            DataInicio = "2024-01-01",
            Situacoes = "APROVADA"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/busca"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_inicio=2024-01-01"));
            Assert.That(handler.LastUri.Query, Does.Contain("situacoes=APROVADA"));
        });
    }

    // --- GetVendaByIdAsync ---

    [Test]
    public async Task WhenGetVendaByIdThenSendsGetAndDeserializesDetail()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"cliente\":{\"uuid\":\"c1\",\"nome\":\"João\"},\"venda\":{\"numero\":1001,\"situacao\":{\"nome\":\"APROVADO\"}}}");
        using var client = BuildClient(handler);

        var venda = await client.Vendas.GetVendaByIdAsync("abc-123");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/abc-123"));
            Assert.That(venda.Cliente!.Nome, Is.EqualTo("João"));
            Assert.That(venda.Venda!.Numero, Is.EqualTo(1001));
            Assert.That(venda.Venda.Situacao!.Nome, Is.EqualTo("APROVADO"));
        });
    }

    [Test]
    public void WhenGetVendaByIdWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Vendas.GetVendaByIdAsync(" "));
    }

    // --- CriarVendaAsync ---

    [Test]
    public async Task WhenCriarVendaThenPostsBodyAndReturnsResponse()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"id\":\"new-id\",\"id_legado\":123,\"numero\":1001}");
        using var client = BuildClient(handler);

        var resposta = await client.Vendas.CriarVendaAsync(new CriacaoVendaRequest
        {
            IdCliente = "cli1",
            Numero = 1001,
            Situacao = "APROVADO",
            DataVenda = "2024-01-01",
            Itens = new List<ItemVendaRequest>
            {
                new ItemVendaRequest { Id = "prod1", Quantidade = 1, Valor = 100 }
            },
            CondicaoPagamento = new CondicaoPagamentoRequest
            {
                OpcaoCondicaoPagamento = "À vista",
                Parcelas = new List<ParcelaRequest>
                {
                    new ParcelaRequest { DataVencimento = "2024-01-01", Valor = 100 }
                }
            }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda"));
            Assert.That(handler.LastBody, Does.Contain("\"id_cliente\":\"cli1\""));
            // Optional fields left null must be omitted from the payload.
            Assert.That(handler.LastBody, Does.Not.Contain("id_categoria"));
            Assert.That(handler.LastBody, Does.Not.Contain("composicao_de_valor"));
            Assert.That(resposta.Id, Is.EqualTo("new-id"));
            Assert.That(resposta.Numero, Is.EqualTo(1001));
        });
    }

    [Test]
    public void WhenCriarVendaWithNullThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Vendas.CriarVendaAsync(null!));
    }

    // --- AtualizarVendaAsync ---

    [Test]
    public async Task WhenAtualizarVendaThenSendsPutToVendaByIdEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"id\":\"abc-123\",\"id_legado\":123}");
        using var client = BuildClient(handler);

        var resposta = await client.Vendas.AtualizarVendaAsync("abc-123", new VendaParaEdicaoRequest
        {
            IdCliente = "cli1",
            Numero = 1001,
            DataVenda = "2024-01-01",
            Situacao = "APROVADO",
            Versao = 1,
            Itens = new List<ItemVendaRequest>
            {
                new ItemVendaRequest { Id = "prod1", Quantidade = 1, Valor = 100 }
            },
            CondicaoPagamento = new CondicaoPagamentoRequest
            {
                OpcaoCondicaoPagamento = "À vista",
                Parcelas = new List<ParcelaRequest>
                {
                    new ParcelaRequest { DataVencimento = "2024-01-01", Valor = 100 }
                }
            }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Put));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/abc-123"));
            Assert.That(handler.LastBody, Does.Contain("\"versao\":1"));
            Assert.That(resposta.Id, Is.EqualTo("abc-123"));
        });
    }

    [Test]
    public void WhenAtualizarVendaWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Vendas.AtualizarVendaAsync("", new VendaParaEdicaoRequest()));
    }

    [Test]
    public void WhenAtualizarVendaWithNullBodyThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Vendas.AtualizarVendaAsync("abc-123", null!));
    }

    // --- ImprimirVendaAsync ---

    [Test]
    public async Task WhenImprimirVendaThenSendsGetToImprimirEndpointAndReturnsBytes()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "%PDF-1.4 conteudo");
        using var client = BuildClient(handler);

        var pdf = await client.Vendas.ImprimirVendaAsync("abc-123");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/abc-123/imprimir"));
            Assert.That(pdf, Is.Not.Null);
            Assert.That(pdf.Length, Is.GreaterThan(0));
            Assert.That(Encoding.UTF8.GetString(pdf), Does.StartWith("%PDF"));
        });
    }

    // --- ExcluirVendasEmLoteAsync ---

    [Test]
    public async Task WhenExcluirVendasEmLoteThenPostsIdsToExclusaoLoteEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"atualizados\":2,\"ignorados\":0}");
        using var client = BuildClient(handler);

        var resposta = await client.Vendas.ExcluirVendasEmLoteAsync(new ExclusaoLote
        {
            Ids = new List<string> { "id1", "id2" }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/exclusao-lote"));
            Assert.That(handler.LastBody, Does.Contain("\"ids\""));
            Assert.That(handler.LastBody, Does.Contain("id1"));
            Assert.That(resposta.Atualizados, Is.EqualTo(2));
            Assert.That(resposta.Ignorados, Is.EqualTo(0));
        });
    }

    [Test]
    public void WhenExcluirVendasEmLoteWithNullThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Vendas.ExcluirVendasEmLoteAsync(null!));
    }

    // --- GetItensVendaAsync ---

    [Test]
    public async Task WhenGetItensVendaThenSendsGetToItensEndpointWithPagination()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"itens\":[{\"id\":\"i1\",\"nome\":\"Produto 1\",\"tipo\":\"PRODUTO\",\"quantidade\":2,\"valor\":100}],\"itens_totais\":1}");
        using var client = BuildClient(handler);

        var itens = await client.Vendas.GetItensVendaAsync("abc-123", pagina: 2, tamanhoPagina: 50);

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/abc-123/itens"));
            Assert.That(handler.LastUri.Query, Does.Contain("pagina=2"));
            Assert.That(handler.LastUri.Query, Does.Contain("tamanho_pagina=50"));
            Assert.That(itens.ItensTotais, Is.EqualTo(1));
            Assert.That(itens.Itens, Has.Count.EqualTo(1));
            Assert.That(itens.Itens[0].Nome, Is.EqualTo("Produto 1"));
            Assert.That(itens.Itens[0].Quantidade, Is.EqualTo(2));
        });
    }

    [Test]
    public void WhenGetItensVendaWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Vendas.GetItensVendaAsync(""));
    }

    // --- GetProximoNumeroAsync ---

    [Test]
    public async Task WhenGetProximoNumeroThenSendsGetAndReturnsLong()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "4512645");
        using var client = BuildClient(handler);

        var proximo = await client.Vendas.GetProximoNumeroAsync();

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/venda/proximo-numero"));
            Assert.That(proximo, Is.EqualTo(4512645L));
        });
    }
}
