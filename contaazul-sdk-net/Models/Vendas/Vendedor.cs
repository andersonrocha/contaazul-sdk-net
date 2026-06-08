using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Vendedor cadastrado na Conta Azul.
    /// </summary>
    public sealed class Vendedor
    {
        /// <summary>ID (UUID) do vendedor.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome do vendedor.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>ID legado do vendedor.</summary>
        [JsonPropertyName("id_legado")]
        public long? IdLegado { get; set; }
    }
}
