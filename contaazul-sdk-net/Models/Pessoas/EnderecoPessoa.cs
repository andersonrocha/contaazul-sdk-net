using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Endereço de uma pessoa. Modelo unificado usado tanto na leitura quanto na
    /// criação/atualização — campos não informados são omitidos na serialização.
    /// </summary>
    public sealed class EnderecoPessoa
    {
        /// <summary>ID do endereço (presente apenas em respostas).</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID da cidade (presente apenas em respostas).</summary>
        [JsonPropertyName("id_cidade")]
        public int? IdCidade { get; set; }

        /// <summary>Logradouro do endereço.</summary>
        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }

        /// <summary>Número do endereço.</summary>
        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        /// <summary>Complemento do endereço.</summary>
        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }

        /// <summary>Bairro do endereço.</summary>
        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        /// <summary>Cidade do endereço.</summary>
        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }

        /// <summary>Estado (UF) do endereço.</summary>
        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        /// <summary>CEP do endereço.</summary>
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        /// <summary>País do endereço (Brasil quando tipo_pessoa for Física ou Jurídica).</summary>
        [JsonPropertyName("pais")]
        public string Pais { get; set; }
    }
}
