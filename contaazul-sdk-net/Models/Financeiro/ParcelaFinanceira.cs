using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Parcela de um evento financeiro (contas a pagar/receber). Retornada por
    /// <c>GET /v1/financeiro/eventos-financeiros/{id_evento}/parcelas</c> e
    /// <c>GET /v1/financeiro/eventos-financeiros/parcelas/{id}</c>.
    /// </summary>
    public sealed class ParcelaFinanceira
    {
        [JsonPropertyName("evento")]
        public Evento Evento { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("referencia")]
        public string Referencia { get; set; }

        [JsonPropertyName("indice")]
        public int? Indice { get; set; }

        [JsonPropertyName("conciliado")]
        public bool? Conciliado { get; set; }

        /// <summary>
        /// Status (<c>PENDENTE</c>=<c>EM_ABERTO</c>, <c>QUITADO</c>=<c>RECEBIDO</c>, <c>CANCELADO</c>,
        /// <c>RENEGOCIADO</c>, <c>RECEBIDO_PARCIAL</c>, <c>ATRASADO</c>, <c>PERDIDO</c>).
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("valor_pago")]
        public decimal? ValorPago { get; set; }

        [JsonPropertyName("perda")]
        public PerdaFinanceira Perda { get; set; }

        [JsonPropertyName("nao_pago")]
        public decimal? NaoPago { get; set; }

        [JsonPropertyName("data_vencimento")]
        public string DataVencimento { get; set; }

        [JsonPropertyName("data_pagamento_previsto")]
        public string DataPagamentoPrevisto { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("nota")]
        public string Nota { get; set; }

        [JsonPropertyName("conta_financeira")]
        public ContaFinanceira ContaFinanceira { get; set; }

        [JsonPropertyName("id_conta_financeira")]
        public string IdContaFinanceira { get; set; }

        [JsonPropertyName("valor_composicao")]
        public ComposicaoValorFinanceiro ValorComposicao { get; set; }

        [JsonPropertyName("metodo_pagamento")]
        public string MetodoPagamento { get; set; }

        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }

        [JsonPropertyName("baixa_agendada")]
        public bool? BaixaAgendada { get; set; }

        [JsonPropertyName("baixas")]
        public List<BaixaFinanceira> Baixas { get; set; }

        [JsonPropertyName("anexos")]
        public List<AnexoParcela> Anexos { get; set; }

        [JsonPropertyName("solicitacoes_cobrancas")]
        public List<SolicitacaoCobranca> SolicitacoesCobrancas { get; set; }

        [JsonPropertyName("id_ultima_solicitacao_pagamento")]
        public string IdUltimaSolicitacaoPagamento { get; set; }

        [JsonPropertyName("id_boleto_bancario_autorizado")]
        public string IdBoletoBancarioAutorizado { get; set; }

        [JsonPropertyName("fatura")]
        public Fatura Fatura { get; set; }

        [JsonPropertyName("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }

        [JsonPropertyName("valor_total_liquido")]
        public decimal? ValorTotalLiquido { get; set; }

        [JsonPropertyName("id_ultimo_solicitacao_cobranca")]
        public string IdUltimoSolicitacaoCobranca { get; set; }

        [JsonPropertyName("renegociacao")]
        public Renegociacao Renegociacao { get; set; }
    }
}
