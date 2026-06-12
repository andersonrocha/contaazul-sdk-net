using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class FinanceiroIntegrationTests : IntegrationTestBase
{
    private static string DataHora(int dias) => DataRelativa(dias) + "T00:00:00";

    [Test]
    public async Task ObterCentrosDeCusto_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterCentrosDeCustoAsync(new CentroDeCustoFiltro { FiltroRapido = "TODOS" });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterCategorias_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterCategoriasAsync(new CategoriaFiltro { PermiteApenasFilhos = false });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterCategoriasDre_RetornaEstrutura()
    {
        var dre = await Client.Financeiro.ObterCategoriasDreAsync();
        Assert.That(dre, Is.Not.Null);
    }

    [Test]
    public async Task ObterContasFinanceiras_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterContasFinanceirasAsync(new ContaFinanceiraFiltro { ApenasAtivo = true });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterSaldoAtual_QuandoExisteConta_RetornaSaldo()
    {
        var contas = await Client.Financeiro.ObterContasFinanceirasAsync(new ContaFinanceiraFiltro { TamanhoPagina = 10 });
        if (contas.Itens is null || contas.Itens.Count == 0)
        {
            Assert.Ignore("Nenhuma conta financeira para testar o saldo atual.");
        }

        var saldo = await Client.Financeiro.ObterSaldoAtualAsync(contas.Itens![0].Id!);
        Assert.That(saldo, Is.Not.Null);
    }

    [Test]
    public async Task ObterTransferencias_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterTransferenciasAsync(new TransferenciaFiltro
        {
            DataInicio = DataRelativa(-365),
            DataFim = DataRelativa(0)
        });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterContasAReceber_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterContasAReceberAsync(new MovimentacaoFinanceiraFiltro
        {
            DataVencimentoDe = DataRelativa(-365),
            DataVencimentoAte = DataRelativa(365)
        });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterContasAPagar_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterContasAPagarAsync(new MovimentacaoFinanceiraFiltro
        {
            DataVencimentoDe = DataRelativa(-365),
            DataVencimentoAte = DataRelativa(365)
        });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterEventosAlterados_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterEventosAlteradosAsync(new PeriodoFinanceiroFiltro
        {
            DataInicio = DataHora(-30),
            DataFim = DataHora(0)
        });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterSaldosIniciais_RetornaResposta()
    {
        var resp = await Client.Financeiro.ObterSaldosIniciaisAsync(new PeriodoFinanceiroFiltro
        {
            DataInicio = DataHora(-30),
            DataFim = DataHora(0)
        });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterParcelaPorId_QuandoExisteContaAReceber_RetornaParcela()
    {
        var contas = await Client.Financeiro.ObterContasAReceberAsync(new MovimentacaoFinanceiraFiltro
        {
            DataVencimentoDe = DataRelativa(-365),
            DataVencimentoAte = DataRelativa(365),
            TamanhoPagina = 10
        });

        if (contas.Itens is null || contas.Itens.Count == 0)
        {
            Assert.Ignore("Nenhuma conta a receber no período para testar a parcela por id.");
        }

        var parcela = await Client.Financeiro.ObterParcelaPorIdAsync(contas.Itens![0].Id!);
        Assert.That(parcela, Is.Not.Null);
    }
}
