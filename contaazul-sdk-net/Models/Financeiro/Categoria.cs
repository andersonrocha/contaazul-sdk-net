using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Categoria financeira (classificação de receita/despesa).</summary>
    public sealed class Categoria
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("versao")]
        public int? Versao { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("categoria_pai")]
        public string CategoriaPai { get; set; }

        /// <summary>Tipo da categoria: <c>RECEITA</c> ou <c>DESPESA</c>.</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("entrada_dre")]
        public string EntradaDre { get; set; }

        [JsonPropertyName("considera_custo_dre")]
        public bool? ConsideraCustoDre { get; set; }
    }
}
