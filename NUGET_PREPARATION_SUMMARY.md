# Preparação para Publicação no NuGet - Resumo

Este documento resume todas as alterações e arquivos criados para preparar o projeto ContaAzul.Sdk.Net para publicação no NuGet.

## ? Arquivos Criados/Modificados

### 1. Configuração do Projeto (.csproj)
**Arquivo**: `contaazul-sdk-net/contaazul-sdk-net.csproj`

**Metadados adicionados**:
- `PackageId`: ContaAzul.Sdk.Net
- `Version`: 1.0.0
- `Authors`: Anderson Rocha
- `Company`: ContaAzul SDK
- `Description`: Descrição completa do pacote
- `PackageTags`: contaazul;api;sdk;dotnet;netstandard;integration;erp
- `PackageProjectUrl`: Link do GitHub
- `RepositoryUrl`: URL do repositório Git
- `RepositoryType`: git
- `PackageLicenseExpression`: MIT
- `PackageReadmeFile`: README.md
- `PackageReleaseNotes`: Notas de versão
- `GenerateDocumentationFile`: true (gera XML de documentação)
- `IncludeSymbols`: true (inclui símbolos de debug)
- `SymbolPackageFormat`: snupkg

### 2. Documentação

#### README.md (atualizado)
- Badges do NuGet e licença
- Seção de características
- Instruções de instalação via NuGet
- Exemplos de uso para todas as APIs:
  - PessoasApi
  - VendasApi
  - NotasFiscaisApi
- Guia de autenticação OAuth2
- Exemplos de tratamento de erros
- Documentação de filtros disponíveis

#### NUGET_PUBLISH_GUIDE.md (novo)
- Guia passo a passo para publicação manual
- Instruções para publicação automatizada via GitHub Actions
- Comandos úteis do NuGet CLI
- Checklist pré-publicação
- Guia de versionamento semântico
- Seção de troubleshooting

#### CHANGELOG.md (novo)
- Histórico de versões seguindo Keep a Changelog
- Documentação da versão 1.0.0
- Seção de mudanças planejadas
- Guia de tipos de mudanças

### 3. Especificação NuGet

#### ContaAzul.Sdk.Net.nuspec (novo)
- Arquivo de especificação opcional do NuGet
- Metadados completos do pacote
- Lista de dependências
- Arquivos a incluir no pacote

### 4. Automação CI/CD

#### .github/workflows/publish-nuget.yml (novo)
**Workflow de publicação**:
- Trigger: Release publicado ou manual
- Build do projeto
- Execução de testes
- Criação do pacote NuGet
- Publicação automática no NuGet.org
- Upload de artefatos

#### .github/workflows/ci.yml (novo)
**Workflow de Integração Contínua**:
- Trigger: Push/PR para main/develop
- Testa em múltiplas versões do .NET (6.0, 7.0, 8.0)
- Build e testes automáticos
- Geração de relatório de cobertura
- Upload para Codecov

### 5. Arquivo de Licença
**Arquivo**: `LICENSE`
- Licença MIT (já existia)

## ?? Como Publicar

### Opção 1: Publicação Manual

```bash
# 1. Navegar até o diretório do projeto
cd contaazul-sdk-net

# 2. Compilar em Release
dotnet build -c Release

# 3. Criar o pacote
dotnet pack -c Release

# 4. Publicar (substitua YOUR_API_KEY)
cd bin\Release
dotnet nuget push ContaAzul.Sdk.Net.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Opção 2: Publicação Automatizada via GitHub

1. **Configurar API Key**:
   - Vá em Settings > Secrets > Actions
   - Crie o secret `NUGET_API_KEY` com sua chave do NuGet.org

2. **Criar Release**:
   - Vá em Releases > Create a new release
   - Crie uma tag (ex: `v1.0.0`)
   - Publique o release
   - O GitHub Actions publicará automaticamente

3. **Ou executar manualmente**:
   - Vá em Actions > Publish to NuGet
   - Clique em "Run workflow"
   - Informe a versão
   - Execute

## ?? Próximos Passos

### Antes de Publicar a Primeira Vez

1. **Obter API Key do NuGet.org**:
   - Acesse: https://www.nuget.org/account/apikeys
   - Crie uma nova API Key com permissões de push
   - Guarde a chave em local seguro

2. **Testar Localmente**:
   ```bash
   # Criar feed local
   mkdir C:\local-nuget-feed
   nuget add bin\Release\ContaAzul.Sdk.Net.1.0.0.nupkg -source C:\local-nuget-feed
   
   # Em projeto de teste
   dotnet nuget add source C:\local-nuget-feed -n LocalFeed
   dotnet add package ContaAzul.Sdk.Net --version 1.0.0
   ```

3. **Verificar Conteúdo do Pacote**:
   ```bash
   # O .nupkg é um arquivo ZIP
   # Renomeie e extraia para verificar o conteúdo
   ```

4. **Atualizar Data de Release**:
   - Atualize as datas no CHANGELOG.md
   - Atualize as datas nas release notes do .csproj

### Após a Publicação

1. **Verificar no NuGet.org**:
   - Aguarde alguns minutos para indexação
   - Acesse: https://www.nuget.org/packages/ContaAzul.Sdk.Net/
   - Verifique todas as informações

2. **Testar Instalação Real**:
   ```bash
   dotnet new console -n TestContaAzulSdk
   cd TestContaAzulSdk
   dotnet add package ContaAzul.Sdk.Net
   ```

3. **Anunciar**:
   - Criar post no GitHub Discussions
   - Atualizar README se necessário
   - Compartilhar nas redes sociais

## ?? Checklist Final

Antes de publicar, verifique:

- [x] Versão atualizada no .csproj (1.0.0)
- [x] README.md completo e atualizado
- [x] CHANGELOG.md atualizado
- [x] Licença MIT configurada
- [x] Documentação XML habilitada
- [x] Workflows do GitHub Actions criados
- [ ] Todos os testes passando
- [ ] API Key do NuGet.org configurada
- [ ] Pacote testado localmente
- [ ] Tag Git criada (ex: v1.0.0)

## ?? Informações do Pacote

| Propriedade | Valor |
|------------|-------|
| Package ID | ContaAzul.Sdk.Net |
| Versão Inicial | 1.0.0 |
| Target Framework | .NET Standard 2.0 |
| Licença | MIT |
| Autor | Anderson Rocha |
| Repositório | https://github.com/andersonrocha/contaazul-dotnet |
| Tags | contaazul, api, sdk, dotnet, netstandard, integration, erp |

## ?? Recursos Adicionais

- [Documentação NuGet](https://docs.microsoft.com/pt-br/nuget/)
- [Guia de Publicação](https://docs.microsoft.com/pt-br/nuget/nuget-org/publish-a-package)
- [Versionamento Semântico](https://semver.org/lang/pt-BR/)
- [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/)
- [GitHub Actions](https://docs.github.com/pt/actions)

## ?? Problemas Conhecidos

Nenhum problema conhecido no momento.

## ?? Dicas

1. **Versionamento**: Sempre incremente a versão para cada publicação. Não é possível republicar a mesma versão.

2. **Testes**: Execute todos os testes antes de publicar:
   ```bash
   dotnet test --verbosity normal
   ```

3. **Validação**: Use o comando de verificação do NuGet:
   ```bash
   dotnet nuget verify bin\Release\ContaAzul.Sdk.Net.1.0.0.nupkg
   ```

4. **Documentação**: Mantenha o README.md sempre atualizado com exemplos funcionais.

5. **Segurança**: NUNCA commite API Keys no repositório. Use sempre GitHub Secrets.

---

**Status**: ? Projeto pronto para publicação no NuGet!

**Última atualização**: 2024-01-XX
