using System.Collections.Generic;
using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Models
{
    public class NotaFiscalListResponse
    {
        [JsonProperty("itens")]
        public List<NotaFiscal> Itens { get; set; }

        [JsonProperty("paginacao")]
        public Paginacao Paginacao { get; set; }
    }
}
