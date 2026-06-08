using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Notificação (e-mail) enviada ao cliente referente à venda.
    /// </summary>
    public sealed class Notificacao
    {
        /// <summary>ID de referência da notificação.</summary>
        [JsonPropertyName("id_referencia")]
        public string IdReferencia { get; set; }

        /// <summary>Destinatário da notificação.</summary>
        [JsonPropertyName("enviado_para")]
        public string EnviadoPara { get; set; }

        /// <summary>Data e hora de envio.</summary>
        [JsonPropertyName("enviado_em")]
        public string EnviadoEm { get; set; }

        /// <summary>Data e hora de abertura.</summary>
        [JsonPropertyName("aberto_em")]
        public string AbertoEm { get; set; }

        /// <summary>Status de visualização (ex.: <c>ENVIADO</c>, <c>LIDO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
