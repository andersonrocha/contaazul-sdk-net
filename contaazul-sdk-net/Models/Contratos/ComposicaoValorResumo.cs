using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Resumo da composição de valores do contrato.
    /// </summary>
    public sealed class ComposicaoValorResumo
    {
        /// <summary>Valor do desconto aplicado.</summary>
        [JsonPropertyName("desconto")]
        public decimal Desconto { get; set; }

        /// <summary>Valor do frete.</summary>
        [JsonPropertyName("frete")]
        public decimal Frete { get; set; }

        /// <summary>Valor bruto do contrato.</summary>
        [JsonPropertyName("valor_bruto")]
        public decimal ValorBruto { get; set; }

        /// <summary>Valor total dos impostos sobre o serviço.</summary>
        [JsonPropertyName("valor_impostos_servico")]
        public decimal ValorImpostosServico { get; set; }

        /// <summary>Valor líquido do contrato.</summary>
        [JsonPropertyName("valor_liquido")]
        public decimal ValorLiquido { get; set; }
    }
}
