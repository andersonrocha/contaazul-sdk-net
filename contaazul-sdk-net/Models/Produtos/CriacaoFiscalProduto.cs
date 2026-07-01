using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Informações fiscais do produto na criação.</summary>
    public sealed class CriacaoFiscalProduto
    {
        /// <summary>CEST (Código Especificador da Substituição Tributária) do produto.</summary>
        [JsonPropertyName("cest")]
        public ReferenciaIdInteiroProduto Cest { get; set; }

        /// <summary>NCM (Nomenclatura Comum do Mercosul) do produto.</summary>
        [JsonPropertyName("ncm")]
        public ReferenciaIdInteiroProduto Ncm { get; set; }

        /// <summary>Origem do produto.</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }

        /// <summary>Tipo do produto.</summary>
        [JsonPropertyName("tipo_produto")]
        public string TipoProduto { get; set; }

        /// <summary>Unidade de medida fiscal do produto.</summary>
        [JsonPropertyName("unidade_medida")]
        public ReferenciaIdInteiroProduto UnidadeMedida { get; set; }
    }
}
