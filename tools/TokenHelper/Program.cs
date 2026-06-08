using System.Diagnostics;
using ContaAzul.Sdk.Net;

// Utilitário interativo para obter os tokens OAuth do ContaAzul e imprimir os
// comandos `dotnet user-secrets` prontos para os testes de integração.
//
// Uso:
//   dotnet run --project tools/TokenHelper
// Os valores podem vir de variáveis de ambiente (CONTAAZUL_CLIENT_ID, CONTAAZUL_CLIENT_SECRET,
// CONTAAZUL_REDIRECT_URI, CONTAAZUL_SCOPE) ou serão solicitados no terminal.

const string DefaultScope = "openid profile aws.cognito.signin.user.admin";

var clientId = Prompt("Client ID", "CONTAAZUL_CLIENT_ID");
var clientSecret = Prompt("Client Secret", "CONTAAZUL_CLIENT_SECRET");
var redirectUri = Prompt("Redirect URI (idêntica à cadastrada no app)", "CONTAAZUL_REDIRECT_URI");
var scope = Environment.GetEnvironmentVariable("CONTAAZUL_SCOPE");
if (string.IsNullOrWhiteSpace(scope)) scope = DefaultScope;

if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret) || string.IsNullOrWhiteSpace(redirectUri))
{
    Console.Error.WriteLine("Client ID, Client Secret e Redirect URI são obrigatórios.");
    return 1;
}

var state = Guid.NewGuid().ToString("N");
var authUrl = ContaAzulOAuthHelper.BuildAuthorizationUrl(clientId, redirectUri, state, scope);

Console.WriteLine();
Console.WriteLine("1) Abra esta URL no navegador, faça login e autorize o acesso:");
Console.WriteLine();
Console.WriteLine(authUrl);
Console.WriteLine();
TryOpenBrowser(authUrl);

Console.WriteLine("2) Após autorizar, o navegador será redirecionado para a sua Redirect URI.");
Console.WriteLine("   Cole abaixo a URL COMPLETA de redirecionamento (ou apenas o valor do parâmetro 'code'):");
Console.Write("> ");
var input = Console.ReadLine()?.Trim() ?? string.Empty;

var code = ExtractCode(input);
if (string.IsNullOrWhiteSpace(code))
{
    Console.Error.WriteLine("Não foi possível extrair o 'code' da entrada informada.");
    return 1;
}

Console.WriteLine();
Console.WriteLine("Trocando o code por tokens...");

try
{
    using var client = new ContaAzulApiClient(clientId, clientSecret);
    var tokens = await client.AuthorizeAsync(code, redirectUri);

    Console.WriteLine();
    Console.WriteLine("=== Tokens obtidos ===");
    Console.WriteLine($"access_token:  {tokens.AccessToken}");
    Console.WriteLine($"refresh_token: {tokens.RefreshToken}");
    Console.WriteLine($"expires_in:    {tokens.ExpiresIn}s");
    Console.WriteLine();

    var secrets = new (string Key, string Value)[]
    {
        ("ContaAzul:ClientId", clientId),
        ("ContaAzul:ClientSecret", clientSecret),
        ("ContaAzul:RefreshToken", tokens.RefreshToken ?? string.Empty),
        ("ContaAzul:AccessToken", tokens.AccessToken ?? string.Empty),
    };

    var testProject = FindTestProject();
    Console.Write("Gravar automaticamente nos User Secrets do projeto de testes? [s/N] ");
    var resposta = Console.ReadLine()?.Trim();
    var gravar = resposta is "s" or "S" or "sim" or "Sim" or "y" or "Y";

    if (gravar && testProject is null)
    {
        Console.WriteLine("Projeto de testes não encontrado automaticamente. Use os comandos abaixo.");
        gravar = false;
    }

    if (gravar)
    {
        Console.WriteLine($"Gravando User Secrets em: {testProject}");
        var todosOk = true;
        foreach (var (key, value) in secrets)
        {
            var ok = RunUserSecrets(testProject!, key, value);
            Console.WriteLine(ok ? $"  ✓ {key}" : $"  ✗ {key} (falhou)");
            todosOk &= ok;
        }
        Console.WriteLine();
        Console.WriteLine(todosOk
            ? "User Secrets gravados. Rode: dotnet test --filter TestCategory=Integration"
            : "Alguns segredos falharam; rode manualmente os comandos abaixo.");
        if (todosOk) return 0;
    }

    Console.WriteLine();
    Console.WriteLine("=== Comandos prontos (User Secrets) ===");
    Console.WriteLine("cd contaazul-sdk-net-tests");
    foreach (var (key, value) in secrets)
    {
        Console.WriteLine($"dotnet user-secrets set \"{key}\" \"{value}\"");
    }
    Console.WriteLine();
    Console.WriteLine("Depois rode: dotnet test --filter TestCategory=Integration");
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Falha ao obter os tokens: {ex.Message}");
    return 1;
}

static string Prompt(string label, string envName)
{
    var fromEnv = Environment.GetEnvironmentVariable(envName);
    if (!string.IsNullOrWhiteSpace(fromEnv)) return fromEnv;

    Console.Write($"{label}: ");
    return Console.ReadLine()?.Trim() ?? string.Empty;
}

static string ExtractCode(string input)
{
    if (string.IsNullOrWhiteSpace(input)) return string.Empty;

    const string marker = "code=";
    var idx = input.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
    if (idx < 0)
    {
        // Assume que a própria entrada já é o code.
        return Uri.UnescapeDataString(input);
    }

    var rest = input.Substring(idx + marker.Length);
    var amp = rest.IndexOf('&');
    if (amp >= 0) rest = rest.Substring(0, amp);
    return Uri.UnescapeDataString(rest);
}

static void TryOpenBrowser(string url)
{
    try
    {
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }
    catch
    {
        // Abertura automática é best-effort; o usuário pode copiar a URL manualmente.
    }
}

// Localiza o csproj do projeto de testes subindo a partir do diretório do binário
// até encontrar a solução (contaazul-sdk.sln).
static string? FindTestProject()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir is not null)
    {
        if (File.Exists(Path.Combine(dir.FullName, "contaazul-sdk.sln")))
        {
            var csproj = Path.Combine(dir.FullName, "contaazul-sdk-net-tests", "contaazul-sdk-net-tests.csproj");
            return File.Exists(csproj) ? csproj : null;
        }
        dir = dir.Parent;
    }
    return null;
}

// Executa `dotnet user-secrets set <key> <value> --project <testProject>`.
static bool RunUserSecrets(string testProject, string key, string value)
{
    try
    {
        var psi = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        psi.ArgumentList.Add("user-secrets");
        psi.ArgumentList.Add("set");
        psi.ArgumentList.Add(key);
        psi.ArgumentList.Add(value);
        psi.ArgumentList.Add("--project");
        psi.ArgumentList.Add(testProject);

        using var proc = Process.Start(psi);
        if (proc is null) return false;
        proc.WaitForExit();
        return proc.ExitCode == 0;
    }
    catch
    {
        return false;
    }
}
