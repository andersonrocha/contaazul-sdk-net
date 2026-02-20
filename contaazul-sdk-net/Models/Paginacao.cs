using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Representa informações de paginação para respostas de listagem da API.
    /// </summary>
    public class Paginacao
    {
        /// <summary>
        /// Número da página atual.
        /// </summary>
        [JsonProperty("pagina_atual")]
        public int PaginaAtual { get; set; }

        /// <summary>
        /// Número total de páginas disponíveis.
        /// </summary>
        [JsonProperty("total_paginas")]
        public int TotalPaginas { get; set; }

        /// <summary>
        /// Quantidade de itens por página.
        /// </summary>
        [JsonProperty("tamanho_pagina")]
        public int TamanhoPagina { get; set; }

        /// <summary>
        /// Quantidade total de itens disponíveis.
        /// </summary>
        [JsonProperty("total_itens")]
        public int TotalItens { get; set; }
    }
}
