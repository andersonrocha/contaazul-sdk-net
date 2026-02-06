# Guia de Contribuição

Obrigado por considerar contribuir com o ContaAzul.Sdk.Net! Este documento fornece diretrizes para contribuições.

## ?? Como Contribuir

### Reportar Bugs

Se você encontrar um bug:

1. Verifique se já não existe uma [issue aberta](https://github.com/andersonrocha/contaazul-dotnet/issues)
2. Se não existir, [crie uma nova issue](https://github.com/andersonrocha/contaazul-dotnet/issues/new) com:
   - Título descritivo
   - Descrição detalhada do problema
   - Passos para reproduzir
   - Comportamento esperado vs. atual
   - Versão do SDK e .NET
   - Código de exemplo (se possível)

### Sugerir Melhorias

Para sugerir novas funcionalidades ou melhorias:

1. Abra uma [nova issue](https://github.com/andersonrocha/contaazul-dotnet/issues/new)
2. Use o prefixo `[Feature Request]` no título
3. Descreva:
   - O problema que a feature resolve
   - Como você imagina a solução
   - Exemplos de uso
   - Alternativas consideradas

### Enviar Pull Requests

1. **Fork o repositório**
   ```bash
   # Via GitHub ou CLI
   gh repo fork andersonrocha/contaazul-dotnet
   ```

2. **Clone seu fork**
   ```bash
   git clone https://github.com/SEU-USUARIO/contaazul-dotnet.git
   cd contaazul-dotnet
   ```

3. **Crie uma branch**
   ```bash
   git checkout -b feature/minha-feature
   # ou
   git checkout -b fix/meu-bugfix
   ```

4. **Faça suas alterações**
   - Siga as [convenções de código](#convenções-de-código)
   - Adicione testes para novas funcionalidades
   - Atualize a documentação se necessário

5. **Commit suas mudanças**
   ```bash
   git add .
   git commit -m "feat: adiciona nova funcionalidade X"
   ```
   
   Siga o [Conventional Commits](#conventional-commits)

6. **Push para seu fork**
   ```bash
   git push origin feature/minha-feature
   ```

7. **Abra um Pull Request**
   - Vá até o repositório original no GitHub
   - Clique em "Compare & pull request"
   - Preencha o template do PR
   - Aguarde a revisão

## ?? Convenções de Código

### Estilo C#

Seguimos as [convenções de código C# da Microsoft](https://docs.microsoft.com/pt-br/dotnet/csharp/fundamentals/coding-style/coding-conventions):

```csharp
// ? BOM
public async Task<PessoaListResponse> GetPessoasAsync(PessoaFiltro filtro = null)
{
    var endpoint = QueryStringBuilder.BuildEndpoint(PessoasEndpoint, filtro);
    return await _client.GetAsync<PessoaListResponse>(endpoint).ConfigureAwait(false);
}

// ? EVITAR
public async Task<PessoaListResponse> getPessoas(PessoaFiltro filtro = null)
{
    var endpoint=QueryStringBuilder.BuildEndpoint(PessoasEndpoint,filtro);
    return await _client.GetAsync<PessoaListResponse>(endpoint);
}
```

### Diretrizes Específicas

1. **Nomenclatura**
   - Classes: `PascalCase`
   - Métodos: `PascalCase`
   - Variáveis locais: `camelCase`
   - Campos privados: `_camelCase`
   - Constantes: `PascalCase`

2. **Async/Await**
   - Sempre use `ConfigureAwait(false)` em bibliotecas
   - Métodos assíncronos devem terminar com `Async`
   - Use `CancellationToken` quando apropriado

3. **Documentação XML**
   ```csharp
   /// <summary>
   /// Obtém a lista de pessoas do ContaAzul.
   /// </summary>
   /// <param name="filtro">Filtros opcionais para a busca.</param>
   /// <param name="cancellationToken">Token para cancelamento da operação.</param>
   /// <returns>Lista paginada de pessoas.</returns>
   public async Task<PessoaListResponse> GetPessoasAsync(
       PessoaFiltro filtro = null, 
       CancellationToken cancellationToken = default)
   {
       // implementação
   }
   ```

4. **Tratamento de Erros**
   - Use exceções para erros excepcionais
   - Valide parâmetros de entrada
   - Documente exceções que podem ser lançadas

## ?? Testes

### Executar Testes

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Testes de uma classe específica
dotnet test --filter FullyQualifiedName~NotasFiscaisApiTests
```

### Escrever Testes

```csharp
using NUnit.Framework;

[TestFixture]
public class MinhaNovaFuncionalidadeTests
{
    [Test]
    public void WhenCondicaoThenResultadoEsperado()
    {
        // Arrange
        var input = "valor";
        
        // Act
        var result = MinhaFuncao(input);
        
        // Assert
        Assert.That(result, Is.EqualTo("esperado"));
    }
}
```

### Cobertura de Testes

- Novas funcionalidades devem ter pelo menos 80% de cobertura
- Casos de erro devem ser testados
- Casos extremos (edge cases) devem ser considerados

## ?? Conventional Commits

Usamos [Conventional Commits](https://www.conventionalcommits.org/pt-br/) para mensagens de commit:

### Formato

```
<tipo>[escopo opcional]: <descrição>

[corpo opcional]

[rodapé opcional]
```

### Tipos

- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Apenas documentação
- `style`: Formatação, ponto e vírgula, etc (sem mudança de código)
- `refactor`: Refatoração de código
- `perf`: Melhoria de performance
- `test`: Adição ou correção de testes
- `chore`: Tarefas de build, configs, etc

### Exemplos

```bash
# Feature
git commit -m "feat(api): adiciona suporte para API de Produtos"

# Bugfix
git commit -m "fix(auth): corrige refresh de token expirado"

# Documentação
git commit -m "docs: atualiza README com exemplos de uso"

# Refatoração
git commit -m "refactor(querybuilder): simplifica construção de endpoint"

# Breaking change
git commit -m "feat(api)!: altera assinatura do método GetPessoas

BREAKING CHANGE: método GetPessoas agora retorna Task<Result<PessoaListResponse>>"
```

## ??? Estrutura do Projeto

```
contaazul-dotnet/
??? contaazul-sdk-net/              # Projeto principal
?   ??? Apis/                       # Classes de API
?   ??? Attributes/                 # Atributos customizados
?   ??? Helpers/                    # Classes auxiliares
?   ??? Http/                       # Base HTTP client
?   ??? Models/                     # Modelos de dados
?   ??? ContaAzulApiClient.cs      # Cliente principal
??? contaazul-sdk-net-tests/       # Projeto de testes
?   ??? *ApiTests.cs               # Testes das APIs
?   ??? *Tests.cs                  # Outros testes
??? .github/                        # GitHub workflows
?   ??? workflows/
?       ??? ci.yml                 # Integração contínua
?       ??? publish-nuget.yml      # Publicação NuGet
??? CHANGELOG.md                    # Histórico de mudanças
??? README.md                       # Documentação principal
??? LICENSE                         # Licença MIT
??? CONTRIBUTING.md                 # Este arquivo
```

## ?? Processo de Revisão

### O que revisamos

1. **Funcionalidade**
   - A mudança faz o que propõe?
   - Há efeitos colaterais?
   - É backward compatible?

2. **Qualidade do Código**
   - Segue as convenções?
   - Está bem documentado?
   - É testável?

3. **Testes**
   - Há testes suficientes?
   - Os testes passam?
   - Cobertura adequada?

4. **Documentação**
   - README atualizado?
   - XML comments completos?
   - CHANGELOG atualizado?

### Tempo de Resposta

- Revisaremos PRs em até 7 dias
- Bugs críticos têm prioridade
- Feedback será construtivo

## ?? Áreas que Precisam de Ajuda

Estamos especialmente interessados em contribuições nas seguintes áreas:

- [ ] Implementação de APIs faltantes (Produtos, Categorias, Contratos)
- [ ] Melhoria da documentação e exemplos
- [ ] Testes de integração
- [ ] Suporte para webhooks
- [ ] Performance e otimizações
- [ ] Correção de bugs conhecidos

## ?? Dicas para Contribuidores

1. **Comece Pequeno**
   - Correções de typos e melhorias na documentação são ótimos primeiros PRs
   - Familiarize-se com o código antes de grandes mudanças

2. **Comunique-se**
   - Abra uma issue antes de grandes features
   - Pergunte se não tiver certeza
   - Seja receptivo a feedback

3. **Siga os Padrões**
   - Veja como o código existente está estruturado
   - Mantenha consistência
   - Use as mesmas bibliotecas e padrões

4. **Teste Localmente**
   - Execute todos os testes antes de enviar PR
   - Teste em diferentes cenários
   - Verifique se a documentação está correta

## ?? Contato

- Issues: https://github.com/andersonrocha/contaazul-dotnet/issues
- Discussions: https://github.com/andersonrocha/contaazul-dotnet/discussions

## ?? Código de Conduta

### Nosso Compromisso

Estamos comprometidos em proporcionar uma experiência acolhedora e inspiradora para todos.

### Comportamento Esperado

- Use linguagem acolhedora e inclusiva
- Respeite diferentes pontos de vista
- Aceite críticas construtivas graciosamente
- Foque no que é melhor para a comunidade

### Comportamento Inaceitável

- Assédio de qualquer tipo
- Trolling ou comentários insultuosos
- Publicar informações privadas de terceiros
- Conduta não profissional

### Aplicação

Violações podem resultar em:
- Aviso
- Banimento temporário
- Banimento permanente

Reporte comportamentos inaceitáveis para: anderson@exemplo.com

## ?? Recursos Adicionais

- [Documentação .NET](https://docs.microsoft.com/pt-br/dotnet/)
- [API ContaAzul](https://developers.contaazul.com/)
- [Conventional Commits](https://www.conventionalcommits.org/pt-br/)
- [Semantic Versioning](https://semver.org/lang/pt-BR/)

---

**Obrigado por contribuir! Cada contribuição, grande ou pequena, é valiosa para o projeto.** ??
