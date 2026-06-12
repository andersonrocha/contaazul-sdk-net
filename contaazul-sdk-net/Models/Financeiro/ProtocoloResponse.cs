using System;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Resposta de protocolo (criação de evento financeiro de contas a pagar/receber).</summary>
    public sealed class ProtocoloResponse
    {
        [JsonPropertyName("protocolId")]
        public string ProtocolId { get; set; }

        /// <summary>Status do protocolo: <c>PENDING</c>, <c>SUCCESS</c> ou <c>ERROR</c>.</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}
