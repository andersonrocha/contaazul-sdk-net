using System;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Produtos;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class ProdutosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ObterProdutos_RetornaResposta()
    {
        var resp = await Client.Produtos.ObterProdutosAsync(new ProdutoFiltro { Status = "ATIVO" });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterCategorias_RetornaResposta()
    {
        var resp = await Client.Produtos.ObterCategoriasAsync();
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterCests_RetornaResposta()
    {
        var resp = await Client.Produtos.ObterCestsAsync(new BuscaTextualFiltro { BuscaTextual = "01" });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterNcms_RetornaResposta()
    {
        var resp = await Client.Produtos.ObterNcmsAsync();
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterUnidadesMedida_RetornaResposta()
    {
        var resp = await Client.Produtos.ObterUnidadesMedidaAsync();
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterMarcasEcommerce_RetornaResposta()
    {
        var resp = await Client.Produtos.ObterMarcasEcommerceAsync(new MarcaEcommerceFiltro { Direcao = "ASC" });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterCategoriasEcommerce_RetornaResposta()
    {
        var resp = await Client.Produtos.ObterCategoriasEcommerceAsync();
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterProdutoPorId_QuandoExisteProduto_RetornaDetalhe()
    {
        var lista = await Client.Produtos.ObterProdutosAsync(new ProdutoFiltro { Status = "ATIVO" });
        if (lista.Items is null || lista.Items.Count == 0)
        {
            Assert.Ignore("Nenhum produto cadastrado para testar o detalhe por id.");
        }

        var detalhe = await Client.Produtos.ObterProdutoPorIdAsync(lista.Items![0].Id!);
        Assert.That(detalhe, Is.Not.Null);
        Assert.That(detalhe.Id, Is.EqualTo(lista.Items[0].Id));
    }

    [Test]
    public async Task CriarObterAtualizarExcluir_Produto_Lifecycle()
    {
        RequireWrite();

        // Pré-requisitos fiscais (NCM e unidade de medida) buscados da própria API.
        var ncms = await Client.Produtos.ObterNcmsAsync();
        var unidades = await Client.Produtos.ObterUnidadesMedidaAsync();
        if (ncms.Items is null || ncms.Items.Count == 0 || unidades.Items is null || unidades.Items.Count == 0)
        {
            Assert.Ignore("Sem NCM/unidade de medida para compor o produto de teste.");
        }

        var criado = await Client.Produtos.CriarProdutoAsync(new CriacaoProduto
        {
            Nome = "SDK Teste Produto",
            CodigoSku = "SDK-" + Guid.NewGuid().ToString("N").Substring(0, 8),
            Estoque = new CriacaoEstoqueProduto { ValorVenda = 10 },
            Fiscal = new CriacaoFiscalProduto
            {
                Ncm = new ReferenciaIdInteiroProduto { Id = ncms.Items![0].Id },
                UnidadeMedida = new ReferenciaIdInteiroProduto { Id = unidades.Items![0].Id }
            }
        });

        Assert.That(criado, Is.Not.Null);
        Assert.That(criado.Id, Is.Not.Null.And.Not.Empty);

        try
        {
            var detalhe = await Client.Produtos.ObterProdutoPorIdAsync(criado.Id!);
            Assert.That(detalhe.Id, Is.EqualTo(criado.Id));

            await Client.Produtos.AtualizarParcialmenteProdutoAsync(criado.Id!, new AtualizacaoParcialProduto
            {
                ValorVenda = 12.5m
            });
        }
        finally
        {
            await Client.Produtos.DeletarProdutoPorIdAsync(criado.Id!);
        }
    }
}
