# Guia de Publicação no NuGet

Este documento descreve o processo para publicar o pacote ContaAzul.Sdk.Net no NuGet.

## Pré-requisitos

1. **Conta no NuGet.org**
   - Crie uma conta em https://www.nuget.org/
   - Obtenha uma API Key em: https://www.nuget.org/account/apikeys

2. **.NET SDK instalado**
   - Certifique-se de ter o .NET SDK instalado
   - Execute: `dotnet --version` para verificar

3. **Código-fonte atualizado**
   - Certifique-se de que todos os testes passam
   - Atualize a versão no arquivo `.csproj`

## Passo a Passo

### 1. Atualizar Versão

Edite o arquivo `contaazul-sdk-net\contaazul-sdk-net.csproj` e atualize a tag `<Version>`:

```xml
<Version>1.0.0</Version>
```

Siga o Versionamento Semântico (SemVer):
- **MAJOR**: Mudanças incompatíveis com versões anteriores
- **MINOR**: Novas funcionalidades mantendo compatibilidade
- **PATCH**: Correções de bugs

### 2. Compilar em Release

```bash
# Navegue até o diretório do projeto
cd contaazul-sdk-net

# Compile em modo Release
dotnet build -c Release
```

### 3. Criar o Pacote NuGet

```bash
# Criar o pacote .nupkg
dotnet pack -c Release

# O pacote será criado em: bin\Release\ContaAzul.Sdk.Net.1.0.0.nupkg
```

### 4. Testar Localmente (Opcional mas Recomendado)

Antes de publicar, teste o pacote localmente:

```bash
# Criar uma pasta local para servir como feed NuGet
mkdir C:\local-nuget-feed

# Adicionar o pacote ao feed local
nuget add bin\Release\ContaAzul.Sdk.Net.1.0.0.nupkg -source C:\local-nuget-feed

# Em um projeto de teste, adicione o feed local
dotnet nuget add source C:\local-nuget-feed -n LocalFeed

# Instale o pacote
dotnet add package ContaAzul.Sdk.Net --version 1.0.0
```

### 5. Publicar no NuGet.org

```bash
# Navegue até a pasta onde está o .nupkg
cd bin\Release

# Publique o pacote (substitua YOUR_API_KEY pela sua chave)
dotnet nuget push ContaAzul.Sdk.Net.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### 6. Verificar Publicação

1. Acesse https://www.nuget.org/packages/ContaAzul.Sdk.Net/
2. Aguarde alguns minutos para o pacote ser indexado
3. Verifique se todas as informações estão corretas:
   - Versão
   - Descrição
   - Tags
   - Dependências
   - README

## Publicação Automatizada com GitHub Actions

Para automatizar o processo, você pode criar um workflow do GitHub Actions:

### Criar `.github/workflows/publish-nuget.yml`:

```yaml
name: Publish to NuGet

on:
  release:
    types: [published]

jobs:
  publish:
    runs-on: ubuntu-latest
    
    steps:
    - name: Checkout
      uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '6.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
      working-directory: ./contaazul-sdk-net
    
    - name: Build
      run: dotnet build -c Release --no-restore
      working-directory: ./contaazul-sdk-net
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
      working-directory: ./contaazul-sdk-net-tests
    
    - name: Pack
      run: dotnet pack -c Release --no-build
      working-directory: ./contaazul-sdk-net
    
    - name: Push to NuGet
      run: dotnet nuget push "bin/Release/*.nupkg" --api-key ${{secrets.NUGET_API_KEY}} --source https://api.nuget.org/v3/index.json --skip-duplicate
      working-directory: ./contaazul-sdk-net
```

### Configurar Secret no GitHub:

1. Acesse: `Settings` > `Secrets and variables` > `Actions`
2. Clique em `New repository secret`
3. Nome: `NUGET_API_KEY`
4. Valor: Sua API Key do NuGet.org
5. Salve

### Criar Release:

1. Vá até `Releases` no GitHub
2. Clique em `Create a new release`
3. Crie uma nova tag (ex: `v1.0.0`)
4. Preencha o título e descrição
5. Publique
6. O GitHub Actions publicará automaticamente no NuGet

## Comandos Úteis

### Ver informações do pacote
```bash
nuget spec contaazul-sdk-net.csproj
```

### Validar pacote antes de publicar
```bash
dotnet nuget verify bin\Release\ContaAzul.Sdk.Net.1.0.0.nupkg
```

### Listar conteúdo do pacote
```bash
# Renomeie .nupkg para .zip
cp bin\Release\ContaAzul.Sdk.Net.1.0.0.nupkg ContaAzul.Sdk.Net.1.0.0.zip

# Extraia e inspecione o conteúdo
```

### Despublicar/Deslistar versão (emergência)
```bash
dotnet nuget delete ContaAzul.Sdk.Net 1.0.0 --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

**Nota**: Isso apenas deslista o pacote, não o remove permanentemente. Ninguém poderá mais instalá-lo, mas quem já instalou continuará funcionando.

## Checklist Pré-Publicação

- [ ] Todos os testes unitários passam
- [ ] Versão atualizada no `.csproj`
- [ ] README.md atualizado
- [ ] CHANGELOG atualizado (se houver)
- [ ] Release notes preenchidas
- [ ] Licença correta (MIT)
- [ ] Documentação XML gerada
- [ ] Pacote testado localmente
- [ ] Commit e push realizados
- [ ] Tag criada no Git (ex: `v1.0.0`)

## Versionamento

Siga este padrão para cada release:

| Tipo de Mudança | Versão Anterior | Nova Versão |
|-----------------|----------------|-------------|
| Correção de bug | 1.0.0 | 1.0.1 |
| Nova feature (compatível) | 1.0.1 | 1.1.0 |
| Breaking change | 1.1.0 | 2.0.0 |

## Suporte

- Documentação NuGet: https://docs.microsoft.com/pt-br/nuget/
- Guia de Publicação: https://docs.microsoft.com/pt-br/nuget/nuget-org/publish-a-package
- GitHub Actions: https://docs.github.com/pt/actions

## Troubleshooting

### Erro: "Package already exists"
- Não é possível republicar a mesma versão
- Incremente a versão e tente novamente

### Erro: "Invalid API Key"
- Verifique se a API Key está correta
- Certifique-se de que a key tem permissões para publicar pacotes

### Pacote não aparece no NuGet.org
- Aguarde alguns minutos (indexação pode demorar)
- Verifique se não há erros de validação no portal
