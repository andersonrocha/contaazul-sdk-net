using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Filtros para a busca de contas financeiras (<c>GET /v1/conta-financeira</c>).</summary>
    public class ContaFinanceiraFiltro : FiltroBase
    {
        /// <summary>Lista de tipos de conta, separados por vírgula.</summary>
        [QueryParameter("tipos")]
        public string Tipos { get; set; }

        [QueryParameter("nome")]
        public string Nome { get; set; }

        [QueryParameter("apenas_ativo")]
        public bool? ApenasAtivo { get; set; }

        [QueryParameter("esconde_conta_digital")]
        public bool? EscondeContaDigital { get; set; }

        [QueryParameter("mostrar_caixinha")]
        public bool? MostrarCaixinha { get; set; }
    }
}
