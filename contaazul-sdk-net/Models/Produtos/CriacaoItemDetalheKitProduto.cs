using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Item do detalhe do kit de produto na criação.</summary>
    public sealed class CriacaoItemDetalheKitProduto
    {
        /// <summary>ID do produto associado ao item no kit.</summary>
        [JsonPropertyName("id_produto")]
        public string IdProduto { get; set; }

        /// <summary>Quantidade do produto associado ao item no kit.</summary>
        [JsonPropertyName("quantidade")]
        public decimal? Quantidade { get; set; }

        /// <summary>Valor total do produto associado ao item no kit.</summary>
        [JsonPropertyName("valor_total")]
        public decimal? ValorTotal { get; set; }

        /// <summary>Valor unitário do produto associado ao item no kit.</summary>
        [JsonPropertyName("valor_unitario")]
        public decimal? ValorUnitario { get; set; }
    }
}
