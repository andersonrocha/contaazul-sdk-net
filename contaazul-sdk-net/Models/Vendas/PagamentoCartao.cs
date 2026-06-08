using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Dados de pagamento com cartão.
    /// </summary>
    public sealed class PagamentoCartao
    {
        /// <summary>Bandeira do cartão (ex.: <c>VISA</c>, <c>MASTERCARD</c>, <c>ELO</c>).</summary>
        [JsonPropertyName("tipo_bandeira")]
        public string TipoBandeira { get; set; }

        /// <summary>Código da transação.</summary>
        [JsonPropertyName("codigo_transacao")]
        public string CodigoTransacao { get; set; }

        /// <summary>ID da adquirente.</summary>
        [JsonPropertyName("id_adquirente")]
        public long? IdAdquirente { get; set; }
    }
}
