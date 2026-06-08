using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Condição de pagamento na criação/edição de venda.
    /// </summary>
    public sealed class CondicaoPagamentoRequest
    {
        /// <summary>Forma de pagamento (ex.: <c>CARTAO_CREDITO</c>, <c>BOLETO_BANCARIO</c>). Opcional.</summary>
        [JsonPropertyName("tipo_pagamento")]
        public string TipoPagamento { get; set; }

        /// <summary>ID (UUID) da conta financeira. Opcional.</summary>
        [JsonPropertyName("id_conta_financeira")]
        public string IdContaFinanceira { get; set; }

        /// <summary>
        /// Opção de condição de pagamento. <b>Obrigatório.</b> Deve estar em um dos formatos:
        /// "À vista"; "30, 60, 90"; ou "3x".
        /// </summary>
        [JsonPropertyName("opcao_condicao_pagamento")]
        public string OpcaoCondicaoPagamento { get; set; }

        /// <summary>NSU da transação. Opcional.</summary>
        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }

        /// <summary>Parcelas do pagamento. <b>Obrigatório.</b></summary>
        [JsonPropertyName("parcelas")]
        public List<ParcelaRequest> Parcelas { get; set; }
    }
}
