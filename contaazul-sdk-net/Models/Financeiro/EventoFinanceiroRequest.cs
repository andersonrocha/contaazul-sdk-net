using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Dados para criar um evento financeiro de contas a pagar ou a receber
    /// (<c>POST /v1/financeiro/eventos-financeiros/contas-a-{pagar,receber}</c>).
    /// </summary>
    public sealed class EventoFinanceiroRequest
    {
        /// <summary>Data de competência (<c>YYYY-MM-DD</c>). Obrigatório.</summary>
        [JsonPropertyName("data_competencia")]
        public string DataCompetencia { get; set; }

        /// <summary>Valor do evento financeiro. Obrigatório.</summary>
        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }

        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Identificador do contato/negociador (UUID). Obrigatório.</summary>
        [JsonPropertyName("contato")]
        public string Contato { get; set; }

        /// <summary>Identificador da conta financeira (UUID). Obrigatório.</summary>
        [JsonPropertyName("conta_financeira")]
        public string ContaFinanceira { get; set; }

        [JsonPropertyName("rateio")]
        public List<CategoriaRateioRequest> Rateio { get; set; }

        /// <summary>Condição de pagamento. Obrigatório.</summary>
        [JsonPropertyName("condicao_pagamento")]
        public CondicaoPagamentoEvento CondicaoPagamento { get; set; }
    }
}
