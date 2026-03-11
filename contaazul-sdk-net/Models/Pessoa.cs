using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class Pessoa
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("tipo_pessoa")]
        public string TipoPessoa { get; set; }

        [JsonPropertyName("codigo_pessoa")]
        public string CodigoPessoa { get; set; }

        [JsonPropertyName("cpf_cnpj")]
        public string CpfCnpj { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("telefone")]
        public string Telefone { get; set; }

        [JsonPropertyName("celular")]
        public string Celular { get; set; }

        [JsonPropertyName("endereco")]
        public Endereco Endereco { get; set; }

        [JsonPropertyName("data_criacao")]
        public string DataCriacao { get; set; }

        [JsonPropertyName("data_alteracao")]
        public string DataAlteracao { get; set; }

        [JsonPropertyName("tipo_perfil")]
        public string TipoPerfil { get; set; }
    }
}
