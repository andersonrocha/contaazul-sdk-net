using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class VendaQuantidades
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("aprovado")]
        public int Aprovado { get; set; }

        [JsonPropertyName("cancelado")]
        public int Cancelado { get; set; }

        [JsonPropertyName("esperando_aprovacao")]
        public int EsperandoAprovacao { get; set; }
    }
}
