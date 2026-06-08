using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Parcela de uma condição de pagamento (resposta).
    /// O campo <see cref="Id"/> é retornado apenas em alguns contextos.
    /// </summary>
    public sealed class Parcela
    {
        /// <summary>ID (UUID) da parcela, quando disponível.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Número da parcela.</summary>
        [JsonPropertyName("numero")]
        public int Numero { get; set; }

        /// <summary>Data de vencimento.</summary>
        [JsonPropertyName("data_vencimento")]
        public string DataVencimento { get; set; }

        /// <summary>Valor da parcela.</summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        /// <summary>Descrição da parcela.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
    }
}
