using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Condição de pagamento de uma negociação (resposta).
    /// </summary>
    public sealed class CondicaoPagamento
    {
        /// <summary>Forma de pagamento (ex.: <c>CARTAO_CREDITO</c>, <c>BOLETO_BANCARIO</c>).</summary>
        [JsonPropertyName("tipo_pagamento")]
        public string TipoPagamento { get; set; }

        /// <summary>ID da conta financeira.</summary>
        [JsonPropertyName("id_conta_financeira")]
        public string IdContaFinanceira { get; set; }

        /// <summary>Indica pagamento à vista.</summary>
        [JsonPropertyName("pagamento_a_vista")]
        public bool? PagamentoAVista { get; set; }

        /// <summary>Parcelas do pagamento.</summary>
        [JsonPropertyName("parcelas")]
        public List<Parcela> Parcelas { get; set; }

        /// <summary>Observações sobre o pagamento.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

        /// <summary>Opção de condição de pagamento.</summary>
        [JsonPropertyName("opcao_condicao_pagamento")]
        public string OpcaoCondicaoPagamento { get; set; }

        /// <summary>NSU da transação.</summary>
        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }

        /// <summary>Dados do pagamento com cartão.</summary>
        [JsonPropertyName("pagamento_cartao")]
        public PagamentoCartao PagamentoCartao { get; set; }
    }
}
