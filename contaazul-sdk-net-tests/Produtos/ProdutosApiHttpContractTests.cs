using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Produtos;

namespace ContaAzul.Sdk.Net.Tests.Produtos;

[TestFixture]
public class ProdutosApiHttpContractTests
{
    // --- Listagem / detalhe ---

    [Test]
    public async Task ObterProdutos_FazGetComFiltros()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"items\":[{\"id\":\"p-1\",\"nome\":\"Produto 01\",\"status\":\"ATIVO\"}],\"totalItems\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Produtos.ObterProdutosAsync(new ProdutoFiltro
        {
            Busca = "cafe",
            Status = "ATIVO",
            ValorVendaInicial = 10.5m,
            IntegracaoEcommerceAtivo = true
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos"));
            Assert.That(handler.LastUri.Query, Does.Contain("busca=cafe"));
            Assert.That(handler.LastUri.Query, Does.Contain("status=ATIVO"));
            Assert.That(handler.LastUri.Query, Does.Contain("valor_venda_inicial=10.5"));
            Assert.That(handler.LastUri.Query, Does.Contain("integracao_ecommerce_ativo=true"));
            Assert.That(handler.LastUri.Query, Does.Contain("pagina=1"));
            Assert.That(handler.LastUri.Query, Does.Contain("tamanho_pagina=10"));
            Assert.That(resp.Items, Has.Count.EqualTo(1));
            Assert.That(resp.Items![0].Id, Is.EqualTo("p-1"));
            Assert.That(resp.TotalItems, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task ObterProdutoPorId_FazGetNoEndpointDoProduto()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"id\":\"p-1\",\"nome\":\"Produto\",\"status\":\"ATIVO\",\"fiscal\":{\"ncm\":{\"codigo\":\"1234\",\"id\":1}}}");
        using var client = TestClientFactory.Build(handler);

        var produto = await client.Produtos.ObterProdutoPorIdAsync("p-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/p-1"));
            Assert.That(produto.Nome, Is.EqualTo("Produto"));
            Assert.That(produto.Fiscal!.Ncm!.Codigo, Is.EqualTo("1234"));
        });
    }

    [Test]
    public void ObterProdutoPorId_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Produtos.ObterProdutoPorIdAsync(" "));
    }

    // --- Criação / atualização / exclusão ---

    [Test]
    public async Task CriarProduto_PostaCorpoNoEndpointDeProdutos()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.Created, "{\"id\":\"p-1\",\"nome\":\"Novo\"}");
        using var client = TestClientFactory.Build(handler);

        var produto = await client.Produtos.CriarProdutoAsync(new CriacaoProduto
        {
            Nome = "Novo",
            CodigoSku = "SKU1",
            Fiscal = new CriacaoFiscalProduto { Ncm = new ReferenciaIdInteiroProduto { Id = 1 } }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos"));
            Assert.That(handler.LastBody, Does.Contain("\"nome\":\"Novo\""));
            Assert.That(handler.LastBody, Does.Contain("\"codigo_sku\":\"SKU1\""));
            Assert.That(handler.LastBody, Does.Contain("\"ncm\":{\"id\":1}"));
            Assert.That(produto.Id, Is.EqualTo("p-1"));
        });
    }

    [Test]
    public void CriarProduto_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Produtos.CriarProdutoAsync(null!));
    }

    [Test]
    public async Task AtualizarParcialmenteProduto_FazPatchNoEndpointDoProduto()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.NoContent);
        using var client = TestClientFactory.Build(handler);

        await client.Produtos.AtualizarParcialmenteProdutoAsync("p-1", new AtualizacaoParcialProduto
        {
            Nome = "Renomeado",
            ValorVenda = 99.9m
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Patch));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/p-1"));
            Assert.That(handler.LastBody, Does.Contain("\"nome\":\"Renomeado\""));
            Assert.That(handler.LastBody, Does.Contain("\"valor_venda\":99.9"));
        });
    }

    [Test]
    public void AtualizarParcialmenteProduto_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Produtos.AtualizarParcialmenteProdutoAsync(" ", new AtualizacaoParcialProduto { Nome = "x" }));
    }

    [Test]
    public void AtualizarParcialmenteProduto_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Produtos.AtualizarParcialmenteProdutoAsync("p-1", null!));
    }

    [Test]
    public async Task DeletarProdutoPorId_FazDeleteNoEndpointDoProduto()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.NoContent);
        using var client = TestClientFactory.Build(handler);

        await client.Produtos.DeletarProdutoPorIdAsync("p-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Delete));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/p-1"));
        });
    }

    [Test]
    public void DeletarProdutoPorId_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Produtos.DeletarProdutoPorIdAsync(""));
    }

    // --- Listagens auxiliares ---

    [Test]
    public async Task ObterCategorias_FazGetComBuscaTextual()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"items\":[{\"id\":1,\"descricao\":\"Eletronicos\",\"uuid\":\"u-1\"}],\"total_items\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Produtos.ObterCategoriasAsync(new BuscaTextualFiltro { BuscaTextual = "Ele" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/categorias"));
            Assert.That(handler.LastUri.Query, Does.Contain("busca_textual=Ele"));
            Assert.That(resp.Items, Has.Count.EqualTo(1));
            Assert.That(resp.Items![0].Id, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task ObterCests_FazGet()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"items\":[{\"codigo\":\"0100100\",\"descricao\":\"x\",\"id\":1}],\"total_items\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Produtos.ObterCestsAsync(new BuscaTextualFiltro { BuscaTextual = "01" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/cest"));
            Assert.That(resp.Items![0].Codigo, Is.EqualTo("0100100"));
        });
    }

    [Test]
    public async Task ObterNcms_FazGet()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"items\":[{\"codigo\":\"0100400\",\"descricao\":\"x\",\"id\":1}],\"total_items\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Produtos.ObterNcmsAsync();

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/ncm"));
            Assert.That(resp.Items![0].Codigo, Is.EqualTo("0100400"));
        });
    }

    [Test]
    public async Task ObterUnidadesMedida_FazGet()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"items\":[{\"abreviacao\":\"Kg\",\"descricao\":\"Kilograma\",\"em_uso\":true,\"id\":10801}],\"total_items\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Produtos.ObterUnidadesMedidaAsync(new BuscaTextualFiltro { BuscaTextual = "Kg" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/unidades-medida"));
            Assert.That(resp.Items![0].Abreviacao, Is.EqualTo("Kg"));
            Assert.That(resp.Items[0].EmUso, Is.True);
        });
    }

    [Test]
    public async Task ObterMarcasEcommerce_FazGetComDirecao()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"items\":[{\"id\":\"m-1\",\"nome\":\"adidas\"}],\"total_items\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Produtos.ObterMarcasEcommerceAsync(new MarcaEcommerceFiltro { Direcao = "DESC", BuscaTextual = "adi" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/ecommerce-marcas"));
            Assert.That(handler.LastUri.Query, Does.Contain("direcao=DESC"));
            Assert.That(handler.LastUri.Query, Does.Contain("busca_textual=adi"));
            Assert.That(resp.Items![0].Nome, Is.EqualTo("adidas"));
        });
    }

    [Test]
    public async Task ObterCategoriasEcommerce_FazGetComBuscaTextual()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"id\":\"root\",\"items\":[{\"id\":\"c-1\",\"descricao\":\"Eletronicos\",\"subcategorias\":[{\"id\":\"c-2\",\"descricao\":\"Celulares\"}]}],\"versao\":1}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Produtos.ObterCategoriasEcommerceAsync("Ele");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/produtos/ecommerce-categorias"));
            Assert.That(handler.LastUri.Query, Does.Contain("busca_textual=Ele"));
            Assert.That(resp.Items, Has.Count.EqualTo(1));
            Assert.That(resp.Items![0].Subcategorias![0].Id, Is.EqualTo("c-2"));
        });
    }

    [Test]
    public void ObterCategoriasEcommerce_ComBuscaVaziaLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Produtos.ObterCategoriasEcommerceAsync(" "));
    }
}
