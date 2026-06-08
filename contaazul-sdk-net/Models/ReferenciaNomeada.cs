using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Referência a uma entidade identificada por ID e nome
    /// (ex.: cliente ou vendedor vinculado a um contrato).
    /// </summary>
    public sealed class ReferenciaNomeada
    {
        /// <summary>ID da entidade.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome da entidade.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
