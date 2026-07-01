using System;
using ContaAzul.Sdk.Net;
using Microsoft.Extensions.Configuration;

namespace ContaAzul.Sdk.Net.Tests.Integration;

/// <summary>
/// Inicializa UM único <see cref="IContaAzulApiClient"/> compartilhado por toda a execução dos
/// testes de integração. Roda uma vez por causa do <c>[SetUpFixture]</c> aplicado ao namespace.
/// <para>
/// Isso é essencial porque o ContaAzul <b>rotaciona o refresh token a cada renovação</b> (cada
/// refresh invalida o token anterior). Se cada fixture criasse o próprio cliente a partir do
/// mesmo refresh token do config, a primeira renovação invalidaria o token para todas as demais
/// fixtures — resultando em uma cascata de erros <c>invalid_grant</c>. Com um cliente único, a
/// renovação ocorre no máximo uma vez e o novo token permanece em memória para todos os testes.
/// </para>
/// <para>
/// Configuração na seção <c>ContaAzul</c>, resolvida nesta ordem (a última vence):
/// <c>appsettings.Integration.json</c> → User Secrets → variáveis de ambiente. Chaves:
/// <c>ClientId</c>, <c>ClientSecret</c>, <c>AccessToken</c>, <c>RefreshToken</c>,
/// <c>ApiBaseUrl</c> (opcional) e <c>AllowWrite</c>.
/// </para>
/// </summary>
[SetUpFixture]
public class IntegrationSetup
{
    private static readonly IConfiguration Configuration =
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Integration.json", optional: true, reloadOnChange: false)
            .AddUserSecrets(typeof(IntegrationSetup).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

    /// <summary>Cliente compartilhado; <see langword="null"/> quando as credenciais não estão configuradas.</summary>
    public static IContaAzulApiClient? Client { get; private set; }

    /// <summary>Motivo para pular os testes (credenciais ausentes), ou <see langword="null"/>.</summary>
    public static string? SkipReason { get; private set; }

    /// <summary>Indica se os testes de escrita estão habilitados (<c>ContaAzul:AllowWrite = true</c>).</summary>
    public static bool AllowWrite =>
        string.Equals(Configuration["ContaAzul:AllowWrite"], "true", StringComparison.OrdinalIgnoreCase);

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        var clientId = Configuration["ContaAzul:ClientId"];
        var clientSecret = Configuration["ContaAzul:ClientSecret"];
        var accessToken = Configuration["ContaAzul:AccessToken"];
        var refreshToken = Configuration["ContaAzul:RefreshToken"];

        if (string.IsNullOrWhiteSpace(clientId) ||
            string.IsNullOrWhiteSpace(clientSecret) ||
            (string.IsNullOrWhiteSpace(accessToken) && string.IsNullOrWhiteSpace(refreshToken)))
        {
            SkipReason =
                "Credenciais de integração ausentes. Configure a seção 'ContaAzul' " +
                "(appsettings.Integration.json, User Secrets ou variáveis de ambiente) com " +
                "ClientId, ClientSecret e AccessToken e/ou RefreshToken.";
            return;
        }

        var options = new ContaAzulApiClientOptions();
        var baseUrl = Configuration["ContaAzul:ApiBaseUrl"];
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            options.BaseUrl = baseUrl;
        }

        var client = new ContaAzulApiClient(clientId, clientSecret, accessToken, refreshToken, options);

        // O ContaAzul rotaciona o refresh token a cada renovação; registramos o novo valor no log
        // para que possa ser persistido no config antes da próxima execução, se necessário.
        client.TokenRefreshed += (_, e) =>
            TestContext.Progress.WriteLine(
                $"[ContaAzul] Tokens renovados. Novo refresh token: {e.RefreshToken}. Expira em: {e.TokenExpiresAt:O}");

        Client = client;
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
        Client?.Dispose();
        Client = null;
    }
}
