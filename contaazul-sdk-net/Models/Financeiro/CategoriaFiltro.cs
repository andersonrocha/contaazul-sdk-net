using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Filtros para a busca de categorias (<c>GET /v1/categorias</c>).</summary>
    public class CategoriaFiltro : FiltroBase
    {
        [QueryParameter("campo_ordenado_ascendente")]
        public string CampoOrdenadoAscendente { get; set; }

        [QueryParameter("campo_ordenado_descendente")]
        public string CampoOrdenadoDescendente { get; set; }

        /// <summary>Busca textual por nome ou código.</summary>
        [QueryParameter("busca")]
        public string Busca { get; set; }

        /// <summary>Tipo da categoria: <c>RECEITA</c> ou <c>DESPESA</c>.</summary>
        [QueryParameter("tipo")]
        public string Tipo { get; set; }

        [QueryParameter("apenas_filhos")]
        public bool? ApenasFilhos { get; set; }

        [QueryParameter("nome")]
        public string Nome { get; set; }

        /// <summary>Permite apenas categorias filhas. Obrigatório pela API.</summary>
        [QueryParameter("permite_apenas_filhos")]
        public bool? PermiteApenasFilhos { get; set; }
    }
}
