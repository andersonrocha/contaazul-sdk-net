using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>
    /// Lista de IDs para exclusão de orçamentos em lote (<c>DELETE /v1/orcamentos</c>).
    /// Máximo de 10 IDs por requisição.
    /// </summary>
    public sealed class ExclusaoLoteOrcamento
    {
        /// <summary>IDs (UUID) dos orçamentos a serem excluídos (mínimo 1, máximo 10).</summary>
        [JsonPropertyName("ids")]
        public List<string> Ids { get; set; }
    }
}
