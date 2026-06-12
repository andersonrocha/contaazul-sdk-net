using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Anexo de uma parcela (boleto, recibo, fatura, etc.).</summary>
    public sealed class AnexoParcela
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>Tipo de conteúdo: <c>FILE</c> ou <c>URL</c>.</summary>
        [JsonPropertyName("tipo_conteudo")]
        public string TipoConteudo { get; set; }

        [JsonPropertyName("referencia")]
        public string Referencia { get; set; }

        /// <summary>
        /// Tipo do anexo: <c>BOLETO_BANCARIO_RFB</c>, <c>BOLETO_BANCARIO</c>, <c>RECIBO</c>,
        /// <c>FATURA</c>, <c>OUTROS</c> ou <c>RECIBO_DIGITAL</c>.
        /// </summary>
        [JsonPropertyName("tipo_anexo")]
        public string TipoAnexo { get; set; }

        [JsonPropertyName("id_parcela")]
        public string IdParcela { get; set; }
    }
}
