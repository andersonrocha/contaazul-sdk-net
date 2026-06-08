using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models.Pessoas;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class PessoasApiHttpContractTests
{
    private const string ClientId = "test-client-id";
    private const string ClientSecret = "test-client-secret";
    private const string ApiBaseUrl = "https://api-v2.contaazul.com";

    private sealed class CapturingHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _status;
        private readonly string _responseBody;

        public HttpMethod LastMethod { get; private set; }
        public Uri LastUri { get; private set; }
        public string LastBody { get; private set; }

        public CapturingHandler(HttpStatusCode status, string responseBody = "")
        {
            _status = status;
            _responseBody = responseBody;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastMethod = request.Method;
            LastUri = request.RequestUri;
            LastBody = request.Content == null
                ? null
                : await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new HttpResponseMessage
            {
                StatusCode = _status,
                Content = new StringContent(_responseBody ?? string.Empty, Encoding.UTF8, "application/json")
            };
        }
    }

    private static ContaAzulApiClient BuildClient(CapturingHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(ApiBaseUrl) };
        return new ContaAzulApiClient(
            ClientId, ClientSecret, "token", null,
            new ContaAzulApiClientOptions { BaseUrl = ApiBaseUrl, HttpClient = httpClient });
    }

    [Test]
    public void WhenConstructorThenCreatesInstanceWithPessoasApi()
    {
        using var client = new ContaAzulApiClient(ClientId, ClientSecret);
        Assert.That(client.Pessoas, Is.Not.Null);
    }

    // --- ObterPessoasAsync ---

    [Test]
    public async Task WhenObterPessoasThenSendsGetToPessoasEndpointWithFilters()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"totalItems\":1,\"items\":[{\"id\":\"p1\",\"nome\":\"Fulano\",\"documento\":\"123\"}]}");
        using var client = BuildClient(handler);

        var resposta = await client.Pessoas.ObterPessoasAsync(new PessoaFiltro
        {
            Busca = "Fulano",
            TipoPerfil = "Cliente",
            ComEndereco = true
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas"));
            Assert.That(handler.LastUri.Query, Does.Contain("busca=Fulano"));
            Assert.That(handler.LastUri.Query, Does.Contain("tipo_perfil=Cliente"));
            Assert.That(handler.LastUri.Query, Does.Contain("com_endereco=true"));
            Assert.That(resposta.TotalItems, Is.EqualTo(1));
            Assert.That(resposta.Items[0].Documento, Is.EqualTo("123"));
        });
    }

    // --- ObterPessoaPorIdAsync / Legado ---

    [Test]
    public async Task WhenObterPessoaPorIdThenSendsGetToPessoaByIdEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{\"id\":\"abc-123\",\"nome\":\"João\"}");
        using var client = BuildClient(handler);

        var pessoa = await client.Pessoas.ObterPessoaPorIdAsync("abc-123");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/abc-123"));
            Assert.That(pessoa.Id, Is.EqualTo("abc-123"));
            Assert.That(pessoa.Nome, Is.EqualTo("João"));
        });
    }

    [Test]
    public void WhenObterPessoaPorIdWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Pessoas.ObterPessoaPorIdAsync("  "));
    }

    [Test]
    public async Task WhenObterPessoaPorLegadoIdThenSendsGetToLegadoEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{\"id\":\"abc-123\"}");
        using var client = BuildClient(handler);

        await client.Pessoas.ObterPessoaPorLegadoIdAsync("12345");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/legado/12345"));
        });
    }

    // --- ObterEmpresaConectadaAsync ---

    [Test]
    public async Task WhenObterEmpresaConectadaThenSendsGetAndReturnsEmpresa()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "{\"razao_social\":\"Conta Azul Software Ltda\",\"documento\":\"05206246000138\"}");
        using var client = BuildClient(handler);

        var empresa = await client.Pessoas.ObterEmpresaConectadaAsync();

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/conta-conectada"));
            Assert.That(empresa.RazaoSocial, Is.EqualTo("Conta Azul Software Ltda"));
            Assert.That(empresa.Documento, Is.EqualTo("05206246000138"));
        });
    }

    // --- CriarPessoaAsync ---

    [Test]
    public async Task WhenCriarPessoaThenPostsBodyAndReturnsResumo()
    {
        var handler = new CapturingHandler(HttpStatusCode.Created,
            "{\"id\":\"new-id\",\"nome\":\"João Silva\",\"origem\":\"API\"}");
        using var client = BuildClient(handler);

        var resumo = await client.Pessoas.CriarPessoaAsync(new PessoaRequest
        {
            Nome = "João Silva",
            TipoPessoa = "Física",
            Cpf = "123.456.789-00",
            Perfis = new List<PerfilPessoa> { new PerfilPessoa { TipoPerfil = "Cliente" } }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas"));
            Assert.That(handler.LastBody, Does.Contain("\"nome\":"));
            Assert.That(handler.LastBody, Does.Contain("\"tipo_pessoa\":"));
            Assert.That(handler.LastBody, Does.Contain("\"cpf\":\"123.456.789-00\""));
            Assert.That(handler.LastBody, Does.Contain("\"tipo_perfil\":\"Cliente\""));
            // Perfil sem id na requisição: id nulo deve ser omitido.
            Assert.That(handler.LastBody, Does.Not.Contain("\"id\""));
            Assert.That(resumo.Id, Is.EqualTo("new-id"));
            Assert.That(resumo.Origem, Is.EqualTo("API"));
        });
    }

    [Test]
    public void WhenCriarPessoaWithNullThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.Created, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Pessoas.CriarPessoaAsync(null!));
    }

    // --- AtualizarPessoaAsync ---

    [Test]
    public async Task WhenAtualizarPessoaThenPutsBodyToPessoaByIdEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{\"id\":\"abc-123\",\"nome\":\"Novo Nome\"}");
        using var client = BuildClient(handler);

        var resumo = await client.Pessoas.AtualizarPessoaAsync("abc-123", new PessoaRequest
        {
            Nome = "Novo Nome",
            TipoPessoa = "Física"
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Put));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/abc-123"));
            Assert.That(handler.LastBody, Does.Contain("\"nome\":\"Novo Nome\""));
            Assert.That(resumo.Id, Is.EqualTo("abc-123"));
        });
    }

    [Test]
    public void WhenAtualizarPessoaWithEmptyIdThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "{}");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Pessoas.AtualizarPessoaAsync("", new PessoaRequest()));
    }

    // --- AtualizarParcialmentePessoaAsync ---

    [Test]
    public async Task WhenAtualizarParcialmentePessoaThenPatchesBodyAndExpectsNoContent()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        await client.Pessoas.AtualizarParcialmentePessoaAsync("abc-123", new AtualizacaoParcialPessoa
        {
            Email = "novo@email.com",
            OptanteSimplesNacional = true
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod.Method, Is.EqualTo("PATCH"));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/abc-123"));
            Assert.That(handler.LastBody, Does.Contain("\"email\":\"novo@email.com\""));
            Assert.That(handler.LastBody, Does.Contain("\"optante_simples_nacional\":true"));
        });
    }

    [Test]
    public void WhenAtualizarParcialmentePessoaWithNullBodyThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Pessoas.AtualizarParcialmentePessoaAsync("abc-123", null!));
    }

    // --- AtivarPessoasEmLoteAsync / InativarPessoasEmLoteAsync ---

    [Test]
    public async Task WhenAtivarPessoasEmLoteThenPostsToAtivarEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK,
            "[{\"ativos\":[\"a1\"],\"inativos\":[],\"todos\":[\"a1\"]}]");
        using var client = BuildClient(handler);

        var resultado = await client.Pessoas.AtivarPessoasEmLoteAsync(new PessoasEmLoteRequest
        {
            Uuids = new List<string> { "a1" }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/ativar"));
            Assert.That(handler.LastBody, Does.Contain("\"uuids\":[\"a1\"]"));
            Assert.That(resultado, Has.Count.EqualTo(1));
            Assert.That(resultado[0].Ativos, Is.EquivalentTo(new[] { "a1" }));
        });
    }

    [Test]
    public async Task WhenInativarPessoasEmLoteThenPostsToInativarEndpoint()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "[]");
        using var client = BuildClient(handler);

        await client.Pessoas.InativarPessoasEmLoteAsync(new PessoasEmLoteRequest
        {
            Uuids = new List<string> { "a1", "a2" }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/inativar"));
        });
    }

    [Test]
    public void WhenAtivarPessoasEmLoteWithEmptyUuidsThenThrowsArgumentException()
    {
        var handler = new CapturingHandler(HttpStatusCode.OK, "[]");
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentException>(
            async () => await client.Pessoas.AtivarPessoasEmLoteAsync(new PessoasEmLoteRequest()));
    }

    // --- ExcluirPessoasEmLoteAsync ---

    [Test]
    public async Task WhenExcluirPessoasEmLoteThenPostsToExcluirEndpointAndExpectsNoContent()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        await client.Pessoas.ExcluirPessoasEmLoteAsync(new PessoasEmLoteRequest
        {
            Uuids = new List<string> { "a1", "a2" }
        });

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/pessoas/excluir"));
            Assert.That(handler.LastBody, Does.Contain("\"uuids\""));
        });
    }

    [Test]
    public void WhenExcluirPessoasEmLoteWithNullThenThrowsArgumentNullException()
    {
        var handler = new CapturingHandler(HttpStatusCode.NoContent);
        using var client = BuildClient(handler);

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await client.Pessoas.ExcluirPessoasEmLoteAsync(null!));
    }
}
