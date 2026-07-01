using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>Cliente associado a um orçamento na listagem.</summary>
    public sealed class ClienteOrcamento
    {
        /// <summary>Email do cliente.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>ID do cliente.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome do cliente.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
