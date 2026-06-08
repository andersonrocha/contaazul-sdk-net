using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Par genérico de <c>nome</c> e <c>descricao</c>, usado por diversos campos da API
    /// (ex.: situação e pendência de uma venda).
    /// </summary>
    public sealed class NomeDescricao
    {
        /// <summary>Nome.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Descrição.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
    }
}
