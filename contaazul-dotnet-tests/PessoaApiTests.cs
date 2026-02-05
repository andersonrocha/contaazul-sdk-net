using System;
using contaazul_dotnet;
using contaazul_dotnet.Models;

namespace contaazul_dotnet_tests;

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

    [Test]
    public void WhenGetPessoaByIdAsyncWithNullIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.GetPessoaByIdAsync(null!));
    }

    [Test]
    public void WhenGetPessoaByIdAsyncWithEmptyIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.GetPessoaByIdAsync(string.Empty));
    }

    [Test]
    public void WhenGetPessoaByIdAsyncWithWhitespaceIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.GetPessoaByIdAsync("   "));
    }

    [Test]
    public void WhenCreatePessoaAsyncWithNullPessoaThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.CreatePessoaAsync(null!));
    }

    [Test]
    public void WhenUpdatePessoaAsyncWithNullIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        var pessoa = new Pessoa { Nome = "Test" };

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.UpdatePessoaAsync(null!, pessoa));
    }

    [Test]
    public void WhenUpdatePessoaAsyncWithEmptyIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);
        var pessoa = new Pessoa { Nome = "Test" };

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.UpdatePessoaAsync(string.Empty, pessoa));
    }

    [Test]
    public void WhenUpdatePessoaAsyncWithNullPessoaThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.UpdatePessoaAsync("id", null!));
    }

    [Test]
    public void WhenDeletePessoaAsyncWithNullIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.DeletePessoaAsync(null!));
    }

    [Test]
    public void WhenDeletePessoaAsyncWithEmptyIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.DeletePessoaAsync(string.Empty));
    }

    [Test]
    public void WhenDeletePessoaAsyncWithWhitespaceIdThenThrowsArgumentNullException()
    {
        var client = new ContaAzulApiClient(ClientId, ClientSecret);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await client.Pessoas.DeletePessoaAsync("   "));
    }
}

