using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Pessoas;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class PessoasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ObterPessoas_RetornaRespostaValida()
    {
        var resposta = await Client.Pessoas.ObterPessoasAsync(new PessoaFiltro { Pagina = 1, TamanhoPagina = 10 });

        Assert.That(resposta, Is.Not.Null);
        // Items pode ser nulo/vazio em uma conta sem pessoas cadastradas.
        Assert.That(resposta.TotalItems, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public async Task ObterEmpresaConectada_RetornaDados()
    {
        var empresa = await Client.Pessoas.ObterEmpresaConectadaAsync();

        // Valida que a resposta desserializa; campos individuais podem vir vazios conforme a conta.
        Assert.That(empresa, Is.Not.Null);
    }

    [Test]
    public async Task ObterPessoaPorId_QuandoExistePessoa_RetornaDetalhe()
    {
        var lista = await Client.Pessoas.ObterPessoasAsync(new PessoaFiltro { Pagina = 1, TamanhoPagina = 10 });
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
