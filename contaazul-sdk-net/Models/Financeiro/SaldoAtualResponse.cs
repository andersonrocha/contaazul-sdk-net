using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Saldo atual de uma conta financeira.</summary>
    public sealed class SaldoAtualResponse
    {
        [JsonPropertyName("saldo_atual")]
        public decimal? SaldoAtual { get; set; }
    }
}
