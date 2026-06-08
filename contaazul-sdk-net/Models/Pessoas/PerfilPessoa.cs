using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Perfil associado a uma pessoa. Modelo unificado usado na leitura e na criação/atualização —
    /// o <see cref="Id"/> aparece apenas em respostas e é omitido nas requisições.
    /// </summary>
    public sealed class PerfilPessoa
    {
        /// <summary>ID do perfil (presente apenas em respostas).</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Tipo de perfil. Valores possíveis: <c>Cliente</c>, <c>Fornecedor</c>, <c>Transportadora</c>.
        /// </summary>
        [JsonPropertyName("tipo_perfil")]
        public string TipoPerfil { get; set; }
    }
}
