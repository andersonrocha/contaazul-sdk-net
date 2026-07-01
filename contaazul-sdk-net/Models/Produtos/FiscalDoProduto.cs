using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Informações fiscais do produto (detalhe).</summary>
    public sealed class FiscalDoProduto
    {
        /// <summary>CEST (Código Especificador da Substituição Tributária) do produto.</summary>
        [JsonPropertyName("cest")]
        public ReferenciaFiscalProduto Cest { get; set; }

        /// <summary>NCM (Nomenclatura Comum do Mercosul) do produto.</summary>
        [JsonPropertyName("ncm")]
        public ReferenciaFiscalProduto Ncm { get; set; }

        /// <summary>Origem do produto (ex.: <c>NACIONAL</c>).</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }

        /// <summary>Tipo do produto (ex.: <c>EMBALAGEM</c>).</summary>
        [JsonPropertyName("tipo_produto")]
        public string TipoProduto { get; set; }

        /// <summary>Unidade de medida do produto.</summary>
        [JsonPropertyName("unidade_medida")]
        public UnidadeMedidaResumoProduto UnidadeMedida { get; set; }
    }
}
