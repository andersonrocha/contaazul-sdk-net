using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>Cidade de emissão da nota fiscal de serviço.</summary>
    public sealed class CidadeEmissaoNotaFiscalServico
    {
        /// <summary>Estado (UF).</summary>
        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        /// <summary>Nome da cidade.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
