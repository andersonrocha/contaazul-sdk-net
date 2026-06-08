using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Modelo para criação de um contrato (venda agendada/recorrente).
    /// </summary>
    public sealed class CriarContrato
    {
        /// <summary>
        /// Composição dos valores da venda, incluindo frete e desconto. Opcional.
        /// </summary>
        [JsonPropertyName("composicao_de_valor")]
        public CriarComposicaoValorContrato ComposicaoDeValor { get; set; }

        /// <summary>Condição de pagamento do contrato. <b>Obrigatório.</b></summary>
        [JsonPropertyName("condicao_pagamento")]
        public CriarCondicaoPagamentoContrato CondicaoPagamento { get; set; }

        /// <summary>ID da categoria. Opcional.</summary>
        [JsonPropertyName("id_categoria")]
        public string IdCategoria { get; set; }

        /// <summary>ID do centro de custo. Opcional.</summary>
        [JsonPropertyName("id_centro_custo")]
        public string IdCentroCusto { get; set; }

        /// <summary>ID do cliente associado ao contrato. <b>Obrigatório.</b></summary>
        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>ID do vendedor responsável pelo contrato. Opcional.</summary>
        [JsonPropertyName("id_vendedor")]
        public string IdVendedor { get; set; }

        /// <summary>Lista de itens do contrato (mínimo 1). <b>Obrigatório.</b></summary>
        [JsonPropertyName("itens")]
        public List<CriarItemVendaContrato> Itens { get; set; }

        /// <summary>Observações gerais sobre o contrato. Opcional.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>Observações específicas para a emissão da nota fiscal. Opcional.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

        /// <summary>Termos de recorrência da venda agendada. <b>Obrigatório.</b></summary>
        [JsonPropertyName("termos")]
        public CriarTermosContrato Termos { get; set; }
    }
}
