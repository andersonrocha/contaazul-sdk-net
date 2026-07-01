using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>Item da listagem de orçamentos (<c>GET /v1/orcamentos</c>).</summary>
    public sealed class ItemOrcamentoPorFiltro
    {
        /// <summary>Cliente do orçamento.</summary>
        [JsonPropertyName("cliente")]
        public ClienteOrcamento Cliente { get; set; }

        /// <summary>Data de alteração do orçamento (ISO 8601, São Paulo/GMT-3).</summary>
        [JsonPropertyName("data_alteracao")]
        public string DataAlteracao { get; set; }

        /// <summary>Data de criação do orçamento.</summary>
        [JsonPropertyName("data_criacao")]
        public string DataCriacao { get; set; }

        /// <summary>Data do orçamento.</summary>
        [JsonPropertyName("data_orcamento")]
        public string DataOrcamento { get; set; }

        /// <summary>ID do orçamento.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID do contrato associado, quando houver.</summary>
        [JsonPropertyName("id_contrato")]
        public string IdContrato { get; set; }

        /// <summary>
        /// Categoria dos itens do orçamento (<c>PRODUTO</c>, <c>SERVICO</c> ou <c>PRODUTO_E_SERVICO</c>).
        /// </summary>
        [JsonPropertyName("itens")]
        public string Itens { get; set; }

        /// <summary>Número do orçamento.</summary>
        [JsonPropertyName("numero")]
        public int? Numero { get; set; }

        /// <summary>Origem do orçamento.</summary>
        [JsonPropertyName("origem")]
        public string Origem { get; set; }

        /// <summary>
        /// Situação do orçamento (<c>ORCAMENTO</c>, <c>ORCAMENTO_ACEITO</c> ou <c>ORCAMENTO_RECUSADO</c>).
        /// </summary>
        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        /// <summary>Total do orçamento.</summary>
        [JsonPropertyName("total")]
        public decimal? Total { get; set; }

        /// <summary>Versão do orçamento.</summary>
        [JsonPropertyName("versao")]
        public int? Versao { get; set; }
    }
}
