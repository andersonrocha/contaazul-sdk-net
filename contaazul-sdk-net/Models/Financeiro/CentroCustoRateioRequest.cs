using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Rateio por centro de custo dentro de uma categoria.</summary>
    public sealed class CentroCustoRateioRequest
    {
        [JsonPropertyName("id_centro_custo")]
        public string IdCentroCusto { get; set; }

        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }
    }
}
