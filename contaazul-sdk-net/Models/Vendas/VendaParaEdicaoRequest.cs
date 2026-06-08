using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Modelo para a edição de uma venda (<c>PUT /v1/venda/{id}</c>).
    /// </summary>
    public sealed class VendaParaEdicaoRequest
    {
        /// <summary>ID (UUID) do cliente. <b>Obrigatório.</b></summary>
        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>Número da venda. <b>Obrigatório.</b></summary>
        [JsonPropertyName("numero")]
        public long Numero { get; set; }

        /// <summary>Data da venda (<c>YYYY-MM-DD</c>). <b>Obrigatório.</b></summary>
        [JsonPropertyName("data_venda")]
        public string DataVenda { get; set; }

        /// <summary>
        /// Situação da venda: <c>EM_ANDAMENTO</c>, <c>APROVADO</c>, <c>FATURADO</c> ou <c>CANCELADO</c>.
        /// <b>Obrigatório.</b>
        /// </summary>
        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        /// <summary>Observações sobre a venda. Opcional.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>Observações sobre o pagamento. Opcional.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

        /// <summary>ID (UUID) da natureza da operação. Opcional.</summary>
        [JsonPropertyName("id_natureza_operacao")]
        public string IdNaturezaOperacao { get; set; }

        /// <summary>Versão da venda. <b>Obrigatório na edição.</b></summary>
        [JsonPropertyName("versao")]
        public int Versao { get; set; }

        /// <summary>Itens da venda. <b>Obrigatório.</b></summary>
        [JsonPropertyName("itens")]
        public List<ItemVendaRequest> Itens { get; set; }

        /// <summary>Composição de valores (frete e desconto). Opcional.</summary>
        [JsonPropertyName("composicao_de_valor")]
        public ValorComposicaoRequest ComposicaoDeValor { get; set; }

        /// <summary>Condição de pagamento. <b>Obrigatório.</b></summary>
        [JsonPropertyName("condicao_pagamento")]
        public CondicaoPagamentoRequest CondicaoPagamento { get; set; }
    }
}
