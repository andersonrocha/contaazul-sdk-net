using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Item de uma venda (produto/serviço) retornado na listagem de itens.
    /// </summary>
    public sealed class ItemVenda
    {
        /// <summary>UUID do item vendido.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>UUID do item (cadastro de produto/serviço).</summary>
        [JsonPropertyName("id_item")]
        public string IdItem { get; set; }

        /// <summary>Nome do item vendido.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Descrição do item vendido.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>
        /// Tipo do item (<c>PRODUTO</c>, <c>SERVICO</c>, <c>ATIVOS_IMOBILIZADOS</c>,
        /// <c>FINANCEIRO</c>, <c>KIT_PRODUTOS</c>).
        /// </summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Quantidade do item vendido.</summary>
        [JsonPropertyName("quantidade")]
        public decimal Quantidade { get; set; }

        /// <summary>Valor unitário do item vendido.</summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        /// <summary>Valor de custo do item vendido.</summary>
        [JsonPropertyName("custo")]
        public decimal Custo { get; set; }
    }
}
