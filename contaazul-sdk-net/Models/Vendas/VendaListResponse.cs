using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Resposta paginada da listagem de vendas (<c>/v1/venda/busca</c>).
    /// </summary>
    public sealed class VendaListResponse
    {
        /// <summary>Lista de vendas da página atual.</summary>
        [JsonPropertyName("itens")]
        public List<Venda> Itens { get; set; }

        /// <summary>Totais de valores por status.</summary>
        [JsonPropertyName("totais")]
        public VendaTotais Totais { get; set; }

        /// <summary>Quantidades de vendas por status.</summary>
        [JsonPropertyName("quantidades")]
        public VendaQuantidades Quantidades { get; set; }

        /// <summary>Total de itens encontrados.</summary>
        [JsonPropertyName("total_itens")]
        public int TotalItens { get; set; }
    }
}
