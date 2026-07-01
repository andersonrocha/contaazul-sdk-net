using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>
    /// Filtros para a listagem de serviços (<c>GET /v1/servicos</c>).
    /// Além de <c>busca_textual</c>, herda <c>pagina</c> e <c>tamanho_pagina</c> de <see cref="FiltroBase"/>.
    /// </summary>
    public sealed class ServicoFiltro : FiltroBase
    {
        /// <summary>Busca textual por nome, código ou descrição do serviço.</summary>
        [QueryParameter("busca_textual")]
        public string BuscaTextual { get; set; }
    }
}
