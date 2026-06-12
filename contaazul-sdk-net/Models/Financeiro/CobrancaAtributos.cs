using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Atributos opcionais de uma cobrança.</summary>
    public sealed class CobrancaAtributos
    {
        [JsonPropertyName("desconto_antecipado")]
        public DescontoAntecipado DescontoAntecipado { get; set; }
    }
}
