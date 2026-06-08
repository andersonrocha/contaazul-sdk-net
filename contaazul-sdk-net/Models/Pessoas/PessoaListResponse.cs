using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Resposta da listagem de pessoas por filtro (<c>GET /v1/pessoas</c>).
    /// </summary>
    public sealed class PessoaListResponse
    {
        /// <summary>Itens resumidos das pessoas encontradas.</summary>
        [JsonPropertyName("items")]
        public List<ItemPessoaResumo> Items { get; set; }

        /// <summary>Total de itens encontrados.</summary>
        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }
    }
}
