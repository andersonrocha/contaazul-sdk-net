using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>Item a ser incluído na criação de um orçamento.</summary>
    public sealed class CriarItemOrcamento
    {
        /// <summary>ID do item (produto ou serviço). Obrigatório.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Quantidade do item; deve ser maior que zero.</summary>
        [JsonPropertyName("quantidade")]
        public decimal? Quantidade { get; set; }

        /// <summary>Valor unitário do item; deve ser maior que zero.</summary>
        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }

        /// <summary>Valor de custo do item.</summary>
        [JsonPropertyName("valor_custo")]
        public decimal? ValorCusto { get; set; }
    }
}
