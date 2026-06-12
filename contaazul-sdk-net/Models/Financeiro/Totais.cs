using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Totais de itens agrupados por status (ativo/inativo/todos).</summary>
    public sealed class Totais
    {
        [JsonPropertyName("ativo")]
        public int? Ativo { get; set; }

        [JsonPropertyName("inativo")]
        public int? Inativo { get; set; }

        [JsonPropertyName("todos")]
        public int? Todos { get; set; }
    }
}
