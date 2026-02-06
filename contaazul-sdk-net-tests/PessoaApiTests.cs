using System;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class PessoaApiTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";

    [Test]
    public void WhenConstructorThenCreatesInstanceWithPessoasApi()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.That(client.Pessoas, Is.Not.Null);
    }
}

