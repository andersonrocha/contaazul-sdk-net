using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Categoria de e-commerce (item da árvore retornada por
    /// <c>GET /v1/produtos/ecommerce-categorias</c>), podendo conter subcategorias.
    /// </summary>
    public sealed class ProdutoEcommerceCategoriaItem
    {
        /// <summary>Descrição da categoria.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID da categoria.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Subcategorias aninhadas.</summary>
        [JsonPropertyName("subcategorias")]
        public List<ProdutoEcommerceCategoriaItem> Subcategorias { get; set; }
    }
}
