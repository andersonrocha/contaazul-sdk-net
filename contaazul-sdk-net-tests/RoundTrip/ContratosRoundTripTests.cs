using ContaAzul.Sdk.Net.Models.Contratos;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class ContratosRoundTripTests
{
    [Test]
    public void ContratosFiltroResposta_RoundTrips()
    {
        const string json = @"{
          ""itens"": [
            {
              ""cliente"": { ""id"": ""cli-1"", ""nome"": ""Empresa X"" },
              ""conta_financeira"": { ""id"": ""cf-1"", ""tipo"": ""BANCO"" },
              ""data_inicio"": ""2024-01-01"",
              ""id"": ""ctr-1"",
              ""numero"": 1014,
              ""proximo_vencimento"": ""2024-02-10"",
              ""status"": ""ATIVO"",
              ""termos"": {
                ""data_fim"": ""2026-10-21"",
                ""tipo_expiracao"": ""DATA"",
                ""vigencia_atual"": 6,
                ""vigencia_total"": 12
              },
              ""tipo_pagamento"": ""BOLETO_BANCARIO"",
              ""total"": 1500.5,
              ""total_proximo_vencimento"": 1500.5
            }
          ],
          ""itens_totais"": 1
        }";

        JsonRoundTrip.Verify<ContratosFiltroResposta>(json);
    }

    [Test]
    public void ContratoResumo_RoundTrips()
    {
        const string json = @"{
          ""cliente"": { ""id"": ""cli-1"", ""nome"": ""Empresa X"" },
          ""composicao_valor"": { ""desconto"": 100, ""frete"": 50, ""valor_bruto"": 1000, ""valor_impostos_servico"": 80, ""valor_liquido"": 1030 },
          ""condicao_pagamento"": { ""dia_vencimento"": 10, ""nome_conta_financeira"": ""Banco X"", ""observacoes_pagamento"": ""obs"", ""tipo_pagamento"": ""BOLETO_BANCARIO"" },
          ""configuracao_recorrencia"": { ""vigencia_restante"": 10, ""vigencia_total"": 12 },
          ""data_proxima_emissao"": ""2024-02-01"",
          ""data_proximo_vencimento"": ""2024-02-10"",
          ""data_ultima_emissao"": ""2024-01-01"",
          ""id"": ""ctr-1"",
          ""id_proxima_venda_agendada"": ""vnd-2"",
          ""id_ultima_venda_confirmada"": ""vnd-1"",
          ""local_prestacao_servico"": { ""nome"": ""Joinville"" },
          ""observacoes"": ""contrato teste"",
          ""status"": ""ATIVO"",
          ""termos"": {
            ""data_fim"": ""2026-10-21"",
            ""data_inicio"": ""2026-08-15"",
            ""dia_emissao_venda"": 15,
            ""intervalo_frequencia"": 1,
            ""numero"": 1,
            ""tipo_expiracao"": ""DATA"",
            ""tipo_frequencia"": ""MENSAL""
          },
          ""vendedor"": { ""id"": ""vd-1"", ""nome"": ""Carlos"" }
        }";

        JsonRoundTrip.Verify<ContratoResumo>(json);
    }

    [Test]
    public void CriarContrato_RoundTrips()
    {
        const string json = @"{
          ""composicao_de_valor"": { ""desconto"": { ""tipo"": ""PERCENTUAL"", ""valor"": 5 }, ""frete"": 20 },
          ""condicao_pagamento"": { ""dia_vencimento"": 10, ""id_conta_financeira"": ""cf-1"", ""primeira_data_vencimento"": ""2025-01-10"", ""tipo_pagamento"": ""BOLETO_BANCARIO"" },
          ""id_categoria"": ""cat-1"",
          ""id_centro_custo"": ""cc-1"",
          ""id_cliente"": ""cli-1"",
          ""id_vendedor"": ""vd-1"",
          ""itens"": [
            { ""descricao"": ""Serviço"", ""id"": ""prod-1"", ""quantidade"": 2, ""valor"": 100.5, ""valor_custo"": 50 }
          ],
          ""observacoes"": ""obs"",
          ""observacoes_pagamento"": ""obs pgto"",
          ""termos"": {
            ""data_fim"": ""2025-12-31"",
            ""data_inicio"": ""2025-01-01"",
            ""dia_emissao_venda"": 5,
            ""intervalo_frequencia"": 1,
            ""numero"": 12,
            ""tipo_expiracao"": ""DATA"",
            ""tipo_frequencia"": ""MENSAL""
          }
        }";

        JsonRoundTrip.Verify<CriarContrato>(json);
    }

    [Test]
    public void ResumoCriacaoContrato_RoundTrips()
    {
        JsonRoundTrip.Verify<ResumoCriacaoContrato>(
            @"{ ""id"": ""ctr-1"", ""id_legado"": 12345, ""id_venda"": ""vnd-1"" }");
    }
}
