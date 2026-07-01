using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Resposta da listagem de categorias de produtos (<c>GET /v1/produtos/categorias</c>).
    /// </summary>
    public sealed class CategoriasDeProduto
    {
        /// <summary>Lista de categorias de produtos.</summary>
        [JsonPropertyName("items")]
        public List<CategoriaProduto> Items { get; set; }

        /// <summary>Total de itens.</summary>
        [JsonPropertyName("total_items")]
        public long? TotalItems { get; set; }
    }
}
