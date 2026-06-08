using ContaAzul.Sdk.Net.Models.Pessoas;
using System.Text.Json;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class PessoaListResponseTests
{
    [Test]
    public void WhenDeserializePessoaListResponseWithNewStructureThenMapsPropertiesCorrectly()
    {
        var json = @"{
          ""totalItems"": 1,
          ""items"": [
            {
              ""id"": ""36e00833-6f6c-4444-ba1a-81c4367a7a89"",
              ""nome"": ""RATAF"",
              ""documento"": ""29197825000120"",
              ""email"": ""lidianemribeiro83@gmail.com"",
              ""telefone"": ""62992992177"",
              ""tipo_pessoa"": ""Jurídica"",
              ""ativo"": true,
              ""perfis"": [""CLIENTE"", ""FORNECEDOR""],
              ""data_criacao"": ""2023-06-23T16:31:00.852"",
              ""data_alteracao"": ""2026-02-10T15:45:01.339338""
            }
          ]
        }";

        var result = JsonSerializer.Deserialize<PessoaListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Items, Is.Not.Null);
            Assert.That(result.Items, Has.Count.EqualTo(1));
            Assert.That(result.TotalItems, Is.EqualTo(1));

            var pessoa = result.Items[0];
            Assert.That(pessoa, Is.Not.Null);
            Assert.That(pessoa.Id, Is.EqualTo("36e00833-6f6c-4444-ba1a-81c4367a7a89"));
            Assert.That(pessoa.Nome, Is.EqualTo("RATAF"));
            Assert.That(pessoa.Documento, Is.EqualTo("29197825000120"));
            Assert.That(pessoa.Email, Is.EqualTo("lidianemribeiro83@gmail.com"));
            Assert.That(pessoa.Telefone, Is.EqualTo("62992992177"));
            Assert.That(pessoa.TipoPessoa, Is.EqualTo("Jurídica"));
            Assert.That(pessoa.Ativo, Is.True);
            Assert.That(pessoa.Perfis, Is.EquivalentTo(new[] { "CLIENTE", "FORNECEDOR" }));
        });
    }

    [Test]
    public void WhenDeserializePessoaListResponseWithEmptyItemsThenMapsCorrectly()
    {
        var json = @"{
          ""totalItems"": 0,
          ""items"": []
        }";

        var result = JsonSerializer.Deserialize<PessoaListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Items, Is.Not.Null);
            Assert.That(result.Items, Has.Count.EqualTo(0));
            Assert.That(result.TotalItems, Is.EqualTo(0));
        });
    }

    [Test]
    public void WhenDeserializePessoaListResponseWithMultipleItemsThenMapsCorrectly()
    {
        var json = @"{
          ""totalItems"": 3,
          ""items"": [
            { ""id"": ""id-1"", ""nome"": ""Pessoa 1"", ""documento"": ""12345678901"", ""tipo_pessoa"": ""Física"" },
            { ""id"": ""id-2"", ""nome"": ""Pessoa 2"", ""documento"": ""98765432100"", ""tipo_pessoa"": ""Física"" },
            { ""id"": ""id-3"", ""nome"": ""Pessoa 3"", ""documento"": ""11122233344"", ""tipo_pessoa"": ""Jurídica"" }
          ]
        }";

        var result = JsonSerializer.Deserialize<PessoaListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Items, Has.Count.EqualTo(3));
            Assert.That(result.TotalItems, Is.EqualTo(3));

            Assert.That(result.Items[0].Id, Is.EqualTo("id-1"));
            Assert.That(result.Items[0].TipoPessoa, Is.EqualTo("Física"));
            Assert.That(result.Items[2].Id, Is.EqualTo("id-3"));
            Assert.That(result.Items[2].TipoPessoa, Is.EqualTo("Jurídica"));
        });
    }

    [Test]
    public void WhenDeserializePessoaDetalheThenMapsNestedCollections()
    {
        var json = @"{
          ""id"": ""550e8400-e29b-41d4-a716-446655440000"",
          ""nome"": ""João Silva"",
          ""tipo_pessoa"": ""FISICA"",
          ""documento"": ""123.456.789-00"",
          ""ativo"": true,
          ""optante_simples_nacional"": true,
          ""enderecos"": [
            { ""id"": ""end-1"", ""logradouro"": ""Rua das Flores"", ""estado"": ""SP"", ""pais"": ""Brasil"" }
          ],
          ""perfis"": [ { ""tipo_perfil"": ""Cliente"" } ],
          ""inscricoes"": [ { ""id"": ""ins-1"", ""indicador_inscricao_estadual"": ""NAO CONTRIBUINTE"" } ],
          ""pessoas_legado"": [ { ""id"": 12345, ""perfil"": ""CLIENTE"", ""uuid"": ""u-1"" } ]
        }";

        var pessoa = JsonSerializer.Deserialize<Pessoa>(json);

        Assert.Multiple(() =>
        {
            Assert.That(pessoa, Is.Not.Null);
            Assert.That(pessoa!.Id, Is.EqualTo("550e8400-e29b-41d4-a716-446655440000"));
            Assert.That(pessoa.Documento, Is.EqualTo("123.456.789-00"));
            Assert.That(pessoa.Enderecos, Has.Count.EqualTo(1));
            Assert.That(pessoa.Enderecos[0].Estado, Is.EqualTo("SP"));
            Assert.That(pessoa.Perfis, Has.Count.EqualTo(1));
            Assert.That(pessoa.Perfis[0].TipoPerfil, Is.EqualTo("Cliente"));
            Assert.That(pessoa.Inscricoes[0].IndicadorInscricaoEstadual, Is.EqualTo("NAO CONTRIBUINTE"));
            Assert.That(pessoa.PessoasLegado[0].Id, Is.EqualTo(12345));
        });
    }
}
