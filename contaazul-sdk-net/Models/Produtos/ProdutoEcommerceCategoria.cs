using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Resposta da listagem de categorias de e-commerce
    /// (<c>GET /v1/produtos/ecommerce-categorias</c>).
    /// </summary>
    public sealed class ProdutoEcommerceCategoria
    {
        /// <summary>ID raiz da árvore de categorias de e-commerce.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Categorias de e-commerce.</summary>
        [JsonPropertyName("items")]
        public List<ProdutoEcommerceCategoriaItem> Items { get; set; }

        /// <summary>Versão.</summary>
        [JsonPropertyName("versao")]
        public int? Versao { get; set; }
    }
}
