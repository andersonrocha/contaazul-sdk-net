using System.Collections.Generic;
using Newtonsoft.Json;

namespace contaazul_dotnet.Models
{
    public class PessoaListResponse
    {
        [JsonProperty("data")]
        public List<Pessoa> Data { get; set; }

        [JsonProperty("pagina")]
        public int Pagina { get; set; }

        [JsonProperty("tamanho_pagina")]
        public int TamanhoPagina { get; set; }

        [JsonProperty("total_registros")]
        public int TotalRegistros { get; set; }

        [JsonProperty("total_paginas")]
        public int TotalPaginas { get; set; }
    }
}
