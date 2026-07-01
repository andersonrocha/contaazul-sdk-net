using System;

namespace ContaAzul.Sdk.Net.Tests.Integration;

/// <summary>
/// Base para os testes de integração ao vivo (batem na API real do ContaAzul).
/// <para>
/// Marcados como <c>[Explicit]</c> + <c>[Category("Integration")]</c>: não rodam no
/// <c>dotnet test</c> padrão. Para executar:
/// <code>dotnet test --filter TestCategory=Integration</code>
/// </para>
/// <para>
/// Todas as fixtures compartilham o MESMO cliente (ver <see cref="IntegrationSetup"/>), criado
/// uma única vez por execução. Isso é necessário por causa da rotação do refresh token do
/// ContaAzul: um cliente por fixture faria a primeira renovação invalidar o token das demais.
/// A configuração (seção <c>ContaAzul</c>) e o <c>AllowWrite</c> também vivem em
/// <see cref="IntegrationSetup"/>.
/// </para>
/// </summary>
[Explicit("Testes de integração ao vivo: requerem credenciais reais do ContaAzul.")]
[Category("Integration")]
public abstract class IntegrationTestBase
{
    /// <summary>Cliente autenticado, compartilhado por toda a execução dos testes de integração.</summary>
    protected IContaAzulApiClient Client => IntegrationSetup.Client!;

    /// <summary>Indica se os testes de escrita estão habilitados (<c>ContaAzul:AllowWrite = true</c>).</summary>
    protected static bool AllowWrite => IntegrationSetup.AllowWrite;

    [OneTimeSetUp]
    public void BaseOneTimeSetUp()
    {
        if (IntegrationSetup.SkipReason != null)
        {
            Assert.Ignore(IntegrationSetup.SkipReason);
        }
    }

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
