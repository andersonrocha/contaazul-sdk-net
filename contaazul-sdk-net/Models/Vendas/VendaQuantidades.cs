using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Quantidades de vendas agrupadas por status (aprovado, cancelado, esperando aprovação e total).
    /// </summary>
    public sealed class VendaQuantidades
    {
        /// <summary>Quantidade total de vendas.</summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        /// <summary>Quantidade de vendas aprovadas.</summary>
        [JsonPropertyName("aprovado")]
        public decimal Aprovado { get; set; }

        /// <summary>Quantidade de vendas canceladas.</summary>
        [JsonPropertyName("cancelado")]
        public decimal Cancelado { get; set; }

        /// <summary>Quantidade de vendas esperando aprovação.</summary>
        [JsonPropertyName("esperando_aprovacao")]
        public decimal EsperandoAprovacao { get; set; }
    }
}
