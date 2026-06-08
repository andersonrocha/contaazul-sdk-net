using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Desconto aplicado a uma venda, por valor ou porcentagem.
    /// </summary>
    public sealed class Desconto
    {
        /// <summary>Tipo de desconto: <c>PORCENTAGEM</c> ou <c>VALOR</c>.</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Valor do desconto.</summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
