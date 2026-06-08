using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Situação de uma negociação.
    /// </summary>
    public sealed class SituacaoNegociacao
    {
        /// <summary>
        /// Nome da situação (ex.: <c>APROVADO</c>, <c>FATURADO</c>, <c>CANCELADO</c>, <c>ORCAMENTO</c>).
        /// </summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Descrição da situação.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Indica se a situação está ativada.</summary>
        [JsonPropertyName("ativado")]
        public bool? Ativado { get; set; }
    }
}
