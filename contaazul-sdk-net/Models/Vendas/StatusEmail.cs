using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Status do envio de e-mail de uma venda.
    /// </summary>
    public sealed class StatusEmail
    {
        /// <summary>Status do envio (ex.: <c>ENVIADO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Data de envio.</summary>
        [JsonPropertyName("enviado_em")]
        public string EnviadoEm { get; set; }
    }
}
