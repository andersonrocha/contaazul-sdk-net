using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Filtros para a busca de transferências (<c>GET /v1/financeiro/transferencias</c>).</summary>
    public class TransferenciaFiltro : FiltroBase
    {
        /// <summary>IDs das contas financeiras (origem ou destino), separados por vírgula.</summary>
        [QueryParameter("ids_conta_financeira")]
        public string IdsContaFinanceira { get; set; }

        /// <summary>Data inicial do período (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>Data final do período (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_fim")]
        public string DataFim { get; set; }
    }
}
