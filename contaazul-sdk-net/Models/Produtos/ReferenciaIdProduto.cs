using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Referência por ID (UUID) usada na criação de produto (categoria de e-commerce, marca e
    /// opções de variação), onde apenas o <c>id</c> é enviado.
    /// </summary>
    public sealed class ReferenciaIdProduto
    {
        /// <summary>ID (UUID) da entidade referenciada.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
