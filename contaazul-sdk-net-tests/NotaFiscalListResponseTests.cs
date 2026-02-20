using ContaAzul.Sdk.Net.Models;
using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class NotaFiscalListResponseTests
{
    [Test]
    public void WhenDeserializeNotaFiscalListResponseWithNewStructureThenMapsPropertiesCorrectly()
    {
        var json = @"{
  ""itens"": [
    {
      ""id"": ""fed6a508-9131-41ed-af70-720da93bfef9"",
      ""id_venda"": ""cd09dd64-a08c-4764-8539-659176215066"",
      ""numero_venda"": ""8448"",
      ""numero_rps"": 7733,
      ""numero_nfse"": 7718,
      ""status"": ""EMITIDA"",
      ""valor_total_nfse"": 480,
      ""data_competencia"": ""2025-12-29"",
      ""nome_cliente"": ""ROE CAPITAL"",
      ""documento_cliente"": ""61248424000164"",
      ""codigo_cnae"": ""6311900"",
      ""escriturado_manualmente"": false
    },
    {
      ""id"": ""28a2fa26-1bf8-487e-985d-bd6bc312521b"",
      ""id_venda"": ""cd09dd64-a08c-4764-8539-659176215066"",
      ""numero_venda"": ""8448"",
      ""numero_rps"": 7730,
      ""numero_nfse"": 7715,
      ""status"": ""CANCELADA"",
      ""valor_total_nfse"": 640,
      ""data_competencia"": ""2025-12-29"",
      ""nome_cliente"": ""ROE CAPITAL"",
      ""documento_cliente"": ""61248424000164"",
      ""codigo_cnae"": ""6311900"",
      ""escriturado_manualmente"": false
    }
  ],
  ""paginacao"": {
    ""pagina_atual"": 1,
    ""total_paginas"": 1,
    ""tamanho_pagina"": 10,
    ""total_itens"": 2
  }
}";

        var result = JsonConvert.DeserializeObject<NotaFiscalListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Itens, Is.Not.Null);
            Assert.That(result.Itens, Has.Count.EqualTo(2));

            Assert.That(result.Paginacao, Is.Not.Null);
            Assert.That(result.Paginacao.PaginaAtual, Is.EqualTo(1));
            Assert.That(result.Paginacao.TotalPaginas, Is.EqualTo(1));
            Assert.That(result.Paginacao.TamanhoPagina, Is.EqualTo(10));
            Assert.That(result.Paginacao.TotalItens, Is.EqualTo(2));

            var primeiraNotaFiscal = result.Itens[0];
            Assert.That(primeiraNotaFiscal, Is.Not.Null);
            Assert.That(primeiraNotaFiscal.Id, Is.EqualTo("fed6a508-9131-41ed-af70-720da93bfef9"));
            Assert.That(primeiraNotaFiscal.Status, Is.EqualTo("EMITIDA"));

            var segundaNotaFiscal = result.Itens[1];
            Assert.That(segundaNotaFiscal, Is.Not.Null);
            Assert.That(segundaNotaFiscal.Id, Is.EqualTo("28a2fa26-1bf8-487e-985d-bd6bc312521b"));
            Assert.That(segundaNotaFiscal.Status, Is.EqualTo("CANCELADA"));
        });
    }

    [Test]
    public void WhenDeserializeNotaFiscalListResponseWithEmptyItensThenMapsCorrectly()
    {
        var json = @"{
  ""itens"": [],
  ""paginacao"": {
    ""pagina_atual"": 1,
    ""total_paginas"": 0,
    ""tamanho_pagina"": 10,
    ""total_itens"": 0
  }
}";

        var result = JsonConvert.DeserializeObject<NotaFiscalListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Itens, Is.Not.Null);
            Assert.That(result.Itens, Has.Count.EqualTo(0));

            Assert.That(result.Paginacao, Is.Not.Null);
            Assert.That(result.Paginacao.PaginaAtual, Is.EqualTo(1));
            Assert.That(result.Paginacao.TotalPaginas, Is.EqualTo(0));
            Assert.That(result.Paginacao.TamanhoPagina, Is.EqualTo(10));
            Assert.That(result.Paginacao.TotalItens, Is.EqualTo(0));
        });
    }

    [Test]
    public void WhenDeserializeNotaFiscalListResponseWithMultiplePagesThenMapsCorrectly()
    {
        var json = @"{
  ""itens"": [
    {
      ""id"": ""id-1"",
      ""status"": ""EMITIDA"",
      ""numero_rps"": 1001,
      ""numero_nfse"": 2001
    },
    {
      ""id"": ""id-2"",
      ""status"": ""PROCESSANDO"",
      ""numero_rps"": 1002,
      ""numero_nfse"": 2002
    },
    {
      ""id"": ""id-3"",
      ""status"": ""CANCELADA"",
      ""numero_rps"": 1003,
      ""numero_nfse"": 2003
    }
  ],
  ""paginacao"": {
    ""pagina_atual"": 2,
    ""total_paginas"": 5,
    ""tamanho_pagina"": 3,
    ""total_itens"": 15
  }
}";

        var result = JsonConvert.DeserializeObject<NotaFiscalListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Itens, Is.Not.Null);
            Assert.That(result.Itens, Has.Count.EqualTo(3));

            Assert.That(result.Paginacao, Is.Not.Null);
            Assert.That(result.Paginacao.PaginaAtual, Is.EqualTo(2));
            Assert.That(result.Paginacao.TotalPaginas, Is.EqualTo(5));
            Assert.That(result.Paginacao.TamanhoPagina, Is.EqualTo(3));
            Assert.That(result.Paginacao.TotalItens, Is.EqualTo(15));

            Assert.That(result.Itens[0].Id, Is.EqualTo("id-1"));
            Assert.That(result.Itens[0].Status, Is.EqualTo("EMITIDA"));

            Assert.That(result.Itens[1].Id, Is.EqualTo("id-2"));
            Assert.That(result.Itens[1].Status, Is.EqualTo("PROCESSANDO"));

            Assert.That(result.Itens[2].Id, Is.EqualTo("id-3"));
            Assert.That(result.Itens[2].Status, Is.EqualTo("CANCELADA"));
        });
    }

    [Test]
    public void WhenDeserializePaginacaoObjectThenMapsAllPropertiesCorrectly()
    {
        var json = @"{
  ""pagina_atual"": 3,
  ""total_paginas"": 10,
  ""tamanho_pagina"": 20,
  ""total_itens"": 200
}";

        var result = JsonConvert.DeserializeObject<Paginacao>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PaginaAtual, Is.EqualTo(3));
            Assert.That(result.TotalPaginas, Is.EqualTo(10));
            Assert.That(result.TamanhoPagina, Is.EqualTo(20));
            Assert.That(result.TotalItens, Is.EqualTo(200));
        });
    }
}
