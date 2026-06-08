using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Parcela na criação/edição de uma condição de pagamento.
    /// </summary>
    public sealed class ParcelaRequest
    {
        /// <summary>Data de vencimento (<c>YYYY-MM-DD</c>). <b>Obrigatório.</b></summary>
        [JsonPropertyName("data_vencimento")]
        public string DataVencimento { get; set; }

        /// <summary>Valor da parcela. <b>Obrigatório.</b></summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        /// <summary>Descrição da parcela. Opcional.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
    }
}
