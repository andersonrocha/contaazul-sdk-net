using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class Venda
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("numero")]
        public int Numero { get; set; }

        [JsonPropertyName("data_emissao")]
        public string DataEmissao { get; set; }

        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("origem")]
        public string Origem { get; set; }

        [JsonPropertyName("valor_total")]
        public decimal ValorTotal { get; set; }

        [JsonPropertyName("valor_desconto")]
        public decimal ValorDesconto { get; set; }

        [JsonPropertyName("valor_liquido")]
        public decimal ValorLiquido { get; set; }

        [JsonPropertyName("cliente")]
        public VendaCliente Cliente { get; set; }

        [JsonPropertyName("vendedor")]
        public VendaVendedor Vendedor { get; set; }

        [JsonPropertyName("data_criacao")]
        public string DataCriacao { get; set; }

        [JsonPropertyName("data_alteracao")]
        public string DataAlteracao { get; set; }
    }
}
