using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.Orcamentos;

namespace ContaAzul.Sdk.Net.Tests.Orcamentos;

[TestFixture]
public class OrcamentosApiHttpContractTests
{
    [Test]
    public async Task ObterOrcamentos_FazGetComFiltros()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"itens\":[{\"id\":\"o-1\",\"numero\":1001,\"situacao\":\"ORCAMENTO\",\"total\":1000," +
            "\"cliente\":{\"id\":\"c-1\",\"nome\":\"Cliente X\"}}],\"total_itens\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Orcamentos.ObterOrcamentosAsync(new OrcamentoFiltro
        {
            TermoBusca = "proposta",
            CampoOrdenadoAscendente = "DATA",
            Situacoes = "ORCAMENTO,ORCAMENTO_ACEITO"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/orcamentos"));
            Assert.That(handler.LastUri.Query, Does.Contain("termo_busca=proposta"));
            Assert.That(handler.LastUri.Query, Does.Contain("campo_ordenado_ascendente=DATA"));
            Assert.That(handler.LastUri.Query, Does.Contain("situacoes=ORCAMENTO%2CORCAMENTO_ACEITO"));
            Assert.That(handler.LastUri.Query, Does.Contain("pagina=1"));
            Assert.That(resp.Itens, Has.Count.EqualTo(1));
            Assert.That(resp.Itens![0].Cliente!.Nome, Is.EqualTo("Cliente X"));
            Assert.That(resp.TotalItens, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task ObterOrcamentoPorId_FazGetNoEndpointDoOrcamento()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"id\":\"o-1\",\"numero\":1,\"situacao\":\"ORCAMENTO\"," +
            "\"composicao_de_valor\":{\"desconto\":{\"tipo\":\"VALOR\",\"valor\":10},\"frete\":5}," +
            "\"itens\":[{\"id\":\"i-1\",\"nome\":\"Produto 01\",\"quantidade\":1,\"tipo\":\"PRODUTO\",\"valor\":10}]}");
        using var client = TestClientFactory.Build(handler);

        var orcamento = await client.Orcamentos.ObterOrcamentoPorIdAsync("o-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/orcamentos/o-1"));
            Assert.That(orcamento.Situacao, Is.EqualTo("ORCAMENTO"));
            Assert.That(orcamento.ComposicaoDeValor!.Frete, Is.EqualTo(5m));
            Assert.That(orcamento.ComposicaoDeValor.Desconto!.Tipo, Is.EqualTo("VALOR"));
            Assert.That(orcamento.Itens, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void ObterOrcamentoPorId_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Orcamentos.ObterOrcamentoPorIdAsync(" "));
    }

    [Test]
    public async Task CriarOrcamento_PostaCorpoNoEndpointDeOrcamentos()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.Created, "{\"id\":\"o-1\"}");
        using var client = TestClientFactory.Build(handler);

        var resumo = await client.Orcamentos.CriarOrcamentoAsync(new CriarOrcamento
        {
            DataOrcamento = "2026-05-01",
            DataValidade = "2026-05-15",
            IdCliente = "cli-1",
            ComposicaoDeValor = new ComposicaoValorOrcamento
            {
                Frete = 5,
                Desconto = new Desconto { Tipo = "VALOR", Valor = 10 }
            },
            Itens = new List<CriarItemOrcamento>
            {
                new CriarItemOrcamento { Id = "item-1", Quantidade = 1, Valor = 10, ValorCusto = 8 }
            }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/orcamentos"));
            Assert.That(handler.LastBody, Does.Contain("\"id_cliente\":\"cli-1\""));
            Assert.That(handler.LastBody, Does.Contain("\"data_orcamento\":\"2026-05-01\""));
            Assert.That(handler.LastBody, Does.Contain("\"frete\":5"));
            Assert.That(handler.LastBody, Does.Contain("\"id\":\"item-1\""));
            Assert.That(resumo.Id, Is.EqualTo("o-1"));
        });
    }

    [Test]
    public void CriarOrcamento_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Orcamentos.CriarOrcamentoAsync(null!));
    }

    [Test]
    public async Task ExcluirOrcamentosEmLote_FazDeleteComCorpo()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.NoContent);
        using var client = TestClientFactory.Build(handler);

        await client.Orcamentos.ExcluirOrcamentosEmLoteAsync(new ExclusaoLoteOrcamento
        {
            Ids = new List<string> { "7d7c9d4a-27aa-457e-b981-2df4c81970f7", "c44e254d-0040-46e2-bccf-6898d0981201" }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Delete));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/orcamentos"));
            Assert.That(handler.LastBody, Does.Contain("\"ids\":[\"7d7c9d4a-27aa-457e-b981-2df4c81970f7\",\"c44e254d-0040-46e2-bccf-6898d0981201\"]"));
        });
    }

    [Test]
    public void ExcluirOrcamentosEmLote_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Orcamentos.ExcluirOrcamentosEmLoteAsync(null!));
    }

    [Test]
    public void ExcluirOrcamentosEmLote_ComIdsVaziosLancaArgumentException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Orcamentos.ExcluirOrcamentosEmLoteAsync(new ExclusaoLoteOrcamento { Ids = new List<string>() }));
    }
}
