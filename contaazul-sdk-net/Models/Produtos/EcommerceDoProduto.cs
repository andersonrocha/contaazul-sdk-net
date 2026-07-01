using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Informações de e-commerce do produto (detalhe).</summary>
    public sealed class EcommerceDoProduto
    {
        /// <summary>Categoria do produto no e-commerce.</summary>
        [JsonPropertyName("categoria_ecommerce")]
        public CategoriaDoProdutoNoEcommerce CategoriaEcommerce { get; set; }

        /// <summary>Condição do produto (<c>NOVO</c> ou <c>USADO</c>).</summary>
        [JsonPropertyName("condicao")]
        public string Condicao { get; set; }

        /// <summary>Descrição adicional do produto para e-commerce.</summary>
        [JsonPropertyName("descricao_adicional")]
        public string DescricaoAdicional { get; set; }

        /// <summary>Descrição SEO do produto.</summary>
        [JsonPropertyName("descricao_seo")]
        public string DescricaoSeo { get; set; }

        /// <summary>Indica se a integração de e-commerce está ativa.</summary>
        [JsonPropertyName("integracao_ativa")]
        public bool? IntegracaoAtiva { get; set; }

        /// <summary>Marca do produto.</summary>
        [JsonPropertyName("marca")]
        public MarcaEcommerceProduto Marca { get; set; }

        /// <summary>Título SEO do produto.</summary>
        [JsonPropertyName("titulo_seo")]
        public string TituloSeo { get; set; }

        /// <summary>URL SEO do produto.</summary>
        [JsonPropertyName("url_seo")]
        public string UrlSeo { get; set; }
    }
}
