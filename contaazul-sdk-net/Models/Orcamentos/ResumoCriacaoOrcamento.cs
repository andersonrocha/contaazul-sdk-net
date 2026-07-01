using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>Resposta da criação de um orçamento (<c>POST /v1/orcamentos</c>).</summary>
    public sealed class ResumoCriacaoOrcamento
    {
        /// <summary>ID do orçamento criado.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
