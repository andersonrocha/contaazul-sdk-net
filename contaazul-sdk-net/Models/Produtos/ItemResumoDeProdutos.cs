using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Item do resumo de produtos retornado na listagem (<c>GET /v1/produtos</c>).
    /// </summary>
    public sealed class ItemResumoDeProdutos
    {
        /// <summary>Código do produto.</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Contagem de agregação do produto.</summary>
        [JsonPropertyName("contagem_agregacao")]
        public int? ContagemAgregacao { get; set; }

        /// <summary>Custo médio do produto.</summary>
        [JsonPropertyName("custo_medio")]
        public decimal? CustoMedio { get; set; }

        /// <summary>EAN do produto.</summary>
        [JsonPropertyName("ean")]
        public string Ean { get; set; }

        /// <summary>Estoque máximo do produto.</summary>
        [JsonPropertyName("estoque_maximo")]
        public decimal? EstoqueMaximo { get; set; }

        /// <summary>Estoque mínimo do produto.</summary>
        [JsonPropertyName("estoque_minimo")]
        public decimal? EstoqueMinimo { get; set; }

        /// <summary>ID do produto.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID legado do produto.</summary>
        [JsonPropertyName("id_legado")]
        public int? IdLegado { get; set; }

        /// <summary>Indica se a integração com e-commerce está ativada.</summary>
        [JsonPropertyName("integracao_ecommerce_ativada")]
        public bool? IntegracaoEcommerceAtivada { get; set; }

        /// <summary>Indica se o produto foi movido.</summary>
        [JsonPropertyName("movido")]
        public bool? Movido { get; set; }

        /// <summary>Nível de estoque do produto (<c>MINIMO</c>, <c>MAXIMO</c>, <c>PADRAO</c>).</summary>
        [JsonPropertyName("nivel_estoque")]
        public string NivelEstoque { get; set; }

        /// <summary>Nome do produto.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Lista de variações do produto.</summary>
        [JsonPropertyName("produtos_variacao")]
        public List<ProdutoNaVariacao> ProdutosVariacao { get; set; }

        /// <summary>Saldo do produto.</summary>
        [JsonPropertyName("saldo")]
        public decimal? Saldo { get; set; }

        /// <summary>Status do produto (<c>ATIVO</c> ou <c>INATIVO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Tipo do produto (ex.: <c>PRODUTO</c>).</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Data da última atualização do produto (ISO 8601).</summary>
        [JsonPropertyName("ultima_atualizacao")]
        public string UltimaAtualizacao { get; set; }

        /// <summary>Valor de venda do produto.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }
    }
}
