using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Solicitação de cobrança vinculada a uma parcela.</summary>
    public sealed class SolicitacaoCobranca
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("status_solicitacao_cobranca")]
        public string StatusSolicitacaoCobranca { get; set; }

        [JsonPropertyName("valor_composicao")]
        public ComposicaoValorFinanceiro ValorComposicao { get; set; }

        [JsonPropertyName("data_vencimento")]
        public string DataVencimento { get; set; }

        [JsonPropertyName("data_quitacao")]
        public string DataQuitacao { get; set; }

        /// <summary>Tipo: <c>BOLETO</c>, <c>LINK_PAGAMENTO</c>, <c>BOLETO_REGISTRADO</c> ou <c>PIX_COBRANCA</c>.</summary>
        [JsonPropertyName("tipo_solicitacao_cobranca")]
        public string TipoSolicitacaoCobranca { get; set; }

        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        [JsonPropertyName("id_referencia")]
        public string IdReferencia { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("detalhe_erro")]
        public string DetalheErro { get; set; }

        [JsonPropertyName("conta_financeira")]
        public ContaFinanceira ContaFinanceira { get; set; }

        [JsonPropertyName("notificacao_cobranca")]
        public NotificacaoCobranca NotificacaoCobranca { get; set; }

        [JsonPropertyName("confirmado_em")]
        public string ConfirmadoEm { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("referencia_externa")]
        public string ReferenciaExterna { get; set; }

        [JsonPropertyName("recuperado")]
        public bool? Recuperado { get; set; }

        [JsonPropertyName("combinado")]
        public bool? Combinado { get; set; }

        [JsonPropertyName("atributos_personalizados")]
        public string AtributosPersonalizados { get; set; }
    }
}
