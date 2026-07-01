using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Dados para criar um novo produto (<c>POST /v1/produtos</c>).
    /// Apenas <see cref="Nome"/> é obrigatório.
    /// </summary>
    public sealed class CriacaoProduto
    {
        /// <summary>Indica se o produto está ativo.</summary>
        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }

        /// <summary>Grupo ou categoria do produto.</summary>
        [JsonPropertyName("categoria")]
        public ReferenciaIdInteiroProduto Categoria { get; set; }

        /// <summary>Código EAN do produto.</summary>
        [JsonPropertyName("codigo_ean")]
        public string CodigoEan { get; set; }

        /// <summary>Código SKU do produto (máx. 20 caracteres).</summary>
        [JsonPropertyName("codigo_sku")]
        public string CodigoSku { get; set; }

        /// <summary>
        /// Conversões de unidade de medida do produto (depende da unidade de medida das
        /// informações fiscais).
        /// </summary>
        [JsonPropertyName("conversoes_unidade_medida")]
        public List<CriacaoConversaoUnidadeMedidaProduto> ConversoesUnidadeMedida { get; set; }

        /// <summary>Descrição do produto (máx. 400 caracteres).</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Detalhes do kit de produto.</summary>
        [JsonPropertyName("detalhe_kit")]
        public CriacaoDetalheKitProduto DetalheKit { get; set; }

        /// <summary>Informações de e-commerce do produto.</summary>
        [JsonPropertyName("ecommerce")]
        public CriacaoEcommerceProduto Ecommerce { get; set; }

        /// <summary>Informações de estoque do produto.</summary>
        [JsonPropertyName("estoque")]
        public CriacaoEstoqueProduto Estoque { get; set; }

        /// <summary>Informações fiscais do produto.</summary>
        [JsonPropertyName("fiscal")]
        public CriacaoFiscalProduto Fiscal { get; set; }

        /// <summary>Formato do produto (<c>SIMPLES</c> ou <c>VARIACAO</c>).</summary>
        [JsonPropertyName("formato")]
        public string Formato { get; set; }

        /// <summary>ID do centro de custo associado ao produto.</summary>
        [JsonPropertyName("id_centro_custo")]
        public string IdCentroCusto { get; set; }

        /// <summary>Nome do produto (obrigatório, máx. 100 caracteres).</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Dimensões e pesos do produto.</summary>
        [JsonPropertyName("pesos_dimensoes")]
        public CriacaoDimensoesPesosProduto PesosDimensoes { get; set; }

        /// <summary>Status do produto (<c>ATIVO</c> ou <c>INATIVO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Unidade de medida fiscal do produto.</summary>
        [JsonPropertyName("unidade_medida")]
        public ReferenciaIdInteiroProduto UnidadeMedida { get; set; }

        /// <summary>Informações de variação do produto.</summary>
        [JsonPropertyName("variacao")]
        public CriacaoVariacaoProduto Variacao { get; set; }
    }
}
