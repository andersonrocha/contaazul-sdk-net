using ContaAzul.Sdk.Net.Models;
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
              ""cpf_cnpj"": ""29197825000120"",
              ""email"": ""lidianemribeiro83@gmail.com"",
              ""telefone"": ""62992992177"",
              ""tipo_pessoa"": ""Jurídica"",
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
            Assert.That(pessoa.CpfCnpj, Is.EqualTo("29197825000120"));
            Assert.That(pessoa.Email, Is.EqualTo("lidianemribeiro83@gmail.com"));
            Assert.That(pessoa.Telefone, Is.EqualTo("62992992177"));
            Assert.That(pessoa.TipoPessoa, Is.EqualTo("Jurídica"));
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
            {
              ""id"": ""id-1"",
              ""nome"": ""Pessoa 1"",
              ""cpf_cnpj"": ""12345678901"",
              ""email"": ""pessoa1@example.com"",
              ""tipo_pessoa"": ""Física""
            },
            {
              ""id"": ""id-2"",
              ""nome"": ""Pessoa 2"",
              ""cpf_cnpj"": ""98765432100"",
              ""email"": ""pessoa2@example.com"",
              ""tipo_pessoa"": ""Física""
            },
            {
              ""id"": ""id-3"",
              ""nome"": ""Pessoa 3"",
              ""cpf_cnpj"": ""11122233344"",
              ""email"": ""pessoa3@example.com"",
              ""tipo_pessoa"": ""Jurídica""
            }
          ]
        }";

        var result = JsonSerializer.Deserialize<PessoaListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Items, Is.Not.Null);
            Assert.That(result.Items, Has.Count.EqualTo(3));
            Assert.That(result.TotalItems, Is.EqualTo(3));

            Assert.That(result.Items[0].Id, Is.EqualTo("id-1"));
            Assert.That(result.Items[0].Nome, Is.EqualTo("Pessoa 1"));
            Assert.That(result.Items[0].TipoPessoa, Is.EqualTo("Física"));

            Assert.That(result.Items[1].Id, Is.EqualTo("id-2"));
            Assert.That(result.Items[1].Nome, Is.EqualTo("Pessoa 2"));
            Assert.That(result.Items[1].TipoPessoa, Is.EqualTo("Física"));

            Assert.That(result.Items[2].Id, Is.EqualTo("id-3"));
            Assert.That(result.Items[2].Nome, Is.EqualTo("Pessoa 3"));
            Assert.That(result.Items[2].TipoPessoa, Is.EqualTo("Jurídica"));
        });
    }
}
