using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.NotasFiscais;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class NotasFiscaisRoundTripTests
{
    [Test]
    public void NotaFiscalProduto_RoundTrips()
    {
        const string json = @"{
          ""chave_acesso"": ""42250323643586000108550010000001151606401726"",
          ""data_emissao"": ""2025-01-15T10:30:00Z"",
          ""nome_destinatario"": ""EMPRESA EXEMPLO LTDA"",
          ""numero_nota"": 123456,
          ""status"": ""EMITIDA""
        }";

        JsonRoundTrip.Verify<NotaFiscal>(json);
    }

    [Test]
    public void RespostaPaginadaNotaFiscalProduto_RoundTrips()
    {
        const string json = @"{
          ""itens"": [
            {
              ""chave_acesso"": ""42250323643586000108550010000001151606401726"",
              ""data_emissao"": ""2025-01-15T10:30:00Z"",
              ""nome_destinatario"": ""EMPRESA EXEMPLO LTDA"",
              ""numero_nota"": 123456,
              ""status"": ""EMITIDA""
            }
          ],
          ""paginacao"": { ""pagina_atual"": 1, ""tamanho_pagina"": 10, ""total_itens"": 50, ""total_paginas"": 5 }
        }";

        JsonRoundTrip.Verify<RespostaPaginada<NotaFiscal>>(json);
    }

    [Test]
    public void NotaFiscalServicoCompleta_RoundTrips()
    {
        const string json = @"{
          ""id"": ""550e8400-e29b-41d4-a716-446655440099"",
          ""id_venda"": ""550e8400-e29b-41d4-a716-446655440000"",
          ""id_contrato"": ""550e8400-e29b-41d4-a716-446655440001"",
          ""numero_venda"": ""1001"",
          ""numero_rps"": 67890,
          ""numero_nfse"": 123,
          ""status"": ""EMITIDA"",
          ""valor_total_nfse"": 1500.5,
          ""data_competencia"": ""2024-01-15"",
          ""nome_cliente"": ""EMPRESA EXEMPLO LTDA"",
          ""documento_cliente"": ""12345678000190"",
          ""codigo_cnae"": ""6201-5/00"",
          ""escriturado_manualmente"": false,
          ""cidade_emissao"": { ""estado"": ""SC"", ""nome"": ""Joinville"" },
          ""informacao_transmissao"": {
            ""data_inicio_cancelamento"": ""2024-01-16T14:20:00"",
            ""data_inicio_emissao"": ""2024-01-15T10:30:00""
          },
          ""informacoes_cancelamento"": {
            ""motivo"": ""Erro na alíquota de ISS informada"",
            ""usuario"": ""Empresa Teste""
          }
        }";

        JsonRoundTrip.Verify<NotaFiscalServico>(json);
    }

    [Test]
    public void RespostaPaginadaNotaFiscalServico_RoundTrips()
    {
        const string json = @"{
          ""itens"": [
            {
              ""id"": ""550e8400-e29b-41d4-a716-446655440099"",
              ""numero_nfse"": 123,
              ""status"": ""EMITIDA"",
              ""valor_total_nfse"": 1500.5,
              ""data_competencia"": ""2024-01-15""
            }
          ],
          ""paginacao"": { ""pagina_atual"": 1, ""tamanho_pagina"": 10, ""total_itens"": 1, ""total_paginas"": 1 }
        }";

        JsonRoundTrip.Verify<RespostaPaginada<NotaFiscalServico>>(json);
    }

    [Test]
    public void LinkNotaFiscalMdfe_RoundTrips()
    {
        const string json = @"{
          ""chaves_acesso"": [
            ""42250323643586000108550010000001151606401726"",
            ""42250323643586000108550010000001141054498495""
          ],
          ""identificador"": ""345345"",
          ""status"": ""ENCERRADO""
        }";

        JsonRoundTrip.Verify<LinkNotaFiscalMdfe>(json);
    }
}
