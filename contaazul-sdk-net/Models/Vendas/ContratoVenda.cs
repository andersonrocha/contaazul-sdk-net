using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Resumo do contrato que originou uma venda (presente no detalhe da venda).
    /// </summary>
    public sealed class ContratoVenda
    {
        /// <summary>UUID do contrato.</summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        /// <summary>Data de início do contrato.</summary>
        [JsonPropertyName("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>Data final do contrato.</summary>
        [JsonPropertyName("data_fim")]
        public string DataFim { get; set; }

        /// <summary>Dia de vencimento de cada fatura gerada pelo contrato.</summary>
        [JsonPropertyName("dia_vencimento")]
        public int? DiaVencimento { get; set; }

        /// <summary>Período de faturamento (<c>MENSAL</c>, <c>SEMANAL</c>, <c>ANUAL</c>).</summary>
        [JsonPropertyName("periodo")]
        public string Periodo { get; set; }

        /// <summary>
        /// Periodicidade: a cada quantos períodos uma fatura é gerada
        /// (ex.: periodicidade 2 com período <c>MENSAL</c> gera fatura a cada 2 meses).
        /// </summary>
        [JsonPropertyName("periodicidade")]
        public int? Periodicidade { get; set; }
    }
}
