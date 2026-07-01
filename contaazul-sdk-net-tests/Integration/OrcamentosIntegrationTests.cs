using System.Collections.Generic;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Orcamentos;
using ContaAzul.Sdk.Net.Models.Pessoas;
using ContaAzul.Sdk.Net.Models.Produtos;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class OrcamentosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ObterOrcamentos_RetornaResposta()
    {
        var resp = await Client.Orcamentos.ObterOrcamentosAsync(new OrcamentoFiltro { TamanhoPagina = 10 });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterOrcamentoPorId_QuandoExisteOrcamento_RetornaDetalhe()
    {
        var lista = await Client.Orcamentos.ObterOrcamentosAsync(new OrcamentoFiltro { TamanhoPagina = 10 });
        if (lista.Itens is null || lista.Itens.Count == 0)
        {
            Assert.Ignore("Nenhum orçamento cadastrado para testar o detalhe por id.");
        }

        var detalhe = await Client.Orcamentos.ObterOrcamentoPorIdAsync(lista.Itens![0].Id!);
        Assert.That(detalhe, Is.Not.Null);
        Assert.That(detalhe.Id, Is.EqualTo(lista.Itens[0].Id));
    }

    [Test]
    public async Task CriarExcluir_Orcamento_Lifecycle()
    {
        RequireWrite();

        // Um orçamento exige um cliente e um item (produto/serviço) reais.
        var clientes = await Client.Pessoas.ObterPessoasAsync(new PessoaFiltro { TamanhoPagina = 10 });
        var produtos = await Client.Produtos.ObterProdutosAsync(new ProdutoFiltro { TamanhoPagina = 10 });
        if (clientes.Items is null || clientes.Items.Count == 0 || produtos.Items is null || produtos.Items.Count == 0)
        {
            Assert.Ignore("É necessário ao menos um cliente e um produto cadastrados para montar um orçamento.");
        }

        var criado = await Client.Orcamentos.CriarOrcamentoAsync(new CriarOrcamento
        {
            DataOrcamento = DataRelativa(0),
            DataValidade = DataRelativa(15),
            IdCliente = clientes.Items![0].Id,
            Itens = new List<CriarItemOrcamento>
            {
                new CriarItemOrcamento { Id = produtos.Items![0].Id, Quantidade = 1, Valor = 10 }
            }
        });

        Assert.That(criado, Is.Not.Null);
        Assert.That(criado.Id, Is.Not.Null.And.Not.Empty);

        await Client.Orcamentos.ExcluirOrcamentosEmLoteAsync(new ExclusaoLoteOrcamento
        {
            Ids = new List<string> { criado.Id! }
        });
    }
}
