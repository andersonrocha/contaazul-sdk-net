using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Resposta da geração/consulta de uma cobrança.</summary>
    public sealed class GerarCobrancaResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Status da cobrança (ex.: <c>AGUARDANDO_CONFIRMACAO</c>, <c>REGISTRADO</c>,
        /// <c>QUITADO</c>, <c>CANCELADO</c>, <c>PAGO</c>...).
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
