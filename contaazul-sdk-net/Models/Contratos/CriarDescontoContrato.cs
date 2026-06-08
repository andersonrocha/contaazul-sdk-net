using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Desconto aplicado ao contrato na criação.
    /// </summary>
    public sealed class CriarDescontoContrato
    {
        /// <summary>Tipo de desconto: <c>VALOR</c> ou <c>PORCENTAGEM</c>. <b>Obrigatório.</b></summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Valor do desconto (mínimo 0). <b>Obrigatório.</b></summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
