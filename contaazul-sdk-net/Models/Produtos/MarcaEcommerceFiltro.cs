using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Filtro para a listagem de marcas de e-commerce (<c>GET /v1/produtos/ecommerce-marcas</c>).
    /// Herda <c>pagina</c>, <c>tamanho_pagina</c> e <c>busca_textual</c>.
    /// </summary>
    public sealed class MarcaEcommerceFiltro : BuscaTextualFiltro
    {
        /// <summary>Direção de ordenação (<c>ASC</c> ou <c>DESC</c>).</summary>
        [QueryParameter("direcao")]
        public string Direcao { get; set; }
    }
}
