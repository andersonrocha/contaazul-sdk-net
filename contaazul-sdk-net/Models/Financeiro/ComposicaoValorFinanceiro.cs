using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Composição de valor usada nos recursos financeiros (baixas, parcelas, transferências,
    /// solicitações de cobrança). Modelo unificado — campos não informados são omitidos.
    /// </summary>
    public sealed class ComposicaoValorFinanceiro
    {
        [JsonPropertyName("valor_bruto")]
        public decimal? ValorBruto { get; set; }

        [JsonPropertyName("valor_liquido")]
        public decimal? ValorLiquido { get; set; }

        [JsonPropertyName("multa")]
        public decimal? Multa { get; set; }

        [JsonPropertyName("juros")]
        public decimal? Juros { get; set; }

        [JsonPropertyName("desconto")]
        public decimal? Desconto { get; set; }

        [JsonPropertyName("taxa")]
        public decimal? Taxa { get; set; }
    }
}
