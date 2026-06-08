using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>Vínculo de uma pessoa com seu registro na API V1 legada.</summary>
    public sealed class PessoaLegada
    {
        /// <summary>ID legado da pessoa.</summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        /// <summary>Perfil da pessoa legada (ex.: <c>CLIENTE</c>).</summary>
        [JsonPropertyName("perfil")]
        public string Perfil { get; set; }

        /// <summary>UUID da pessoa legada.</summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
    }
}
