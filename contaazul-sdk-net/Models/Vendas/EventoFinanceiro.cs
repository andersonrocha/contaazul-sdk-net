using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Evento financeiro associado a uma venda.
    /// </summary>
    public sealed class EventoFinanceiro
    {
        /// <summary>UUID do evento financeiro.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
