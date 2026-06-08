using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Resposta da exclusão de vendas em lote.
    /// </summary>
    public sealed class ExclusaoResponse
    {
        /// <summary>Quantidade de vendas excluídas.</summary>
        [JsonPropertyName("atualizados")]
        public int Atualizados { get; set; }

        /// <summary>Quantidade de vendas ignoradas.</summary>
        [JsonPropertyName("ignorados")]
        public int Ignorados { get; set; }
    }
}
