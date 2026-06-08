using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.NotasFiscais;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
public class NotasFiscaisIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ObterNotasFiscaisProduto_RetornaRespostaPaginada()
    {
        var resposta = await Client.NotasFiscais.ObterNotasFiscaisAsync(new NotaFiscalFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            DataInicial = DataRelativa(-30),
            DataFinal = DataRelativa(0)
        });

        Assert.That(resposta, Is.Not.Null);
        Assert.That(resposta.Itens, Is.Not.Null, "A consulta deve retornar a coleção de itens (mesmo que vazia).");
        Assert.That(resposta.Paginacao, Is.Not.Null);
    }

    [Test]
    public async Task ObterNotasFiscaisServico_RetornaRespostaPaginada()
    {
        // Intervalo máximo permitido pela API de NFS-e: 15 dias.
        var resposta = await Client.NotasFiscais.ObterNotasFiscaisServicoAsync(new NotaFiscalServicoFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            DataCompetenciaDe = DataRelativa(-15),
            DataCompetenciaAte = DataRelativa(0)
        });

        Assert.That(resposta, Is.Not.Null);
        Assert.That(resposta.Itens, Is.Not.Null, "A consulta deve retornar a coleção de itens (mesmo que vazia).");
        Assert.That(resposta.Paginacao, Is.Not.Null);
    }
}
