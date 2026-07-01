using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Resposta da listagem de marcas de e-commerce (<c>GET /v1/produtos/ecommerce-marcas</c>).
    /// </summary>
    public sealed class MarcaDeEcommerce
    {
        /// <summary>Lista de marcas de e-commerce.</summary>
        [JsonPropertyName("items")]
        public List<MarcaEcommerceProduto> Items { get; set; }

        /// <summary>Total de itens.</summary>
        [JsonPropertyName("total_items")]
        public long? TotalItems { get; set; }
    }
}
