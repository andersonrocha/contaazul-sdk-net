using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Dados resumidos de um contrato recorrente retornado na listagem.
    /// </summary>
    public sealed class ItemContrato
    {
        /// <summary>Dados do cliente vinculado ao contrato.</summary>
        [JsonPropertyName("cliente")]
        public ReferenciaNomeada Cliente { get; set; }

        /// <summary>Conta financeira vinculada ao contrato.</summary>
        [JsonPropertyName("conta_financeira")]
        public ContaFinanceiraContrato ContaFinanceira { get; set; }

        /// <summary>Data de início do contrato.</summary>
        [JsonPropertyName("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>ID do contrato.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Número do contrato.</summary>
        [JsonPropertyName("numero")]
        public int Numero { get; set; }

        /// <summary>Data do próximo vencimento.</summary>
        [JsonPropertyName("proximo_vencimento")]
        public string ProximoVencimento { get; set; }

        /// <summary>Status do contrato (ex.: <c>ATIVO</c>, <c>INATIVO</c>, <c>DELETADO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Termos de vigência do contrato.</summary>
        [JsonPropertyName("termos")]
        public TermosContrato Termos { get; set; }

        /// <summary>Tipo de pagamento (ex.: <c>BOLETO_BANCARIO</c>).</summary>
        [JsonPropertyName("tipo_pagamento")]
        public string TipoPagamento { get; set; }

        /// <summary>Valor total do contrato.</summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        /// <summary>Valor da próxima cobrança.</summary>
        [JsonPropertyName("total_proximo_vencimento")]
        public decimal TotalProximoVencimento { get; set; }
    }
}
