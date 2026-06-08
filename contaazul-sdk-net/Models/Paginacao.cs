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
        public long PaginaAtual { get; set; }

        /// <summary>
        /// Número total de páginas disponíveis.
        /// </summary>
        [JsonPropertyName("total_paginas")]
        public long TotalPaginas { get; set; }

        /// <summary>
        /// Quantidade de itens por página.
        /// <para>
        /// Alguns endpoints retornam <see cref="long.MaxValue"/> como sentinela (sem limite),
        /// por isso o tipo é <see cref="long"/>.
        /// </para>
        /// </summary>
        [JsonPropertyName("tamanho_pagina")]
        public long TamanhoPagina { get; set; }

        /// <summary>
        /// Quantidade total de itens disponíveis.
        /// </summary>
        [JsonPropertyName("total_itens")]
        public long TotalItens { get; set; }
    }
}
