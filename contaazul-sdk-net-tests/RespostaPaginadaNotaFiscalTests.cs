using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.NotasFiscais;
using System.Text.Json;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class RespostaPaginadaNotaFiscalTests
{
    [Test]
    public void WhenDeserializeNotasFiscaisServicoThenMapsPropertiesCorrectly()
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
      ""escriturado_manualmente"": false,
      ""cidade_emissao"": { ""estado"": ""SC"", ""nome"": ""Joinville"" }
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

        var result = JsonSerializer.Deserialize<RespostaPaginada<NotaFiscalServico>>(json);

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

            var primeira = result.Itens[0];
            Assert.That(primeira.Id, Is.EqualTo("fed6a508-9131-41ed-af70-720da93bfef9"));
            Assert.That(primeira.Status, Is.EqualTo("EMITIDA"));
            Assert.That(primeira.NumeroNfse, Is.EqualTo(7718));
            Assert.That(primeira.ValorTotalNfse, Is.EqualTo(480m));
            Assert.That(primeira.CidadeEmissao, Is.Not.Null);
            Assert.That(primeira.CidadeEmissao!.Estado, Is.EqualTo("SC"));
            Assert.That(primeira.CidadeEmissao.Nome, Is.EqualTo("Joinville"));

            var segunda = result.Itens[1];
            Assert.That(segunda.Id, Is.EqualTo("28a2fa26-1bf8-487e-985d-bd6bc312521b"));
            Assert.That(segunda.Status, Is.EqualTo("CANCELADA"));
        });
    }

    [Test]
    public void WhenDeserializeNotasFiscaisProdutoThenMapsPropertiesCorrectly()
    {
        var json = @"{
  ""itens"": [
    {
      ""chave_acesso"": ""42250323643586000108550010000001151606401726"",
      ""data_emissao"": ""2025-01-15T10:30:00Z"",
      ""nome_destinatario"": ""EMPRESA EXEMPLO LTDA"",
      ""numero_nota"": 123456,
      ""status"": ""EMITIDA""
    }
  ],
  ""paginacao"": {
    ""pagina_atual"": 1,
    ""total_paginas"": 1,
    ""tamanho_pagina"": 10,
    ""total_itens"": 1
  }
}";

        var result = JsonSerializer.Deserialize<RespostaPaginada<NotaFiscal>>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Itens, Has.Count.EqualTo(1));

            var nota = result.Itens[0];
            Assert.That(nota.ChaveAcesso, Is.EqualTo("42250323643586000108550010000001151606401726"));
            Assert.That(nota.NomeDestinatario, Is.EqualTo("EMPRESA EXEMPLO LTDA"));
            Assert.That(nota.NumeroNota, Is.EqualTo(123456));
            Assert.That(nota.Status, Is.EqualTo("EMITIDA"));

            Assert.That(result.Paginacao.TotalItens, Is.EqualTo(1));
        });
    }

    [Test]
    public void WhenDeserializeWithEmptyItensThenMapsCorrectly()
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

        var result = JsonSerializer.Deserialize<RespostaPaginada<NotaFiscalServico>>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Itens, Is.Not.Null);
            Assert.That(result.Itens, Has.Count.EqualTo(0));
            Assert.That(result.Paginacao.TotalItens, Is.EqualTo(0));
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

        var result = JsonSerializer.Deserialize<Paginacao>(json);

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
