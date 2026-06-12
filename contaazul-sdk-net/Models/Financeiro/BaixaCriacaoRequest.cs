using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Dados para criar uma baixa em uma parcela
    /// (<c>POST /v1/financeiro/eventos-financeiros/parcelas/{parcela_id}/baixa</c>).
    /// </summary>
    public sealed class BaixaCriacaoRequest
    {
        /// <summary>Data do pagamento (<c>YYYY-MM-DD</c>). Obrigatório.</summary>
        [JsonPropertyName("data_pagamento")]
        public string DataPagamento { get; set; }

        /// <summary>Composição do valor. Obrigatório (<c>valor_bruto</c> obrigatório).</summary>
        [JsonPropertyName("composicao_valor")]
        public ComposicaoValorFinanceiro ComposicaoValor { get; set; }

        /// <summary>Conta financeira (UUID). Obrigatório.</summary>
        [JsonPropertyName("conta_financeira")]
        public string ContaFinanceira { get; set; }

        /// <summary>Método de pagamento (ex.: <c>CARTAO_CREDITO</c>, <c>PIX_PAGAMENTO_INSTANTANEO</c>).</summary>
        [JsonPropertyName("metodo_pagamento")]
        public string MetodoPagamento { get; set; }

        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }
    }
}
