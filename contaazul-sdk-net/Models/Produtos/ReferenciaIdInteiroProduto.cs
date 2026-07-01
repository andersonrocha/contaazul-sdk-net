using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Referência por ID inteiro usada na criação de produto (grupo/categoria, CEST, NCM e
    /// unidade de medida — fiscal e de conversão), onde apenas o <c>id</c> é enviado.
    /// </summary>
    public sealed class ReferenciaIdInteiroProduto
    {
        /// <summary>ID da entidade referenciada.</summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
