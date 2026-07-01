using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Categoria do produto no e-commerce (descrição + id).
    /// </summary>
    public sealed class CategoriaDoProdutoNoEcommerce
    {
        /// <summary>Descrição da categoria do produto no e-commerce.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID da categoria do produto no e-commerce.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
