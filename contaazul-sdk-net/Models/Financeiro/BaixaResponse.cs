using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Detalhe de uma baixa retornado por
    /// <c>GET .../parcelas/{parcela_id}/baixa</c> e <c>GET .../parcelas/baixa/{baixa_id}</c>.
    /// Aqui a conta financeira é retornada como UUID (string).
    /// </summary>
    public sealed class BaixaResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("data_pagamento")]
        public string DataPagamento { get; set; }

        [JsonPropertyName("valor_composicao")]
        public ComposicaoValorFinanceiro ValorComposicao { get; set; }

        [JsonPropertyName("conta_financeira")]
        public string ContaFinanceira { get; set; }

        [JsonPropertyName("id_reconciliacao")]
        public string IdReconciliacao { get; set; }

        [JsonPropertyName("id_parcela")]
        public string IdParcela { get; set; }

        [JsonPropertyName("id_solicitacao_cobranca")]
        public string IdSolicitacaoCobranca { get; set; }

        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        [JsonPropertyName("metodo_pagamento")]
        public string MetodoPagamento { get; set; }

        /// <summary>Origem (ex.: <c>LANCAMENTO_FINANCEIRO</c>, <c>VENDA</c>, <c>SALDO_CONTA_BANCARIA</c>...).</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }

        [JsonPropertyName("id_recibo_digital")]
        public string IdReciboDigital { get; set; }

        /// <summary>Tipo de evento financeiro: <c>RECEITA</c> ou <c>DESPESA</c>.</summary>
        [JsonPropertyName("tipo_evento_financeiro")]
        public string TipoEventoFinanceiro { get; set; }

        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }

        [JsonPropertyName("id_referencia")]
        public string IdReferencia { get; set; }

        [JsonPropertyName("atualizado_em")]
        public DateTime? AtualizadoEm { get; set; }

        [JsonPropertyName("anexos")]
        public List<AnexoBaixa> Anexos { get; set; }
    }
}
