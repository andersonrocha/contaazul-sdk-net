using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Composição de valores (frete e desconto) na criação/edição de venda.
    /// </summary>
    public sealed class ValorComposicaoRequest
    {
        /// <summary>Valor de frete. Opcional.</summary>
        [JsonPropertyName("frete")]
        public decimal? Frete { get; set; }

        /// <summary>Desconto aplicado à venda. Opcional.</summary>
        [JsonPropertyName("desconto")]
        public Desconto Desconto { get; set; }
    }
}
