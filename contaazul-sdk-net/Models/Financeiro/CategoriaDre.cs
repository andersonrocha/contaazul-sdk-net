using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Item (recursivo) da estrutura de DRE.</summary>
    public sealed class CategoriaDre
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("posicao")]
        public long? Posicao { get; set; }

        [JsonPropertyName("indica_totalizador")]
        public bool? IndicaTotalizador { get; set; }

        [JsonPropertyName("representa_soma_custo_medio")]
        public bool? RepresentaSomaCustoMedio { get; set; }

        [JsonPropertyName("subitens")]
        public List<CategoriaDre> Subitens { get; set; }

        [JsonPropertyName("categorias_financeiras")]
        public List<CategoriaFinanceira> CategoriasFinanceiras { get; set; }
    }
}
