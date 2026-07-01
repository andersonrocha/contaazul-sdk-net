using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Resposta da listagem de produtos (<c>GET /v1/produtos</c>).
    /// </summary>
    public sealed class ResumoDeProdutos
    {
        /// <summary>Lista de itens do resumo de produtos.</summary>
        [JsonPropertyName("items")]
        public List<ItemResumoDeProdutos> Items { get; set; }

        /// <summary>Total de itens encontrados.</summary>
        [JsonPropertyName("totalItems")]
        public long? TotalItems { get; set; }
    }
}
