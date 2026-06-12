using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Resposta da criação/atualização de uma baixa.</summary>
    public sealed class BaixaCriacaoResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

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
