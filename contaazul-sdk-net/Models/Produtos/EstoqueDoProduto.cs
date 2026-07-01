using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Informações de estoque do produto (detalhe).</summary>
    public sealed class EstoqueDoProduto
    {
        /// <summary>Custo médio do produto.</summary>
        [JsonPropertyName("custo_medio")]
        public decimal? CustoMedio { get; set; }

        /// <summary>Estoque máximo do produto.</summary>
        [JsonPropertyName("maximumStock")]
        public decimal? EstoqueMaximo { get; set; }

        /// <summary>Estoque mínimo do produto.</summary>
        [JsonPropertyName("minimumStock")]
        public decimal? EstoqueMinimo { get; set; }

        /// <summary>Saldo disponível do produto.</summary>
        [JsonPropertyName("quantidade_disponivel")]
        public decimal? QuantidadeDisponivel { get; set; }

        /// <summary>Saldo reservado do produto.</summary>
        [JsonPropertyName("quantidade_reservada")]
        public decimal? QuantidadeReservada { get; set; }

        /// <summary>Saldo total do estoque do produto.</summary>
        [JsonPropertyName("quantidade_total")]
        public decimal? QuantidadeTotal { get; set; }

        /// <summary>Valor de venda do produto.</summary>
        [JsonPropertyName("valor_venda")]
        public decimal? ValorVenda { get; set; }
    }
}
