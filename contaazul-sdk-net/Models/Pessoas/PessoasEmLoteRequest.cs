using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// IDs das pessoas a serem processadas em lote (ativar, inativar ou excluir).
    /// </summary>
    public sealed class PessoasEmLoteRequest
    {
        /// <summary>UUIDs das pessoas. Para ativar/inativar, o máximo é 10 IDs por requisição.</summary>
        [JsonPropertyName("uuids")]
        public List<string> Uuids { get; set; }
    }
}
