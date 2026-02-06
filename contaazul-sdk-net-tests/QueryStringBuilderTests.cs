using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class QueryStringBuilderTests
{
    private const string TestEndpoint = "/v1/test";

    [Test]
    public void WhenBuildEndpointWithNullFilterThenReturnsOnlyEndpoint()
    {
        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, (PessoaFiltro)null);

        Assert.That(result, Is.EqualTo(TestEndpoint));
    }

    [Test]
    public void WhenBuildEndpointWithEmptyFilterThenReturnsEndpointWithDefaultPaginationParams()
    {
        var filtro = new PessoaFiltro();

        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtro);

        Assert.That(result, Is.EqualTo($"{TestEndpoint}?pagina=1&tamanho_pagina=10"));
    }

    [Test]
    public void WhenBuildEndpointWithSingleParameterThenReturnsCorrectEndpoint()
    {
        var filtro = new PessoaFiltro
        {
            Busca = "test"
        };

        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("busca=test"));
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
            Assert.That(result, Does.StartWith($"{TestEndpoint}?"));
        });
    }

    [Test]
    public void WhenBuildEndpointWithMultipleParametersThenReturnsCorrectEndpoint()
    {
        var filtro = new PessoaFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Busca = "João"
        };

        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
            Assert.That(result, Does.Contain("busca=Jo%C3%A3o"));
            Assert.That(result, Does.StartWith($"{TestEndpoint}?"));
        });
    }

    [Test]
    public void WhenBuildEndpointWithBooleanThenReturnsLowerCaseValue()
    {
        var filtro = new PessoaFiltro
        {
            ComEndereco = true
        };

        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("com_endereco=true"));
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
            Assert.That(result, Does.StartWith($"{TestEndpoint}?"));
        });
    }

    [Test]
    public void WhenBuildEndpointWithSpecialCharactersThenEncodesCorrectly()
    {
        var filtro = new PessoaFiltro
        {
            Busca = "João & Maria"
        };

        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("busca=Jo%C3%A3o%20%26%20Maria"));
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
            Assert.That(result, Does.StartWith($"{TestEndpoint}?"));
        });
    }

    [Test]
    public void WhenBuildEndpointWithAllParametersThenReturnsCompleteEndpoint()
    {
        var filtro = new PessoaFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            TipoOrdenacao = "nome",
            OrdemOrdenacao = "asc",
            Busca = "test",
            Ids = "1,2,3",
            Documentos = "123456789",
            Paises = "Brasil",
            Cidades = "São Paulo",
            Ufs = "SP",
            CodigosPessoa = "C001",
            Emails = "test@example.com",
            TiposPessoa = "FISICA",
            Nomes = "João",
            Telefones = "11999999999",
            DataCriacaoInicio = "2024-01-01",
            DataCriacaoFim = "2024-12-31",
            DataAlteracaoDe = "2024-01-01",
            DataAlteracaoAte = "2024-12-31",
            TipoPerfil = "CLIENTE",
            ComEndereco = true
        };

        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
            Assert.That(result, Does.Contain("tipo_ordenacao=nome"));
            Assert.That(result, Does.Contain("ordem_ordenacao=asc"));
            Assert.That(result, Does.Contain("busca=test"));
            Assert.That(result, Does.Contain("com_endereco=true"));
            Assert.That(result, Does.StartWith($"{TestEndpoint}?"));
        });
    }

    [Test]
    public void WhenBuildEndpointWithCustomPaginationThenIncludesCustomValues()
    {
        var filtroWithValue = new PessoaFiltro { Pagina = 5, TamanhoPagina = 20 };
        var filtroWithDefaultValue = new PessoaFiltro();

        var resultWithValue = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtroWithValue);
        var resultWithDefaultValue = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtroWithDefaultValue);

        Assert.Multiple(() =>
        {
            Assert.That(resultWithValue, Does.Contain("pagina=5"));
            Assert.That(resultWithValue, Does.Contain("tamanho_pagina=20"));
            Assert.That(resultWithDefaultValue, Does.Contain("pagina=1"));
            Assert.That(resultWithDefaultValue, Does.Contain("tamanho_pagina=10"));
        });
    }

    [Test]
    public void WhenBuildEndpointWithEmptyStringThenExcludesParameter()
    {
        var filtro = new PessoaFiltro
        {
            Busca = "",
            TiposPessoa = "FISICA"
        };

        var result = QueryStringBuilder.BuildEndpoint(TestEndpoint, filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Not.Contain("busca="));
            Assert.That(result, Does.Contain("tipos_pessoa=FISICA"));
            Assert.That(result, Does.StartWith($"{TestEndpoint}?"));
        });
    }

    [Test]
    public void WhenBuildEndpointWithNullOrEmptyEndpointThenThrowsArgumentException()
    {
        var filtro = new PessoaFiltro();

        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentException>(() => QueryStringBuilder.BuildEndpoint(null, filtro));
            Assert.Throws<ArgumentException>(() => QueryStringBuilder.BuildEndpoint("", filtro));
            Assert.Throws<ArgumentException>(() => QueryStringBuilder.BuildEndpoint("   ", filtro));
        });
    }
}
