using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class VendaListResponse
    {
        [JsonPropertyName("itens")]
        public List<Venda> Itens { get; set; }

        [JsonPropertyName("totais")]
        public VendaTotais Totais { get; set; }

        [JsonPropertyName("quantidades")]
        public VendaQuantidades Quantidades { get; set; }

        [JsonPropertyName("total_itens")]
        public int TotalItens { get; set; }
    }
}
