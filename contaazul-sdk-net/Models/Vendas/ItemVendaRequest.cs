using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Item de uma venda na criação/edição.
    /// </summary>
    public sealed class ItemVendaRequest
    {
        /// <summary>ID (UUID) do item (produto/serviço). <b>Obrigatório.</b></summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Descrição do item. Opcional.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Quantidade do item. <b>Obrigatório.</b></summary>
        [JsonPropertyName("quantidade")]
        public decimal Quantidade { get; set; }

        /// <summary>Valor unitário do item. <b>Obrigatório.</b></summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        /// <summary>Valor de custo do item. Opcional.</summary>
        [JsonPropertyName("valor_custo")]
        public decimal? ValorCusto { get; set; }

        /// <summary>Itens que compõem o kit, quando o item for um kit. Opcional.</summary>
        [JsonPropertyName("itens_kit")]
        public List<ItemKitRequest> ItensKit { get; set; }
    }
}
