using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class FiltroBaseTests
{
    private class TestFiltro : FiltroBase
    {
        // Classe de teste concreta para testar a classe base
    }

    [Test]
    public void WhenCreateFiltroWithoutSetValuesThenUsesDefaultValues()
    {
        var filtro = new TestFiltro();

        Assert.Multiple(() =>
        {
            Assert.That(filtro.Pagina, Is.EqualTo(1));
            Assert.That(filtro.TamanhoPagina, Is.EqualTo(10));
        });
    }

    [Test]
    public void WhenSetPaginaThenReturnsSetValue()
    {
        var filtro = new TestFiltro { Pagina = 5 };

        Assert.That(filtro.Pagina, Is.EqualTo(5));
    }

    [Test]
    public void WhenSetTamanhoPaginaThenReturnsSetValue()
    {
        var filtro = new TestFiltro { TamanhoPagina = 20 };

        Assert.That(filtro.TamanhoPagina, Is.EqualTo(20));
    }

    [Test]
    public void WhenSetPaginaToNullThenReturnsDefaultValue()
    {
        var filtro = new TestFiltro { Pagina = null };

        Assert.That(filtro.Pagina, Is.EqualTo(1));
    }

    [Test]
    public void WhenSetTamanhoPaginaToNullThenReturnsDefaultValue()
    {
        var filtro = new TestFiltro { TamanhoPagina = null };

        Assert.That(filtro.TamanhoPagina, Is.EqualTo(10));
    }

    [Test]
    public void WhenPessoaFiltroInheritsFiltroBaseThenHasDefaultValues()
    {
        var filtro = new PessoaFiltro();

        Assert.Multiple(() =>
        {
            Assert.That(filtro.Pagina, Is.EqualTo(1));
            Assert.That(filtro.TamanhoPagina, Is.EqualTo(10));
        });
    }

    [Test]
    public void WhenVendaFiltroInheritsFiltroBaseThenHasDefaultValues()
    {
        var filtro = new VendaFiltro();

        Assert.Multiple(() =>
        {
            Assert.That(filtro.Pagina, Is.EqualTo(1));
            Assert.That(filtro.TamanhoPagina, Is.EqualTo(10));
        });
    }

    [Test]
    public void WhenChangePaginaAndTamanhoPaginaThenBothReturnNewValues()
    {
        var filtro = new TestFiltro
        {
            Pagina = 3,
            TamanhoPagina = 50
        };

        Assert.Multiple(() =>
        {
            Assert.That(filtro.Pagina, Is.EqualTo(3));
            Assert.That(filtro.TamanhoPagina, Is.EqualTo(50));
        });
    }
}
