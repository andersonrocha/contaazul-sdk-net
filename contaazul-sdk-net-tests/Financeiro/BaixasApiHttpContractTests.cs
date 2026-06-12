using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Tests.Financeiro;

[TestFixture]
public class BaixasApiHttpContractTests
{
    [Test]
    public async Task CriarBaixa_PostaCorpoNaParcela()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"id\":\"b-1\",\"versao\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Baixas.CriarBaixaAsync("parc-1", new BaixaCriacaoRequest
        {
            DataPagamento = "2024-01-01",
            ContaFinanceira = "cf-1",
            ComposicaoValor = new ComposicaoValorFinanceiro { ValorBruto = 150 }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/parcelas/parc-1/baixa"));
            Assert.That(handler.LastBody, Does.Contain("\"valor_bruto\":150"));
            Assert.That(resp.Id, Is.EqualTo("b-1"));
            Assert.That(resp.Versao, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task ListarBaixasPorParcela_FazGetERetornaLista()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "[{\"id\":\"b-1\"},{\"id\":\"b-2\"}]");
        using var client = TestClientFactory.Build(handler);

        var baixas = await client.Baixas.ListarBaixasPorParcelaAsync("parc-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/parcelas/parc-1/baixa"));
            Assert.That(baixas, Has.Count.EqualTo(2));
            Assert.That(baixas[0].Id, Is.EqualTo("b-1"));
        });
    }

    [Test]
    public async Task ObterBaixaPorId_FazGetNoEndpointDaBaixa()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"id\":\"b-1\",\"versao\":3}");
        using var client = TestClientFactory.Build(handler);

        var baixa = await client.Baixas.ObterBaixaPorIdAsync("b-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/parcelas/baixa/b-1"));
            Assert.That(baixa.Versao, Is.EqualTo(3));
        });
    }

    [Test]
    public async Task AtualizarBaixa_FazPatchNoEndpointDaBaixa()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"id\":\"b-1\",\"versao\":2}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Baixas.AtualizarBaixaAsync("b-1", new BaixaAtualizacaoRequest { Versao = 1, Observacao = "ajuste" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod.Method, Is.EqualTo("PATCH"));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/parcelas/baixa/b-1"));
            Assert.That(handler.LastBody, Does.Contain("\"versao\":1"));
            Assert.That(resp.Versao, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task DeletarBaixa_FazDeleteNoEndpointDaBaixa()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK);
        using var client = TestClientFactory.Build(handler);

        await client.Baixas.DeletarBaixaAsync("b-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Delete));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/parcelas/baixa/b-1"));
        });
    }

    [Test]
    public void CriarBaixa_ComParcelaVaziaLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Baixas.CriarBaixaAsync(" ", new BaixaCriacaoRequest()));
    }
}
