using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Resposta paginada dos itens de uma venda (<c>/v1/venda/{id_venda}/itens</c>).
    /// </summary>
    public sealed class ItensPaginados
    {
        /// <summary>Itens da venda na página atual.</summary>
        [JsonPropertyName("itens")]
        public List<ItemVenda> Itens { get; set; }

        /// <summary>Total de itens encontrados.</summary>
        [JsonPropertyName("itens_totais")]
        public int ItensTotais { get; set; }

        /// <summary>Totais agrupados dos itens.</summary>
        [JsonPropertyName("totais")]
        public TotaisItens Totais { get; set; }
    }
}
