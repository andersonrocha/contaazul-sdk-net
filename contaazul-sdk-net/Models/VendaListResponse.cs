using System.Collections.Generic;
using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Models
{
    public class VendaListResponse
    {
        [JsonProperty("itens")]
        public List<Venda> Itens { get; set; }

        [JsonProperty("totais")]
        public VendaTotais Totais { get; set; }

        [JsonProperty("quantidades")]
        public VendaQuantidades Quantidades { get; set; }

        [JsonProperty("total_itens")]
        public int TotalItens { get; set; }
    }

    public class VendaTotais
    {
        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("aprovado")]
        public decimal Aprovado { get; set; }

        [JsonProperty("cancelado")]
        public decimal Cancelado { get; set; }

        [JsonProperty("esperando_aprovacao")]
        public decimal EsperandoAprovacao { get; set; }
    }

    public class VendaQuantidades
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("aprovado")]
        public int Aprovado { get; set; }

        [JsonProperty("cancelado")]
        public int Cancelado { get; set; }

        [JsonProperty("esperando_aprovacao")]
        public int EsperandoAprovacao { get; set; }
    }
}
