using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Representa informações de paginação para respostas de listagem da API.
    /// </summary>
    public sealed class Paginacao
    {
        /// <summary>
        /// Número da página atual.
        /// </summary>
        [JsonPropertyName("pagina_atual")]
        public int PaginaAtual { get; set; }

        /// <summary>
        /// Número total de páginas disponíveis.
        /// </summary>
        [JsonPropertyName("total_paginas")]
        public int TotalPaginas { get; set; }

        /// <summary>
        /// Quantidade de itens por página.
        /// </summary>
        [JsonPropertyName("tamanho_pagina")]
        public int TamanhoPagina { get; set; }

        /// <summary>
        /// Quantidade total de itens disponíveis.
        /// </summary>
        [JsonPropertyName("total_itens")]
        public int TotalItens { get; set; }
    }
}
