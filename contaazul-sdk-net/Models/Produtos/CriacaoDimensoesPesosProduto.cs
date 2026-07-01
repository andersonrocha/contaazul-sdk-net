using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Dimensões e pesos do produto na criação.</summary>
    public sealed class CriacaoDimensoesPesosProduto
    {
        /// <summary>Altura do produto em centímetros.</summary>
        [JsonPropertyName("altura")]
        public decimal? Altura { get; set; }

        /// <summary>Largura do produto em centímetros.</summary>
        [JsonPropertyName("largura")]
        public decimal? Largura { get; set; }

        /// <summary>Peso bruto do produto em quilogramas.</summary>
        [JsonPropertyName("peso_bruto")]
        public decimal? PesoBruto { get; set; }

        /// <summary>Peso líquido do produto em quilogramas.</summary>
        [JsonPropertyName("peso_liquido")]
        public decimal? PesoLiquido { get; set; }

        /// <summary>Profundidade do produto em centímetros.</summary>
        [JsonPropertyName("profundidade")]
        public decimal? Profundidade { get; set; }

        /// <summary>Número de volumes do produto.</summary>
        [JsonPropertyName("volumes")]
        public int? Volumes { get; set; }
    }
}
