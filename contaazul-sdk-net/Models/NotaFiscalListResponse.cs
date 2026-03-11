using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class NotaFiscalListResponse
    {
        [JsonPropertyName("itens")]
        public List<NotaFiscal> Itens { get; set; }

        [JsonPropertyName("paginacao")]
        public Paginacao Paginacao { get; set; }
    }
}
