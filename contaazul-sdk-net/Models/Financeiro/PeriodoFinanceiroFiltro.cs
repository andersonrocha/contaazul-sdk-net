using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Filtro por período (<c>data_inicio</c>/<c>data_fim</c>) usado pelos endpoints de
    /// eventos alterados e saldos iniciais. As datas são ISO 8601 (São Paulo/GMT-3).
    /// </summary>
    public class PeriodoFinanceiroFiltro : FiltroBase
    {
        /// <summary>Data inicial do período. Obrigatório.</summary>
        [QueryParameter("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>Data final do período. Obrigatório.</summary>
        [QueryParameter("data_fim")]
        public string DataFim { get; set; }
    }
}
