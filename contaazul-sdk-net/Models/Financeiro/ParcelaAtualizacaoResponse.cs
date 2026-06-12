using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Resposta da atualização parcial de uma parcela.</summary>
    public sealed class ParcelaAtualizacaoResponse
    {
        [JsonPropertyName("nota")]
        public string Nota { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("vencimento")]
        public string Vencimento { get; set; }

        [JsonPropertyName("composicao_valor")]
        public ComposicaoValorFinanceiro ComposicaoValor { get; set; }

        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("data_pagamento_esperado")]
        public string DataPagamentoEsperado { get; set; }

        [JsonPropertyName("metodo_pagamento")]
        public string MetodoPagamento { get; set; }

        [JsonPropertyName("perda")]
        public PerdaFinanceira Perda { get; set; }

        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }

        [JsonPropertyName("pagamento_agendado")]
        public bool? PagamentoAgendado { get; set; }

        [JsonPropertyName("conta_financeira")]
        public ParcelaContaFinanceira ContaFinanceira { get; set; }
    }
}
