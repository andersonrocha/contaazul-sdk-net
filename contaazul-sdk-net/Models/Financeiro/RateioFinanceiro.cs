using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Rateio de categoria em um evento financeiro.</summary>
    public sealed class RateioFinanceiro
    {
        [JsonPropertyName("id_categoria")]
        public string IdCategoria { get; set; }

        [JsonPropertyName("nome_categoria")]
        public string NomeCategoria { get; set; }

        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }

        [JsonPropertyName("valor_bruto")]
        public decimal? ValorBruto { get; set; }

        [JsonPropertyName("rateio_centro_custo")]
        public List<RateioCentroCusto> RateioCentroCusto { get; set; }
    }
}
