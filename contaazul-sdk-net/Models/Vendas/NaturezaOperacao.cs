using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Natureza da operação de uma venda.
    /// </summary>
    public sealed class NaturezaOperacao
    {
        /// <summary>UUID da natureza da operação.</summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// Tipo da operação (<c>VENDA</c>, <c>REMESSA</c>, <c>COMPRA</c>, <c>DEVOLUCAO</c>).
        /// </summary>
        [JsonPropertyName("tipo_operacao")]
        public string TipoOperacao { get; set; }

        /// <summary>Template da natureza da operação (ex.: <c>VENDA_MERCADORIAS</c>).</summary>
        [JsonPropertyName("template_operacao")]
        public string TemplateOperacao { get; set; }

        /// <summary>Rótulo da natureza da operação.</summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>Indica se há mudança financeira.</summary>
        [JsonPropertyName("mudanca_financeira")]
        public bool? MudancaFinanceira { get; set; }

        /// <summary>
        /// Mudança de estoque (<c>ENTRADA_ESTOQUE</c>, <c>SAIDA_ESTOQUE</c>, <c>NAO_ALTERA_ESTOQUE</c>).
        /// </summary>
        [JsonPropertyName("mudanca_estoque")]
        public string MudancaEstoque { get; set; }
    }
}
