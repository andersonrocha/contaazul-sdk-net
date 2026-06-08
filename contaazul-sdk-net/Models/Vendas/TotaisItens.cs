using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Totais agrupados dos itens de uma venda.
    /// </summary>
    public sealed class TotaisItens
    {
        /// <summary>Quantidade total de produtos vendidos.</summary>
        [JsonPropertyName("quantidade_produtos")]
        public int QuantidadeProdutos { get; set; }

        /// <summary>Quantidade total de serviços vendidos.</summary>
        [JsonPropertyName("quantidade_servicos")]
        public int QuantidadeServicos { get; set; }

        /// <summary>Quantidade total de itens não conciliados.</summary>
        [JsonPropertyName("quantidade_nao_conciliados")]
        public int QuantidadeNaoConciliados { get; set; }
    }
}
