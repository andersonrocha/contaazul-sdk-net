using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Rateio de valor por categoria em um evento financeiro.</summary>
    public sealed class CategoriaRateioRequest
    {
        /// <summary>Identificador da categoria. Obrigatório.</summary>
        [JsonPropertyName("id_categoria")]
        public string IdCategoria { get; set; }

        /// <summary>Valor atribuído à categoria. Obrigatório.</summary>
        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }

        [JsonPropertyName("rateio_centro_custo")]
        public List<CentroCustoRateioRequest> RateioCentroCusto { get; set; }
    }
}
