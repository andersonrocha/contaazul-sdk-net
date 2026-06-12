using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Lado (origem/destino) de uma transferência entre contas financeiras.</summary>
    public sealed class Quitacao
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("composicao_valor")]
        public ComposicaoValorFinanceiro ComposicaoValor { get; set; }

        [JsonPropertyName("conta_financeira")]
        public ContaFinanceiraDetalhes ContaFinanceira { get; set; }
    }
}
