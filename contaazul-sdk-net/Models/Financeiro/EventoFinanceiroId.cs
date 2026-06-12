using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Identificador de um evento financeiro alterado.</summary>
    public sealed class EventoFinanceiroId
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
