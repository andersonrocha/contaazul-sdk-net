using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Resposta da listagem de CESTs de produtos (<c>GET /v1/produtos/cest</c>).
    /// </summary>
    public sealed class CESTsDeProduto
    {
        /// <summary>Lista de CESTs.</summary>
        [JsonPropertyName("items")]
        public List<ReferenciaFiscalProduto> Items { get; set; }

        /// <summary>Total de itens.</summary>
        [JsonPropertyName("total_items")]
        public long? TotalItems { get; set; }
    }
}
