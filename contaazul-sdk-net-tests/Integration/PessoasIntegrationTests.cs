using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Pessoas;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
public class PessoasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ObterPessoas_RetornaRespostaValida()
    {
        var resposta = await Client.Pessoas.ObterPessoasAsync(new PessoaFiltro { Pagina = 1, TamanhoPagina = 10 });

        Assert.That(resposta, Is.Not.Null);
        Assert.That(resposta.Items, Is.Not.Null, "A listagem deve retornar a coleção de itens (mesmo que vazia).");
        Assert.That(resposta.TotalItems, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task ObterEmpresaConectada_RetornaDados()
    {
        var empresa = await Client.Pessoas.ObterEmpresaConectadaAsync();

        Assert.That(empresa, Is.Not.Null);
        Assert.That(empresa.Documento, Is.Not.Null.And.Not.Empty, "A empresa conectada deve ter um documento (CNPJ).");
    }

    [Test]
    public async Task ObterPessoaPorId_QuandoExistePessoa_RetornaDetalhe()
    {
        var lista = await Client.Pessoas.ObterPessoasAsync(new PessoaFiltro { Pagina = 1, TamanhoPagina = 1 });
        if (lista.Items is null || lista.Items.Count == 0)
        {
            Assert.Ignore("Nenhuma pessoa cadastrada para testar a consulta por id.");
        }

        var id = lista.Items![0].Id;
        var pessoa = await Client.Pessoas.ObterPessoaPorIdAsync(id!);

        Assert.That(pessoa, Is.Not.Null);
        Assert.That(pessoa.Id, Is.EqualTo(id));
    }
}
