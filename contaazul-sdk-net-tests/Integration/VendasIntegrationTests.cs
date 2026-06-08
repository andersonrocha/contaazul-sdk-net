using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Vendas;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
public class VendasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task GetVendedores_RetornaLista()
    {
        var vendedores = await Client.Vendas.GetVendedoresAsync();

        Assert.That(vendedores, Is.Not.Null);
    }

    [Test]
    public async Task GetVendas_RetornaRespostaPaginada()
    {
        var resposta = await Client.Vendas.GetVendasAsync(new VendaFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            DataInicio = DataRelativa(-30),
            DataFim = DataRelativa(0)
        });

        Assert.That(resposta, Is.Not.Null);
        Assert.That(resposta.Itens, Is.Not.Null, "A busca deve retornar a coleção de itens (mesmo que vazia).");
        Assert.That(resposta.TotalItens, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task GetProximoNumero_RetornaValor()
    {
        var proximo = await Client.Vendas.GetProximoNumeroAsync();

        Assert.That(proximo, Is.Null.Or.GreaterThan(0));
    }

    [Test]
    public async Task GetVendaPorId_EItens_QuandoExisteVenda()
    {
        var lista = await Client.Vendas.GetVendasAsync(new VendaFiltro
        {
            Pagina = 1,
            TamanhoPagina = 1,
            DataInicio = DataRelativa(-90),
            DataFim = DataRelativa(0)
        });

        if (lista.Itens is null || lista.Itens.Count == 0)
        {
            Assert.Ignore("Nenhuma venda no período para testar consulta por id/itens.");
        }

        var id = lista.Itens![0].Id;

        var venda = await Client.Vendas.GetVendaByIdAsync(id!);
        Assert.That(venda, Is.Not.Null);
        Assert.That(venda.Venda, Is.Not.Null);

        var itens = await Client.Vendas.GetItensVendaAsync(id!, pagina: 1, tamanhoPagina: 10);
        Assert.That(itens, Is.Not.Null);
        Assert.That(itens.Itens, Is.Not.Null);
    }
}
