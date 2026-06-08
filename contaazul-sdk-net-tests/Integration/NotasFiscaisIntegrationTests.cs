using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.NotasFiscais;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class NotasFiscaisIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ObterNotasFiscaisProduto_RetornaRespostaPaginada()
    {
        // A API limita o intervalo a 15 dias (mesma regra da NFS-e).
        var resposta = await Client.NotasFiscais.ObterNotasFiscaisAsync(new NotaFiscalFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            DataInicial = DataRelativa(-15),
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
