using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Anexo de uma baixa (recibo digital/recibo).</summary>
    public sealed class AnexoBaixa
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("referencia")]
        public string Referencia { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Tipo do anexo: <c>RECIBO_DIGITAL</c> ou <c>RECIBO</c>.</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Tipo de conteúdo: <c>FILE</c> ou <c>URL</c>.</summary>
        [JsonPropertyName("tipo_conteudo")]
        public string TipoConteudo { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
