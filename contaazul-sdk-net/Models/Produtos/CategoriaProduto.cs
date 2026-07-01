using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Grupo ou categoria de um produto. Usado tanto na listagem de categorias
    /// (<c>GET /v1/produtos/categorias</c>) quanto no detalhe do produto.
    /// </summary>
    public sealed class CategoriaProduto
    {
        /// <summary>Descrição da categoria.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID da categoria.</summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>UUID da categoria.</summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
    }
}
