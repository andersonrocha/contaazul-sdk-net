using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Parcela da condição de pagamento de um evento financeiro.</summary>
    public sealed class ParcelaCondicaoPagamentoRequest
    {
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("data_vencimento")]
        public string DataVencimento { get; set; }

        [JsonPropertyName("nota")]
        public string Nota { get; set; }

        [JsonPropertyName("conta_financeira")]
        public string ContaFinanceira { get; set; }

        [JsonPropertyName("detalhe_valor")]
        public ComposicaoValorFinanceiro DetalheValor { get; set; }

        [JsonPropertyName("metodo_pagamento")]
        public string MetodoPagamento { get; set; }
    }
}
