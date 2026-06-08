using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Outro contato de uma pessoa. Modelo unificado usado tanto na leitura quanto na
    /// criação/atualização — campos não informados são omitidos na serialização.
    /// </summary>
    public sealed class OutroContatoPessoa
    {
        /// <summary>ID do contato (presente apenas em respostas).</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome do contato.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Cargo do contato.</summary>
        [JsonPropertyName("cargo")]
        public string Cargo { get; set; }

        /// <summary>Email do contato.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>Telefone celular do contato.</summary>
        [JsonPropertyName("telefone_celular")]
        public string TelefoneCelular { get; set; }

        /// <summary>Telefone comercial do contato.</summary>
        [JsonPropertyName("telefone_comercial")]
        public string TelefoneComercial { get; set; }
    }
}
