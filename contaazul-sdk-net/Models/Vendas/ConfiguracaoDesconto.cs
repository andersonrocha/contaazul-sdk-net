using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Configuração de desconto da negociação.
    /// </summary>
    public sealed class ConfiguracaoDesconto
    {
        /// <summary>Tipo de desconto (<c>PORCENTAGEM</c> ou <c>VALOR</c>).</summary>
        [JsonPropertyName("tipo_desconto")]
        public string TipoDesconto { get; set; }

        /// <summary>Taxa/valor do desconto.</summary>
        [JsonPropertyName("taxa_desconto")]
        public decimal TaxaDesconto { get; set; }
    }
}
