using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Saldo inicial de uma conta financeira em uma data de competência.</summary>
    public sealed class SaldoInicialContaFinanceira
    {
        /// <summary>Tipo: <c>RECEITA</c> ou <c>DESPESA</c>.</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("id_conta_financeira")]
        public string IdContaFinanceira { get; set; }

        [JsonPropertyName("data_competencia")]
        public string DataCompetencia { get; set; }

        [JsonPropertyName("saldo_inicial")]
        public decimal? SaldoInicial { get; set; }
    }
}
