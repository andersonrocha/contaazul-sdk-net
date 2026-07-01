using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Imagem associada a um produto.</summary>
    public sealed class ImagemDoProduto
    {
        /// <summary>Descrição da imagem.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID da imagem.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome da imagem.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Tamanho da imagem em bytes.</summary>
        [JsonPropertyName("tamanho")]
        public long? Tamanho { get; set; }

        /// <summary>URL temporária da imagem.</summary>
        [JsonPropertyName("url_imagem")]
        public string UrlImagem { get; set; }
    }
}
