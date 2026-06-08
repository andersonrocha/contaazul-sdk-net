using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Dados de negociação de uma venda (presente no detalhe da venda).
    /// </summary>
    public sealed class Negociacao
    {
        /// <summary>UUID da negociação.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Status da negociação (ex.: <c>EM_ANDAMENTO</c>, <c>CONTRATO</c>, <c>CANCELADO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>ID legado.</summary>
        [JsonPropertyName("id_legado")]
        public long? IdLegado { get; set; }

        /// <summary>Tipo de negociação (<c>VENDA</c> ou <c>COMPRA</c>).</summary>
        [JsonPropertyName("tipo_negociacao")]
        public string TipoNegociacao { get; set; }

        /// <summary>Número da venda.</summary>
        [JsonPropertyName("numero")]
        public int Numero { get; set; }

        /// <summary>ID da categoria.</summary>
        [JsonPropertyName("id_categoria")]
        public string IdCategoria { get; set; }

        /// <summary>Data de compromisso.</summary>
        [JsonPropertyName("data_compromisso")]
        public string DataCompromisso { get; set; }

        /// <summary>Configuração de desconto da negociação.</summary>
        [JsonPropertyName("configuracao_de_desconto")]
        public ConfiguracaoDesconto ConfiguracaoDeDesconto { get; set; }

        /// <summary>Composição de valores da negociação.</summary>
        [JsonPropertyName("composicao_valor")]
        public ComposicaoValor ComposicaoValor { get; set; }

        /// <summary>Condição de pagamento da negociação.</summary>
        [JsonPropertyName("condicao_pagamento")]
        public CondicaoPagamento CondicaoPagamento { get; set; }

        /// <summary>Totais de itens da negociação.</summary>
        [JsonPropertyName("total_itens")]
        public ItensTotaisNegociacao TotalItens { get; set; }

        /// <summary>Observações da venda.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>ID do cliente.</summary>
        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>Versão da venda.</summary>
        [JsonPropertyName("versao")]
        public int Versao { get; set; }

        /// <summary>Tipo de pendência (<c>nome</c> e <c>descricao</c>).</summary>
        [JsonPropertyName("tipo_pendencia")]
        public NomeDescricao TipoPendencia { get; set; }

        /// <summary>Situação da negociação.</summary>
        [JsonPropertyName("situacao")]
        public SituacaoNegociacao Situacao { get; set; }

        /// <summary>ID da natureza da operação.</summary>
        [JsonPropertyName("id_natureza_operacao")]
        public string IdNaturezaOperacao { get; set; }

        /// <summary>ID do centro de custo.</summary>
        [JsonPropertyName("id_centro_custo")]
        public string IdCentroCusto { get; set; }

        /// <summary>Introdução da venda em orçamento.</summary>
        [JsonPropertyName("introducao")]
        public string Introducao { get; set; }

        /// <summary>Origem da venda.</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }
    }
}
