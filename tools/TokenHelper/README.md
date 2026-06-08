# ContaAzul Token Helper

Utilitário de console que executa o fluxo OAuth do ContaAzul e imprime os **tokens** e os
comandos `dotnet user-secrets` prontos para os testes de integração.

## Uso

```bash
dotnet run --project tools/TokenHelper
```

Ele vai:
1. Pedir (ou ler de variáveis de ambiente) **Client ID**, **Client Secret** e **Redirect URI**.
2. Gerar a URL de autorização e tentar abrir no navegador.
3. Após você autorizar, pedir a **URL de redirecionamento** (ou só o `code`).
4. Trocar o `code` por tokens e imprimir:
   - `access_token`, `refresh_token`, `expires_in`
   - os comandos `dotnet user-secrets set "ContaAzul:..."` prontos para colar.

## Variáveis de ambiente (opcionais)

Evitam digitar os valores no prompt:

| Variável | Descrição |
|----------|-----------|
| `CONTAAZUL_CLIENT_ID` | Client ID do app |
| `CONTAAZUL_CLIENT_SECRET` | Client Secret do app |
| `CONTAAZUL_REDIRECT_URI` | Redirect URI cadastrada no app |
| `CONTAAZUL_SCOPE` | Escopo (padrão: `openid profile aws.cognito.signin.user.admin`) |

> A **Redirect URI** precisa ser idêntica à cadastrada no app OAuth do ContaAzul.
> Depois de obter os tokens, configure-os conforme o
> [guia de testes de integração](../../contaazul-sdk-net-tests/Integration/README.md).
