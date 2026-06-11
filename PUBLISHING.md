# Publicação do pacote (NuGet.org)

Este SDK é publicado no **NuGet.org** (registro público do .NET) automaticamente via
GitHub Actions quando um **Release** é criado no GitHub.

## Pré-requisitos (uma vez)

1. **Repositório público no GitHub** em `github.com/andersonrocha/contaazul-sdk-net`
   (o `origin` já aponta para lá). Em *Settings → General*, deixe o repositório **Public**.

2. **Conta no NuGet.org** e uma **API Key**:
   - Acesse https://www.nuget.org → *API Keys* → *Create*.
   - Escopo: *Push new packages and package versions*; Glob pattern: `ContaAzul.Sdk.Net` (ou `*`).
   - Copie a chave.

3. **Secret no GitHub**: *Settings → Secrets and variables → Actions → New repository secret*
   - Nome: `NUGET_API_KEY`
   - Valor: a API key do NuGet.org.

> O `PackageId` é **`ContaAzul.Sdk.Net`**. Confirme que está disponível em
> https://www.nuget.org/packages/ContaAzul.Sdk.Net antes do primeiro push.

## Publicar uma versão

### Opção A — via Release (recomendado)
1. Atualize o `CHANGELOG.md` e, se quiser, a `<Version>`/`<PackageReleaseNotes>` no
   [csproj](contaazul-sdk-net/contaazul-sdk-net.csproj).
2. No GitHub: *Releases → Draft a new release*.
3. Crie uma **tag** no formato `v<versão>` (ex.: `v0.1.0`). O workflow remove o `v` e usa
   `0.1.0` como versão do pacote.
4. *Publish release* → o workflow [publish-nuget.yml](.github/workflows/publish-nuget.yml)
   builda, testa (integração é excluída), empacota e dá push no NuGet.org.

### Opção B — manual (workflow_dispatch)
*Actions → Publish to NuGet → Run workflow* e informe a versão (ex.: `0.1.0`).

## Publicar localmente (opcional)

```powershell
dotnet pack contaazul-sdk-net/contaazul-sdk-net.csproj -c Release -p:Version=0.1.0 -o ./artifacts
dotnet nuget push "./artifacts/*.nupkg" --api-key <SUA_KEY> --source https://api.nuget.org/v3/index.json --skip-duplicate
```

## Notas

- O pacote inclui **símbolos** (`.snupkg`) e **Source Link** (debug com o código-fonte do GitHub).
- Alvo **netstandard2.0** → compatível com .NET Framework 4.6.1+, .NET Core 2.0+ e .NET 5–9.
- Dependências fixadas em **8.0.x (LTS)** para máxima compatibilidade.
- Os testes de **integração** são `[Explicit]` e **não** rodam no CI/publish (não vazam
  credenciais nem batem na API real).
