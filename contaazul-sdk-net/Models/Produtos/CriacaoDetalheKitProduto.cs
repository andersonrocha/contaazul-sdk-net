using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Detalhes do kit de produto na criação.</summary>
    public sealed class CriacaoDetalheKitProduto
    {
        /// <summary>Lista de itens do kit de produto.</summary>
        [JsonPropertyName("itens")]
        public List<CriacaoItemDetalheKitProduto> Itens { get; set; }

        /// <summary>Valor de venda do kit de produto.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }
    }
}
