using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Modelo para a criação de uma venda (<c>POST /v1/venda</c>).
    /// </summary>
    public sealed class CriacaoVendaRequest
    {
        /// <summary>ID (UUID) do cliente. <b>Obrigatório.</b></summary>
        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>Número da venda. <b>Obrigatório.</b></summary>
        [JsonPropertyName("numero")]
        public long Numero { get; set; }

        /// <summary>Situação da venda: <c>EM_ANDAMENTO</c> ou <c>APROVADO</c>. <b>Obrigatório.</b></summary>
        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        /// <summary>Data da venda (<c>YYYY-MM-DD</c>). <b>Obrigatório.</b></summary>
        [JsonPropertyName("data_venda")]
        public string DataVenda { get; set; }

        /// <summary>ID da categoria. Opcional.</summary>
        [JsonPropertyName("id_categoria")]
        public string IdCategoria { get; set; }

        /// <summary>ID do centro de custo. Opcional.</summary>
        [JsonPropertyName("id_centro_custo")]
        public string IdCentroCusto { get; set; }

        /// <summary>ID do vendedor. Opcional.</summary>
        [JsonPropertyName("id_vendedor")]
        public string IdVendedor { get; set; }

        /// <summary>Observações sobre a venda. Opcional.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>Observações sobre o pagamento. Opcional.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

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
