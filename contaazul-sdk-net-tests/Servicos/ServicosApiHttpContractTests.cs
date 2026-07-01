using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Servicos;

namespace ContaAzul.Sdk.Net.Tests.Servicos;

[TestFixture]
public class ServicosApiHttpContractTests
{
    [Test]
    public async Task ObterServicos_FazGetComFiltros()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"itens\":[{\"id\":\"s-1\",\"descricao\":\"Consultoria\",\"status\":\"ATIVO\",\"tipo_servico\":\"PRESTADO\"}]," +
            "\"paginacao\":{\"pagina_atual\":1,\"tamanho_pagina\":10,\"total_itens\":1,\"total_paginas\":1}}");
        using var client = TestClientFactory.Build(handler);

        var resp = await client.Servicos.ObterServicosAsync(new ServicoFiltro { BuscaTextual = "consult" });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/servicos"));
            Assert.That(handler.LastUri.Query, Does.Contain("busca_textual=consult"));
            Assert.That(handler.LastUri.Query, Does.Contain("pagina=1"));
            Assert.That(handler.LastUri.Query, Does.Contain("tamanho_pagina=10"));
            Assert.That(resp.Itens, Has.Count.EqualTo(1));
            Assert.That(resp.Itens![0].Descricao, Is.EqualTo("Consultoria"));
            Assert.That(resp.Paginacao!.TotalItens, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task ObterServicoPorId_FazGetNoEndpointDoServico()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"id\":\"s-1\",\"descricao\":\"Consultoria\",\"tipo_servico\":\"PRESTADO\",\"preco\":500}");
        using var client = TestClientFactory.Build(handler);

        var servico = await client.Servicos.ObterServicoPorIdAsync("s-1");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/servicos/s-1"));
            Assert.That(servico.Descricao, Is.EqualTo("Consultoria"));
            Assert.That(servico.Preco, Is.EqualTo(500m));
        });
    }

    [Test]
    public void ObterServicoPorId_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Servicos.ObterServicoPorIdAsync(" "));
    }

    [Test]
    public async Task CriarServico_PostaCorpoNoEndpointDeServicos()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.Created, "{\"id\":\"s-1\",\"descricao\":\"Novo servico\"}");
        using var client = TestClientFactory.Build(handler);

        var servico = await client.Servicos.CriarServicoAsync(new CriarServico
        {
            Descricao = "Novo servico",
            Codigo = "SERV001",
            Preco = 500,
            TipoServico = "PRESTADO"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/servicos"));
            Assert.That(handler.LastBody, Does.Contain("\"descricao\":\"Novo servico\""));
            Assert.That(handler.LastBody, Does.Contain("\"tipo_servico\":\"PRESTADO\""));
            Assert.That(servico.Id, Is.EqualTo("s-1"));
        });
    }

    [Test]
    public void CriarServico_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Servicos.CriarServicoAsync(null!));
    }

    [Test]
    public async Task AtualizarParcialmenteServico_FazPatchNoEndpointDoServico()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.NoContent);
        using var client = TestClientFactory.Build(handler);

        await client.Servicos.AtualizarParcialmenteServicoAsync("s-1", new AtualizacaoParcialServico
        {
            Preco = 120.5m,
            TipoServico = "AMBOS"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Patch));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/servicos/s-1"));
            Assert.That(handler.LastBody, Does.Contain("\"preco\":120.5"));
            Assert.That(handler.LastBody, Does.Contain("\"tipo_servico\":\"AMBOS\""));
        });
    }

    [Test]
    public void AtualizarParcialmenteServico_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Servicos.AtualizarParcialmenteServicoAsync("", new AtualizacaoParcialServico { Preco = 1 }));
    }

    [Test]
    public void AtualizarParcialmenteServico_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Servicos.AtualizarParcialmenteServicoAsync("s-1", null!));
    }

    [Test]
    public async Task DeletarServicosEmLote_FazDeleteComCorpo()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.NoContent);
        using var client = TestClientFactory.Build(handler);

        await client.Servicos.DeletarServicosEmLoteAsync(new ParametrosParaDeletarServicosEmLote
        {
            Ids = new System.Collections.Generic.List<int> { 73233, 73234, 73235 }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Delete));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/servicos"));
            Assert.That(handler.LastBody, Does.Contain("\"ids\":[73233,73234,73235]"));
        });
    }

    [Test]
    public void DeletarServicosEmLote_ComNuloLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Servicos.DeletarServicosEmLoteAsync(null!));
    }

    [Test]
    public void DeletarServicosEmLote_ComIdsVaziosLancaArgumentException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Servicos.DeletarServicosEmLoteAsync(new ParametrosParaDeletarServicosEmLote
            {
                Ids = new System.Collections.Generic.List<int>()
            }));
    }
}
