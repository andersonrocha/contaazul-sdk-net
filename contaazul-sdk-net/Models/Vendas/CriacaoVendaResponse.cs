using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Resposta da criação de uma venda.
    /// </summary>
    public sealed class CriacaoVendaResponse
    {
        /// <summary>ID (UUID) da venda criada.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID legado da venda.</summary>
        [JsonPropertyName("id_legado")]
        public long? IdLegado { get; set; }

        /// <summary>ID do cliente associado à venda.</summary>
        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>Número da venda.</summary>
        [JsonPropertyName("numero")]
        public long? Numero { get; set; }

        /// <summary>Origem da venda.</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }

        /// <summary>ID da categoria da venda.</summary>
        [JsonPropertyName("id_categoria")]
        public string IdCategoria { get; set; }

        /// <summary>Data da venda.</summary>
        [JsonPropertyName("data_venda")]
        public string DataVenda { get; set; }

        /// <summary>Situação da venda (<c>nome</c> e <c>descricao</c>).</summary>
        [JsonPropertyName("situacao")]
        public NomeDescricao Situacao { get; set; }

        /// <summary>Pendência da venda (<c>nome</c> e <c>descricao</c>).</summary>
        [JsonPropertyName("pendencia")]
        public NomeDescricao Pendencia { get; set; }

        /// <summary>Composição de valores da venda.</summary>
        [JsonPropertyName("valor_composicao")]
        public ComposicaoValorResponse ValorComposicao { get; set; }

        /// <summary>Condição de pagamento da venda.</summary>
        [JsonPropertyName("condicao_pagamento")]
        public CondicaoPagamentoResponse CondicaoPagamento { get; set; }

        /// <summary>Observações sobre a venda.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>ID do vendedor responsável pela venda.</summary>
        [JsonPropertyName("id_vendedor")]
        public string IdVendedor { get; set; }

        /// <summary>Versão da venda.</summary>
        [JsonPropertyName("versao")]
        public long? Versao { get; set; }
    }
}
