using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Produto associado à variação, na criação.</summary>
    public sealed class CriacaoProdutoVariacaoProduto
    {
        /// <summary>Código EAN do produto associado à variação.</summary>
        [JsonPropertyName("codigo_ean")]
        public string CodigoEan { get; set; }

        /// <summary>Código SKU do produto associado à variação.</summary>
        [JsonPropertyName("codigo_sku")]
        public string CodigoSku { get; set; }

        /// <summary>Nome do produto associado à variação.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Lista de opções de variação do produto associado.</summary>
        [JsonPropertyName("opcoes")]
        public List<ReferenciaIdProduto> Opcoes { get; set; }

        /// <summary>Saldo do produto associado à variação.</summary>
        [JsonPropertyName("saldo")]
        public decimal? Saldo { get; set; }

        /// <summary>Valor de venda do produto associado à variação.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }
    }
}
