using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Dados para gerar uma nova cobrança
    /// (<c>POST /v1/financeiro/eventos-financeiros/contas-a-receber/gerar-cobranca</c>).
    /// </summary>
    public sealed class GerarCobrancaRequest
    {
        /// <summary>
        /// Conta bancária (UUID). Deve ser do tipo <c>COBRANCAS_CONTA_AZUL</c>. Obrigatório.
        /// </summary>
        [JsonPropertyName("conta_bancaria")]
        public string ContaBancaria { get; set; }

        /// <summary>Descrição da fatura. Obrigatório.</summary>
        [JsonPropertyName("descricao_fatura")]
        public string DescricaoFatura { get; set; }

        /// <summary>Identificador da parcela (UUID). Obrigatório.</summary>
        [JsonPropertyName("id_parcela")]
        public string IdParcela { get; set; }

        /// <summary>Data de vencimento (<c>YYYY-MM-DD</c>). Obrigatório.</summary>
        [JsonPropertyName("data_vencimento")]
        public string DataVencimento { get; set; }

        /// <summary>
        /// Tipo da cobrança: <c>LINK_PAGAMENTO</c>, <c>PIX_COBRANCA</c> ou <c>BOLETO</c>. Obrigatório.
        /// </summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("atributos")]
        public CobrancaAtributos Atributos { get; set; }

        /// <summary>Número máximo de parcelas exibido na fatura do cartão.</summary>
        [JsonPropertyName("maximo_parcelas")]
        public int? MaximoParcelas { get; set; }
    }
}
