using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Filtros para a listagem de produtos (<c>GET /v1/produtos</c>).
    /// Além destes, herda <c>pagina</c> e <c>tamanho_pagina</c> de <see cref="FiltroBase"/>.
    /// </summary>
    public class ProdutoFiltro : FiltroBase
    {
        /// <summary>Campo de ordenação (<c>NOME</c>, <c>CODIGO</c> ou <c>VALOR_VENDA</c>).</summary>
        [QueryParameter("campo_ordenacao")]
        public string CampoOrdenacao { get; set; }

        /// <summary>Direção de ordenação (<c>ASC</c> ou <c>DESC</c>).</summary>
        [QueryParameter("direcao_ordenacao")]
        public string DirecaoOrdenacao { get; set; }

        /// <summary>Busca textual pelo nome, EAN ou SKU do produto.</summary>
        [QueryParameter("busca")]
        public string Busca { get; set; }

        /// <summary>Status do produto (<c>ATIVO</c> ou <c>INATIVO</c>).</summary>
        [QueryParameter("status")]
        public string Status { get; set; }

        /// <summary>Filtra produtos com integração de e-commerce ativa.</summary>
        [QueryParameter("integracao_ecommerce_ativo")]
        public bool? IntegracaoEcommerceAtivo { get; set; }

        /// <summary>Filtra produtos que são kits.</summary>
        [QueryParameter("produtos_kit_ativo")]
        public bool? ProdutosKitAtivo { get; set; }

        /// <summary>Intervalo inicial do valor de venda.</summary>
        [QueryParameter("valor_venda_inicial")]
        public decimal? ValorVendaInicial { get; set; }

        /// <summary>Intervalo final do valor de venda.</summary>
        [QueryParameter("valor_venda_final")]
        public decimal? ValorVendaFinal { get; set; }

        /// <summary>SKU do produto para filtro exato.</summary>
        [QueryParameter("sku")]
        public string Sku { get; set; }

        /// <summary>Data de alteração de (ISO 8601, São Paulo/GMT-3).</summary>
        [QueryParameter("data_alteracao_de")]
        public string DataAlteracaoDe { get; set; }

        /// <summary>Data de alteração até (ISO 8601, São Paulo/GMT-3).</summary>
        [QueryParameter("data_alteracao_ate")]
        public string DataAlteracaoAte { get; set; }
    }
}
