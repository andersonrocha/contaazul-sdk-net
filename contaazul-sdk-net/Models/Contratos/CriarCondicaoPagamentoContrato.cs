using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Condição de pagamento para a criação de um contrato.
    /// </summary>
    public sealed class CriarCondicaoPagamentoContrato
    {
        /// <summary>Dia do mês para vencimento do pagamento (1-31). <b>Obrigatório.</b></summary>
        [JsonPropertyName("dia_vencimento")]
        public int DiaVencimento { get; set; }

        /// <summary>ID da conta financeira associada ao pagamento. Opcional.</summary>
        [JsonPropertyName("id_conta_financeira")]
        public string IdContaFinanceira { get; set; }

        /// <summary>
        /// Data do primeiro vencimento no formato <c>YYYY-MM-DD</c>. <b>Obrigatório.</b>
        /// </summary>
        [JsonPropertyName("primeira_data_vencimento")]
        public string PrimeiraDataVencimento { get; set; }

        /// <summary>Forma de pagamento (ex.: <c>BOLETO_BANCARIO</c>). <b>Obrigatório.</b></summary>
        [JsonPropertyName("tipo_pagamento")]
        public string TipoPagamento { get; set; }
    }
}
