using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Tests.Financeiro;

[TestFixture]
public class FinanceiroApiHttpContractTests
{
    // --- Centros de custo ---

    [Test]
    public async Task ObterCentrosDeCusto_FazGetComFiltros()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"itens_totais\":1,\"items\":[{\"id\":\"cc-1\",\"nome\":\"Contabilidade\"}],\"totais\":{\"ativo\":1,\"inativo\":0,\"todos\":1}}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.ObterCentrosDeCustoAsync(new CentroDeCustoFiltro { FiltroRapido = "ATIVO", Busca = "Cont" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/centro-de-custo"));
            Assert.That(handler.LastUri.Query, Does.Contain("filtro_rapido=ATIVO"));
            Assert.That(handler.LastUri.Query, Does.Contain("busca=Cont"));
            Assert.That(resp.Items, Has.Count.EqualTo(1));
            Assert.That(resp.Totais!.Ativo, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task CriarCentroDeCusto_PostaCorpo()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"id\":\"cc-1\",\"nome\":\"Novo\",\"ativo\":true}");
        using var client = TestClientFactory.Build(handler);

        var cc = await client.Financeiro.CriarCentroDeCustoAsync(new CriarCentroDeCustoRequest { Nome = "Novo", Codigo = "010" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/centro-de-custo"));
            Assert.That(handler.LastBody, Does.Contain("\"codigo\":\"010\""));
            Assert.That(cc.Id, Is.EqualTo("cc-1"));
        });
    }

    [Test]
    public void CriarCentroDeCusto_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Financeiro.CriarCentroDeCustoAsync(null!));
    }

    // --- Categorias ---

    [Test]
    public async Task ObterCategorias_FazGetEDesserializa()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"itens_totais\":1,\"itens\":[{\"id\":\"c-1\",\"nome\":\"Eletrônicos\",\"tipo\":\"RECEITA\"}]}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.ObterCategoriasAsync(new CategoriaFiltro { Tipo = "RECEITA", PermiteApenasFilhos = true });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/categorias"));
            Assert.That(handler.LastUri.Query, Does.Contain("permite_apenas_filhos=true"));
            Assert.That(resp.Itens, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public async Task ObterCategoriasDre_FazGet()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"itens\":[{\"id\":\"dre-1\",\"descricao\":\"x\"}]}");
        using var client = TestClientFactory.Build(handler);

        var dre = await client.Financeiro.ObterCategoriasDreAsync();

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/categorias-dre"));
            Assert.That(dre.Itens, Has.Count.EqualTo(1));
        });
    }

    // --- Contas financeiras ---

    [Test]
    public async Task ObterContasFinanceiras_FazGetComTipos()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"itens_totais\":1,\"itens\":[{\"id\":\"cf-1\",\"nome\":\"Conta\",\"tipo\":\"CONTA_CORRENTE\"}]}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.ObterContasFinanceirasAsync(new ContaFinanceiraFiltro { Tipos = "CONTA_CORRENTE", ApenasAtivo = true });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/conta-financeira"));
            Assert.That(handler.LastUri.Query, Does.Contain("tipos=CONTA_CORRENTE"));
            Assert.That(handler.LastUri.Query, Does.Contain("apenas_ativo=true"));
            Assert.That(resp.Itens[0].Id, Is.EqualTo("cf-1"));
        });
    }

    [Test]
    public async Task ObterSaldoAtual_FazGetNoEndpointDeSaldo()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"saldo_atual\":1000.36}");
        using var client = TestClientFactory.Build(handler);

        var saldo = await client.Financeiro.ObterSaldoAtualAsync("cf-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/conta-financeira/cf-1/saldo-atual"));
            Assert.That(saldo.SaldoAtual, Is.EqualTo(1000.36m));
        });
    }

    [Test]
    public void ObterSaldoAtual_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Financeiro.ObterSaldoAtualAsync(""));
    }

    // --- Transferências ---

    [Test]
    public async Task ObterTransferencias_FazGet()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"itens_totais\":0,\"itens\":[]}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.ObterTransferenciasAsync(new TransferenciaFiltro { DataInicio = "2026-01-01", DataFim = "2026-12-31" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/transferencias"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_inicio=2026-01-01"));
            Assert.That(resp.Itens, Is.Not.Null);
        });
    }

    // --- Eventos (criação) ---

    [Test]
    public async Task CriarContaAReceber_PostaEventoNoEndpointCorreto()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.Accepted, "{\"protocolId\":\"pr-1\",\"status\":\"SUCCESS\"}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.CriarContaAReceberAsync(new EventoFinanceiroRequest
        {
            DataCompetencia = "2024-07-15",
            Valor = 100,
            Contato = "ct-1",
            ContaFinanceira = "cf-1"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/contas-a-receber"));
            Assert.That(resp.ProtocolId, Is.EqualTo("pr-1"));
        });
    }

    [Test]
    public async Task CriarContaAPagar_PostaEventoNoEndpointCorreto()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.Accepted, "{\"protocolId\":\"pr-2\",\"status\":\"PENDING\"}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.CriarContaAPagarAsync(new EventoFinanceiroRequest { DataCompetencia = "2024-07-15", Valor = 50 });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/contas-a-pagar"));
            Assert.That(resp.Status, Is.EqualTo("PENDING"));
        });
    }

    // --- Buscas de contas a receber/pagar ---

    [Test]
    public async Task ObterContasAReceber_FazGetComVencimento()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"itens_totais\":1,\"itens\":[{\"id\":\"c-1\",\"cliente\":{\"id\":\"cli-1\",\"nome\":\"X\"}}]}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.ObterContasAReceberAsync(new MovimentacaoFinanceiraFiltro
        {
            DataVencimentoDe = "2027-08-15",
            DataVencimentoAte = "2027-08-20"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/contas-a-receber/buscar"));
            Assert.That(handler.LastUri.Query, Does.Contain("data_vencimento_de=2027-08-15"));
            Assert.That(resp.Itens[0].Cliente!.Nome, Is.EqualTo("X"));
        });
    }

    [Test]
    public async Task ObterContasAPagar_FazGetComVencimento()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"itens_totais\":0,\"itens\":[]}");
        using var client = TestClientFactory.Build(handler);

        await client.Financeiro.ObterContasAPagarAsync(new MovimentacaoFinanceiraFiltro
        {
            DataVencimentoDe = "2027-08-15",
            DataVencimentoAte = "2027-08-20"
        });

        Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/contas-a-pagar/buscar"));
    }

    [Test]
    public void ObterContasAReceber_SemVencimentoLancaArgumentException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentException>(
            async () => await client.Financeiro.ObterContasAReceberAsync(new MovimentacaoFinanceiraFiltro { DataVencimentoDe = "2027-08-15" }));
    }

    // --- Parcelas ---

    [Test]
    public async Task ObterParcelasPorEvento_FazGet()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "[{\"id\":\"p-1\",\"status\":\"PENDENTE\"}]");
        using var client = TestClientFactory.Build(handler);

        var parcelas = await client.Financeiro.ObterParcelasPorEventoAsync("ev-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/ev-1/parcelas"));
            Assert.That(parcelas[0].Status, Is.EqualTo("PENDENTE"));
        });
    }

    [Test]
    public async Task ObterParcelaPorId_FazGet()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"id\":\"p-1\",\"versao\":1}");
        using var client = TestClientFactory.Build(handler);

        var parcela = await client.Financeiro.ObterParcelaPorIdAsync("p-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/parcelas/p-1"));
            Assert.That(parcela.Versao, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task AtualizarParcela_FazPatch()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"versao\":2}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.AtualizarParcelaAsync("p-1", new ParcelaAtualizacaoRequest { Versao = 1, Nota = "x" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod.Method, Is.EqualTo("PATCH"));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/parcelas/p-1"));
            Assert.That(handler.LastBody, Does.Contain("\"versao\":1"));
            Assert.That(resp.Versao, Is.EqualTo(2));
        });
    }

    // --- Eventos alterados / saldos iniciais ---

    [Test]
    public async Task ObterEventosAlterados_FazGetComPeriodo()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"itens_totais\":1,\"itens\":[{\"id\":\"ev-1\"}]}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Financeiro.ObterEventosAlteradosAsync(new PeriodoFinanceiroFiltro
        {
            DataInicio = "2026-01-01T00:00:00",
            DataFim = "2026-03-12T23:59:59"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/alteracoes"));
            Assert.That(resp.Itens[0].Id, Is.EqualTo("ev-1"));
        });
    }

    [Test]
    public async Task ObterSaldosIniciais_FazGetComPeriodo()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK, "{\"itens_totais\":0,\"itens\":[]}");
        using var client = TestClientFactory.Build(handler);

        await client.Financeiro.ObterSaldosIniciaisAsync(new PeriodoFinanceiroFiltro
        {
            DataInicio = "2026-01-01T00:00:00",
            DataFim = "2026-03-12T23:59:59"
        });

        Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/financeiro/eventos-financeiros/saldo-inicial"));
    }

    [Test]
    public void ObterSaldosIniciais_SemPeriodoLancaArgumentException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentException>(
            async () => await client.Financeiro.ObterSaldosIniciaisAsync(new PeriodoFinanceiroFiltro { DataInicio = "2026-01-01T00:00:00" }));
    }
}
