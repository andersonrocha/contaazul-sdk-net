using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Representa uma venda retornada na listagem (<c>/v1/venda/busca</c>).
    /// </summary>
    public sealed class Venda
    {
        /// <summary>ID (UUID) da venda.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Valor total da venda.</summary>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        /// <summary>ID legado da venda.</summary>
        [JsonPropertyName("id_legado")]
        public long? IdLegado { get; set; }

        /// <summary>Data da venda.</summary>
        [JsonPropertyName("data")]
        public string Data { get; set; }

        /// <summary>Data de criação da venda.</summary>
        [JsonPropertyName("criado_em")]
        public string CriadoEm { get; set; }

        /// <summary>Data de alteração da venda (ISO 8601, São Paulo/GMT-3).</summary>
        [JsonPropertyName("data_alteracao")]
        public string DataAlteracao { get; set; }

        /// <summary>Tipo da venda.</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        /// <summary>Tipo dos itens da venda.</summary>
        [JsonPropertyName("itens")]
        public string TipoItens { get; set; }

        /// <summary>Indica se há condição de pagamento.</summary>
        [JsonPropertyName("condicao_pagamento")]
        public bool? CondicaoPagamento { get; set; }

        /// <summary>Número da venda.</summary>
        [JsonPropertyName("numero")]
        public int Numero { get; set; }

        /// <summary>Cliente da venda.</summary>
        [JsonPropertyName("cliente")]
        public Cliente Cliente { get; set; }

        /// <summary>Situação da venda (<c>nome</c> e <c>descricao</c>).</summary>
        [JsonPropertyName("situacao")]
        public NomeDescricao Situacao { get; set; }

        /// <summary>Versão da venda.</summary>
        [JsonPropertyName("versao")]
        public int Versao { get; set; }

        /// <summary>Status do envio de e-mail da venda.</summary>
        [JsonPropertyName("status_email")]
        public StatusEmail StatusEmail { get; set; }

        /// <summary>ID do contrato que originou a venda, quando aplicável.</summary>
        [JsonPropertyName("id_contrato")]
        public string IdContrato { get; set; }

        /// <summary>Origem da venda.</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }
    }
}
