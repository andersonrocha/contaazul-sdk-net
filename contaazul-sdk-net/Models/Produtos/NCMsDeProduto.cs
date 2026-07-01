using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Resposta da listagem de NCMs de produtos (<c>GET /v1/produtos/ncm</c>).
    /// </summary>
    public sealed class NCMsDeProduto
    {
        /// <summary>Lista de NCMs.</summary>
        [JsonPropertyName("items")]
        public List<ReferenciaFiscalProduto> Items { get; set; }

        /// <summary>Total de itens.</summary>
        [JsonPropertyName("total_items")]
        public long? TotalItems { get; set; }
    }
}
