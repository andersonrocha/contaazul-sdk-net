using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Informações de perda financeira de uma parcela.</summary>
    public sealed class PerdaFinanceira
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }
    }
}
