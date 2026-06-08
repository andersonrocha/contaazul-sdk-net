using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Modelo detalhado de um contrato de venda recorrente.
    /// </summary>
    public sealed class ContratoResumo
    {
        /// <summary>Dados do cliente vinculado ao contrato.</summary>
        [JsonPropertyName("cliente")]
        public ReferenciaNomeada Cliente { get; set; }

        /// <summary>Composição de valores do contrato.</summary>
        [JsonPropertyName("composicao_valor")]
        public ComposicaoValorResumo ComposicaoValor { get; set; }

        /// <summary>Condições de pagamento do contrato.</summary>
        [JsonPropertyName("condicao_pagamento")]
        public CondicaoPagamentoResumo CondicaoPagamento { get; set; }

        /// <summary>Configuração de recorrência do contrato.</summary>
        [JsonPropertyName("configuracao_recorrencia")]
        public ConfiguracaoRecorrenciaResumo ConfiguracaoRecorrencia { get; set; }

        /// <summary>Data da próxima emissão.</summary>
        [JsonPropertyName("data_proxima_emissao")]
        public string DataProximaEmissao { get; set; }

        /// <summary>Data do próximo vencimento.</summary>
        [JsonPropertyName("data_proximo_vencimento")]
        public string DataProximoVencimento { get; set; }

        /// <summary>Data da última emissão.</summary>
        [JsonPropertyName("data_ultima_emissao")]
        public string DataUltimaEmissao { get; set; }

        /// <summary>ID do contrato.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID da próxima venda agendada.</summary>
        [JsonPropertyName("id_proxima_venda_agendada")]
        public string IdProximaVendaAgendada { get; set; }

        /// <summary>ID da última venda confirmada.</summary>
        [JsonPropertyName("id_ultima_venda_confirmada")]
        public string IdUltimaVendaConfirmada { get; set; }

        /// <summary>Local de prestação de serviço.</summary>
        [JsonPropertyName("local_prestacao_servico")]
        public LocalPrestacaoServicoResumo LocalPrestacaoServico { get; set; }

        /// <summary>Observações adicionais sobre o contrato.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>Status do contrato (ex.: <c>ATIVO</c>, <c>INATIVO</c>, <c>DELETADO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Termos do contrato.</summary>
        [JsonPropertyName("termos")]
        public TermosContrato Termos { get; set; }

        /// <summary>Vendedor responsável pelo contrato.</summary>
        [JsonPropertyName("vendedor")]
        public ReferenciaNomeada Vendedor { get; set; }
    }
}
