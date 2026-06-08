using System;
using ContaAzul.Sdk.Net;
using Microsoft.Extensions.Configuration;

namespace ContaAzul.Sdk.Net.Tests.Integration;

/// <summary>
/// Base para os testes de integração ao vivo (batem na API real do ContaAzul).
/// <para>
/// Marcados como <c>[Explicit]</c> + <c>[Category("Integration")]</c>: não rodam no
/// <c>dotnet test</c> padrão. Para executar:
/// <code>dotnet test --filter TestCategory=Integration</code>
/// </para>
/// <para>
/// Configuração na seção <c>ContaAzul</c>, resolvida nesta ordem (a última vence):
/// <list type="number">
/// <item><c>appsettings.Integration.json</c> (opcional; no .gitignore)</item>
/// <item>User Secrets (<c>dotnet user-secrets</c>)</item>
/// <item>Variáveis de ambiente (<c>ContaAzul__ClientId</c>, <c>ContaAzul__ClientSecret</c>, ...)</item>
/// </list>
/// Chaves: <c>ClientId</c>, <c>ClientSecret</c>, <c>AccessToken</c>, <c>RefreshToken</c>,
/// <c>ApiBaseUrl</c> (opcional) e <c>AllowWrite</c> (habilita os testes de escrita).
/// Se as credenciais não estiverem presentes, a fixture é ignorada (<c>Assert.Ignore</c>).
/// </para>
/// </summary>
[Explicit("Testes de integração ao vivo: requerem credenciais reais do ContaAzul.")]
[Category("Integration")]
public abstract class IntegrationTestBase
{
    private static readonly IConfiguration Configuration = BuildConfiguration();

    /// <summary>Cliente autenticado disponível para os testes.</summary>
    protected IContaAzulApiClient Client { get; private set; } = null!;

    /// <summary>Indica se os testes de escrita estão habilitados (<c>ContaAzul:AllowWrite = true</c>).</summary>
    protected static bool AllowWrite =>
        string.Equals(Configuration["ContaAzul:AllowWrite"], "true", StringComparison.OrdinalIgnoreCase);

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Integration.json", optional: true, reloadOnChange: false)
            .AddUserSecrets(typeof(IntegrationTestBase).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

    [OneTimeSetUp]
    public void BaseOneTimeSetUp()
    {
        var clientId = Configuration["ContaAzul:ClientId"];
        var clientSecret = Configuration["ContaAzul:ClientSecret"];
        var accessToken = Configuration["ContaAzul:AccessToken"];
        var refreshToken = Configuration["ContaAzul:RefreshToken"];

        if (string.IsNullOrWhiteSpace(clientId) ||
            string.IsNullOrWhiteSpace(clientSecret) ||
            (string.IsNullOrWhiteSpace(accessToken) && string.IsNullOrWhiteSpace(refreshToken)))
        {
            Assert.Ignore(
                "Credenciais de integração ausentes. Configure a seção 'ContaAzul' " +
                "(appsettings.Integration.json, User Secrets ou variáveis de ambiente) com " +
                "ClientId, ClientSecret e AccessToken e/ou RefreshToken.");
        }

        var options = new ContaAzulApiClientOptions();
        var baseUrl = Configuration["ContaAzul:ApiBaseUrl"];
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            options.BaseUrl = baseUrl;
        }

        Client = new ContaAzulApiClient(clientId, clientSecret, accessToken, refreshToken, options);

        // O ContaAzul rotaciona o refresh token a cada renovação; registramos os novos
        // valores no log do teste para que possam ser persistidos manualmente, se necessário.
        Client.TokenRefreshed += (_, e) =>
            TestContext.Progress.WriteLine(
                $"[ContaAzul] Tokens renovados. Novo refresh token: {e.RefreshToken}. Expira em: {e.TokenExpiresAt:O}");
    }

    [OneTimeTearDown]
    public void BaseOneTimeTearDown() => Client?.Dispose();

    /// <summary>Pula o teste atual quando a escrita não está habilitada.</summary>
    protected static void RequireWrite()
    {
        if (!AllowWrite)
        {
            Assert.Ignore("Escrita desabilitada. Defina ContaAzul:AllowWrite = true para executar este teste.");
        }
    }

    /// <summary>Data (YYYY-MM-DD) deslocada de <paramref name="dias"/> a partir de hoje (UTC).</summary>
    protected static string DataRelativa(int dias) =>
        DateTime.UtcNow.Date.AddDays(dias).ToString("yyyy-MM-dd");
}
