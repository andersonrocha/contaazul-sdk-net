using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Produto listado dentro da variação de um item do resumo de produtos.</summary>
    public sealed class ProdutoNaVariacao
    {
        /// <summary>Contagem de agregação do produto na variação.</summary>
        [JsonPropertyName("aggregationCount")]
        public int? ContagemAgregacao { get; set; }

        /// <summary>Código do produto na variação.</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Custo médio do produto na variação.</summary>
        [JsonPropertyName("custo_medio")]
        public decimal? CustoMedio { get; set; }

        /// <summary>EAN do produto na variação.</summary>
        [JsonPropertyName("ean")]
        public string Ean { get; set; }

        /// <summary>Estoque máximo do produto na variação.</summary>
        [JsonPropertyName("estoque_maximo")]
        public decimal? EstoqueMaximo { get; set; }

        /// <summary>Estoque mínimo do produto na variação.</summary>
        [JsonPropertyName("estoque_minimo")]
        public decimal? EstoqueMinimo { get; set; }

        /// <summary>ID do produto na variação.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID legado do produto na variação.</summary>
        [JsonPropertyName("id_legado")]
        public int? IdLegado { get; set; }

        /// <summary>ID do produto pai da variação.</summary>
        [JsonPropertyName("id_produto_pai_variacao")]
        public string IdProdutoPaiVariacao { get; set; }

        /// <summary>Indica se a integração com e-commerce está ativada.</summary>
        [JsonPropertyName("integracao_ecommerce_ativada")]
        public bool? IntegracaoEcommerceAtivada { get; set; }

        /// <summary>Indica se o produto na variação foi movido.</summary>
        [JsonPropertyName("movido")]
        public bool? Movido { get; set; }

        /// <summary>Nível de estoque do produto na variação (<c>MINIMO</c>, <c>MAXIMO</c>, <c>PADRAO</c>).</summary>
        [JsonPropertyName("nivel_estoque")]
        public string NivelEstoque { get; set; }

        /// <summary>Nome do produto na variação.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Saldo do produto na variação.</summary>
        [JsonPropertyName("saldo")]
        public decimal? Saldo { get; set; }

        /// <summary>Status do produto na variação (<c>ATIVO</c> ou <c>INATIVO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Tipo do produto na variação (<c>PRODUTO</c>, <c>KIT_PRODUTO</c>, <c>VARIACAO_PRODUTO</c>).</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Valor de venda do produto na variação.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }
    }
}
