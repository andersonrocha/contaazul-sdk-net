using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class VendaTotais
    {
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("aprovado")]
        public decimal Aprovado { get; set; }

        [JsonPropertyName("cancelado")]
        public decimal Cancelado { get; set; }

        [JsonPropertyName("esperando_aprovacao")]
        public decimal EsperandoAprovacao { get; set; }
    }
}
