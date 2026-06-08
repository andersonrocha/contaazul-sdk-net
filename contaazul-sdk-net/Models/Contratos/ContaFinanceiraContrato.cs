using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Conta financeira vinculada ao contrato.
    /// </summary>
    public sealed class ContaFinanceiraContrato
    {
        /// <summary>ID da conta financeira.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Tipo de conta financeira (ex.: <c>CONTA_CORRENTE</c>, <c>CARTAO_CREDITO</c>, <c>POUPANCA</c>).
        /// </summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }
    }
}
