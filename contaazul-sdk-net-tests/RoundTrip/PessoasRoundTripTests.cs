using System.Collections.Generic;
using ContaAzul.Sdk.Net.Models.Pessoas;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class PessoasRoundTripTests
{
    [Test]
    public void PessoaDetalheCompleta_RoundTrips()
    {
        const string json = @"{
          ""id"": ""550e8400-e29b-41d4-a716-446655440000"",
          ""nome"": ""João Silva"",
          ""nome_empresa"": ""Empresa LTDA"",
          ""tipo_pessoa"": ""FISICA"",
          ""codigo"": ""CLI001"",
          ""documento"": ""123.456.789-00"",
          ""rg"": ""12.345.678-9"",
          ""data_nascimento"": ""1990-01-01"",
          ""email"": ""joao.silva@email.com, maria.silva@email.com"",
          ""telefone_celular"": ""(11) 98765-4321"",
          ""telefone_comercial"": ""(11) 1234-5678"",
          ""ativo"": true,
          ""optante_simples_nacional"": true,
          ""orgao_publico"": true,
          ""observacao"": ""Observação geral"",
          ""criado_em"": ""2024-01-15"",
          ""data_alteracao"": ""2024-01-15T10:30:00"",
          ""atrasos_pagamentos"": 750.25,
          ""atrasos_recebimentos"": 1500.5,
          ""pagamentos_mes_atual"": 2500,
          ""recebimentos_mes_atual"": 5000,
          ""contato_cobranca_faturamento"": {
            ""emails"": [""carlos.oliveira@email.com"", ""joao.silva@email.com""],
            ""whatsapp"": ""5511999999999""
          },
          ""mensagem_pagamentos_abertos"": { ""numero"": 10, ""total"": 10 },
          ""enderecos"": [
            {
              ""id"": ""550e8400-e29b-41d4-a716-446655440000"",
              ""id_cidade"": 3550308,
              ""logradouro"": ""Rua das Flores"",
              ""numero"": ""123"",
              ""complemento"": ""Apto 45"",
              ""bairro"": ""Centro"",
              ""cidade"": ""São Paulo"",
              ""estado"": ""SP"",
              ""cep"": ""12345-678"",
              ""pais"": ""Brasil""
            }
          ],
          ""inscricoes"": [
            {
              ""id"": ""550e8400-e29b-41d4-a716-446655440000"",
              ""indicador_inscricao_estadual"": ""NAO CONTRIBUINTE"",
              ""inscricao_estadual"": ""123456789"",
              ""inscricao_municipal"": ""12345678901234"",
              ""inscricao_suframa"": ""123456789""
            }
          ],
          ""lembretes_vencimento"": [
            { ""id"": ""550e8400-e29b-41d4-a716-446655440000"", ""ativo"": true, ""email"": ""lembrete@email.com"" }
          ],
          ""outros_contatos"": [
            {
              ""id"": ""550e8400-e29b-41d4-a716-446655440000"",
              ""nome"": ""Maria Silva"",
              ""cargo"": ""Escritório Central"",
              ""email"": ""maria.silva@email.com"",
              ""telefone_celular"": ""11987654321"",
              ""telefone_comercial"": ""1112345678""
            }
          ],
          ""perfis"": [ { ""id"": ""550e8400-e29b-41d4-a716-446655440000"", ""tipo_perfil"": ""Cliente"" } ],
          ""pessoas_legado"": [ { ""id"": 12345, ""perfil"": ""CLIENTE"", ""uuid"": ""550e8400-e29b-41d4-a716-446655440000"" } ]
        }";

        JsonRoundTrip.Verify<Pessoa>(json);
    }

    [Test]
    public void PessoaListResponse_RoundTrips()
    {
        const string json = @"{
          ""totalItems"": 1,
          ""items"": [
            {
              ""id"": ""550e8400-e29b-41d4-a716-446655440000"",
              ""id_legado"": 12345,
              ""uuid_legado"": ""550e8400-e29b-41d4-a716-446655440000"",
              ""nome"": ""João Silva"",
              ""tipo_pessoa"": ""FISICA"",
              ""documento"": ""123.456.789-00"",
              ""email"": ""joao.silva@email.com"",
              ""telefone"": ""(11) 1234-5678"",
              ""ativo"": true,
              ""observacoes_gerais"": ""Cliente preferencial"",
              ""perfis"": [""CLIENTE"", ""FORNECEDOR"", ""TRANSPORTADORA""],
              ""endereco"": { ""logradouro"": ""Rua das Flores"", ""estado"": ""SP"", ""pais"": ""Brasil"" },
              ""data_criacao"": ""2024-01-15T10:30:00"",
              ""data_alteracao"": ""2024-01-15T10:30:00""
            }
          ]
        }";

        JsonRoundTrip.Verify<PessoaListResponse>(json);
    }

    [Test]
    public void Empresa_RoundTrips()
    {
        const string json = @"{
          ""razao_social"": ""Conta Azul Software Ltda"",
          ""nome_fantasia"": ""Conta Azul"",
          ""documento"": ""05206246000138"",
          ""email"": ""api@contaazul.com"",
          ""data_fundacao"": ""2012-01-01""
        }";

        JsonRoundTrip.Verify<Empresa>(json);
    }

    [Test]
    public void PessoaRequest_RoundTrips()
    {
        const string json = @"{
          ""nome"": ""João Silva"",
          ""tipo_pessoa"": ""Física"",
          ""nome_fantasia"": ""Empresa LTDA"",
          ""cpf"": ""123.456.789-00"",
          ""cnpj"": ""12.345.678/0001-90"",
          ""rg"": ""12.345.678-9"",
          ""codigo"": ""CLI001"",
          ""data_nascimento"": ""1990-01-01"",
          ""email"": ""joao.silva@email.com"",
          ""telefone_celular"": ""11983899529"",
          ""telefone_comercial"": ""1138185004"",
          ""observacao"": ""Cliente preferencial"",
          ""ativo"": true,
          ""optante_simples"": true,
          ""agencia_publica"": true,
          ""contato_cobranca_faturamento"": { ""emails"": [""carlos.oliveira@email.com""], ""whatsapp"": ""5511999999999"" },
          ""enderecos"": [
            { ""logradouro"": ""Rua das Flores"", ""numero"": ""123"", ""bairro"": ""Centro"", ""cidade"": ""São Paulo"", ""estado"": ""SP"", ""cep"": ""12345-678"", ""pais"": ""Brasil"" }
          ],
          ""inscricoes"": [
            { ""indicador_inscricao_estadual"": ""NAO CONTRIBUINTE"", ""inscricao_estadual"": ""123456789"" }
          ],
          ""outros_contatos"": [
            { ""nome"": ""Maria Silva"", ""cargo"": ""Gerente"", ""email"": ""maria.silva@email.com"", ""telefone_celular"": ""11983899529"", ""telefone_comercial"": ""1138185004"" }
          ],
          ""perfis"": [ { ""tipo_perfil"": ""Cliente"" } ]
        }";

        JsonRoundTrip.Verify<PessoaRequest>(json);
    }

    [Test]
    public void AtualizacaoParcialPessoa_RoundTrips()
    {
        const string json = @"{
          ""nome"": ""João Silva"",
          ""nome_empresa"": ""Empresa LTDA"",
          ""tipo_pessoa"": ""Física"",
          ""cpf"": ""123.456.789-00"",
          ""email"": ""joao.silva@email.com"",
          ""telefone_celular"": ""11987654321"",
          ""observacao"": ""Cliente preferencial"",
          ""ativo"": true,
          ""optante_simples_nacional"": true,
          ""agencia_publica"": true,
          ""enderecos"": [ { ""logradouro"": ""Rua das Flores"", ""estado"": ""SP"", ""pais"": ""Brasil"" } ],
          ""inscricoes"": [ { ""indicador_inscricao_estadual"": ""NAO CONTRIBUINTE"" } ],
          ""perfis"": [ { ""tipo_perfil"": ""Cliente"" } ]
        }";

        JsonRoundTrip.Verify<AtualizacaoParcialPessoa>(json);
    }

    [Test]
    public void ResumoPessoa_RoundTrips()
    {
        const string json = @"{
          ""id"": ""550e8400-e29b-41d4-a716-446655440000"",
          ""nome"": ""João Silva"",
          ""nome_fantasia"": ""Empresa LTDA"",
          ""tipo_pessoa"": ""FISICA"",
          ""cpf"": ""123.456.789-00"",
          ""cnpj"": ""12.345.678/0001-90"",
          ""rg"": ""12.345.678-9"",
          ""codigo"": ""CLI001"",
          ""data_nascimento"": ""1990-01-01"",
          ""email"": ""joao.silva@email.com"",
          ""telefone_celular"": ""11987654321"",
          ""telefone_comercial"": ""1112345678"",
          ""observacao"": ""Cliente preferencial"",
          ""ativo"": true,
          ""optante_simples"": false,
          ""agencia_publica"": false,
          ""estrangeiro"": false,
          ""origem"": ""API"",
          ""contato_cobranca_faturamento"": { ""emails"": [""carlos.oliveira@email.com""], ""whatsapp"": ""5511999999999"" },
          ""enderecos"": [ { ""id"": ""end-1"", ""logradouro"": ""Rua das Flores"", ""estado"": ""SP"", ""pais"": ""Brasil"" } ],
          ""inscricoes"": [ { ""id"": ""ins-1"", ""indicador_inscricao_estadual"": ""NAO CONTRIBUINTE"" } ],
          ""outros_contatos"": [ { ""id"": ""ct-1"", ""nome"": ""Maria Silva"" } ],
          ""perfis"": [ { ""id"": ""pf-1"", ""tipo_perfil"": ""Cliente"" } ]
        }";

        JsonRoundTrip.Verify<ResumoPessoa>(json);
    }

    [Test]
    public void StatusPessoasEmLoteResultado_RoundTrips()
    {
        const string json = @"[
          {
            ""ativos"": [""4BBDAE00-2242-4703-9310-9946D6C2D0A3""],
            ""inativos"": [""B6270060-3AB7-4E66-A99B-71084E385F47""],
            ""todos"": [""4BBDAE00-2242-4703-9310-9946D6C2D0A3"", ""B6270060-3AB7-4E66-A99B-71084E385F47""]
          }
        ]";

        JsonRoundTrip.Verify<List<StatusPessoasEmLoteResultado>>(json);
    }

    [Test]
    public void PessoasEmLoteRequest_RoundTrips()
    {
        const string json = @"{
          ""uuids"": [""4BBDAE00-2242-4703-9310-9946D6C2D0A3"", ""B6270060-3AB7-4E66-A99B-71084E385F47""]
        }";

        JsonRoundTrip.Verify<PessoasEmLoteRequest>(json);
    }
}
