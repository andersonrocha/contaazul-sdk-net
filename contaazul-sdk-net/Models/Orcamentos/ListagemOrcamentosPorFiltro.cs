using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>
    /// Resposta da listagem de orçamentos por filtro (<c>GET /v1/orcamentos</c>).
    /// </summary>
    public sealed class ListagemOrcamentosPorFiltro
    {
        /// <summary>Lista de orçamentos.</summary>
        [JsonPropertyName("itens")]
        public List<ItemOrcamentoPorFiltro> Itens { get; set; }

        /// <summary>Total de itens encontrados.</summary>
        [JsonPropertyName("total_itens")]
        public long? TotalItens { get; set; }
    }
}
