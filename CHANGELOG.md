# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Versionamento Semântico](https://semver.org/lang/pt-BR/).

## [0.2.2] - 2026-06-24

> Primeira versão publicada no nuget.org. Consolida todo o desenvolvimento das
> seções 0.1.0 a 0.2.2 abaixo (que nunca chegaram a ser publicadas isoladamente).

### Corrigido
- `BuildAuthorizationUrl` separa os scopes com `+` (ex.: `openid+profile+...`) em vez de `%20`.

## [0.2.1] - Não lançado

### Corrigido
- `BuildAuthorizationUrl` agora envia o `redirect_uri` literal (sem percent-encode de `:`/`/`),
  formato que o servidor de autorização do ContaAzul interpreta corretamente.

## [0.2.0] - Não lançado

> Desenvolvimento inicial (major `0`): a API pública ainda pode mudar entre versões `0.x`.

### Adicionado
- API de Cobranças (`CobrancasApi`): gerar, consultar e cancelar cobranças
- API de Baixas (`BaixasApi`): criar, listar, consultar, atualizar e excluir baixas de parcelas
- API de Financeiro (`FinanceiroApi`): centros de custo, categorias, categorias DRE, contas
  financeiras, saldo atual, transferências, contas a pagar/receber, parcelas, eventos
  alterados e saldos iniciais
- Modelos financeiros unificados (`ComposicaoValorFinanceiro`, `RespostaItens<T>`,
  `ContaPagarReceber`) para evitar duplicação

## [0.1.0] - Não lançado

> 4 das 11 APIs do ContaAzul estão implementadas; as demais virão nas próximas versões `0.x`.
> A `1.0.0` será publicada quando todas as APIs estiverem prontas e a superfície pública estável.

### Adicionado
- Implementação inicial do SDK ContaAzul para .NET
- Suporte completo para autenticação OAuth2
- Refresh automático de tokens de acesso
- API de Pessoas (`PessoasApi`)
  - `ObterPessoasAsync`: listagem por filtro; `ObterPessoaPorIdAsync` e `ObterPessoaPorLegadoIdAsync`: detalhe
  - `ObterEmpresaConectadaAsync`: dados da empresa vinculada ao token
  - `CriarPessoaAsync` (POST), `AtualizarPessoaAsync` (PUT) e `AtualizarParcialmentePessoaAsync` (PATCH)
  - `AtivarPessoasEmLoteAsync`, `InativarPessoasEmLoteAsync` e `ExcluirPessoasEmLoteAsync`: operações em lote
  - Submodelos unificados (`EnderecoPessoa`, `InscricaoPessoa`, `OutroContatoPessoa`, `PerfilPessoa`, `ContatoCobrancaFaturamento`)
- API de Vendas (`VendasApi`)
  - Listagem de vendas com filtros avançados
  - Suporte para 28 parâmetros de filtro diferentes
  - Filtros por data, cliente, vendedor, situação, tipo, etc.
- API de Notas Fiscais (`NotasFiscaisApi`)
  - `ObterNotasFiscaisAsync`: notas fiscais de produto (NF-e) por filtro
  - `ObterNotasFiscaisServicoAsync`: notas fiscais de serviço (NFS-e) por filtro
  - `VincularNotaFiscalMdfeAsync`: vínculo de notas fiscais a um MDF-e
  - `ObterNotaFiscalPorChaveAsync`: consulta do XML de uma nota por chave de acesso
  - Resposta paginada genérica `RespostaPaginada<T>`
- Sistema robusto de construção automática de query strings
- Tratamento automático de erros 401 com refresh de token
- Documentação XML completa
- Suporte para .NET Standard 2.0
- Compatibilidade com .NET Framework 4.6.1+, .NET Core 2.0+ e .NET 5+

### Características Técnicas
- Totalmente assíncrono (async/await)
- Uso de `CancellationToken` em todos os métodos
- Sistema de atributos para mapeamento de parâmetros de query
- Encoding automático de URLs
- Tratamento de tipos nullable
- Conversão automática de tipos (bool, int, string, DateTime, etc.)

### Dependências
- Newtonsoft.Json 13.0.1
- System.Net.Http 4.3.4

## [Unreleased]

### Planejado
- API de Produtos
- API de Categorias
- API de Contratos
- API de Anexos
- Suporte para webhooks
- Cache de requisições
- Rate limiting
- Retry policies personalizáveis
- Logging estruturado
- Métricas e telemetria

---

## Tipos de mudanças

- **Adicionado** para novas funcionalidades
- **Modificado** para mudanças em funcionalidades existentes
- **Obsoleto** para funcionalidades que serão removidas em breve
- **Removido** para funcionalidades removidas
- **Corrigido** para correções de bugs
- **Segurança** para vulnerabilidades corrigidas

## Versionamento

Este projeto segue o [Versionamento Semântico](https://semver.org/lang/pt-BR/):

- **MAJOR** (X.0.0): Mudanças incompatíveis com versões anteriores
- **MINOR** (x.X.0): Novas funcionalidades mantendo compatibilidade
- **PATCH** (x.x.X): Correções de bugs e melhorias menores
