using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>
    /// Filtros para consulta de notas fiscais de serviço (NFS-e) —
    /// <c>GET /v1/notas-fiscais-servico</c>.
    /// <para>
    /// <see cref="DataCompetenciaDe"/> e <see cref="DataCompetenciaAte"/> são obrigatórios
    /// (intervalo máximo de 15 dias).
    /// </para>
    /// </summary>
    public class NotaFiscalServicoFiltro : FiltroBase
    {
        /// <summary>Data de competência inicial no formato <c>YYYY-MM-DD</c>. Obrigatório.</summary>
        [QueryParameter("data_competencia_de")]
        public string DataCompetenciaDe { get; set; }

        /// <summary>Data de competência final no formato <c>YYYY-MM-DD</c>. Obrigatório.</summary>
        [QueryParameter("data_competencia_ate")]
        public string DataCompetenciaAte { get; set; }

        /// <summary>Lista de UUIDs das notas fiscais de serviço, separados por vírgula.</summary>
        [QueryParameter("ids")]
        public string Ids { get; set; }

        /// <summary>Lista de IDs de clientes (UUIDs), separados por vírgula.</summary>
        [QueryParameter("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>Número da venda.</summary>
        [QueryParameter("numero_venda")]
        public long? NumeroVenda { get; set; }

        /// <summary>Número inicial da NFS-e.</summary>
        [QueryParameter("numero_nfse_inicial")]
        public long? NumeroNfseInicial { get; set; }

        /// <summary>Número final da NFS-e.</summary>
        [QueryParameter("numero_nfse_final")]
        public long? NumeroNfseFinal { get; set; }

        /// <summary>Número inicial do RPS.</summary>
        [QueryParameter("numero_rps_inicial")]
        public long? NumeroRpsInicial { get; set; }

        /// <summary>Número final do RPS.</summary>
        [QueryParameter("numero_rps_final")]
        public long? NumeroRpsFinal { get; set; }

        /// <summary>
        /// Status da nota fiscal, separados por vírgula. Valores possíveis: <c>PENDENTE</c>,
        /// <c>PRONTA_ENVIO</c>, <c>AGUARDANDO_RETORNO</c>, <c>EM_ESPERA</c>, <c>EMITINDO</c>,
        /// <c>EMITIDA</c>, <c>CANCELADA</c>, <c>FALHA</c>, <c>FALHA_CANCELAMENTO</c>,
        /// <c>CORRIGIDA_SUCESSO</c>, <c>AGUARDANDO_CORRECAO</c>, <c>FALHA_CORRECAO</c>,
        /// <c>DENEGADA</c>, <c>CANCELAMENTO_MANUAL</c>.
        /// </summary>
        [QueryParameter("status")]
        public string Status { get; set; }

        /// <summary>Tipo de negociação. Valores possíveis: <c>VENDA</c>, <c>CONTRATO</c>.</summary>
        [QueryParameter("tipo_negociacao")]
        public string TipoNegociacao { get; set; }
    }
}
