using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Dados para atualização parcial de uma baixa
    /// (<c>PATCH /v1/financeiro/eventos-financeiros/parcelas/baixa/{baixa_id}</c>).
    /// </summary>
    public sealed class BaixaAtualizacaoRequest
    {
        /// <summary>Versão atual do registro (será incrementada no sucesso). Obrigatório.</summary>
        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("data_pagamento")]
        public string DataPagamento { get; set; }

        [JsonPropertyName("composicao_valor")]
        public ComposicaoValorFinanceiro ComposicaoValor { get; set; }

        [JsonPropertyName("conta_financeira")]
        public string ContaFinanceira { get; set; }

        [JsonPropertyName("metodo_pagamento")]
        public string MetodoPagamento { get; set; }

        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }
    }
}
