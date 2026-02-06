# ContaAzul .NET SDK

[![NuGet](https://img.shields.io/nuget/v/ContaAzul.Sdk.Net.svg)](https://www.nuget.org/packages/ContaAzul.Sdk.Net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

SDK não oficial em .NET Standard 2.0 para integração com a API do ContaAzul.

## ?? Características

- ? Suporte completo para autenticação OAuth2
- ? Refresh automático de tokens
- ? API de Pessoas
- ? API de Vendas
- ? API de Notas Fiscais de Serviço
- ? Suporte para .NET Standard 2.0
- ? Totalmente assíncrono
- ? Documentação XML completa

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

Primeiro, você precisa obter o código de autorização através do fluxo OAuth2 do ContaAzul. Depois, use o SDK para trocar o código por um token de acesso:

```csharp
using contaazul_dotnet;
using System;
using System.Threading.Tasks;

// Inicializar o cliente com suas credenciais
var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

// Trocar o código de autorização por um token de acesso
// O SDK automaticamente envia as credenciais no formato Basic Auth:
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
// Renovar o token quando expirar
var newTokenResponse = await client.RefreshTokenAsync();
Console.WriteLine($"Novo Access Token: {newTokenResponse.AccessToken}");
```

### 3. Usar Token Existente

Se você já possui um token de acesso armazenado:

```csharp
var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

client.SetAccessToken("seu-access-token-existente");
```

### 4. API de Pessoas (PessoaApi)

A API de Pessoas é acessada através da propriedade `Pessoas` do cliente:

```csharp
using contaazul_dotnet;
using contaazul_dotnet.Models;

// Criar instância do cliente
var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

// Autorizar
await client.AuthorizeAsync(code, redirectUri);

// Listar todas as pessoas com filtros
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

Console.WriteLine($"Total de registros: {pessoas.TotalRegistros}");
Console.WriteLine($"Total de páginas: {pessoas.TotalPaginas}");

foreach (var pessoa in pessoas.Data)
{
    Console.WriteLine($"ID: {pessoa.Id}, Nome: {pessoa.Nome}, CPF/CNPJ: {pessoa.CpfCnpj}");
}

// Obter uma pessoa específica
var pessoa = await client.Pessoas.GetPessoaByIdAsync("pessoa-id");

// Criar uma nova pessoa
var novaPessoa = new Pessoa
{
    Nome = "João da Silva",
    TipoPessoa = "FISICA",
    CpfCnpj = "12345678900",
    Email = "joao@example.com",
    Telefone = "(11) 98765-4321",
    Endereco = new Endereco
    {
        Logradouro = "Rua Exemplo",
        Numero = "123",
        Bairro = "Centro",
        Cidade = "São Paulo",
        Uf = "SP",
        Cep = "01234-567",
        Pais = "Brasil"
    }
};
var pessoaCriada = await client.Pessoas.CreatePessoaAsync(novaPessoa);

// Atualizar uma pessoa
novaPessoa.Nome = "João da Silva Atualizado";
var pessoaAtualizada = await client.Pessoas.UpdatePessoaAsync("pessoa-id", novaPessoa);

// Deletar uma pessoa
await client.Pessoas.DeletePessoaAsync("pessoa-id");
```

### 5. API de Vendas (VendaApi)

A API de Vendas é acessada através da propriedade `Vendas` do cliente:

```csharp
using contaazul_dotnet;
using contaazul_dotnet.Models;

// Criar instância do cliente
var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

// Autorizar
await client.AuthorizeAsync(code, redirectUri);

// Buscar vendas com filtros
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

Console.WriteLine($"Total de vendas: {vendas.TotalRegistros}");

foreach (var venda in vendas.Data)
{
    Console.WriteLine($"Venda #{venda.Numero} - Cliente: {venda.Cliente?.Nome} - Valor: R$ {venda.ValorLiquido}");
}
```

#### Filtros Disponíveis na VendaApi:

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

// Criar instância do cliente
var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

// Autorizar
await client.AuthorizeAsync(code, redirectUri);

// Buscar notas fiscais com filtros
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

Console.WriteLine($"Total de notas fiscais: {notasFiscais.TotalRegistros}");

foreach (var nota in notasFiscais.Data)
{
    Console.WriteLine($"NFS-e #{nota.Numero} - Status: {nota.Status} - Valor: R$ {nota.ValorTotal}");
}
```

#### Filtros Disponíveis na NotasFiscaisApi:

- **Paginação**: `Pagina`, `TamanhoPagina`
- **Datas**: `DataCompetenciaDe`, `DataCompetenciaAte`
- **Identificadores**: `Ids`, `IdCliente`, `NumeroVenda`
- **Numeração**: `NumeroNfseInicial`, `NumeroNfseFinal`, `NumeroRpsInicial`, `NumeroRpsFinal`
- **Status**: `Status`, `TipoNegociacao`


```

## Estrutura do SDK

### Cliente Principal

- **`ContaAzulApiClient`**: Cliente principal do SDK. Gerencia autenticação OAuth2 e fornece acesso a todas as APIs através de propriedades.

### APIs Disponíveis

Todas as APIs são acessadas através de propriedades do `ContaAzulApiClient`:

- **`client.Pessoas`**: API para gerenciar pessoas (clientes, fornecedores, etc.).
- **`client.Vendas`**: API para gerenciar vendas.
- **`client.NotasFiscais`**: API para gerenciar notas fiscais de serviço.

### Classes Base

- **`HttpClientBase`**: Classe base com métodos HTTP genéricos (GET, POST, PUT, DELETE).
- **`QueryStringBuilder`**: Construtor automático de query strings usando reflection e atributos.

### Sistema de Atributos

O SDK utiliza um sistema baseado em atributos para construir automaticamente query strings a partir de objetos de filtro:

```csharp
// Definição do filtro com atributos
public class PessoaFiltro
{
    [QueryParameter("pagina")]
    public int? Pagina { get; set; }

    [QueryParameter("tamanho_pagina")]
    public int? TamanhoPagina { get; set; }

    [QueryParameter("busca")]
    public string Busca { get; set; }

    // ... outras propriedades
}

// Uso - query string construída automaticamente
var filtro = new PessoaFiltro { Pagina = 1, Busca = "João" };
var pessoas = await client.Pessoas.GetPessoasAsync(filtro);
// URL: /v1/pessoas?pagina=1&busca=Jo%C3%A3o
```

**Benefícios:**
- ? Manutenção simplificada
- ? Evita erros de digitação nos nomes dos parâmetros
- ? URL encoding automático
- ? Suporte a tipos nullable (apenas valores definidos são incluídos)
- ? Conversão automática de tipos (bool, int, string, etc.)

### Modelos

- **`TokenResponse`**: Resposta do endpoint de autenticação OAuth2.
- **`ApiResponse<T>`**: Wrapper genérico para respostas da API.
- **`Pessoa`**: Modelo de pessoa com todos os campos.
- **`PessoaListResponse`**: Resposta paginada da listagem de pessoas.
- **`PessoaFiltro`**: Filtros para busca de pessoas.
- **`Venda`**: Modelo de venda com todos os campos.
- **`VendaListResponse`**: Resposta paginada da listagem de vendas.
- **`VendaFiltro`**: Filtros para busca de vendas (28 parâmetros disponíveis).
- **`NotaFiscal`**: Modelo de nota fiscal de serviço.
- **`NotaFiscalListResponse`**: Resposta paginada da listagem de notas fiscais.
- **`NotaFiscalFiltro`**: Filtros para busca de notas fiscais.
- **`Endereco`**: Modelo de endereço.

### Exemplo de Uso Completo

```csharp
// 1. Criar cliente principal
var client = new ContaAzulApiClient("client-id", "client-secret");

// 2. Autenticar
await client.AuthorizeAsync(authCode, redirectUri);

// 3. Usar a API de Pessoas
var pessoas = await client.Pessoas.GetPessoasAsync();

// 4. O token é compartilhado automaticamente
Console.WriteLine($"Token atual: {client.AccessToken}");
```

## Métodos HTTP Genéricos

O SDK fornece métodos genéricos para todas as operações HTTP:

```csharp
// GET
var response = await GetAsync<TResponse>(endpoint);

// POST
var response = await PostAsync<TRequest, TResponse>(endpoint, data);

// PUT
var response = await PutAsync<TRequest, TResponse>(endpoint, data);

// DELETE
var response = await DeleteAsync<TResponse>(endpoint);
await DeleteAsync(endpoint); // Sem retorno
```

## Boas Práticas

### 1. Usar CancellationToken

```csharp
var cts = new CancellationTokenSource();
var customers = await customerApi.GetCustomersAsync<CustomerListResponse>(cts.Token);
```

### 2. Tratamento de Erros

```csharp
try
{
    var pessoa = await client.Pessoas.GetPessoaByIdAsync("invalid-id");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Erro na requisição: {ex.Message}");
}
```

### 3. Disposable Pattern

```csharp
using (var client = new ContaAzulApiClient(clientId, clientSecret))
{
    await client.AuthorizeAsync(code, redirectUri);
    // ... usar o cliente
} // Recursos são liberados automaticamente
```

## Configuração Avançada

### Injetar HttpClient Customizado

```csharp
var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30)
};

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret",
    httpClient: httpClient
);
```

## Requisitos

- .NET Standard 2.0 ou superior
- Newtonsoft.Json 13.0.1
- System.Net.Http 4.3.4

## Testes

O projeto inclui testes unitários completos usando NUnit. Para executar os testes:

```bash
dotnet test
```

### Estrutura de Testes

Os testes cobrem:
- ? Validação de parâmetros nulos e vazios
- ? Comportamento do construtor
- ? Método `AuthorizeAsync` com diferentes cenários
- ? Método `RefreshTokenAsync` sem autorização prévia
- ? Método `SetAccessToken` para definir tokens manualmente

### Executar com Cobertura de Código

```bash
dotnet-coverage collect -f cobertura -o coverage.cobertura.xml dotnet test
```

## Contribuindo

Contribuições são bem-vindas! Por favor, abra uma issue ou pull request.

## Licença

MIT

