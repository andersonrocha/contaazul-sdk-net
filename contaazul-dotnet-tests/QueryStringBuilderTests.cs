using contaazul_dotnet.Helpers;
using contaazul_dotnet.Models;

namespace contaazul_dotnet_tests;

[TestFixture]
public class QueryStringBuilderTests
{
    [Test]
    public void WhenBuildQueryStringWithNullFilterThenReturnsEmptyString()
    {
        var result = QueryStringBuilder.BuildQueryString<PessoaFiltro>(null);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void WhenBuildQueryStringWithEmptyFilterThenReturnsDefaultPaginationParams()
    {
        var filtro = new PessoaFiltro();

        var result = QueryStringBuilder.BuildQueryString(filtro);

        Assert.That(result, Is.EqualTo("pagina=1&tamanho_pagina=10"));
    }

    [Test]
    public void WhenBuildQueryStringWithSingleParameterThenReturnsCorrectQueryString()
    {
        var filtro = new PessoaFiltro
        {
            Busca = "test"
        };

        var result = QueryStringBuilder.BuildQueryString(filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("busca=test"));
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
        });
    }

    [Test]
    public void WhenBuildQueryStringWithMultipleParametersThenReturnsCorrectQueryString()
    {
        var filtro = new PessoaFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Busca = "João"
        };

        var result = QueryStringBuilder.BuildQueryString(filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
            Assert.That(result, Does.Contain("busca=Jo%C3%A3o"));
            Assert.That(result.Split('&').Length, Is.EqualTo(3));
        });
    }

    [Test]
    public void WhenBuildQueryStringWithBooleanThenReturnsLowerCaseValue()
    {
        var filtro = new PessoaFiltro
        {
            ComEndereco = true
        };

        var result = QueryStringBuilder.BuildQueryString(filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("com_endereco=true"));
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
        });
    }

    [Test]
    public void WhenBuildQueryStringWithSpecialCharactersThenEncodesCorrectly()
    {
        var filtro = new PessoaFiltro
        {
            Busca = "João & Maria"
        };

        var result = QueryStringBuilder.BuildQueryString(filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("busca=Jo%C3%A3o%20%26%20Maria"));
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
        });
    }

    [Test]
    public void WhenBuildQueryStringWithAllParametersThenReturnsCompleteQueryString()
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

        var result = QueryStringBuilder.BuildQueryString(filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("pagina=1"));
            Assert.That(result, Does.Contain("tamanho_pagina=10"));
            Assert.That(result, Does.Contain("tipo_ordenacao=nome"));
            Assert.That(result, Does.Contain("ordem_ordenacao=asc"));
            Assert.That(result, Does.Contain("busca=test"));
            Assert.That(result, Does.Contain("com_endereco=true"));
            Assert.That(result.Split('&').Length, Is.EqualTo(21));
        });
    }

    [Test]
    public void WhenBuildQueryStringWithCustomPaginationThenIncludesCustomValues()
    {
        var filtroWithValue = new PessoaFiltro { Pagina = 5, TamanhoPagina = 20 };
        var filtroWithDefaultValue = new PessoaFiltro();

        var resultWithValue = QueryStringBuilder.BuildQueryString(filtroWithValue);
        var resultWithDefaultValue = QueryStringBuilder.BuildQueryString(filtroWithDefaultValue);

        Assert.Multiple(() =>
        {
            Assert.That(resultWithValue, Does.Contain("pagina=5"));
            Assert.That(resultWithValue, Does.Contain("tamanho_pagina=20"));
            Assert.That(resultWithDefaultValue, Does.Contain("pagina=1"));
            Assert.That(resultWithDefaultValue, Does.Contain("tamanho_pagina=10"));
        });
    }

    [Test]
    public void WhenBuildQueryStringWithEmptyStringThenExcludesParameter()
    {
        var filtro = new PessoaFiltro
        {
            Busca = "",
            TiposPessoa = "FISICA"
        };

        var result = QueryStringBuilder.BuildQueryString(filtro);

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Not.Contain("busca="));
            Assert.That(result, Does.Contain("tipos_pessoa=FISICA"));
        });
    }
}
