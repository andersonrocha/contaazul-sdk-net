using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Servicos;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class ServicosIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ObterServicos_RetornaResposta()
    {
        var resp = await Client.Servicos.ObterServicosAsync(new ServicoFiltro { TamanhoPagina = 10 });
        Assert.That(resp, Is.Not.Null);
    }

    [Test]
    public async Task ObterServicoPorId_QuandoExisteServico_RetornaDetalhe()
    {
        var lista = await Client.Servicos.ObterServicosAsync(new ServicoFiltro { TamanhoPagina = 10 });
        if (lista.Itens is null || lista.Itens.Count == 0)
        {
            Assert.Ignore("Nenhum serviço cadastrado para testar o detalhe por id.");
        }

        var detalhe = await Client.Servicos.ObterServicoPorIdAsync(lista.Itens![0].Id!);
        Assert.That(detalhe, Is.Not.Null);
        Assert.That(detalhe.Id, Is.EqualTo(lista.Itens[0].Id));
    }

    [Test]
    public async Task CriarObterAtualizarExcluir_Servico_Lifecycle()
    {
        RequireWrite();

        var criado = await Client.Servicos.CriarServicoAsync(new CriarServico
        {
            Descricao = "SDK Teste Servico",
            Codigo = "SDK" + Guid.NewGuid().ToString("N").Substring(0, 8),
            Preco = 100,
            Status = "ATIVO",
            TipoServico = "PRESTADO"
        });

        Assert.That(criado, Is.Not.Null);
        Assert.That(criado.Id, Is.Not.Null.And.Not.Empty);

        try
        {
            var detalhe = await Client.Servicos.ObterServicoPorIdAsync(criado.Id!);
            Assert.That(detalhe.Id, Is.EqualTo(criado.Id));

            await Client.Servicos.AtualizarParcialmenteServicoAsync(criado.Id!, new AtualizacaoParcialServico
            {
                Preco = 150
            });
        }
        finally
        {
            // A exclusão é em lote e usa o id legado (id_servico) retornado na criação.
            if (criado.IdServico.HasValue)
            {
                await Client.Servicos.DeletarServicosEmLoteAsync(new ParametrosParaDeletarServicosEmLote
                {
                    Ids = new List<int> { criado.IdServico.Value }
                });
            }
        }
    }
}
