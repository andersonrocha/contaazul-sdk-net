using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Condição de pagamento retornada na criação de uma venda.
    /// </summary>
    public sealed class CondicaoPagamentoResponse
    {
        /// <summary>ID legado da condição de pagamento.</summary>
        [JsonPropertyName("id_legado")]
        public long? IdLegado { get; set; }

        /// <summary>Forma de pagamento (ex.: <c>CARTAO_CREDITO</c>).</summary>
        [JsonPropertyName("tipo_pagamento")]
        public string TipoPagamento { get; set; }

        /// <summary>ID (UUID) da conta financeira.</summary>
        [JsonPropertyName("id_conta_financeira")]
        public string IdContaFinanceira { get; set; }

        /// <summary>Opção de condição de pagamento.</summary>
        [JsonPropertyName("opcao_condicao_pagamento")]
        public string OpcaoCondicaoPagamento { get; set; }

        /// <summary>Parcelas do pagamento.</summary>
        [JsonPropertyName("parcelas")]
        public List<Parcela> Parcelas { get; set; }

        /// <summary>Observações sobre o pagamento.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

        /// <summary>NSU da transação.</summary>
        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }

        /// <summary>Troco total.</summary>
        [JsonPropertyName("troco_total")]
        public decimal? TrocoTotal { get; set; }
    }
}
