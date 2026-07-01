using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Item que compõe um kit de produtos.</summary>
    public sealed class ItemDoKitDeProdutos
    {
        /// <summary>Código do produto associado ao item.</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>ID do centro de custo associado ao item.</summary>
        [JsonPropertyName("costCenterId")]
        public string CostCenterId { get; set; }

        /// <summary>ID do item no kit.</summary>
        [JsonPropertyName("id_item")]
        public string IdItem { get; set; }

        /// <summary>ID do produto associado ao item.</summary>
        [JsonPropertyName("id_produto")]
        public string IdProduto { get; set; }

        /// <summary>Nome do produto associado ao item.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Quantidade do produto no kit.</summary>
        [JsonPropertyName("quantidade")]
        public decimal? Quantidade { get; set; }

        /// <summary>Valor total do item no kit.</summary>
        [JsonPropertyName("valor_total")]
        public decimal? ValorTotal { get; set; }

        /// <summary>Valor unitário do produto no kit.</summary>
        [JsonPropertyName("valor_unitario")]
        public decimal? ValorUnitario { get; set; }

        /// <summary>Versão do item no kit.</summary>
        [JsonPropertyName("versao")]
        public int? Versao { get; set; }
    }
}
