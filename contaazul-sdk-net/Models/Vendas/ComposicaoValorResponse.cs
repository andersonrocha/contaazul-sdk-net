using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Composição de valores retornada na criação de uma venda.
    /// </summary>
    public sealed class ComposicaoValorResponse
    {
        /// <summary>Valor bruto da venda.</summary>
        [JsonPropertyName("valor_bruto")]
        public decimal ValorBruto { get; set; }

        /// <summary>Desconto aplicado à venda.</summary>
        [JsonPropertyName("desconto")]
        public Desconto Desconto { get; set; }

        /// <summary>Valor do frete.</summary>
        [JsonPropertyName("frete")]
        public decimal Frete { get; set; }

        /// <summary>Valor líquido da venda.</summary>
        [JsonPropertyName("valor_liquido")]
        public decimal ValorLiquido { get; set; }
    }
}
