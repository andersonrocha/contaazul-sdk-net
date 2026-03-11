using System;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class NotaFiscal
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("numero_rps")]
        public int? NumeroRps { get; set; }

        [JsonPropertyName("serie_rps")]
        public string SerieRps { get; set; }

        [JsonPropertyName("data_emissao")]
        public DateTime? DataEmissao { get; set; }

        [JsonPropertyName("data_competencia")]
        public DateTime? DataCompetencia { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("tipo_negociacao")]
        public string TipoNegociacao { get; set; }

        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        [JsonPropertyName("numero_venda")]
        public string NumeroVenda { get; set; }

        [JsonPropertyName("valor_total")]
        public decimal? ValorTotal { get; set; }
    }
}
