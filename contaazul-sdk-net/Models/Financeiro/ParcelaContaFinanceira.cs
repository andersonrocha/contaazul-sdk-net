using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Conta financeira resumida retornada na atualização de parcela.</summary>
    public sealed class ParcelaContaFinanceira
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("versao")]
        public int? Versao { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("agencia")]
        public string Agencia { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("banco")]
        public string Banco { get; set; }
    }
}
