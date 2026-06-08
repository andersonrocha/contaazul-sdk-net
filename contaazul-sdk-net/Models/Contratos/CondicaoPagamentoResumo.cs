using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Resumo das condições de pagamento do contrato.
    /// </summary>
    public sealed class CondicaoPagamentoResumo
    {
        /// <summary>Dia do mês para vencimento do pagamento.</summary>
        [JsonPropertyName("dia_vencimento")]
        public int DiaVencimento { get; set; }

        /// <summary>Nome da conta financeira vinculada ao pagamento.</summary>
        [JsonPropertyName("nome_conta_financeira")]
        public string NomeContaFinanceira { get; set; }

        /// <summary>Observações sobre o pagamento.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

        /// <summary>Tipo de pagamento do contrato (ex.: <c>CARTAO_CREDITO</c>).</summary>
        [JsonPropertyName("tipo_pagamento")]
        public string TipoPagamento { get; set; }
    }
}
