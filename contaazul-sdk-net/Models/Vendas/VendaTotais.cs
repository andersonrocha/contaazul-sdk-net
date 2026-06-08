using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Valores das vendas agrupados por status (aprovado, cancelado, esperando aprovação e total).
    /// </summary>
    public sealed class VendaTotais
    {
        /// <summary>Valor total de todas as vendas.</summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        /// <summary>Valor das vendas aprovadas.</summary>
        [JsonPropertyName("aprovado")]
        public decimal Aprovado { get; set; }

        /// <summary>Valor das vendas canceladas.</summary>
        [JsonPropertyName("cancelado")]
        public decimal Cancelado { get; set; }

        /// <summary>Valor das vendas esperando aprovação.</summary>
        [JsonPropertyName("esperando_aprovacao")]
        public decimal EsperandoAprovacao { get; set; }
    }
}
