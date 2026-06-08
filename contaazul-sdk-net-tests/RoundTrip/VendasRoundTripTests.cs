using System.Collections.Generic;
using ContaAzul.Sdk.Net.Models.Vendas;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class VendasRoundTripTests
{
    [Test]
    public void ListaVendedores_RoundTrips()
    {
        const string json = @"[
          { ""id"": ""v-1"", ""nome"": ""Carlos"", ""id_legado"": 10 },
          { ""id"": ""v-2"", ""nome"": ""Ana"" }
        ]";

        JsonRoundTrip.Verify<List<Vendedor>>(json);
    }

    [Test]
    public void VendaListResponse_RoundTrips()
    {
        const string json = @"{
          ""itens"": [
            {
              ""id"": ""venda-1"",
              ""total"": 1500.5,
              ""id_legado"": 12345,
              ""data"": ""2024-01-15"",
              ""criado_em"": ""2024-01-15"",
              ""data_alteracao"": ""2024-01-16"",
              ""tipo"": ""VENDA"",
              ""itens"": ""PRODUTO"",
              ""condicao_pagamento"": true,
              ""numero"": 1001,
              ""cliente"": { ""id"": ""cli-1"", ""nome"": ""Empresa X"", ""email"": ""x@email.com"", ""telefone"": ""1130000000"", ""endereco"": ""Rua A"", ""cidade"": ""São Paulo"", ""estado"": ""SP"", ""pais"": ""Brasil"", ""cep"": ""01000-000"" },
              ""situacao"": { ""nome"": ""APROVADA"", ""descricao"": ""Aprovada"" },
              ""versao"": 3,
              ""status_email"": { ""status"": ""ENVIADO"", ""enviado_em"": ""2024-01-15"" },
              ""id_contrato"": ""ctr-1"",
              ""origem"": ""API""
            }
          ],
          ""totais"": { ""total"": 1000, ""aprovado"": 500, ""cancelado"": 200, ""esperando_aprovacao"": 300 },
          ""quantidades"": { ""total"": 10, ""aprovado"": 5, ""cancelado"": 3, ""esperando_aprovacao"": 2 },
          ""total_itens"": 10
        }";

        JsonRoundTrip.Verify<VendaListResponse>(json);
    }

    [Test]
    public void ObterVendaResponse_RoundTrips()
    {
        const string json = @"{
          ""cliente"": { ""uuid"": ""cli-1"", ""tipo_pessoa"": ""JURIDICA"", ""documento"": ""12345678000190"", ""nome"": ""Empresa X"" },
          ""evento_financeiro"": { ""id"": ""ef-1"" },
          ""notificacao"": { ""id_referencia"": ""ref-1"", ""enviado_para"": ""x@email.com"", ""enviado_em"": ""2024-01-15"", ""aberto_em"": ""2024-01-16"", ""status"": ""LIDO"" },
          ""natureza_operacao"": { ""uuid"": ""no-1"", ""tipo_operacao"": ""VENDA"", ""template_operacao"": ""T1"", ""label"": ""Venda"", ""mudanca_financeira"": true, ""mudanca_estoque"": ""SAIDA"" },
          ""venda"": {
            ""id"": ""venda-1"",
            ""status"": ""APROVADA"",
            ""id_legado"": 12345,
            ""tipo_negociacao"": ""VENDA"",
            ""numero"": 1001,
            ""id_categoria"": ""cat-1"",
            ""data_compromisso"": ""2024-01-15"",
            ""configuracao_de_desconto"": { ""tipo_desconto"": ""PERCENTUAL"", ""taxa_desconto"": 10.0 },
            ""composicao_valor"": { ""valor_bruto"": 1000, ""desconto"": 100, ""frete"": 50, ""impostos"": 80, ""impostos_deduzidos"": 20, ""seguro"": 10, ""despesas_incidentais"": 5, ""valor_liquido"": 1025 },
            ""condicao_pagamento"": { ""tipo_pagamento"": ""BOLETO_BANCARIO"", ""id_conta_financeira"": ""cf-1"", ""pagamento_a_vista"": false, ""parcelas"": [ { ""id"": ""p-1"", ""numero"": 1, ""data_vencimento"": ""2024-02-15"", ""valor"": 1025, ""descricao"": ""Parcela 1"" } ], ""observacoes_pagamento"": ""obs"", ""opcao_condicao_pagamento"": ""A_VISTA"", ""nsu"": ""123"", ""pagamento_cartao"": { ""tipo_bandeira"": ""VISA"", ""codigo_transacao"": ""tx-1"", ""id_adquirente"": 7 } },
            ""total_itens"": { ""contagem_produtos"": 2, ""contagem_servicos"": 1, ""contagem_nao_conciliados"": 0 },
            ""observacoes"": ""venda teste"",
            ""id_cliente"": ""cli-1"",
            ""versao"": 2,
            ""tipo_pendencia"": { ""nome"": ""NENHUMA"", ""descricao"": ""Sem pendência"" },
            ""situacao"": { ""nome"": ""APROVADA"", ""descricao"": ""Aprovada"", ""ativado"": true },
            ""id_natureza_operacao"": ""no-1"",
            ""id_centro_custo"": ""cc-1"",
            ""introducao"": ""intro"",
            ""origem"": ""API""
          },
          ""vendedor"": { ""id"": ""vd-1"", ""nome"": ""Carlos"", ""id_legado"": 9 },
          ""contrato"": { ""uuid"": ""ctr-1"", ""data_inicio"": ""2024-01-01"", ""data_fim"": ""2024-12-31"", ""dia_vencimento"": 10, ""periodo"": ""MENSAL"", ""periodicidade"": 1 }
        }";

        JsonRoundTrip.Verify<ObterVendaResponse>(json);
    }

    [Test]
    public void CriacaoVendaRequest_RoundTrips()
    {
        const string json = @"{
          ""id_cliente"": ""cli-1"",
          ""numero"": 1001,
          ""situacao"": ""APROVADA"",
          ""data_venda"": ""2024-01-15"",
          ""id_categoria"": ""cat-1"",
          ""id_centro_custo"": ""cc-1"",
          ""id_vendedor"": ""vd-1"",
          ""observacoes"": ""obs"",
          ""observacoes_pagamento"": ""obs pgto"",
          ""itens"": [
            { ""id"": ""prod-1"", ""descricao"": ""Produto"", ""quantidade"": 2, ""valor"": 100.5, ""valor_custo"": 50, ""itens_kit"": [ { ""id_produto"": ""p-1"", ""id_kit"": ""k-1"", ""quantidade"": 1, ""valor"": 10 } ] }
          ],
          ""composicao_de_valor"": { ""frete"": 20, ""desconto"": { ""tipo"": ""PERCENTUAL"", ""valor"": 5 } },
          ""condicao_pagamento"": { ""tipo_pagamento"": ""BOLETO_BANCARIO"", ""id_conta_financeira"": ""cf-1"", ""opcao_condicao_pagamento"": ""A_VISTA"", ""nsu"": ""1"", ""parcelas"": [ { ""data_vencimento"": ""2024-02-15"", ""valor"": 201, ""descricao"": ""P1"" } ] }
        }";

        JsonRoundTrip.Verify<CriacaoVendaRequest>(json);
    }

    [Test]
    public void CriacaoVendaResponse_RoundTrips()
    {
        const string json = @"{
          ""id"": ""venda-1"",
          ""id_legado"": 12345,
          ""id_cliente"": ""cli-1"",
          ""numero"": 1001,
          ""origem"": ""API"",
          ""id_categoria"": ""cat-1"",
          ""data_venda"": ""2024-01-15"",
          ""situacao"": { ""nome"": ""APROVADA"", ""descricao"": ""Aprovada"" },
          ""pendencia"": { ""nome"": ""NENHUMA"", ""descricao"": ""Sem pendência"" },
          ""valor_composicao"": { ""valor_bruto"": 1000, ""desconto"": { ""tipo"": ""PERCENTUAL"", ""valor"": 5 }, ""frete"": 20, ""valor_liquido"": 1015 },
          ""condicao_pagamento"": { ""id_legado"": 99, ""tipo_pagamento"": ""BOLETO_BANCARIO"", ""id_conta_financeira"": ""cf-1"", ""opcao_condicao_pagamento"": ""A_VISTA"", ""parcelas"": [ { ""id"": ""p-1"", ""numero"": 1, ""data_vencimento"": ""2024-02-15"", ""valor"": 1015, ""descricao"": ""P1"" } ], ""observacoes_pagamento"": ""obs"", ""nsu"": ""1"", ""troco_total"": 0 },
          ""observacoes"": ""obs"",
          ""id_vendedor"": ""vd-1"",
          ""versao"": 1
        }";

        JsonRoundTrip.Verify<CriacaoVendaResponse>(json);
    }

    [Test]
    public void VendaParaEdicaoRequest_RoundTrips()
    {
        const string json = @"{
          ""id_cliente"": ""cli-1"",
          ""numero"": 1001,
          ""data_venda"": ""2024-01-15"",
          ""situacao"": ""APROVADA"",
          ""observacoes"": ""obs"",
          ""observacoes_pagamento"": ""obs pgto"",
          ""id_natureza_operacao"": ""no-1"",
          ""versao"": 2,
          ""itens"": [ { ""id"": ""prod-1"", ""descricao"": ""Produto"", ""quantidade"": 1, ""valor"": 100 } ],
          ""composicao_de_valor"": { ""frete"": 0, ""desconto"": { ""tipo"": ""PERCENTUAL"", ""valor"": 0 } },
          ""condicao_pagamento"": { ""tipo_pagamento"": ""DINHEIRO"", ""id_conta_financeira"": ""cf-1"", ""opcao_condicao_pagamento"": ""A_VISTA"", ""nsu"": ""1"", ""parcelas"": [ { ""data_vencimento"": ""2024-02-15"", ""valor"": 100, ""descricao"": ""P1"" } ] }
        }";

        JsonRoundTrip.Verify<VendaParaEdicaoRequest>(json);
    }

    [Test]
    public void VendaEditadaResponse_RoundTrips()
    {
        JsonRoundTrip.Verify<VendaEditadaResponse>(@"{ ""id"": ""venda-1"", ""id_legado"": 12345 }");
    }

    [Test]
    public void ItensPaginados_RoundTrips()
    {
        const string json = @"{
          ""itens"": [
            { ""id"": ""it-1"", ""id_item"": ""prod-1"", ""nome"": ""Produto"", ""descricao"": ""Desc"", ""tipo"": ""PRODUTO"", ""quantidade"": 2, ""valor"": 100.5, ""custo"": 50 }
          ],
          ""itens_totais"": 1,
          ""totais"": { ""quantidade_produtos"": 1, ""quantidade_servicos"": 0, ""quantidade_nao_conciliados"": 0 }
        }";

        JsonRoundTrip.Verify<ItensPaginados>(json);
    }

    [Test]
    public void ExclusaoLote_RoundTrips()
    {
        JsonRoundTrip.Verify<ExclusaoLote>(@"{ ""ids"": [""v-1"", ""v-2""] }");
    }

    [Test]
    public void ExclusaoResponse_RoundTrips()
    {
        JsonRoundTrip.Verify<ExclusaoResponse>(@"{ ""atualizados"": 2, ""ignorados"": 1 }");
    }
}
