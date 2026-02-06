using System;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class NotasFiscaisApiTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    [Test]
    public void WhenConstructorThenCreatesInstanceWithNotasFiscaisApi()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.That(client.NotasFiscais, Is.Not.Null);
    }

    [Test]
    public void WhenCreateNotaFiscalFiltroWithAllParametersThenAllPropertiesSet()
    {
        var filtro = new NotaFiscalFiltro
        {
            Pagina = 1,
            TamanhoPagina = 10,
            DataCompetenciaDe = "2024-01-01",
            DataCompetenciaAte = "2024-01-15",
            Ids = "id1,id2",
            IdCliente = "cliente123",
            NumeroVenda = 1001,
            NumeroNfseInicial = 100,
            NumeroNfseFinal = 200,
            NumeroRpsInicial = 1000,
            NumeroRpsFinal = 2000,
            Status = "PENDENTE",
            TipoNegociacao = "VENDA"
        };

        Assert.Multiple(() =>
        {
            Assert.That(filtro.Pagina, Is.EqualTo(1));
            Assert.That(filtro.TamanhoPagina, Is.EqualTo(10));
            Assert.That(filtro.DataCompetenciaDe, Is.EqualTo("2024-01-01"));
            Assert.That(filtro.DataCompetenciaAte, Is.EqualTo("2024-01-15"));
            Assert.That(filtro.Ids, Is.EqualTo("id1,id2"));
            Assert.That(filtro.IdCliente, Is.EqualTo("cliente123"));
            Assert.That(filtro.NumeroVenda, Is.EqualTo(1001));
            Assert.That(filtro.NumeroNfseInicial, Is.EqualTo(100));
            Assert.That(filtro.NumeroNfseFinal, Is.EqualTo(200));
            Assert.That(filtro.NumeroRpsInicial, Is.EqualTo(1000));
            Assert.That(filtro.NumeroRpsFinal, Is.EqualTo(2000));
            Assert.That(filtro.Status, Is.EqualTo("PENDENTE"));
            Assert.That(filtro.TipoNegociacao, Is.EqualTo("VENDA"));
        });
    }
}
