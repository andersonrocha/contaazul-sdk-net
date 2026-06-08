using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Resposta paginada da listagem de contratos.
    /// </summary>
    public sealed class ContratosFiltroResposta
    {
        /// <summary>Lista de contratos encontrados.</summary>
        [JsonPropertyName("itens")]
        public List<ItemContrato> Itens { get; set; }

        /// <summary>Total de contratos encontrados.</summary>
        [JsonPropertyName("itens_totais")]
        public int ItensTotais { get; set; }
    }
}
