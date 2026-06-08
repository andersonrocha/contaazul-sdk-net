using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    /// <summary>
    /// Resposta paginada genérica no formato <c>{ "itens": [...], "paginacao": {...} }</c>,
    /// usada pelos endpoints que retornam o objeto <see cref="Paginacao"/>.
    /// </summary>
    /// <typeparam name="T">Tipo de cada item da lista.</typeparam>
    public sealed class RespostaPaginada<T>
    {
        /// <summary>Itens da página atual.</summary>
        [JsonPropertyName("itens")]
        public List<T> Itens { get; set; }

        /// <summary>Informações de paginação.</summary>
        [JsonPropertyName("paginacao")]
        public Paginacao Paginacao { get; set; }
    }
}
