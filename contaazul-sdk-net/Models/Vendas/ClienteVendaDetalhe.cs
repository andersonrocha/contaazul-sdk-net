using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Resumo do cliente retornado no detalhe de uma venda.
    /// </summary>
    public sealed class ClienteVendaDetalhe
    {
        /// <summary>UUID do cliente.</summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        /// <summary>Tipo de pessoa (ex.: <c>Física</c>, <c>Jurídica</c>).</summary>
        [JsonPropertyName("tipo_pessoa")]
        public string TipoPessoa { get; set; }

        /// <summary>Documento (CPF/CNPJ).</summary>
        [JsonPropertyName("documento")]
        public string Documento { get; set; }

        /// <summary>Nome do cliente.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
