using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>Mensagens de pagamentos em aberto de uma pessoa.</summary>
    public sealed class MensagensPagamentosAbertos
    {
        /// <summary>Número de mensagens de pagamentos em aberto.</summary>
        [JsonPropertyName("numero")]
        public int? Numero { get; set; }

        /// <summary>Total de mensagens de pagamentos em aberto.</summary>
        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }
}
