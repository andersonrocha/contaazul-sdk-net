using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Filtros para a busca de contas a receber e a pagar
    /// (<c>GET .../contas-a-receber/buscar</c> e <c>GET .../contas-a-pagar/buscar</c>).
    /// <para><see cref="DataVencimentoDe"/> e <see cref="DataVencimentoAte"/> são obrigatórios.</para>
    /// </summary>
    public class MovimentacaoFinanceiraFiltro : FiltroBase
    {
        [QueryParameter("campo_ordenado_ascendente")]
        public string CampoOrdenadoAscendente { get; set; }

        [QueryParameter("campo_ordenado_descendente")]
        public string CampoOrdenadoDescendente { get; set; }

        [QueryParameter("descricao")]
        public string Descricao { get; set; }

        /// <summary>Data de vencimento de (<c>YYYY-MM-DD</c>). Obrigatório.</summary>
        [QueryParameter("data_vencimento_de")]
        public string DataVencimentoDe { get; set; }

        /// <summary>Data de vencimento até (<c>YYYY-MM-DD</c>). Obrigatório.</summary>
        [QueryParameter("data_vencimento_ate")]
        public string DataVencimentoAte { get; set; }

        [QueryParameter("data_competencia_de")]
        public string DataCompetenciaDe { get; set; }

        [QueryParameter("data_competencia_ate")]
        public string DataCompetenciaAte { get; set; }

        [QueryParameter("data_pagamento_de")]
        public string DataPagamentoDe { get; set; }

        [QueryParameter("data_pagamento_ate")]
        public string DataPagamentoAte { get; set; }

        [QueryParameter("data_alteracao_de")]
        public string DataAlteracaoDe { get; set; }

        [QueryParameter("data_alteracao_ate")]
        public string DataAlteracaoAte { get; set; }

        [QueryParameter("valor_de")]
        public string ValorDe { get; set; }

        [QueryParameter("valor_ate")]
        public string ValorAte { get; set; }

        /// <summary>
        /// Status, separados por vírgula (<c>PERDIDO</c>, <c>RECEBIDO</c>, <c>EM_ABERTO</c>,
        /// <c>RENEGOCIADO</c>, <c>RECEBIDO_PARCIAL</c>, <c>ATRASADO</c>).
        /// </summary>
        [QueryParameter("status")]
        public string Status { get; set; }

        [QueryParameter("ids_contas_financeiras")]
        public string IdsContasFinanceiras { get; set; }

        [QueryParameter("ids_categorias")]
        public string IdsCategorias { get; set; }

        [QueryParameter("ids_centros_de_custo")]
        public string IdsCentrosDeCusto { get; set; }

        [QueryParameter("ids_clientes")]
        public string IdsClientes { get; set; }
    }
}
