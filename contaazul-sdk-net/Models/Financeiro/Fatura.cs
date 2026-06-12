using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Fatura associada a uma parcela.</summary>
    public sealed class Fatura
    {
        [JsonPropertyName("numero")]
        public long? Numero { get; set; }

        [JsonPropertyName("rps")]
        public long? Rps { get; set; }

        /// <summary>Tipo de fatura: <c>NFE</c>, <c>NFSE</c> ou <c>NFCE</c>.</summary>
        [JsonPropertyName("tipo_fatura")]
        public string TipoFatura { get; set; }
    }
}
