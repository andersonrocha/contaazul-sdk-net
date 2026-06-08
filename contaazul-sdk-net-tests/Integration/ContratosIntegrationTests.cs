using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Contratos;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class ContratosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ListarContratos_RetornaResposta()
    {
        var resposta = await Client.Contratos.ListarContratosAsync(new ContratoFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            DataInicio = DataRelativa(-365),
            DataFim = DataRelativa(365)
        });

        Assert.That(resposta, Is.Not.Null);
        Assert.That(resposta.Itens, Is.Not.Null, "A listagem deve retornar a coleção de itens (mesmo que vazia).");
        Assert.That(resposta.ItensTotais, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task ObterProximoNumero_RetornaInteiro()
    {
        var proximo = await Client.Contratos.ObterProximoNumeroAsync();

        Assert.That(proximo, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task ObterContratoPorId_QuandoExisteContrato_RetornaDetalhe()
    {
        var lista = await Client.Contratos.ListarContratosAsync(new ContratoFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            DataInicio = DataRelativa(-365),
            DataFim = DataRelativa(365)
        });

        if (lista.Itens is null || lista.Itens.Count == 0)
        {
            Assert.Ignore("Nenhum contrato no período para testar a consulta por id.");
        }

        var id = lista.Itens![0].Id;
        var contrato = await Client.Contratos.ObterContratoPorIdAsync(id!);

        Assert.That(contrato, Is.Not.Null);
    }
}
