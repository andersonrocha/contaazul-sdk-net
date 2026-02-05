using Newtonsoft.Json;

namespace contaazul_dotnet.Models
{
    public class Venda
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("data_emissao")]
        public string DataEmissao { get; set; }

        [JsonProperty("situacao")]
        public string Situacao { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("origem")]
        public string Origem { get; set; }

        [JsonProperty("valor_total")]
        public decimal ValorTotal { get; set; }

        [JsonProperty("valor_desconto")]
        public decimal ValorDesconto { get; set; }

        [JsonProperty("valor_liquido")]
        public decimal ValorLiquido { get; set; }

        [JsonProperty("cliente")]
        public VendaCliente Cliente { get; set; }

        [JsonProperty("vendedor")]
        public VendaVendedor Vendedor { get; set; }

        [JsonProperty("data_criacao")]
        public string DataCriacao { get; set; }

        [JsonProperty("data_alteracao")]
        public string DataAlteracao { get; set; }
    }

    public class VendaCliente
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }

    public class VendaVendedor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }
}
