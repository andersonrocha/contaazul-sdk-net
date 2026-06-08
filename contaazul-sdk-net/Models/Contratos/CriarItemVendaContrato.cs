using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Item de venda para a criação de um contrato.
    /// </summary>
    public sealed class CriarItemVendaContrato
    {
        /// <summary>Descrição do item da venda (máx. 500 caracteres). Opcional.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID do item da venda (produto/serviço). <b>Obrigatório.</b></summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Quantidade do item da venda (mínimo 0). <b>Obrigatório.</b></summary>
        [JsonPropertyName("quantidade")]
        public decimal Quantidade { get; set; }

        /// <summary>Valor unitário do item da venda (mínimo 0). <b>Obrigatório.</b></summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        /// <summary>Valor de custo do item da venda. Opcional.</summary>
        [JsonPropertyName("valor_custo")]
        public decimal? ValorCusto { get; set; }
    }
}
