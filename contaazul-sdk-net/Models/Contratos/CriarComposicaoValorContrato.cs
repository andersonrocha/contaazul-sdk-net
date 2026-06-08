using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Composição de valor do contrato na criação, incluindo frete e desconto.
    /// </summary>
    public sealed class CriarComposicaoValorContrato
    {
        /// <summary>Detalhes do desconto aplicado. Opcional.</summary>
        [JsonPropertyName("desconto")]
        public CriarDescontoContrato Desconto { get; set; }

        /// <summary>Valor de frete (mínimo 0). Opcional.</summary>
        [JsonPropertyName("frete")]
        public decimal? Frete { get; set; }
    }
}
