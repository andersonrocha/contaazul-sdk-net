using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Filtros para a busca de centros de custo (<c>GET /v1/centro-de-custo</c>).</summary>
    public class CentroDeCustoFiltro : FiltroBase
    {
        /// <summary>Busca textual por nome ou código.</summary>
        [QueryParameter("busca")]
        public string Busca { get; set; }

        /// <summary>Filtro rápido: <c>ATIVO</c>, <c>INATIVO</c> ou <c>TODOS</c>.</summary>
        [QueryParameter("filtro_rapido")]
        public string FiltroRapido { get; set; }

        [QueryParameter("campo_ordenado_ascendente")]
        public string CampoOrdenadoAscendente { get; set; }

        [QueryParameter("campo_ordenado_descendente")]
        public string CampoOrdenadoDescendente { get; set; }
    }
}
