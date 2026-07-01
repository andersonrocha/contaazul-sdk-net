using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Protocolos
{
    /// <summary>
    /// Protocolo de acompanhamento de um evento financeiro enviado ao ERP
    /// (<c>GET /v1/protocolo/{id}</c>).
    /// </summary>
    public sealed class Protocolo
    {
        /// <summary>Identificador (UUID) do protocolo.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Mensagem do protocolo.</summary>
        [JsonPropertyName("resposta")]
        public string Resposta { get; set; }

        /// <summary>
        /// Status do protocolo: <c>PENDING</c> (em processamento), <c>SUCCESS</c> (criado)
        /// ou <c>ERROR</c> (erro na criação).
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Identificador (UUID) do evento financeiro associado.</summary>
        [JsonPropertyName("evento_financeiro_id")]
        public string EventoFinanceiroId { get; set; }
    }
}
