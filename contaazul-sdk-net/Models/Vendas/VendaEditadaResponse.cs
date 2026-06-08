using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Resposta da edição de uma venda.
    /// </summary>
    public sealed class VendaEditadaResponse
    {
        /// <summary>ID (UUID) da venda editada.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID legado da venda.</summary>
        [JsonPropertyName("id_legado")]
        public long? IdLegado { get; set; }
    }
}
