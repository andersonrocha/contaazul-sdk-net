using System;
using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Models
{
    public class NotaFiscal
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("numero_rps")]
        public string NumeroRps { get; set; }

        [JsonProperty("serie_rps")]
        public string SerieRps { get; set; }

        [JsonProperty("data_emissao")]
        public DateTime? DataEmissao { get; set; }

        [JsonProperty("data_competencia")]
        public DateTime? DataCompetencia { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("tipo_negociacao")]
        public string TipoNegociacao { get; set; }

        [JsonProperty("id_cliente")]
        public string IdCliente { get; set; }

        [JsonProperty("numero_venda")]
        public int? NumeroVenda { get; set; }

        [JsonProperty("valor_total")]
        public decimal? ValorTotal { get; set; }
    }
}
