using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Referência de origem de um evento financeiro.</summary>
    public sealed class ReferenciaEvento
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("revisao")]
        public string Revisao { get; set; }

        /// <summary>Origem (ex.: <c>LANCAMENTO_FINANCEIRO</c>, <c>VENDA</c>, <c>RENEGOCIACAO</c>...).</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }
    }
}
