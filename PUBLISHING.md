# Publicação do pacote (NuGet.org)

Este SDK é publicado no **NuGet.org** (registro público do .NET) automaticamente via
GitHub Actions quando um **Release** é criado no GitHub.

A publicação usa **Trusted Publishing (OIDC)**: o workflow troca o token do GitHub Actions
por uma **API key temporária** (validade de 1h) emitida pelo NuGet.org. **Não** há
`NUGET_API_KEY` de longa duração armazenado no repositório.

## Pré-requisitos (uma vez)

1. **Repositório no GitHub** em `github.com/andersonrocha/contaazul-sdk-net`
   (o `origin` já aponta para lá). Funciona com repositório público ou privado — se for
   **privado**, a política de Trusted Publishing nasce "temporariamente ativa por 7 dias"
   até o primeiro publish (proteção anti-resurrection); basta publicar dentro do prazo.

2. **Conta no NuGet.org** com a opção **Trusted Publishing** disponível (em rollout gradual).

3. **Política de Trusted Publishing** no NuGet.org:
   - Acesse https://www.nuget.org → clique no seu usuário → *Trusted Publishing* → adicione uma política:

   | Campo | Valor |
   |-------|-------|
   | Repository Owner | `andersonrocha` |
   | Repository | `contaazul-sdk-net` |
   | Workflow File | `publish-nuget.yml` *(só o nome, sem `.github/workflows/`)* |
   | Environment | *(vazio)* |
   | Owner da política | seu usuário ou a organização dona do pacote |

4. **Secret no GitHub** com o nome de usuário do NuGet.org: *Settings → Secrets and variables →
   Actions → New repository secret*
   - Nome: `NUGET_USER`
   - Valor: seu **nome de usuário (perfil)** do NuGet.org — **não** o e-mail.

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

> O Trusted Publishing (OIDC) vale **apenas para o CI**. Para um push manual da sua máquina
> você ainda precisa de uma **API key pessoal** do NuGet.org (*API Keys → Create*).

```powershell
dotnet pack contaazul-sdk-net/contaazul-sdk-net.csproj -c Release -p:Version=0.2.2 -o ./artifacts
dotnet nuget push "./artifacts/*.nupkg" --api-key <SUA_KEY> --source https://api.nuget.org/v3/index.json --skip-duplicate
```

## Notas

- O pacote inclui **símbolos** (`.snupkg`) e **Source Link** (debug com o código-fonte do GitHub).
- Alvo **netstandard2.0** → compatível com .NET Framework 4.6.1+, .NET Core 2.0+ e .NET 5–9.
- Dependências fixadas em **8.0.x (LTS)** para máxima compatibilidade.
- Os testes de **integração** são `[Explicit]` e **não** rodam no CI/publish (não vazam
  credenciais nem batem na API real).
