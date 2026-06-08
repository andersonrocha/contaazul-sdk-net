using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Contagens de itens de uma negociação.
    /// </summary>
    public sealed class ItensTotaisNegociacao
    {
        /// <summary>Quantidade de produtos.</summary>
        [JsonPropertyName("contagem_produtos")]
        public long ContagemProdutos { get; set; }

        /// <summary>Quantidade de serviços.</summary>
        [JsonPropertyName("contagem_servicos")]
        public long ContagemServicos { get; set; }

        /// <summary>Quantidade de itens não conciliados.</summary>
        [JsonPropertyName("contagem_nao_conciliados")]
        public long ContagemNaoConciliados { get; set; }
    }
}
