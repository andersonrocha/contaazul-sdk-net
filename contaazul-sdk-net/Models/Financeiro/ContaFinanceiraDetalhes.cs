using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Detalhes resumidos de uma conta financeira em uma transferência.</summary>
    public sealed class ContaFinanceiraDetalhes
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("instituicao_bancaria")]
        public InstituicaoBancaria InstituicaoBancaria { get; set; }
    }
}
