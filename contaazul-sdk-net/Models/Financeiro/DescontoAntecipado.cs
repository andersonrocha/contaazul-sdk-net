using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Desconto antecipado de uma cobrança. Informe apenas um entre
    /// <see cref="Valor"/> e <see cref="Percentual"/>.
    /// </summary>
    public sealed class DescontoAntecipado
    {
        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }

        [JsonPropertyName("percentual")]
        public decimal? Percentual { get; set; }

        [JsonPropertyName("dias_antes_vencer")]
        public int? DiasAntesVencer { get; set; }
    }
}
