using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Estrutura de DRE (Demonstração do Resultado do Exercício).</summary>
    public sealed class EstruturaDre
    {
        [JsonPropertyName("itens")]
        public List<CategoriaDre> Itens { get; set; }
    }
}
