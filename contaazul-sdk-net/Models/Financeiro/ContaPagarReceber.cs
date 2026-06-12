using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>
    /// Parcela de conta a pagar ou a receber (modelo unificado). <see cref="Cliente"/> é preenchido
    /// nas contas a receber e <see cref="Fornecedor"/> nas contas a pagar.
    /// </summary>
    public sealed class ContaPagarReceber
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("data_vencimento")]
        public string DataVencimento { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Status traduzido (<c>PERDIDO</c>, <c>RECEBIDO</c>, <c>EM_ABERTO</c>, <c>RENEGOCIADO</c>,
        /// <c>RECEBIDO_PARCIAL</c>, <c>ATRASADO</c>).
        /// </summary>
        [JsonPropertyName("status_traduzido")]
        public string StatusTraduzido { get; set; }

        [JsonPropertyName("total")]
        public decimal? Total { get; set; }

        [JsonPropertyName("nao_pago")]
        public decimal? NaoPago { get; set; }

        [JsonPropertyName("pago")]
        public decimal? Pago { get; set; }

        [JsonPropertyName("data_criacao")]
        public DateTime? DataCriacao { get; set; }

        [JsonPropertyName("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }

        [JsonPropertyName("data_competencia")]
        public string DataCompetencia { get; set; }

        [JsonPropertyName("categorias")]
        public List<ReferenciaNomeada> Categorias { get; set; }

        [JsonPropertyName("centros_custo")]
        public List<ReferenciaNomeada> CentrosCusto { get; set; }

        /// <summary>Cliente (contas a receber).</summary>
        [JsonPropertyName("cliente")]
        public ReferenciaNomeada Cliente { get; set; }

        /// <summary>Fornecedor (contas a pagar).</summary>
        [JsonPropertyName("fornecedor")]
        public ReferenciaNomeada Fornecedor { get; set; }

        [JsonPropertyName("renegociacao")]
        public Renegociacao Renegociacao { get; set; }
    }
}
