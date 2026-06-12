using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Rateio por centro de custo dentro de um rateio de categoria do evento.</summary>
    public sealed class RateioCentroCusto
    {
        [JsonPropertyName("id_centro_custo")]
        public string IdCentroCusto { get; set; }

        [JsonPropertyName("nome_centro_custo")]
        public string NomeCentroCusto { get; set; }

        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }
    }
}
