using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class BaixasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ListarBaixasPorParcela_QuandoExisteParcela_RetornaLista()
    {
        // Obtém uma parcela real a partir das contas a receber para então listar suas baixas.
        var contas = await Client.Financeiro.ObterContasAReceberAsync(new MovimentacaoFinanceiraFiltro
        {
            DataVencimentoDe = DataRelativa(-365),
            DataVencimentoAte = DataRelativa(365),
            TamanhoPagina = 10
        });

        if (contas.Itens is null || contas.Itens.Count == 0)
        {
            Assert.Ignore("Nenhuma conta a receber no período para testar a listagem de baixas.");
        }

        var baixas = await Client.Baixas.ListarBaixasPorParcelaAsync(contas.Itens![0].Id!);
        Assert.That(baixas, Is.Not.Null);
    }
}
