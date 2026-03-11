using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class VendaCliente
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
