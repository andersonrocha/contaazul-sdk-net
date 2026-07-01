using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Marca de e-commerce (id + nome). Usada na listagem de marcas
    /// (<c>GET /v1/produtos/ecommerce-marcas</c>) e na marca do produto no e-commerce.
    /// </summary>
    public sealed class MarcaEcommerceProduto
    {
        /// <summary>ID da marca.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome da marca.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
