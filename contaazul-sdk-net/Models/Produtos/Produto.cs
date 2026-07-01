using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Detalhe completo de um produto (<c>GET /v1/produtos/{id}</c> e resposta de
    /// <c>POST /v1/produtos</c>).
    /// </summary>
    public sealed class Produto
    {
        /// <summary>Indica se o produto está ativo.</summary>
        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }

        /// <summary>Grupo ou categoria do produto.</summary>
        [JsonPropertyName("categoria")]
        public CategoriaProduto Categoria { get; set; }

        /// <summary>Código de barras (EAN) do produto.</summary>
        [JsonPropertyName("codigo_ean")]
        public string CodigoEan { get; set; }

        /// <summary>Código interno (SKU) do produto.</summary>
        [JsonPropertyName("codigo_sku")]
        public string CodigoSku { get; set; }

        /// <summary>Conversões de unidade de medida do produto.</summary>
        [JsonPropertyName("conversao_unidade_medida")]
        public List<ConversaoDeUnidadeDeMedidaDoProduto> ConversaoUnidadeMedida { get; set; }

        /// <summary>Descrição detalhada do produto.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Detalhes do kit de produtos.</summary>
        [JsonPropertyName("detalhe_kit")]
        public DetalheDoKitDeProdutos DetalheKit { get; set; }

        /// <summary>Informações de e-commerce do produto.</summary>
        [JsonPropertyName("ecommerce")]
        public EcommerceDoProduto Ecommerce { get; set; }

        /// <summary>Informações de estoque do produto.</summary>
        [JsonPropertyName("estoque")]
        public EstoqueDoProduto Estoque { get; set; }

        /// <summary>Informações fiscais do produto.</summary>
        [JsonPropertyName("fiscal")]
        public FiscalDoProduto Fiscal { get; set; }

        /// <summary>Formato do produto (<c>SIMPLES</c> ou <c>VARIACAO</c>).</summary>
        [JsonPropertyName("formato")]
        public string Formato { get; set; }

        /// <summary>ID do produto (UUID).</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID do centro de custo associado ao produto.</summary>
        [JsonPropertyName("id_centro_custo")]
        public string IdCentroCusto { get; set; }

        /// <summary>ID do produto no sistema legado.</summary>
        [JsonPropertyName("id_legado")]
        public int? IdLegado { get; set; }

        /// <summary>Lista de imagens do produto.</summary>
        [JsonPropertyName("imagens")]
        public List<ImagemDoProduto> Imagens { get; set; }

        /// <summary>Nome do produto.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Dimensões e pesos do produto.</summary>
        [JsonPropertyName("pesos_dimensoes")]
        public DimensoesDoProduto PesosDimensoes { get; set; }

        /// <summary>Status atual do produto (<c>ATIVO</c> ou <c>INATIVO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Data da última atualização do produto (ISO 8601).</summary>
        [JsonPropertyName("ultima_atualizacao")]
        public string UltimaAtualizacao { get; set; }

        /// <summary>Unidade de medida fiscal do produto.</summary>
        [JsonPropertyName("unidade_medida")]
        public UnidadeMedidaResumoProduto UnidadeMedida { get; set; }

        /// <summary>Informações de variação do produto.</summary>
        [JsonPropertyName("variacao")]
        public VariacaoDoProduto Variacao { get; set; }

        /// <summary>Versão do produto.</summary>
        [JsonPropertyName("versao")]
        public int? Versao { get; set; }
    }
}
