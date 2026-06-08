using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>Contato para cobrança e faturamento de uma pessoa.</summary>
    public sealed class ContatoCobrancaFaturamento
    {
        /// <summary>Emails do contato de cobrança e faturamento.</summary>
        [JsonPropertyName("emails")]
        public List<string> Emails { get; set; }

        /// <summary>Telefone (WhatsApp) do contato de cobrança e faturamento.</summary>
        [JsonPropertyName("whatsapp")]
        public string Whatsapp { get; set; }
    }
}
