using System.Collections.Generic;
using Newtonsoft.Json;

namespace ContaAzul.Sdk.Net.Models
{
    public class PessoaListResponse
    {
        [JsonProperty("items")]
        public List<Pessoa> Items { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
    }
}
