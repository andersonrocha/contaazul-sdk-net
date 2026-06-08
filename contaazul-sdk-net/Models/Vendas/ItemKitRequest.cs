using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Item que compõe um kit, na criação/edição de venda.
    /// </summary>
    public sealed class ItemKitRequest
    {
        /// <summary>ID (UUID) do produto que pertence ao kit. <b>Obrigatório.</b></summary>
        [JsonPropertyName("id_produto")]
        public string IdProduto { get; set; }

        /// <summary>ID (UUID) do kit que contém o produto. <b>Obrigatório.</b></summary>
        [JsonPropertyName("id_kit")]
        public string IdKit { get; set; }

        /// <summary>Quantidade do item do kit. <b>Obrigatório.</b></summary>
        [JsonPropertyName("quantidade")]
        public decimal Quantidade { get; set; }

        /// <summary>Valor do item do kit. <b>Obrigatório.</b></summary>
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
