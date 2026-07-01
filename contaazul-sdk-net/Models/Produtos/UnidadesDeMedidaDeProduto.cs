using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Resposta da listagem de unidades de medida de produtos
    /// (<c>GET /v1/produtos/unidades-medida</c>).
    /// </summary>
    public sealed class UnidadesDeMedidaDeProduto
    {
        /// <summary>Lista de unidades de medida.</summary>
        [JsonPropertyName("items")]
        public List<UnidadeMedidaProduto> Items { get; set; }

        /// <summary>Total de itens.</summary>
        [JsonPropertyName("total_items")]
        public long? TotalItems { get; set; }
    }
}
