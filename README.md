# ContaAzul .NET SDK

[![NuGet](https://img.shields.io/nuget/v/ContaAzul.Sdk.Net.svg)](https://www.nuget.org/packages/ContaAzul.Sdk.Net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

SDK não oficial em .NET Standard 2.0 para integração com a API do ContaAzul.

## ✨ Características

- ✅ Suporte completo para autenticação OAuth2
- ✅ Refresh automático de tokens
- ✅ Construtor com tokens armazenados (restauração de sessão)
- ✅ Evento `TokenRefreshed` para persistência automática de tokens
- ✅ API de Pessoas (CRUD, operações em lote, empresa conectada)
- ✅ API de Vendas (busca, detalhe, criação, edição, itens, PDF)
- ✅ API de Notas Fiscais (produto/NF-e e serviço/NFS-e, MDF-e, XML)
- ✅ API de Contratos (vendas recorrentes/agendadas)
- ✅ API de Cobranças (contas a receber)
- ✅ API de Baixas (baixas de parcelas)
- ✅ API de Financeiro (centros de custo, categorias, contas, parcelas, saldos)
- ✅ API de Produtos (catálogo/inventário e tabelas fiscais/e-commerce)
- ✅ API de Serviços (catálogo de serviços)
- ✅ API de Protocolos (acompanhamento de eventos financeiros)
- ✅ API de Orçamentos (propostas comerciais)
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
<PackageReference Include="ContaAzul.Sdk.Net" Version="0.2.2" />
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
var pessoas = await client.Pessoas.ObterPessoasAsync(filtro);
// Gera automaticamente: /v1/pessoas?pagina=1&tamanho_pagina=10&tipos_pessoa=FISICA&com_endereco=true&busca=Jo%C3%A3o

Console.WriteLine($"Total de itens: {pessoas.TotalItems}");

foreach (var pessoa in pessoas.Items)
{
    Console.WriteLine($"ID: {pessoa.Id}, Nome: {pessoa.Nome}, Documento: {pessoa.Documento}");
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

**Demais operações da `PessoasApi`:**

```csharp
// Detalhe por id (UUID) ou por id legado
Pessoa pessoa = await client.Pessoas.ObterPessoaPorIdAsync("550e8400-e29b-41d4-a716-446655440000");
Pessoa legada = await client.Pessoas.ObterPessoaPorLegadoIdAsync("12345");

// Empresa conectada ao token
Empresa empresa = await client.Pessoas.ObterEmpresaConectadaAsync();

// Criar
ResumoPessoa criada = await client.Pessoas.CriarPessoaAsync(new PessoaRequest
{
    Nome = "João Silva",
    TipoPessoa = "Física",
    Cpf = "123.456.789-00",
    Perfis = new List<PerfilPessoa> { new PerfilPessoa { TipoPerfil = "Cliente" } }
});

// Atualização integral (PUT) e parcial (PATCH)
await client.Pessoas.AtualizarPessoaAsync(criada.Id, new PessoaRequest { Nome = "João S.", TipoPessoa = "Física" });
await client.Pessoas.AtualizarParcialmentePessoaAsync(criada.Id, new AtualizacaoParcialPessoa { Email = "novo@email.com" });

// Operações em lote (ativar/inativar retornam o resultado; excluir retorna 204)
var lote = new PessoasEmLoteRequest { Uuids = new List<string> { criada.Id } };
await client.Pessoas.AtivarPessoasEmLoteAsync(lote);
await client.Pessoas.InativarPessoasEmLoteAsync(lote);
await client.Pessoas.ExcluirPessoasEmLoteAsync(lote);
```

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

### 6. API de Notas Fiscais (NotasFiscaisApi)

A API de Notas Fiscais é acessada através da propriedade `NotasFiscais` do cliente e cobre
notas de produto (NF-e), notas de serviço (NFS-e), vínculo a MDF-e e consulta de XML por chave:

```csharp
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.NotasFiscais;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

await client.AuthorizeAsync(code, redirectUri);
```

#### 6.1. Notas fiscais de produto (NF-e)

`DataInicial` e `DataFinal` (formato `YYYY-MM-DD`) são obrigatórios.

```csharp
var filtro = new NotaFiscalFiltro
{
    Pagina = 1,
    TamanhoPagina = 10,
    DataInicial = "2024-01-01",
    DataFinal = "2024-01-15",
    DocumentoTomador = "12345678900",
    NumeroNota = "1234",
    IdVenda = "550e8400-e29b-41d4-a716-446655440000"
};

RespostaPaginada<NotaFiscal> notas = await client.NotasFiscais.ObterNotasFiscaisAsync(filtro);

Console.WriteLine($"Total: {notas.Paginacao?.TotalItens}");
foreach (var nota in notas.Itens)
{
    Console.WriteLine($"NF-e #{nota.NumeroNota} - {nota.NomeDestinatario} - Status: {nota.Status}");
}
```

#### 6.2. Notas fiscais de serviço (NFS-e)

`DataCompetenciaDe` e `DataCompetenciaAte` são obrigatórios (intervalo máximo de 15 dias).

```csharp
var filtroServico = new NotaFiscalServicoFiltro
{
    Pagina = 1,
    TamanhoPagina = 10,
    DataCompetenciaDe = "2024-01-01",
    DataCompetenciaAte = "2024-01-15",
    IdCliente = "cliente-id",
    NumeroVenda = 1001,
    Status = "EMITIDA",
    TipoNegociacao = "VENDA",
    NumeroNfseInicial = 100,
    NumeroNfseFinal = 200
};

RespostaPaginada<NotaFiscalServico> servico =
    await client.NotasFiscais.ObterNotasFiscaisServicoAsync(filtroServico);

foreach (var nfse in servico.Itens)
{
    Console.WriteLine($"NFS-e #{nfse.NumeroNfse} - Status: {nfse.Status} - R$ {nfse.ValorTotalNfse}");
}
```

#### 6.3. Vincular notas fiscais a um MDF-e

```csharp
await client.NotasFiscais.VincularNotaFiscalMdfeAsync(new LinkNotaFiscalMdfe
{
    ChavesAcesso = new List<string>
    {
        "42250323643586000108550010000001151606401726",
        "42250323643586000108550010000001141054498495"
    },
    Identificador = "345345",
    Status = "ENCERRADO" // AUTORIZADO, ENCERRADO ou CANCELADO
});
```

#### 6.4. Obter o XML de uma nota fiscal por chave

```csharp
string xml = await client.NotasFiscais.ObterNotaFiscalPorChaveAsync(
    "42250323643586000108550010000001151606401726");
```

**Filtros de NF-e (`NotaFiscalFiltro`):** `Pagina`, `TamanhoPagina`, `DataInicial`*, `DataFinal`*, `DocumentoTomador`, `NumeroNota`, `IdVenda` (* obrigatórios).

**Filtros de NFS-e (`NotaFiscalServicoFiltro`):** `Pagina`, `TamanhoPagina`, `DataCompetenciaDe`*, `DataCompetenciaAte`*, `Ids`, `IdCliente`, `NumeroVenda`, `NumeroNfseInicial`, `NumeroNfseFinal`, `NumeroRpsInicial`, `NumeroRpsFinal`, `Status`, `TipoNegociacao` (* obrigatórios).

### 7. API de Produtos (ProdutosApi)

A API de Produtos (inventário) é acessada através da propriedade `Produtos` do cliente e cobre o
catálogo de produtos (listar, criar, detalhar, atualizar parcialmente e excluir) e as tabelas
auxiliares (categorias, CEST, NCM, unidades de medida, categorias e marcas de e-commerce):

```csharp
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models.Produtos;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

await client.AuthorizeAsync(code, redirectUri);
```

#### 7.1. Listar produtos

```csharp
var filtro = new ProdutoFiltro
{
    Pagina = 1,
    TamanhoPagina = 20,
    Busca = "café",
    Status = "ATIVO",           // ATIVO ou INATIVO
    CampoOrdenacao = "NOME",    // NOME, CODIGO ou VALOR_VENDA
    ValorVendaInicial = 10,
    ValorVendaFinal = 500
};

ResumoDeProdutos resumo = await client.Produtos.ObterProdutosAsync(filtro);

Console.WriteLine($"Total: {resumo.TotalItems}");
foreach (var p in resumo.Items)
{
    Console.WriteLine($"{p.Codigo} - {p.Nome} - R$ {p.ValorVenda} (saldo: {p.Saldo})");
}
```

#### 7.2. Detalhar, criar, atualizar e excluir

```csharp
// Detalhe completo (estoque, fiscal, e-commerce, variações, dimensões, etc.)
Produto produto = await client.Produtos.ObterProdutoPorIdAsync("produto-id");

// Criar — apenas "Nome" é obrigatório
Produto novo = await client.Produtos.CriarProdutoAsync(new CriacaoProduto
{
    Nome = "Café Torrado 500g",
    CodigoSku = "CAFE500",
    Estoque = new CriacaoEstoqueProduto { ValorVenda = 24.90m, EstoqueDisponivel = 100 },
    Fiscal = new CriacaoFiscalProduto
    {
        Ncm = new ReferenciaIdInteiroProduto { Id = 1 },
        UnidadeMedida = new ReferenciaIdInteiroProduto { Id = 1 }
    }
});

// Atualização parcial (PATCH) — só os campos informados mudam
await client.Produtos.AtualizarParcialmenteProdutoAsync(novo.Id, new AtualizacaoParcialProduto
{
    ValorVenda = 27.50m
});

// Excluir
await client.Produtos.DeletarProdutoPorIdAsync(novo.Id);
```

#### 7.3. Tabelas auxiliares (fiscal e e-commerce)

```csharp
CategoriasDeProduto categorias   = await client.Produtos.ObterCategoriasAsync(new BuscaTextualFiltro { BuscaTextual = "bebidas" });
CESTsDeProduto cests             = await client.Produtos.ObterCestsAsync(new BuscaTextualFiltro { BuscaTextual = "0100" });
NCMsDeProduto ncms               = await client.Produtos.ObterNcmsAsync();
UnidadesDeMedidaDeProduto uns    = await client.Produtos.ObterUnidadesMedidaAsync();
MarcaDeEcommerce marcas          = await client.Produtos.ObterMarcasEcommerceAsync(new MarcaEcommerceFiltro { Direcao = "ASC" });
ProdutoEcommerceCategoria arvore = await client.Produtos.ObterCategoriasEcommerceAsync("eletrônicos");
```

**Filtros de produtos (`ProdutoFiltro`):** `Pagina`, `TamanhoPagina`, `CampoOrdenacao` (`NOME`/`CODIGO`/`VALOR_VENDA`), `DirecaoOrdenacao` (`ASC`/`DESC`), `Busca`, `Status` (`ATIVO`/`INATIVO`), `IntegracaoEcommerceAtivo`, `ProdutosKitAtivo`, `ValorVendaInicial`, `ValorVendaFinal`, `Sku`, `DataAlteracaoDe`, `DataAlteracaoAte`.

**Filtros auxiliares:** `BuscaTextualFiltro` (`Pagina`, `TamanhoPagina`, `BuscaTextual`) para categorias, CEST, NCM e unidades de medida; `MarcaEcommerceFiltro` adiciona `Direcao`. As categorias de e-commerce aceitam apenas busca textual (parâmetro `string` no método).

### 8. API de Serviços (ServicosApi)

A API de Serviços é acessada através da propriedade `Servicos` do cliente e cobre o catálogo de
serviços da empresa (listar, criar, detalhar, atualizar parcialmente e excluir em lote):

```csharp
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models.Servicos;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

await client.AuthorizeAsync(code, redirectUri);
```

#### 8.1. Listar e detalhar

```csharp
var filtro = new ServicoFiltro { Pagina = 1, TamanhoPagina = 20, BuscaTextual = "consultoria" };

ServicosPorFiltro servicos = await client.Servicos.ObterServicosAsync(filtro);

Console.WriteLine($"Total: {servicos.Paginacao?.TotalItens}");
foreach (var s in servicos.Itens)
{
    Console.WriteLine($"{s.Codigo} - {s.Descricao} - R$ {s.Preco}");
}

Servico servico = await client.Servicos.ObterServicoPorIdAsync("servico-id");
```

#### 8.2. Criar, atualizar e excluir em lote

```csharp
// Criar — apenas "Descricao" é obrigatório
Servico novo = await client.Servicos.CriarServicoAsync(new CriarServico
{
    Descricao = "Consultoria técnica",
    Codigo = "SERV001",
    Preco = 500,
    TipoServico = "PRESTADO",   // PRESTADO, TOMADO ou AMBOS
    Status = "ATIVO"
});

// Atualização parcial (PATCH) — só os campos informados mudam
await client.Servicos.AtualizarParcialmenteServicoAsync(novo.Id, new AtualizacaoParcialServico
{
    Preco = 550
});

// Exclusão em lote (usa o id legado `id_servico` retornado na criação)
await client.Servicos.DeletarServicosEmLoteAsync(new ParametrosParaDeletarServicosEmLote
{
    Ids = new List<int> { novo.IdServico.Value }
});
```

**Filtros de serviços (`ServicoFiltro`):** `Pagina`, `TamanhoPagina`, `BuscaTextual`.

### 9. API de Protocolos (ProtocolosApi)

A API de Protocolos é acessada através da propriedade `Protocolos` e permite acompanhar o
processamento assíncrono de eventos financeiros enviados ao ERP. Ao enviar um evento (ex.: uma
conta a pagar/receber), a Conta Azul retorna um protocolo cujo status pode ser consultado:

```csharp
using ContaAzul.Sdk.Net.Models.Protocolos;

Protocolo protocolo = await client.Protocolos.ObterProtocoloPorIdAsync("protocolo-id");

// Status: PENDING (em processamento), SUCCESS (criado) ou ERROR (erro na criação)
Console.WriteLine($"{protocolo.Status}: {protocolo.Resposta}");
```

### 10. API de Orçamentos (OrcamentosApi)

A API de Orçamentos é acessada através da propriedade `Orcamentos` e cobre propostas comerciais
(listar com filtros, detalhar, criar e excluir em lote):

```csharp
using ContaAzul.Sdk.Net;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.Orcamentos;

var client = new ContaAzulApiClient(
    clientId: "seu-client-id",
    clientSecret: "seu-client-secret"
);

await client.AuthorizeAsync(code, redirectUri);
```

#### 10.1. Listar e detalhar

```csharp
var filtro = new OrcamentoFiltro
{
    Pagina = 1,
    TamanhoPagina = 20,
    TermoBusca = "proposta",
    CampoOrdenadoDescendente = "DATA",          // DATA, NUMERO ou CLIENTE
    Situacoes = "ORCAMENTO,ORCAMENTO_ACEITO",   // múltiplos valores separados por vírgula
    DataInicio = "2026-01-01",
    DataFim = "2026-12-31"
};

ListagemOrcamentosPorFiltro orcamentos = await client.Orcamentos.ObterOrcamentosAsync(filtro);

Console.WriteLine($"Total: {orcamentos.TotalItens}");
foreach (var o in orcamentos.Itens)
{
    Console.WriteLine($"#{o.Numero} - {o.Cliente?.Nome} - {o.Situacao} - R$ {o.Total}");
}

Orcamento orcamento = await client.Orcamentos.ObterOrcamentoPorIdAsync("orcamento-id");
```

#### 10.2. Criar e excluir em lote

```csharp
ResumoCriacaoOrcamento novo = await client.Orcamentos.CriarOrcamentoAsync(new CriarOrcamento
{
    DataOrcamento = "2026-05-01",
    DataValidade = "2026-05-15",              // não pode ser anterior à data do orçamento
    IdCliente = "cliente-id",
    Itens = new List<CriarItemOrcamento>
    {
        new CriarItemOrcamento { Id = "produto-ou-servico-id", Quantidade = 2, Valor = 150 }
    },
    ComposicaoDeValor = new ComposicaoValorOrcamento
    {
        Frete = 20,
        Desconto = new Desconto { Tipo = "PORCENTAGEM", Valor = 10 }   // VALOR ou PORCENTAGEM
    }
});

// Exclusão em lote (máximo de 10 IDs)
await client.Orcamentos.ExcluirOrcamentosEmLoteAsync(new ExclusaoLoteOrcamento
{
    Ids = new List<string> { novo.Id }
});
```

**Filtros de orçamentos (`OrcamentoFiltro`):** `Pagina`, `TamanhoPagina`, `CampoOrdenadoAscendente`, `CampoOrdenadoDescendente`, `TermoBusca`, `DataInicio`, `DataFim`, `DataCriacaoDe`, `DataCriacaoAte`, `DataAlteracaoDe`, `DataAlteracaoAte`, `IdsVendedores`, `IdsClientes`, `IdsNaturezaOperacao`, `IdsCategorias`, `IdsProdutos`, `Situacoes`, `Origens`, `Numeros`, `IdsLegadoDonos`, `IdsLegadoClientes`, `IdsLegadoProdutos` (campos de múltiplos valores separados por vírgula).

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

- **`client.Pessoas`**: gerencia pessoas (clientes, fornecedores, transportadoras) — CRUD, operações em lote e empresa conectada.
- **`client.Vendas`**: busca e detalha vendas, cria/edita, itens, exclusão em lote, PDF e vendedores.
- **`client.NotasFiscais`**: notas fiscais de produto (NF-e) e serviço (NFS-e), vínculo a MDF-e e consulta de XML por chave.
- **`client.Contratos`**: contratos (vendas recorrentes/agendadas) — listagem, detalhe, criação, encerramento e remoção.
- **`client.Cobrancas`**: gera, consulta e cancela cobranças (contas a receber).
- **`client.Baixas`**: cria, lista, consulta, atualiza e exclui baixas de parcelas.
- **`client.Financeiro`**: centros de custo, categorias, categorias DRE, contas financeiras, saldos, transferências, contas a pagar/receber, parcelas e eventos.
- **`client.Produtos`**: catálogo de produtos (inventário) e tabelas fiscais/e-commerce (categorias, CEST, NCM, unidades de medida, marcas).
- **`client.Servicos`**: catálogo de serviços — listar, criar, detalhar, atualizar parcialmente e excluir em lote.
- **`client.Protocolos`**: acompanhamento do processamento assíncrono de eventos financeiros (status `PENDING`/`SUCCESS`/`ERROR`).
- **`client.Orcamentos`**: propostas comerciais — listar com filtros, detalhar, criar e excluir em lote.

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
var pessoas = await client.Pessoas.ObterPessoasAsync(filtro);
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
- **`Pessoa`**: Cadastro completo de uma pessoa (detalhe por id).
- **`ItemPessoaResumo`**: Item resumido retornado na listagem por filtro.
- **`PessoaListResponse`**: Resposta da listagem de pessoas (`Items`, `TotalItems`).
- **`PessoaFiltro`**: Filtros para busca de pessoas.
- **`PessoaRequest`**: Dados para criar (POST) ou atualizar integralmente (PUT) uma pessoa.
- **`AtualizacaoParcialPessoa`**: Dados para atualização parcial (PATCH).
- **`ResumoPessoa`**: Resumo retornado ao criar/atualizar uma pessoa.
- **`PessoasEmLoteRequest`** / **`StatusPessoasEmLoteResultado`**: Requisição e resultado das operações em lote.
- **`Empresa`**: Dados da empresa conectada ao token.
- **`EnderecoPessoa`**, **`InscricaoPessoa`**, **`OutroContatoPessoa`**, **`PerfilPessoa`**, **`ContatoCobrancaFaturamento`**: Submodelos unificados de pessoa.
- **`Venda`**: Modelo de venda com todos os campos.
- **`VendaListResponse`**: Resposta da listagem de vendas (`Itens`, `TotalItens`, `Totais`, `Quantidades`).
- **`VendaFiltro`**: Filtros para busca de vendas (28 parâmetros disponíveis).
- **`NotaFiscal`**: Modelo de nota fiscal de produto (NF-e).
- **`NotaFiscalServico`**: Modelo de nota fiscal de serviço (NFS-e).
- **`NotaFiscalFiltro`** / **`NotaFiscalServicoFiltro`**: Filtros de busca de NF-e e NFS-e.
- **`LinkNotaFiscalMdfe`**: Dados para vincular notas fiscais a um MDF-e.
- **`RespostaPaginada<T>`**: Resposta paginada genérica (`Itens`, `Paginacao`).
- **`Paginacao`**: Informações de paginação (`PaginaAtual`, `TotalPaginas`, `TamanhoPagina`, `TotalItens`).
- **`ResumoDeProdutos`** / **`ItemResumoDeProdutos`**: Resposta e item da listagem de produtos.
- **`Produto`**: Detalhe completo de um produto (estoque, fiscal, e-commerce, variações, dimensões).
- **`CriacaoProduto`** / **`AtualizacaoParcialProduto`**: Dados para criar (POST) e atualizar parcialmente (PATCH) um produto.
- **`ProdutoFiltro`**: Filtros para busca de produtos.
- **`CategoriasDeProduto`**, **`CESTsDeProduto`**, **`NCMsDeProduto`**, **`UnidadesDeMedidaDeProduto`**, **`MarcaDeEcommerce`**, **`ProdutoEcommerceCategoria`**: Respostas das listagens auxiliares de produtos.
- **`Servico`**: Detalhe completo de um serviço.
- **`ServicosPorFiltro`**: Resposta paginada da listagem de serviços (`Itens`, `Paginacao`).
- **`CriarServico`** / **`AtualizacaoParcialServico`**: Dados para criar (POST) e atualizar parcialmente (PATCH) um serviço.
- **`ServicoFiltro`**: Filtros para busca de serviços.
- **`ParametrosParaDeletarServicosEmLote`**: IDs para exclusão de serviços em lote.
- **`Protocolo`**: Protocolo de acompanhamento de um evento financeiro (status `PENDING`/`SUCCESS`/`ERROR`).
- **`Orcamento`**: Detalhe completo de um orçamento.
- **`ListagemOrcamentosPorFiltro`**: Resposta da listagem de orçamentos (`Itens`, `TotalItens`).
- **`CriarOrcamento`** / **`ResumoCriacaoOrcamento`**: Dados para criar (POST) e resposta com o ID criado.
- **`OrcamentoFiltro`**: Filtros para busca de orçamentos.
- **`ComposicaoValorOrcamento`**, **`ItemOrcamento`**, **`ClienteOrcamento`**: Submodelos de orçamento.
- **`ExclusaoLoteOrcamento`**: IDs para exclusão de orçamentos em lote.
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
var pessoas = await client.Pessoas.ObterPessoasAsync();

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
var pessoas = await client.Pessoas.ObterPessoasAsync(null, cts.Token);
```

### 3. Tratamento de Erros

```csharp
using ContaAzul.Sdk.Net.Exceptions;

try
{
    var pessoas = await client.Pessoas.ObterPessoasAsync();
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
