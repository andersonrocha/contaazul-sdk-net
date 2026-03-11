using ContaAzul.Sdk.Net.Models;
using System.Text.Json;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class PaginacaoTests
{
    [Test]
    public void WhenDeserializePaginacaoWithAllPropertiesThenMapsCorrectly()
    {
        var json = @"{
  ""pagina_atual"": 5,
  ""total_paginas"": 20,
  ""tamanho_pagina"": 50,
  ""total_itens"": 1000
}";

        var result = JsonSerializer.Deserialize<Paginacao>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PaginaAtual, Is.EqualTo(5));
            Assert.That(result.TotalPaginas, Is.EqualTo(20));
            Assert.That(result.TamanhoPagina, Is.EqualTo(50));
            Assert.That(result.TotalItens, Is.EqualTo(1000));
        });
    }

    [Test]
    public void WhenDeserializePaginacaoWithZeroValuesThenMapsCorrectly()
    {
        var json = @"{
  ""pagina_atual"": 0,
  ""total_paginas"": 0,
  ""tamanho_pagina"": 0,
  ""total_itens"": 0
}";

        var result = JsonSerializer.Deserialize<Paginacao>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PaginaAtual, Is.EqualTo(0));
            Assert.That(result.TotalPaginas, Is.EqualTo(0));
            Assert.That(result.TamanhoPagina, Is.EqualTo(0));
            Assert.That(result.TotalItens, Is.EqualTo(0));
        });
    }

    [Test]
    public void WhenDeserializePaginacaoWithLargeNumbersThenMapsCorrectly()
    {
        var json = @"{
  ""pagina_atual"": 999,
  ""total_paginas"": 9999,
  ""tamanho_pagina"": 100,
  ""total_itens"": 999900
}";

        var result = JsonSerializer.Deserialize<Paginacao>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PaginaAtual, Is.EqualTo(999));
            Assert.That(result.TotalPaginas, Is.EqualTo(9999));
            Assert.That(result.TamanhoPagina, Is.EqualTo(100));
            Assert.That(result.TotalItens, Is.EqualTo(999900));
        });
    }

    [Test]
    public void WhenSerializePaginacaoThenProducesCorrectJson()
    {
        var paginacao = new Paginacao
        {
            PaginaAtual = 3,
            TotalPaginas = 10,
            TamanhoPagina = 25,
            TotalItens = 250
        };

        var json = JsonSerializer.Serialize(paginacao);
        var result = JsonSerializer.Deserialize<Paginacao>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PaginaAtual, Is.EqualTo(3));
            Assert.That(result.TotalPaginas, Is.EqualTo(10));
            Assert.That(result.TamanhoPagina, Is.EqualTo(25));
            Assert.That(result.TotalItens, Is.EqualTo(250));
        });
    }

    [Test]
    public void WhenCreatePaginacaoObjectThenAllPropertiesCanBeSet()
    {
        var paginacao = new Paginacao
        {
            PaginaAtual = 1,
            TotalPaginas = 5,
            TamanhoPagina = 10,
            TotalItens = 50
        };

        Assert.Multiple(() =>
        {
            Assert.That(paginacao.PaginaAtual, Is.EqualTo(1));
            Assert.That(paginacao.TotalPaginas, Is.EqualTo(5));
            Assert.That(paginacao.TamanhoPagina, Is.EqualTo(10));
            Assert.That(paginacao.TotalItens, Is.EqualTo(50));
        });
    }
}
