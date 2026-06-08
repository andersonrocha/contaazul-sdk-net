using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>Lembrete de vencimento associado a uma pessoa.</summary>
    public sealed class LembreteVencimento
    {
        /// <summary>ID do lembrete de vencimento.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Indica se o lembrete de vencimento está ativo.</summary>
        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }

        /// <summary>Email do lembrete de vencimento.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
