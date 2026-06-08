using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>
    /// Nota fiscal de serviço (NFS-e) emitida no ERP.
    /// Retornada por <c>GET /v1/notas-fiscais-servico</c>.
    /// </summary>
    public sealed class NotaFiscalServico
    {
        /// <summary>UUID da nota fiscal de serviço.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>UUID da venda.</summary>
        [JsonPropertyName("id_venda")]
        public string IdVenda { get; set; }

        /// <summary>UUID do contrato.</summary>
        [JsonPropertyName("id_contrato")]
        public string IdContrato { get; set; }

        /// <summary>Número da venda.</summary>
        [JsonPropertyName("numero_venda")]
        public string NumeroVenda { get; set; }

        /// <summary>Número do RPS.</summary>
        [JsonPropertyName("numero_rps")]
        public int? NumeroRps { get; set; }

        /// <summary>Número da NFS-e.</summary>
        [JsonPropertyName("numero_nfse")]
        public int? NumeroNfse { get; set; }

        /// <summary>
        /// Status da nota fiscal. Valores possíveis: <c>PENDENTE</c>, <c>PRONTA_ENVIO</c>,
        /// <c>AGUARDANDO_RETORNO</c>, <c>EM_ESPERA</c>, <c>EMITINDO</c>, <c>EMITIDA</c>,
        /// <c>CANCELADA</c>, <c>FALHA</c>, <c>FALHA_CANCELAMENTO</c>, <c>CORRIGIDA_SUCESSO</c>,
        /// <c>AGUARDANDO_CORRECAO</c>, <c>FALHA_CORRECAO</c>, <c>DENEGADA</c>, <c>CANCELAMENTO_MANUAL</c>.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Valor total da NFS-e.</summary>
        [JsonPropertyName("valor_total_nfse")]
        public decimal? ValorTotalNfse { get; set; }

        /// <summary>Data de competência (formato <c>YYYY-MM-DD</c>).</summary>
        [JsonPropertyName("data_competencia")]
        public string DataCompetencia { get; set; }

        /// <summary>Nome do cliente.</summary>
        [JsonPropertyName("nome_cliente")]
        public string NomeCliente { get; set; }

        /// <summary>Documento do cliente.</summary>
        [JsonPropertyName("documento_cliente")]
        public string DocumentoCliente { get; set; }

        /// <summary>Código CNAE.</summary>
        [JsonPropertyName("codigo_cnae")]
        public string CodigoCnae { get; set; }

        /// <summary>Indica se foi escriturado manualmente.</summary>
        [JsonPropertyName("escriturado_manualmente")]
        public bool? EscrituradoManualmente { get; set; }

        /// <summary>Cidade de emissão.</summary>
        [JsonPropertyName("cidade_emissao")]
        public CidadeEmissaoNotaFiscalServico CidadeEmissao { get; set; }

        /// <summary>Informações de transmissão.</summary>
        [JsonPropertyName("informacao_transmissao")]
        public InformacaoTransmissaoNotaFiscalServico InformacaoTransmissao { get; set; }

        /// <summary>Informações de cancelamento.</summary>
        [JsonPropertyName("informacoes_cancelamento")]
        public InformacoesCancelamentoNotaFiscalServico InformacoesCancelamento { get; set; }
    }
}
