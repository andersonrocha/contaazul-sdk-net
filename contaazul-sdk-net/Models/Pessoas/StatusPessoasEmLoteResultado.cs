using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Resultado do processamento em lote de ativação/inativação de pessoas, detalhando
    /// quais IDs ficaram ativos, quais ficaram inativos e todos os IDs fornecidos.
    /// </summary>
    public sealed class StatusPessoasEmLoteResultado
    {
        /// <summary>IDs das pessoas ativadas.</summary>
        [JsonPropertyName("ativos")]
        public List<string> Ativos { get; set; }

        /// <summary>IDs das pessoas inativadas.</summary>
        [JsonPropertyName("inativos")]
        public List<string> Inativos { get; set; }

        /// <summary>Todos os IDs fornecidos na requisição.</summary>
        [JsonPropertyName("todos")]
        public List<string> Todos { get; set; }
    }
}
