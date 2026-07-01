using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>
    /// Composição de valor de um orçamento (frete e desconto). Usada tanto na criação
    /// (<c>POST /v1/orcamentos</c>) quanto na resposta de detalhe, cujo formato é idêntico.
    /// </summary>
    public sealed class ComposicaoValorOrcamento
    {
        /// <summary>Desconto aplicado ao orçamento (por <c>VALOR</c> ou <c>PORCENTAGEM</c>).</summary>
        [JsonPropertyName("desconto")]
        public Desconto Desconto { get; set; }

        /// <summary>Valor do frete do orçamento.</summary>
        [JsonPropertyName("frete")]
        public decimal? Frete { get; set; }
    }
}
