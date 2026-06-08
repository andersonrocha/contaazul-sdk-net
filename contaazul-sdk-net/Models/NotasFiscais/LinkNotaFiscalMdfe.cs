using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>
    /// Dados para vincular uma ou mais notas fiscais a um MDF-e
    /// (Manifesto Eletrônico de Documentos Fiscais) — <c>POST /v1/notas-fiscais/vinculo-mdfe</c>.
    /// </summary>
    public sealed class LinkNotaFiscalMdfe
    {
        /// <summary>Chaves de acesso das notas fiscais vinculadas ao MDF-e. Obrigatório.</summary>
        [JsonPropertyName("chaves_acesso")]
        public List<string> ChavesAcesso { get; set; }

        /// <summary>Identificador do MDF-e. Obrigatório.</summary>
        [JsonPropertyName("identificador")]
        public string Identificador { get; set; }

        /// <summary>
        /// Status do MDF-e. Valores possíveis: <c>AUTORIZADO</c>, <c>ENCERRADO</c>, <c>CANCELADO</c>.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
