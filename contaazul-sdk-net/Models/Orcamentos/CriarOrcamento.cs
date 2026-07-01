using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>
    /// Dados para criar um novo orçamento (<c>POST /v1/orcamentos</c>).
    /// <see cref="DataOrcamento"/>, <see cref="DataValidade"/>, <see cref="IdCliente"/> e
    /// <see cref="Itens"/> (ao menos um) são obrigatórios.
    /// </summary>
    public sealed class CriarOrcamento
    {
        /// <summary>Composição dos valores do orçamento (frete e desconto).</summary>
        [JsonPropertyName("composicao_de_valor")]
        public ComposicaoValorOrcamento ComposicaoDeValor { get; set; }

        /// <summary>Data do orçamento (<c>YYYY-MM-DD</c>). Obrigatório.</summary>
        [JsonPropertyName("data_orcamento")]
        public string DataOrcamento { get; set; }

        /// <summary>
        /// Data de validade (<c>YYYY-MM-DD</c>); não pode ser anterior à data do orçamento. Obrigatório.
        /// </summary>
        [JsonPropertyName("data_validade")]
        public string DataValidade { get; set; }

        /// <summary>Descrição do orçamento.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID do cliente. Obrigatório.</summary>
        [JsonPropertyName("id_cliente")]
        public string IdCliente { get; set; }

        /// <summary>
        /// ID do vendedor; se não informado ou inexistente, será utilizado o vendedor padrão.
        /// </summary>
        [JsonPropertyName("id_vendedor")]
        public string IdVendedor { get; set; }

        /// <summary>Lista de itens do orçamento; deve conter ao menos um item. Obrigatório.</summary>
        [JsonPropertyName("itens")]
        public List<CriarItemOrcamento> Itens { get; set; }

        /// <summary>Observações gerais.</summary>
        [JsonPropertyName("observacoes")]
        public string Observacoes { get; set; }

        /// <summary>Observações sobre o pagamento.</summary>
        [JsonPropertyName("observacoes_pagamento")]
        public string ObservacoesPagamento { get; set; }

        /// <summary>Previsão de entrega.</summary>
        [JsonPropertyName("previsao_entrega")]
        public string PrevisaoEntrega { get; set; }
    }
}
