using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Resumo da condição de pagamento de um evento financeiro.</summary>
    public sealed class CondicaoPagamentoEventoResumo
    {
        [JsonPropertyName("quantidade_parcelas")]
        public int? QuantidadeParcelas { get; set; }

        [JsonPropertyName("montante_fixo")]
        public bool? MontanteFixo { get; set; }
    }
}
