using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Pessoas;

namespace ContaAzul.Sdk.Net.Tests.Integration;

/// <summary>
/// Testes de integração que ALTERAM dados na conta real. Só rodam com
/// <c>CONTAAZUL_ALLOW_WRITE=true</c>. Criam um registro temporário e o removem ao final.
/// </summary>
[TestFixture]
[Explicit("Teste de integração ao vivo (escrita): requer credenciais reais e ContaAzul:AllowWrite=true.")]
[Category("Integration")]
public class PessoasWriteIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task CriarObterAtualizarExcluir_Pessoa_Lifecycle()
    {
        RequireWrite();

        var sufixo = Guid.NewGuid().ToString("N").Substring(0, 8);
        string? id = null;

        try
        {
            var criada = await Client.Pessoas.CriarPessoaAsync(new PessoaRequest
            {
                Nome = $"SDK Integração {sufixo}",
                TipoPessoa = "Física",
                Perfis = new List<PerfilPessoa> { new PerfilPessoa { TipoPerfil = "Cliente" } }
            });

            Assert.That(criada, Is.Not.Null);
            Assert.That(criada.Id, Is.Not.Null.And.Not.Empty, "A criação deve retornar o id da pessoa.");
            id = criada.Id;

            var obtida = await Client.Pessoas.ObterPessoaPorIdAsync(id!);
            Assert.That(obtida.Id, Is.EqualTo(id));

            await Client.Pessoas.AtualizarParcialmentePessoaAsync(id!, new AtualizacaoParcialPessoa
            {
                Observacao = "Atualizado pelo teste de integração do SDK."
            });
        }
        finally
        {
            if (id is not null)
            {
                await Client.Pessoas.ExcluirPessoasEmLoteAsync(
                    new PessoasEmLoteRequest { Uuids = new List<string> { id } });
            }
        }
    }
}
