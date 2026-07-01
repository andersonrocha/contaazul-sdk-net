using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Filtro de paginação com busca textual (<c>busca_textual</c>), usado pelas listagens
    /// auxiliares de produtos: categorias, CEST, NCM e unidades de medida.
    /// </summary>
    public class BuscaTextualFiltro : FiltroBase
    {
        /// <summary>Busca textual pela descrição (ou código, quando aplicável).</summary>
        [QueryParameter("busca_textual")]
        public string BuscaTextual { get; set; }
    }
}
