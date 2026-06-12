using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Informações da instituição bancária.</summary>
    public sealed class InstituicaoBancaria
    {
        [JsonPropertyName("codigo")]
        public int? Codigo { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
