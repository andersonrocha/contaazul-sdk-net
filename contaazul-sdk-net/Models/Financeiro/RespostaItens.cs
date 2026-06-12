using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Resposta paginada genérica no formato <c>{ "itens_totais": n, "itens": [...], "totais": {...} }</c>,
    /// usada por vários endpoints financeiros. <see cref="Totais"/> é nulo quando o endpoint não o retorna.
    /// </summary>
    /// <typeparam name="T">Tipo de cada item.</typeparam>
    public sealed class RespostaItens<T>
    {
        [JsonPropertyName("itens")]
        public List<T> Itens { get; set; }

        [JsonPropertyName("itens_totais")]
        public long? ItensTotais { get; set; }

        [JsonPropertyName("totais")]
        public Totais Totais { get; set; }
    }
}
