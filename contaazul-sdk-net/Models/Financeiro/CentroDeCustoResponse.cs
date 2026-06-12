using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Resposta da listagem de centros de custo. Observação: este endpoint usa a chave
    /// <c>items</c> (em inglês), diferente dos demais que usam <c>itens</c>.
    /// </summary>
    public sealed class CentroDeCustoResponse
    {
        [JsonPropertyName("items")]
        public List<CentroDeCusto> Items { get; set; }

        [JsonPropertyName("itens_totais")]
        public long? ItensTotais { get; set; }

        [JsonPropertyName("totais")]
        public Totais Totais { get; set; }
    }
}
