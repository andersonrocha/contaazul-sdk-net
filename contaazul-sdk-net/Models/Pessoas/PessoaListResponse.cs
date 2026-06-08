using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    public sealed class PessoaListResponse
    {
        [JsonPropertyName("items")]
        public List<Pessoa> Items { get; set; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }
    }
}
