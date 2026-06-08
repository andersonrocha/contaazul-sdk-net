using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Composição detalhada dos valores de uma negociação.
    /// </summary>
    public sealed class ComposicaoValor
    {
        /// <summary>Valor bruto.</summary>
        [JsonPropertyName("valor_bruto")]
        public decimal ValorBruto { get; set; }

        /// <summary>Valor de desconto.</summary>
        [JsonPropertyName("desconto")]
        public decimal Desconto { get; set; }

        /// <summary>Valor do frete.</summary>
        [JsonPropertyName("frete")]
        public decimal Frete { get; set; }

        /// <summary>Valor dos impostos.</summary>
        [JsonPropertyName("impostos")]
        public decimal Impostos { get; set; }

        /// <summary>Valor dos impostos deduzidos.</summary>
        [JsonPropertyName("impostos_deduzidos")]
        public decimal ImpostosDeduzidos { get; set; }

        /// <summary>Valor do seguro.</summary>
        [JsonPropertyName("seguro")]
        public decimal Seguro { get; set; }

        /// <summary>Despesas incidentais.</summary>
        [JsonPropertyName("despesas_incidentais")]
        public decimal DespesasIncidentais { get; set; }

        /// <summary>Valor líquido.</summary>
        [JsonPropertyName("valor_liquido")]
        public decimal ValorLiquido { get; set; }
    }
}
