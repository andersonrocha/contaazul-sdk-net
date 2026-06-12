using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Categoria financeira associada a um item da DRE.</summary>
    public sealed class CategoriaFinanceira
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }
    }
}
