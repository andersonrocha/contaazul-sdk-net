using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Detalhe do kit de produtos (itens e valor de venda do kit).</summary>
    public sealed class DetalheDoKitDeProdutos
    {
        /// <summary>Lista de itens que compõem o kit.</summary>
        [JsonPropertyName("items")]
        public List<ItemDoKitDeProdutos> Items { get; set; }

        /// <summary>Valor de venda do kit.</summary>
        [JsonPropertyName("valor_venda_kit")]
        public decimal? ValorVendaKit { get; set; }
    }
}
