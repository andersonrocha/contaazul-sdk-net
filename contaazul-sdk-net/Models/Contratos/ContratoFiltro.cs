using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Filtros para a busca de contratos (vendas agendadas/recorrentes).
    /// <para>
    /// Parâmetros de múltiplos valores (<see cref="ClienteId"/>, <see cref="TipoPagamento"/>)
    /// devem ser informados como valores separados por vírgula
    /// (ex.: <c>"uuid1,uuid2"</c>).
    /// </para>
    /// </summary>
    public class ContratoFiltro : FiltroBase
    {
        /// <summary>
        /// Campo para ordenação ascendente. Aceita <c>DATA_INICIO</c> ou <c>DATA_FIM</c>.
        /// Quando informado, desconsidera <see cref="CampoOrdenadoDescendente"/>.
        /// </summary>
        [QueryParameter("campo_ordenado_ascendente")]
        public string CampoOrdenadoAscendente { get; set; }

        /// <summary>
        /// Campo para ordenação descendente. Aceita <c>DATA_INICIO</c> ou <c>DATA_FIM</c>.
        /// Não deve ser informado em conjunto com <see cref="CampoOrdenadoAscendente"/>.
        /// </summary>
        [QueryParameter("campo_ordenado_descendente")]
        public string CampoOrdenadoDescendente { get; set; }

        /// <summary>Busca textual por nome do contrato.</summary>
        [QueryParameter("busca_textual")]
        public string BuscaTextual { get; set; }

        /// <summary>
        /// IDs dos clientes (UUID). Vários valores separados por vírgula.
        /// </summary>
        [QueryParameter("cliente_id")]
        public string ClienteId { get; set; }

        /// <summary>
        /// Data de início do intervalo de busca, no formato <c>YYYY-MM-DD</c>. <b>Obrigatório.</b>
        /// </summary>
        [QueryParameter("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>
        /// Data de fim do intervalo de busca, no formato <c>YYYY-MM-DD</c>. <b>Obrigatório.</b>
        /// </summary>
        [QueryParameter("data_fim")]
        public string DataFim { get; set; }

        /// <summary>
        /// Tipos de pagamento (ex.: <c>BOLETO_BANCARIO</c>, <c>CARTAO_CREDITO</c>).
        /// Vários valores separados por vírgula.
        /// </summary>
        [QueryParameter("tipo_pagamento")]
        public string TipoPagamento { get; set; }

        /// <summary>
        /// Status dos contratos. Aceita <c>TODOS</c>, <c>ATIVO</c>, <c>INATIVO</c>
        /// ou <c>PROXIMO_AO_VENCIMENTO</c>.
        /// </summary>
        [QueryParameter("status")]
        public string Status { get; set; }
    }
}
