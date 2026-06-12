using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Item (canal) de uma notificação de cobrança.</summary>
    public sealed class ItemNotificacaoCobranca
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("sms")]
        public string Sms { get; set; }

        [JsonPropertyName("whatsapp")]
        public string Whatsapp { get; set; }

        /// <summary>Status de entrega: <c>ENVIADO</c> ou <c>INVALIDO</c>.</summary>
        [JsonPropertyName("status_entrega")]
        public string StatusEntrega { get; set; }
    }
}
