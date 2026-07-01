using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Informações de estoque do produto na criação.</summary>
    public sealed class CriacaoEstoqueProduto
    {
        /// <summary>Custo médio do produto.</summary>
        [JsonPropertyName("custo_medio")]
        public decimal? CustoMedio { get; set; }

        /// <summary>Estoque disponível do produto.</summary>
        [JsonPropertyName("estoque_disponivel")]
        public decimal? EstoqueDisponivel { get; set; }

        /// <summary>Estoque máximo do produto.</summary>
        [JsonPropertyName("estoque_maximo")]
        public decimal? EstoqueMaximo { get; set; }

        /// <summary>Estoque mínimo do produto.</summary>
        [JsonPropertyName("estoque_minimo")]
        public decimal? EstoqueMinimo { get; set; }

        /// <summary>Valor de venda do produto.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }
    }
}
