using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Produto pertencente à variação (detalhe).</summary>
    public sealed class ProdutosComVariacao
    {
        /// <summary>Código EAN do produto com variação.</summary>
        [JsonPropertyName("codigo_ean")]
        public string CodigoEan { get; set; }

        /// <summary>Código SKU do produto com variação.</summary>
        [JsonPropertyName("codigo_sku")]
        public string CodigoSku { get; set; }

        /// <summary>ID do produto com variação.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Indica se o produto com variação foi movido.</summary>
        [JsonPropertyName("movido")]
        public bool? Movido { get; set; }

        /// <summary>Nome do produto com variação.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Lista de opções do produto com variação.</summary>
        [JsonPropertyName("opcoes")]
        public List<OpcaoVariacaoProduto> Opcoes { get; set; }

        /// <summary>Indica se o produto com variação está relacionado manualmente.</summary>
        [JsonPropertyName("relacionado_manualmente")]
        public bool? RelacionadoManualmente { get; set; }

        /// <summary>Saldo do produto com variação.</summary>
        [JsonPropertyName("saldo")]
        public decimal? Saldo { get; set; }

        /// <summary>Valor de venda do produto com variação.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }

        /// <summary>Versão do produto com variação.</summary>
        [JsonPropertyName("versao")]
        public int? Versao { get; set; }
    }
}
