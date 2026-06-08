using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Requisição de exclusão de vendas em lote (mínimo 1, máximo 10 IDs).
    /// </summary>
    public sealed class ExclusaoLote
    {
        /// <summary>Lista de UUIDs das vendas a serem excluídas (1 a 10). <b>Obrigatório.</b></summary>
        [JsonPropertyName("ids")]
        public List<string> Ids { get; set; }
    }
}
