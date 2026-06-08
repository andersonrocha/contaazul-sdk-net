using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Detalhes completos de uma venda (<c>/v1/venda/{id}</c>).
    /// </summary>
    public sealed class ObterVendaResponse
    {
        /// <summary>Cliente da venda.</summary>
        [JsonPropertyName("cliente")]
        public ClienteVendaDetalhe Cliente { get; set; }

        /// <summary>Evento financeiro associado à venda.</summary>
        [JsonPropertyName("evento_financeiro")]
        public EventoFinanceiro EventoFinanceiro { get; set; }

        /// <summary>Notificação (e-mail) da venda.</summary>
        [JsonPropertyName("notificacao")]
        public Notificacao Notificacao { get; set; }

        /// <summary>Natureza da operação da venda.</summary>
        [JsonPropertyName("natureza_operacao")]
        public NaturezaOperacao NaturezaOperacao { get; set; }

        /// <summary>Dados de negociação da venda.</summary>
        [JsonPropertyName("venda")]
        public Negociacao Venda { get; set; }

        /// <summary>Vendedor responsável.</summary>
        [JsonPropertyName("vendedor")]
        public Vendedor Vendedor { get; set; }

        /// <summary>Contrato que originou a venda, quando aplicável.</summary>
        [JsonPropertyName("contrato")]
        public ContratoVenda Contrato { get; set; }
    }
}
