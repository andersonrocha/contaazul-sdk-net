using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Conta financeira (conta bancária, cartão, poupança, etc.).</summary>
    public sealed class ContaFinanceira
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Instituição bancária (enum, ex.: <c>BANCO_BRASIL</c>, <c>ITAU</c>, <c>NUBANK</c>...).</summary>
        [JsonPropertyName("banco")]
        public string Banco { get; set; }

        [JsonPropertyName("codigo_banco")]
        public int? CodigoBanco { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }

        /// <summary>Tipo da conta (ex.: <c>CONTA_CORRENTE</c>, <c>COBRANCAS_CONTA_AZUL</c>...).</summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("conta_padrao")]
        public bool? ContaPadrao { get; set; }

        [JsonPropertyName("possui_config_boleto_bancario")]
        public bool? PossuiConfigBoletoBancario { get; set; }

        [JsonPropertyName("agencia")]
        public string Agencia { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }
    }
}
