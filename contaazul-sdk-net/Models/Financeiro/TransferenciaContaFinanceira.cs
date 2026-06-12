using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Transferência entre contas financeiras.</summary>
    public sealed class TransferenciaContaFinanceira
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("origem")]
        public Quitacao Origem { get; set; }

        [JsonPropertyName("destino")]
        public Quitacao Destino { get; set; }
    }
}
