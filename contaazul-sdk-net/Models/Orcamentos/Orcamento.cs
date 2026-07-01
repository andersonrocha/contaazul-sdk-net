using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>
    /// Detalhe completo de um orçamento (<c>GET /v1/orcamentos/{id}</c>).
    /// </summary>
    public sealed class Orcamento
    {
        /// <summary>Composição de valor do orçamento (frete e desconto).</summary>
        [JsonPropertyName("composicao_de_valor")]
        public ComposicaoValorOrcamento ComposicaoDeValor { get; set; }

        /// <summary>Data do orçamento (<c>YYYY-MM-DD</c>).</summary>
        [JsonPropertyName("data_orcamento")]
        public string DataOrcamento { get; set; }

        /// <summary>Data de validade do orçamento (<c>YYYY-MM-DD</c>).</summary>
        [JsonPropertyName("data_validade")]
        public string DataValidade { get; set; }

        /// <summary>Descrição do orçamento.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID do orçamento.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID do cliente do orçamento.</summary>
        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>ID do vendedor responsável pelo orçamento.</summary>
        [JsonPropertyName("id_vendedor")]
        public string IdVendedor { get; set; }

        /// <summary>Itens do orçamento.</summary>
        [JsonPropertyName("itens")]
        public List<ItemOrcamento> Itens { get; set; }

        /// <summary>Número do orçamento.</summary>
        [JsonPropertyName("numero")]
        public int? Numero { get; set; }

        /// <summary>Observações gerais do orçamento.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>Observações de pagamento do orçamento.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

        /// <summary>Previsão de entrega do orçamento.</summary>
        [JsonPropertyName("previsao_entrega")]
        public string PrevisaoEntrega { get; set; }

        /// <summary>
        /// Situação atual do orçamento (<c>ORCAMENTO</c>, <c>ORCAMENTO_ACEITO</c> ou <c>ORCAMENTO_RECUSADO</c>).
        /// </summary>
        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        /// <summary>Versão do orçamento.</summary>
        [JsonPropertyName("versao")]
        public int? Versao { get; set; }
    }
}
