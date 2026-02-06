# Exemplos de Uso Avançado - ContaAzul.Sdk.Net

Este documento contém exemplos práticos e avançados de uso do SDK ContaAzul.Sdk.Net.

## ?? Índice

1. [Configuração Inicial](#configuração-inicial)
2. [Gerenciamento de Tokens](#gerenciamento-de-tokens)
3. [Uso com Dependency Injection](#uso-com-dependency-injection)
4. [Paginação Automática](#paginação-automática)
5. [Tratamento de Erros](#tratamento-de-erros)
6. [Exemplos Completos](#exemplos-completos)

## Configuração Inicial

### Armazenamento Seguro de Tokens

```csharp
using ContaAzul.Sdk.Net;
using Microsoft.Extensions.Configuration;

public class ContaAzulService
{
    private readonly IConfiguration _configuration;
    private ContaAzulApiClient _client;
    
    public ContaAzulService(IConfiguration configuration)
    {
        _configuration = configuration;
        InitializeClient();
    }
    
    private void InitializeClient()
    {
        var clientId = _configuration["ContaAzul:ClientId"];
        var clientSecret = _configuration["ContaAzul:ClientSecret"];
        var accessToken = _configuration["ContaAzul:AccessToken"];
        var refreshToken = _configuration["ContaAzul:RefreshToken"];
        
        _client = new ContaAzulApiClient(
            clientId, 
            clientSecret, 
            accessToken, 
            refreshToken
        );
    }
}
```

### appsettings.json

```json
{
  "ContaAzul": {
    "ClientId": "seu-client-id",
    "ClientSecret": "seu-client-secret",
    "AccessToken": "",
    "RefreshToken": "",
    "RedirectUri": "https://seu-app.com/callback"
  }
}
```

## Gerenciamento de Tokens

### Classe Helper para Gerenciar Tokens

```csharp
using System;
using System.IO;
using System.Text.Json;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

public class TokenManager
{
    private const string TokenFilePath = "contaazul_tokens.json";
    
    public class TokenData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
    
    public static TokenData LoadTokens()
    {
        if (!File.Exists(TokenFilePath))
            return null;
            
        var json = File.ReadAllText(TokenFilePath);
        return JsonSerializer.Deserialize<TokenData>(json);
    }
    
    public static void SaveTokens(string accessToken, string refreshToken, int expiresIn)
    {
        var tokenData = new TokenData
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn)
        };
        
        var json = JsonSerializer.Serialize(tokenData, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        
        File.WriteAllText(TokenFilePath, json);
    }
    
    public static bool IsTokenExpired(TokenData tokenData)
    {
        return tokenData == null || DateTime.UtcNow >= tokenData.ExpiresAt.AddMinutes(-5);
    }
}

// Uso
public class ContaAzulAuthService
{
    private ContaAzulApiClient _client;
    
    public async Task<ContaAzulApiClient> GetAuthenticatedClientAsync()
    {
        var tokens = TokenManager.LoadTokens();
        
        if (tokens == null)
        {
            throw new InvalidOperationException(
                "Tokens não encontrados. Execute o fluxo de autenticação primeiro."
            );
        }
        
        _client = new ContaAzulApiClient(
            "client-id",
            "client-secret",
            tokens.AccessToken,
            tokens.RefreshToken
        );
        
        // Verifica se o token expirou
        if (TokenManager.IsTokenExpired(tokens))
        {
            var newTokens = await _client.RefreshTokenAsync();
            TokenManager.SaveTokens(
                newTokens.AccessToken,
                newTokens.RefreshToken,
                newTokens.ExpiresIn
            );
        }
        
        return _client;
    }
}
```

## Uso com Dependency Injection

### ASP.NET Core

```csharp
// Startup.cs ou Program.cs
using Microsoft.Extensions.DependencyInjection;

public void ConfigureServices(IServiceCollection services)
{
    // Registrar como Singleton
    services.AddSingleton<IContaAzulService, ContaAzulService>();
    
    // Ou registrar com factory
    services.AddSingleton<ContaAzulApiClient>(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        return new ContaAzulApiClient(
            config["ContaAzul:ClientId"],
            config["ContaAzul:ClientSecret"],
            config["ContaAzul:AccessToken"],
            config["ContaAzul:RefreshToken"]
        );
    });
}

// Interface do serviço
public interface IContaAzulService
{
    Task<PessoaListResponse> GetPessoasAsync(PessoaFiltro filtro = null);
    Task<VendaListResponse> GetVendasAsync(VendaFiltro filtro = null);
    Task<NotaFiscalListResponse> GetNotasFiscaisAsync(NotaFiscalFiltro filtro = null);
}

// Implementação
public class ContaAzulService : IContaAzulService
{
    private readonly ContaAzulApiClient _client;
    
    public ContaAzulService(ContaAzulApiClient client)
    {
        _client = client;
    }
    
    public async Task<PessoaListResponse> GetPessoasAsync(PessoaFiltro filtro = null)
    {
        return await _client.Pessoas.GetPessoasAsync(filtro);
    }
    
    public async Task<VendaListResponse> GetVendasAsync(VendaFiltro filtro = null)
    {
        return await _client.Vendas.GetVendasAsync(filtro);
    }
    
    public async Task<NotaFiscalListResponse> GetNotasFiscaisAsync(NotaFiscalFiltro filtro = null)
    {
        return await _client.NotasFiscais.GetNotasFiscaisAsync(filtro);
    }
}

// Controller
[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IContaAzulService _contaAzulService;
    
    public PessoasController(IContaAzulService contaAzulService)
    {
        _contaAzulService = contaAzulService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PessoaFiltro filtro)
    {
        try
        {
            var pessoas = await _contaAzulService.GetPessoasAsync(filtro);
            return Ok(pessoas);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
```

## Paginação Automática

### Buscar Todos os Registros (Todas as Páginas)

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

public class PaginacaoHelper
{
    public static async Task<List<Pessoa>> GetTodasPessoasAsync(
        ContaAzulApiClient client,
        PessoaFiltro filtroBase = null)
    {
        var todasPessoas = new List<Pessoa>();
        var paginaAtual = 1;
        var continuar = true;
        
        while (continuar)
        {
            var filtro = filtroBase ?? new PessoaFiltro();
            filtro.Pagina = paginaAtual;
            filtro.TamanhoPagina = 100; // Máximo por página
            
            var resultado = await client.Pessoas.GetPessoasAsync(filtro);
            
            if (resultado.Data != null && resultado.Data.Count > 0)
            {
                todasPessoas.AddRange(resultado.Data);
                
                // Verifica se há mais páginas
                continuar = paginaAtual < resultado.TotalPaginas;
                paginaAtual++;
            }
            else
            {
                continuar = false;
            }
            
            // Aguarda um pouco para evitar rate limiting
            await Task.Delay(100);
        }
        
        return todasPessoas;
    }
}

// Uso
var client = new ContaAzulApiClient("client-id", "client-secret", "token", "refresh");
var todasPessoas = await PaginacaoHelper.GetTodasPessoasAsync(client);
Console.WriteLine($"Total de pessoas: {todasPessoas.Count}");
```

### Paginação com IAsyncEnumerable (.NET Core 3.0+)

```csharp
public class PaginacaoAsyncHelper
{
    public static async IAsyncEnumerable<Pessoa> GetPessoasAsyncEnumerable(
        ContaAzulApiClient client,
        PessoaFiltro filtroBase = null)
    {
        var paginaAtual = 1;
        var continuar = true;
        
        while (continuar)
        {
            var filtro = filtroBase ?? new PessoaFiltro();
            filtro.Pagina = paginaAtual;
            filtro.TamanhoPagina = 100;
            
            var resultado = await client.Pessoas.GetPessoasAsync(filtro);
            
            if (resultado.Data != null && resultado.Data.Count > 0)
            {
                foreach (var pessoa in resultado.Data)
                {
                    yield return pessoa;
                }
                
                continuar = paginaAtual < resultado.TotalPaginas;
                paginaAtual++;
                
                await Task.Delay(100);
            }
            else
            {
                continuar = false;
            }
        }
    }
}

// Uso com await foreach
var client = new ContaAzulApiClient("client-id", "client-secret", "token", "refresh");

await foreach (var pessoa in PaginacaoAsyncHelper.GetPessoasAsyncEnumerable(client))
{
    Console.WriteLine($"{pessoa.Id} - {pessoa.Nome}");
    
    // Processar cada pessoa individualmente
    // Útil para grandes volumes de dados
}
```

## Tratamento de Erros

### Wrapper com Retry e Circuit Breaker (Polly)

```csharp
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class ContaAzulResilientService
{
    private readonly ContaAzulApiClient _client;
    private readonly AsyncPolicy<PessoaListResponse> _retryPolicy;
    
    public ContaAzulResilientService(ContaAzulApiClient client)
    {
        _client = client;
        
        // Política de retry com exponential backoff
        _retryPolicy = Policy<PessoaListResponse>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                3, 
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Tentativa {retryCount} após {timespan.TotalSeconds}s");
                }
            );
    }
    
    public async Task<PessoaListResponse> GetPessoasComRetryAsync(PessoaFiltro filtro = null)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            return await _client.Pessoas.GetPessoasAsync(filtro);
        });
    }
}
```

### Tratamento Abrangente de Erros

```csharp
public class ContaAzulServiceWithErrorHandling
{
    private readonly ContaAzulApiClient _client;
    private readonly ILogger<ContaAzulServiceWithErrorHandling> _logger;
    
    public async Task<Result<PessoaListResponse>> GetPessoasSeguroAsync(
        PessoaFiltro filtro = null)
    {
        try
        {
            var pessoas = await _client.Pessoas.GetPessoasAsync(filtro);
            return Result<PessoaListResponse>.Success(pessoas);
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("401"))
        {
            _logger.LogWarning("Token inválido ou expirado. Tentando renovar...");
            
            try
            {
                await _client.RefreshTokenAsync();
                var pessoas = await _client.Pessoas.GetPessoasAsync(filtro);
                return Result<PessoaListResponse>.Success(pessoas);
            }
            catch (Exception refreshEx)
            {
                _logger.LogError(refreshEx, "Falha ao renovar token");
                return Result<PessoaListResponse>.Failure(
                    "Não foi possível renovar o token. Reautenticação necessária."
                );
            }
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("429"))
        {
            _logger.LogWarning("Rate limit atingido");
            return Result<PessoaListResponse>.Failure("Muitas requisições. Tente novamente mais tarde.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro na requisição HTTP");
            return Result<PessoaListResponse>.Failure($"Erro na comunicação: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado");
            return Result<PessoaListResponse>.Failure("Erro inesperado. Tente novamente.");
        }
    }
}

// Classe Result
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public string ErrorMessage { get; }
    
    private Result(bool isSuccess, T data, string errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }
    
    public static Result<T> Success(T data) => new Result<T>(true, data, null);
    public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
}
```

## Exemplos Completos

### Sincronização de Clientes do ContaAzul para Banco Local

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

public class SincronizacaoService
{
    private readonly ContaAzulApiClient _client;
    private readonly ApplicationDbContext _dbContext;
    
    public async Task SincronizarClientesAsync()
    {
        Console.WriteLine("Iniciando sincronização de clientes...");
        
        var filtro = new PessoaFiltro
        {
            TipoPerfil = "CLIENTE",
            TamanhoPagina = 100
        };
        
        var paginaAtual = 1;
        var totalSincronizados = 0;
        
        while (true)
        {
            filtro.Pagina = paginaAtual;
            var resultado = await _client.Pessoas.GetPessoasAsync(filtro);
            
            if (resultado.Data == null || !resultado.Data.Any())
                break;
            
            foreach (var pessoaContaAzul in resultado.Data)
            {
                var clienteLocal = await _dbContext.Clientes
                    .FirstOrDefaultAsync(c => c.IdContaAzul == pessoaContaAzul.Id);
                
                if (clienteLocal == null)
                {
                    // Criar novo cliente
                    clienteLocal = new Cliente
                    {
                        IdContaAzul = pessoaContaAzul.Id,
                        Nome = pessoaContaAzul.Nome,
                        CpfCnpj = pessoaContaAzul.CpfCnpj,
                        Email = pessoaContaAzul.Email,
                        Telefone = pessoaContaAzul.Telefone
                    };
                    _dbContext.Clientes.Add(clienteLocal);
                }
                else
                {
                    // Atualizar cliente existente
                    clienteLocal.Nome = pessoaContaAzul.Nome;
                    clienteLocal.Email = pessoaContaAzul.Email;
                    clienteLocal.Telefone = pessoaContaAzul.Telefone;
                    clienteLocal.UltimaAtualizacao = DateTime.UtcNow;
                }
                
                totalSincronizados++;
            }
            
            await _dbContext.SaveChangesAsync();
            
            Console.WriteLine($"Página {paginaAtual}/{resultado.TotalPaginas} - {totalSincronizados} clientes sincronizados");
            
            if (paginaAtual >= resultado.TotalPaginas)
                break;
            
            paginaAtual++;
            await Task.Delay(200); // Evitar rate limiting
        }
        
        Console.WriteLine($"Sincronização concluída! Total: {totalSincronizados} clientes");
    }
}
```

### Relatório de Vendas com Exportação

```csharp
public class RelatorioVendasService
{
    private readonly ContaAzulApiClient _client;
    
    public async Task<byte[]> GerarRelatorioVendasCsvAsync(
        DateTime dataInicio, 
        DateTime dataFim)
    {
        var filtro = new VendaFiltro
        {
            DataInicio = dataInicio.ToString("yyyy-MM-dd"),
            DataFim = dataFim.ToString("yyyy-MM-dd"),
            Situacoes = "APROVADA",
            TamanhoPagina = 100
        };
        
        var todasVendas = new List<Venda>();
        var paginaAtual = 1;
        
        // Buscar todas as vendas do período
        while (true)
        {
            filtro.Pagina = paginaAtual;
            var resultado = await _client.Vendas.GetVendasAsync(filtro);
            
            if (resultado.Data == null || !resultado.Data.Any())
                break;
            
            todasVendas.AddRange(resultado.Data);
            
            if (paginaAtual >= resultado.TotalPaginas)
                break;
            
            paginaAtual++;
        }
        
        // Gerar CSV
        var csv = new StringBuilder();
        csv.AppendLine("Número,Data,Cliente,Valor Total,Situação");
        
        foreach (var venda in todasVendas)
        {
            csv.AppendLine($"{venda.Numero},{venda.DataEmissao:dd/MM/yyyy},{venda.Cliente?.Nome},{venda.ValorTotal:F2},{venda.Situacao}");
        }
        
        return Encoding.UTF8.GetBytes(csv.ToString());
    }
}
```

---

Para mais exemplos e documentação, visite: https://github.com/andersonrocha/contaazul-dotnet
