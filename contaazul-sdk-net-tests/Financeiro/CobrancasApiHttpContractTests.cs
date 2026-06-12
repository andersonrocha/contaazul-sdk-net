using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Tests.Financeiro;

[TestFixture]
public class CobrancasApiHttpContractTests
{
    [Test]
    public async Task CriarCobranca_PostaCorpoNoEndpointDeGerarCobranca()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"id\":\"cob-1\",\"url\":\"http://x\",\"status\":\"REGISTRADO\"}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Cobrancas.CriarCobrancaAsync(new GerarCobrancaRequest
        {
            ContaBancaria = "cb-1",
            DescricaoFatura = "Fatura #1",
            IdParcela = "p-1",
            DataVencimento = "2024-01-10",
            Tipo = "BOLETO"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/contas-a-receber/gerar-cobranca"));
            Assert.That(handler.LastBody, Does.Contain("\"conta_bancaria\":\"cb-1\""));
            Assert.That(handler.LastBody, Does.Contain("\"tipo\":\"BOLETO\""));
            Assert.That(resp.Id, Is.EqualTo("cob-1"));
            Assert.That(resp.Status, Is.EqualTo("REGISTRADO"));
        });
    }

    [Test]
    public void CriarCobranca_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Cobrancas.CriarCobrancaAsync(null!));
    }

    [Test]
    public async Task ObterCobrancaPorId_FazGetNoEndpointDaCobranca()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"id\":\"cob-1\",\"status\":\"PAGO\"}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Cobrancas.ObterCobrancaPorIdAsync("cob-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/contas-a-receber/cobranca/cob-1"));
            Assert.That(resp.Status, Is.EqualTo("PAGO"));
        });
    }

    [Test]
    public async Task DeletarCobrancaPorId_FazDeleteNoEndpointDaCobranca()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK);
        using var client = TestClientFactory.Build(handler);

        await client.Cobrancas.DeletarCobrancaPorIdAsync("cob-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Delete));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/contas-a-receber/cobranca/cob-1"));
        });
    }

    [Test]
    public void ObterCobrancaPorId_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Cobrancas.ObterCobrancaPorIdAsync(" "));
    }
}
