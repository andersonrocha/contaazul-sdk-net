# Testes de Integração ao Vivo

Estes testes batem na **API real** do ContaAzul (`https://api-v2.contaazul.com`).

São marcados com `[Explicit]` + `[Category("Integration")]`, portanto **não** rodam no
`dotnet test` padrão nem no CI. Eles só executam quando filtrados explicitamente e quando
as credenciais estão presentes (caso contrário são ignorados com `Assert.Ignore`).

## Configuração (seção `ContaAzul`)

As credenciais ficam na seção `ContaAzul`, resolvida nesta ordem (a última vence):
**`appsettings.Integration.json`** → **User Secrets** → **variáveis de ambiente**.

| Chave | Obrigatória | Descrição |
|-------|:-----------:|-----------|
| `ClientId` | ✅ | Client ID do app OAuth |
| `ClientSecret` | ✅ | Client Secret do app OAuth |
| `AccessToken` | ☑️* | Access token válido (≈1h) |
| `RefreshToken` | ☑️* | Refresh token (o cliente renova sozinho quando o access expira) |
| `ApiBaseUrl` | — | Opcional: sobrescreve a URL base (ex.: ambiente de testes) |
| `AllowWrite` | — | `true` habilita os testes que criam/alteram/excluem dados |

\* Informe **ao menos um** entre `AccessToken` e `RefreshToken`.

> **Como obter os tokens:** use o utilitário [`tools/TokenHelper`](../../tools/TokenHelper/README.md)
> (`dotnet run --project tools/TokenHelper`) — ele executa o fluxo OAuth e imprime os tokens
> junto com os comandos `dotnet user-secrets set` prontos para colar.

> **Rotação de refresh token:** o ContaAzul troca o refresh token a cada renovação.
> Quando isso ocorre durante os testes, o novo valor é escrito no log do teste
> (`TestContext.Progress`) para que você possa atualizá-lo na sua configuração.

### Opção A — appsettings.Integration.json

Copie o template e preencha (o arquivo real está no `.gitignore`):

```bash
cp appsettings.Integration.template.json appsettings.Integration.json
# edite appsettings.Integration.json com suas credenciais
```

### Opção B — User Secrets (recomendado para a máquina do dev)

```bash
cd contaazul-sdk-net-tests
dotnet user-secrets set "ContaAzul:ClientId" "..."
dotnet user-secrets set "ContaAzul:ClientSecret" "..."
dotnet user-secrets set "ContaAzul:AccessToken" "..."     # e/ou ContaAzul:RefreshToken
dotnet user-secrets set "ContaAzul:AllowWrite" "true"      # opcional
```

### Opção C — variáveis de ambiente (CI)

Use o separador `__` (duplo underscore): `ContaAzul__ClientId`, `ContaAzul__ClientSecret`,
`ContaAzul__AccessToken`, `ContaAzul__RefreshToken`, `ContaAzul__AllowWrite`.

## Como executar

```bash
dotnet test --filter TestCategory=Integration
```

Para incluir os testes de escrita, defina `ContaAzul:AllowWrite = true` (em qualquer uma das
fontes acima) antes de rodar.

## Escopo

- **Somente leitura (padrão):** Pessoas, Vendas, Contratos e Notas Fiscais — listagens,
  consulta por id, empresa conectada e próximo número. Não alteram dados.
- **Escrita (opt-in via `CONTAAZUL_ALLOW_WRITE=true`):** ciclo de vida de Pessoa
  (criar → obter → atualizar parcial → excluir), com remoção garantida no `finally`.

Os testes de leitura validam que a **serialização/desserialização real** funciona ponta a
ponta — pegando qualquer divergência de schema que os exemplos do OpenAPI não cobrem.
