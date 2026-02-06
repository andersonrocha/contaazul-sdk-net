# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Versionamento Semântico](https://semver.org/lang/pt-BR/).

## [1.0.0] - 2024-01-XX

### Adicionado
- Implementação inicial do SDK ContaAzul para .NET
- Suporte completo para autenticação OAuth2
- Refresh automático de tokens de acesso
- API de Pessoas (`PessoasApi`)
  - Listagem de pessoas com filtros avançados
  - Suporte para paginação
  - Filtros por tipo de pessoa, perfil, endereço, datas, etc.
- API de Vendas (`VendasApi`)
  - Listagem de vendas com filtros avançados
  - Suporte para 28 parâmetros de filtro diferentes
  - Filtros por data, cliente, vendedor, situação, tipo, etc.
- API de Notas Fiscais de Serviço (`NotasFiscaisApi`)
  - Listagem de notas fiscais com filtros
  - Filtros por data de competência, cliente, número, status, etc.
  - Suporte para faixas de numeração NFS-e e RPS
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
