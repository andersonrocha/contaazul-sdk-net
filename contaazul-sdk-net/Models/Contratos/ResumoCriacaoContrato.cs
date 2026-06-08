using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Resumo retornado após a criação de um contrato.
    /// </summary>
    public sealed class ResumoCriacaoContrato
    {
        /// <summary>ID do contrato criado.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID legado do contrato.</summary>
        [JsonPropertyName("id_legado")]
        public int IdLegado { get; set; }

        /// <summary>ID da venda gerada pelo contrato.</summary>
        [JsonPropertyName("id_venda")]
        public string IdVenda { get; set; }
    }
}
