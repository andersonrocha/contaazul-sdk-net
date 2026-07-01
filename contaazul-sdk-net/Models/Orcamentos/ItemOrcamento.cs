using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>Item de um orçamento (detalhe).</summary>
    public sealed class ItemOrcamento
    {
        /// <summary>Custo do item do orçamento.</summary>
        [JsonPropertyName("custo")]
        public decimal? Custo { get; set; }

        /// <summary>Descrição do item do orçamento.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID do item do orçamento.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome do item do orçamento.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Quantidade do item do orçamento.</summary>
        [JsonPropertyName("quantidade")]
        public decimal? Quantidade { get; set; }

        /// <summary>Tipo do item do orçamento (<c>PRODUTO</c> ou <c>SERVICO</c>).</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Valor do item do orçamento.</summary>
        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }
    }
}
