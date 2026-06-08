# ContaAzul .NET SDK

[![NuGet](https://img.shields.io/nuget/v/ContaAzul.Sdk.Net.svg)](https://www.nuget.org/packages/ContaAzul.Sdk.Net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

SDK não oficial em .NET Standard 2.0 para integração com a API do ContaAzul.

## ✨ Características

- ✅ Suporte completo para autenticação OAuth2
- ✅ Refresh automático de tokens
- ✅ Construtor com tokens armazenados (restauração de sessão)
- ✅ Evento `TokenRefreshed` para persistência automática de tokens
- ✅ API de Pessoas
- ✅ API de Vendas
- ✅ API de Notas Fiscais de Serviço
- ✅ Suporte para .NET Standard 2.0
- ✅ Totalmente assíncrono
- ✅ Política de retry com backoff exponencial configurável
- ✅ Rate limiting configurável (sliding-window)
- ✅ Suporte a injeção de dependências (`IContaAzulApiClient`)
- ✅ Logging estruturado via `ILogger`
- ✅ Documentação XML completa

## Instalação

### Via NuGet Package Manager

```bash
Install-Package ContaAzul.Sdk.Net
```

### Via .NET CLI

```bash
dotnet add package ContaAzul.Sdk.Net
```

### Via PackageReference

```xml
<PackageReference Include="ContaAzul.Sdk.Net" Version="1.0.0" />
```

## Uso

### 1. Autenticação OAuth2

O SDK utiliza **Basic Authentication** para autenticação OAuth2, conforme especificação da API do ContaAzul. As credenciais `clientId:clientSecret` são automaticamente codificadas em Base64 e enviadas no header `Authorization`.

#### 1.1 Construir URL de Autorização

Use `ContaAzulOAuthHelper.BuildAuthorizationUrl` para gerar a URL de redirecionamento OAuth2:

```csharp
using ContaAzul.Sdk.Net;

var authorizationUrl = ContaAzulOAuthHelper.BuildAuthorizationUrl(
    clientId: "seu-client-id",
    redirectUri: "https://seu-app.com/callback",
    state: Guid.NewGuid().ToString(),
    scope: "openid profile aws.cognito.signin.user.admin"
);

// Redirecione o usuário para authorizationUrl
```

#### 1.2 Trocar o código por tokens

Após o usuário autorizar, troque o código de autorização por tokens:

```csharp
using ContaAzul.Sdk.Net;
using System;
using System.Threading.Tasks;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

// O SDK envia as credenciais no formato Basic Auth:
// Authorization: Basic base64(clientId:clientSecret)
var tokenResponse = await client.AuthorizeAsync(
    code: "codigo-de-autorizacao",
    redirectUri: "https://seu-app.com/callback"
);

Console.WriteLine($"Access Token: {tokenResponse.AccessToken}");
Console.WriteLine($"Refresh Token: {tokenResponse.RefreshToken}");
Console.WriteLine($"Expires In: {tokenResponse.ExpiresIn} segundos");
```

### 2. Renovar Token

```csharp
var newTokenResponse = await client.RefreshTokenAsync();
Console.WriteLine($"Novo Access Token: {newTokenResponse.AccessToken}");
```

### 3. Restaurar Sessão Existente

Se você já possui tokens armazenados, use o construtor com tokens para restaurar a sessão sem precisar re-autenticar:

```csharp
using ContaAzul.Sdk.Net;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret",
    accessToken: "seu-access-token-armazenado",
    refreshToken: "seu-refresh-token-armazenado",
    options: new ContaAzulApiClientOptions
    {
        TokenExpiresAt = DateTime.UtcNow.AddHours(1) // data de expiração armazenada
    }
);
```

> **Importante:** O ContaAzul rotaciona o refresh token a cada renovação. Assine o evento `TokenRefreshed` para persistir os novos tokens automaticamente:

```csharp
client.TokenRefreshed += (sender, args) =>
{
    // Persista os tokens atualizados no seu armazenamento
    Console.WriteLine($"Novo Access Token: {args.AccessToken}");
    Console.WriteLine($"Novo Refresh Token: {args.RefreshToken}");
    Console.WriteLine($"Expira em: {args.TokenExpiresAt}");
};
```

### 4. API de Pessoas (PessoasApi)

A API de Pessoas é acessada através da propriedade `Pessoas` do cliente:

```csharp
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

await client.AuthorizeAsync(code, redirectUri);

// Listar pessoas com filtros
var filtro = new PessoaFiltro
{
    Pagina = 1,
    TamanhoPagina = 10,
    TiposPessoa = "FISICA",
    ComEndereco = true,
    Busca = "João"
};
var pessoas = await client.Pessoas.GetPessoasAsync(filtro);
// Gera automaticamente: /v1/pessoas?pagina=1&tamanho_pagina=10&tipos_pessoa=FISICA&com_endereco=true&busca=Jo%C3%A3o

Console.WriteLine($"Total de itens: {pessoas.TotalItems}");

foreach (var pessoa in pessoas.Items)
{
    Console.WriteLine($"ID: {pessoa.Id}, Nome: {pessoa.Nome}, CPF/CNPJ: {pessoa.CpfCnpj}");
}
```

**Filtros disponíveis em `PessoaFiltro`:**

- **Paginação**: `Pagina`, `TamanhoPagina`
- **Ordenação**: `TipoOrdenacao`, `OrdemOrdenacao`
- **Busca**: `Busca`, `Nomes`, `Emails`, `Telefones`, `Documentos`
- **Localização**: `Paises`, `Cidades`, `Ufs`
- **Identificadores**: `Ids`, `CodigosPessoa`
- **Tipo**: `TiposPessoa`, `TipoPerfil`
- **Datas**: `DataCriacaoInicio`, `DataCriacaoFim`, `DataAlteracaoDe`, `DataAlteracaoAte`
- **Outros**: `ComEndereco`

### 5. API de Vendas (VendasApi)

A API de Vendas é acessada através da propriedade `Vendas` do cliente:

```csharp
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

await client.AuthorizeAsync(code, redirectUri);

var filtro = new VendaFiltro
{
    Pagina = 1,
    TamanhoPagina = 10,
    DataInicio = "2024-01-01",
    DataFim = "2024-12-31",
    Situacoes = "APROVADA",
    Tipos = "VENDA",
    Pendente = false
};
var vendas = await client.Vendas.GetVendasAsync(filtro);
// Gera: /v1/venda/busca?pagina=1&tamanho_pagina=10&data_inicio=2024-01-01&data_fim=2024-12-31&situacoes=APROVADA&tipos=VENDA&pendente=false

Console.WriteLine($"Total de vendas: {vendas.TotalItens}");

foreach (var venda in vendas.Itens)
{
    Console.WriteLine($"Venda #{venda.Numero} - Cliente: {venda.Cliente?.Nome} - Valor: R$ {venda.ValorLiquido}");
}
```

**Filtros disponíveis em `VendaFiltro`:**

- **Paginação**: `Pagina`, `TamanhoPagina`
- **Ordenação**: `CampoOrdenadoAscendente`, `CampoOrdenadoDescendente`
- **Busca**: `TermoBusca`
- **Datas**: `DataInicio`, `DataFim`, `DataCriacaoDe`, `DataCriacaoAte`, `DataAlteracaoDe`, `DataAlteracaoAte`
- **Identificadores**: `IdsVendedores`, `IdsClientes`, `IdsNaturezaOperacao`, `IdsCategorias`, `IdsProdutos`
- **Status**: `Situacoes`, `Tipos`, `Origens`, `Pendente`
- **Outros**: `Numeros`, `Totais`
- **Legados**: `IdsLegadoDonos`, `IdsLegadoClientes`, `IdsLegadoProdutos`, `IdsLegadoCategorias`

### 6. API de Notas Fiscais de Serviço (NotasFiscaisApi)

A API de Notas Fiscais é acessada através da propriedade `NotasFiscais` do cliente:

```csharp
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

await client.AuthorizeAsync(code, redirectUri);

var filtro = new NotaFiscalFiltro
{
    Pagina = 1,
    TamanhoPagina = 10,
    DataCompetenciaDe = "2024-01-01",
    DataCompetenciaAte = "2024-01-15",
    IdCliente = "cliente-id",
    NumeroVenda = 1001,
    Status = "PENDENTE",
    TipoNegociacao = "VENDA",
    NumeroNfseInicial = 100,
    NumeroNfseFinal = 200
};
var notasFiscais = await client.NotasFiscais.GetNotasFiscaisAsync(filtro);

Console.WriteLine($"Total de notas fiscais: {notasFiscais.Paginacao?.TotalItens}");

foreach (var nota in notasFiscais.Itens)
{
    Console.WriteLine($"NFS-e #{nota.Numero} - Status: {nota.Status} - Valor: R$ {nota.ValorTotal}");
}
```

**Filtros disponíveis em `NotaFiscalFiltro`:**

- **Paginação**: `Pagina`, `TamanhoPagina`
- **Datas**: `DataCompetenciaDe`, `DataCompetenciaAte`
- **Identificadores**: `Ids`, `IdCliente`, `NumeroVenda`
- **Numeração**: `NumeroNfseInicial`, `NumeroNfseFinal`, `NumeroRpsInicial`, `NumeroRpsFinal`
- **Status**: `Status`, `TipoNegociacao`

## Injeção de Dependências

Use o método de extensão `AddContaAzulApiClient` para registrar o cliente no contêiner de DI:

```csharp
using ContaAzul.Sdk.Net.Extensions;

services.AddContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);
```

Injete `IContaAzulApiClient` nos seus serviços:

```csharp
public class MeuServico
{
    private readonly IContaAzulApiClient _client;

    public MeuServico(IContaAzulApiClient client)
    {
        _client = client;
    }
}
```

## Estrutura do SDK

### Cliente Principal

- **`ContaAzulApiClient`**: Cliente principal do SDK. Gerencia autenticação OAuth2 e fornece acesso a todas as APIs através de propriedades.
- **`IContaAzulApiClient`**: Interface do cliente. Use para injeção de dependências e mock em testes unitários.

### Helpers

- **`ContaAzulOAuthHelper`**: Utilitários estáticos para o fluxo OAuth2, como `BuildAuthorizationUrl`.

### APIs Disponíveis

Todas as APIs são acessadas através de propriedades do `ContaAzulApiClient`:

- **`client.Pessoas`**: API para listar e filtrar pessoas (clientes, fornecedores, etc.).
- **`client.Vendas`**: API para buscar e filtrar vendas.
- **`client.NotasFiscais`**: API para buscar e filtrar notas fiscais de serviço.

### Classes Base

- **`HttpClientBase`**: Classe base com métodos HTTP genéricos (GET, POST, PUT, PATCH, DELETE).
- **`QueryStringBuilder`**: Construtor automático de query strings usando reflection e atributos.

### Opções de Configuração

- **`ContaAzulApiClientOptions`**: Agrupa todas as configurações opcionais do cliente (URL base, `HttpClient`, logger, timeout, retry, rate limit e expiração de token).
- **`RetryOptions`**: Configura retry com backoff exponencial (padrão: 3 tentativas, delay inicial de 1s, multiplicador 2x).
- **`RateLimitOptions`**: Configura rate limiting via sliding-window (padrão: 10 req/s).
- **`HttpOptions`**: Configura timeout HTTP (padrão: 30 segundos).

### Sistema de Atributos

O SDK utiliza um sistema baseado em atributos para construir automaticamente query strings a partir de objetos de filtro:

```csharp
public class PessoaFiltro
{
    [QueryParameter("pagina")]
    public int? Pagina { get; set; }

    [QueryParameter("tamanho_pagina")]
    public int? TamanhoPagina { get; set; }

    [QueryParameter("busca")]
    public string Busca { get; set; }
}

var filtro = new PessoaFiltro { Pagina = 1, Busca = "João" };
var pessoas = await client.Pessoas.GetPessoasAsync(filtro);
// URL: /v1/pessoas?pagina=1&busca=Jo%C3%A3o
```

**Benefícios:**
- ✅ Manutenção simplificada
- ✅ Evita erros de digitação nos nomes dos parâmetros
- ✅ URL encoding automático
- ✅ Suporte a tipos nullable (apenas valores definidos são incluídos)
- ✅ Conversão automática de tipos (bool, int, string, etc.)

### Modelos

- **`TokenResponse`**: Resposta do endpoint de autenticação OAuth2.
- **`TokenRefreshedEventArgs`**: Argumentos do evento `TokenRefreshed` com os novos tokens e data de expiração.
- **`Pessoa`**: Modelo de pessoa com todos os campos.
- **`PessoaListResponse`**: Resposta da listagem de pessoas (`Items`, `TotalItems`).
- **`PessoaFiltro`**: Filtros para busca de pessoas.
- **`Venda`**: Modelo de venda com todos os campos.
- **`VendaListResponse`**: Resposta da listagem de vendas (`Itens`, `TotalItens`, `Totais`, `Quantidades`).
- **`VendaFiltro`**: Filtros para busca de vendas (28 parâmetros disponíveis).
- **`NotaFiscal`**: Modelo de nota fiscal de serviço.
- **`NotaFiscalListResponse`**: Resposta da listagem de notas fiscais (`Itens`, `Paginacao`).
- **`NotaFiscalFiltro`**: Filtros para busca de notas fiscais.
- **`Paginacao`**: Informações de paginação (`PaginaAtual`, `TotalPaginas`, `TamanhoPagina`, `TotalItens`).
- **`Endereco`**: Modelo de endereço.
- **`ApiError`**: Modelo de erro retornado pela API.

### Exemplo de Uso Completo

```csharp
// 1. Criar cliente principal
var client = new ContaAzulApiClient("client-id", "client-secret");

// 2. Assinar evento para persistir tokens rotacionados
client.TokenRefreshed += (_, args) =>
{
    // Salvar args.AccessToken, args.RefreshToken e args.TokenExpiresAt
};

// 3. Autenticar
await client.AuthorizeAsync(authCode, redirectUri);

// 4. Usar a API de Pessoas
var pessoas = await client.Pessoas.GetPessoasAsync();

// 5. O token é compartilhado automaticamente entre todas as APIs
Console.WriteLine($"Token atual: {client.AccessToken}");
```

## Boas Práticas

### 1. Persistir Tokens com TokenRefreshed

```csharp
client.TokenRefreshed += (_, args) =>
{
    // Salvar no banco de dados, arquivo, etc.
    storage.Save("access_token", args.AccessToken);
    storage.Save("refresh_token", args.RefreshToken);
    storage.Save("token_expires_at", args.TokenExpiresAt.ToString("O"));
};
```

### 2. Usar CancellationToken

```csharp
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
var pessoas = await client.Pessoas.GetPessoasAsync(null, cts.Token);
```

### 3. Tratamento de Erros

```csharp
using ContaAzul.Sdk.Net.Exceptions;

try
{
    var pessoas = await client.Pessoas.GetPessoasAsync();
}
catch (ContaAzulException ex)
{
    Console.WriteLine($"Erro da API: {ex.Message} (HTTP {ex.StatusCode})");
    Console.WriteLine($"Resposta: {ex.ResponseContent}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Erro de rede: {ex.Message}");
}
```

### 4. Disposable Pattern

```csharp
using (var client = new ContaAzulApiClient(clientId, clientSecret))
{
    await client.AuthorizeAsync(code, redirectUri);
    // ... usar o cliente
} // Recursos são liberados automaticamente
```

## Configuração Avançada

### ContaAzulApiClientOptions

Use `ContaAzulApiClientOptions` para configurar todas as opções opcionais em um único objeto:

```csharp
var options = new ContaAzulApiClientOptions
{
    HttpOptions = new HttpOptions
    {
        DefaultTimeout = TimeSpan.FromSeconds(30)
    },
    RetryOptions = new RetryOptions
    {
        MaxRetries = 3,
        InitialDelay = TimeSpan.FromSeconds(1),
        BackoffMultiplier = 2.0
    },
    RateLimitOptions = new RateLimitOptions
    {
        RequestsPerSecond = 10
    }
};

var client = new ContaAzulApiClient("client-id", "client-secret", options);
```

### Desabilitar Retry ou Rate Limiting

```csharp
client.RetryOptions = RetryOptions.None;         // Desabilita retry
client.RateLimitOptions = RateLimitOptions.None; // Desabilita rate limiting
```

### Injetar HttpClient Customizado

```csharp
var options = new ContaAzulApiClientOptions
{
    HttpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) }
};

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret",
    options: options
);
```

### Logging

```csharp
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret",
    options: new ContaAzulApiClientOptions
    {
        Logger = loggerFactory.CreateLogger<ContaAzulApiClient>()
    }
);
```

## Requisitos

- .NET Standard 2.0 ou superior
- System.Text.Json 8.0.5
- Microsoft.Extensions.DependencyInjection.Abstractions 8.0.0
- Microsoft.Extensions.Logging.Abstractions 8.0.0
- System.Net.Http 4.3.4

## Testes

O projeto inclui testes unitários completos usando NUnit. Para executar os testes:

```bash
dotnet test
```

### Estrutura de Testes

Os testes cobrem:
- ✅ Validação de parâmetros nulos e vazios
- ✅ Comportamento do construtor
- ✅ Método `AuthorizeAsync` com diferentes cenários
- ✅ Método `RefreshTokenAsync`
- ✅ Evento `TokenRefreshed`
- ✅ Expiração e renovação automática de tokens
- ✅ Retry policy com backoff exponencial
- ✅ Rate limiting
- ✅ Timeout e ciclo de vida do `HttpClient`
- ✅ Thread safety

### Executar com Cobertura de Código

```bash
dotnet-coverage collect -f cobertura -o coverage.cobertura.xml dotnet test
```

## Contribuindo

Contribuições são bem-vindas! Por favor, abra uma issue ou pull request.

## Licença

MIT
