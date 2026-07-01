using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>Natureza operacional do serviço.</summary>
    public sealed class NaturezaOperacionalDoServico
    {
        /// <summary>ID da natureza operacional.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
