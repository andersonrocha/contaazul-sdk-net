using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>
    /// Parâmetros para deletar serviços em lote (<c>DELETE /v1/servicos</c>).
    /// </summary>
    public sealed class ParametrosParaDeletarServicosEmLote
    {
        /// <summary>IDs (legado) dos serviços a serem deletados.</summary>
        [JsonPropertyName("ids")]
        public List<int> Ids { get; set; }
    }
}
