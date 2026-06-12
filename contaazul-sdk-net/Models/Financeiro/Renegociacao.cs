using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Informações de renegociação. <see cref="IdEvento"/> é preenchido apenas no contexto
    /// de contas a receber.
    /// </summary>
    public sealed class Renegociacao
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("valor")]
        public decimal? Valor { get; set; }

        [JsonPropertyName("id_evento")]
        public string IdEvento { get; set; }
    }
}
