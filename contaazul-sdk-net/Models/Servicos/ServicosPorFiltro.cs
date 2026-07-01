using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>
    /// Resposta paginada da listagem de serviços (<c>GET /v1/servicos</c>).
    /// </summary>
    public sealed class ServicosPorFiltro
    {
        /// <summary>Lista de serviços.</summary>
        [JsonPropertyName("itens")]
        public List<Servico> Itens { get; set; }

        /// <summary>Informações de paginação.</summary>
        [JsonPropertyName("paginacao")]
        public Paginacao Paginacao { get; set; }
    }
}
