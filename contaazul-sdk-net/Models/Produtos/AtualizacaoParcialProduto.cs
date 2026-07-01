using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Dados para atualização parcial de um produto (<c>PATCH /v1/produtos/{id}</c>).
    /// Apenas os campos informados são alterados.
    /// </summary>
    public sealed class AtualizacaoParcialProduto
    {
        /// <summary>ID do Código CEST do produto.</summary>
        [JsonPropertyName("cest")]
        public int? Cest { get; set; }

        /// <summary>Código EAN do produto.</summary>
        [JsonPropertyName("codigo_ean")]
        public string CodigoEan { get; set; }

        /// <summary>Código (SKU) do produto.</summary>
        [JsonPropertyName("codigo_sku")]
        public string CodigoSku { get; set; }

        /// <summary>ID do Código NCM do produto.</summary>
        [JsonPropertyName("ncm")]
        public int? Ncm { get; set; }

        /// <summary>Nome do produto.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Peso bruto do produto.</summary>
        [JsonPropertyName("peso_bruto")]
        public decimal? PesoBruto { get; set; }

        /// <summary>Peso líquido do produto.</summary>
        [JsonPropertyName("peso_liquido")]
        public decimal? PesoLiquido { get; set; }

        /// <summary>ID da unidade de medida do produto.</summary>
        [JsonPropertyName("unidade_medida")]
        public int? UnidadeMedida { get; set; }

        /// <summary>Valor de venda do produto.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }
    }
}
