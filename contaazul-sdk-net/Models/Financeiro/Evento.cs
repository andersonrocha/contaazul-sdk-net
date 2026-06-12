using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Evento financeiro ao qual uma parcela pertence.</summary>
    public sealed class Evento
    {
        [JsonPropertyName("data_competencia")]
        public string DataCompetencia { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("condicao_pagamento")]
        public CondicaoPagamentoEventoResumo CondicaoPagamento { get; set; }

        [JsonPropertyName("referencia")]
        public ReferenciaEvento Referencia { get; set; }

        [JsonPropertyName("agendado")]
        public bool? Agendado { get; set; }

        /// <summary>Tipo: <c>RECEITA</c> ou <c>DESPESA</c>.</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("codigo_referencia")]
        public string CodigoReferencia { get; set; }

        [JsonPropertyName("rateio")]
        public List<RateioFinanceiro> Rateio { get; set; }
    }
}
