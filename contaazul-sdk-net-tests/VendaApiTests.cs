using System;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class VendaApiTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    [Test]
    public void WhenConstructorThenCreatesInstanceWithVendasApi()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.That(client.Vendas, Is.Not.Null);
    }

    [Test]
    public void WhenCreateVendaFiltroWithAllParametersThenAllPropertiesSet()
    {
        var filtro = new VendaFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            CampoOrdenadoAscendente = "numero",
            CampoOrdenadoDescendente = "data",
            TermoBusca = "test",
            DataInicio = "2024-01-01",
            DataFim = "2024-12-31",
            DataCriacaoDe = "2024-01-01",
            DataCriacaoAte = "2024-12-31",
            DataAlteracaoDe = "2024-01-01",
            DataAlteracaoAte = "2024-12-31",
            IdsVendedores = "id1,id2",
            IdsClientes = "id3,id4",
            IdsNaturezaOperacao = "id5",
            Situacoes = "APROVADA",
            Tipos = "VENDA",
            Origens = "MANUAL",
            Numeros = 123,
            IdsCategorias = "cat1",
            IdsProdutos = "prod1",
            Pendente = true,
            Totais = "total",
            IdsLegadoDonos = 1,
            IdsLegadoClientes = 2,
            IdsLegadoProdutos = 3,
            IdsLegadoCategorias = 4
        };

        Assert.Multiple(() =>
        {
            Assert.That(filtro.Pagina, Is.EqualTo(1));
            Assert.That(filtro.TamanhoPagina, Is.EqualTo(10));
            Assert.That(filtro.TermoBusca, Is.EqualTo("test"));
            Assert.That(filtro.DataInicio, Is.EqualTo("2024-01-01"));
            Assert.That(filtro.Pendente, Is.True);
        });
    }
}

