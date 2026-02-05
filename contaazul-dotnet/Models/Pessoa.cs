using Newtonsoft.Json;

namespace contaazul_dotnet.Models
{
    public class Pessoa
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("tipo_pessoa")]
        public string TipoPessoa { get; set; }

        [JsonProperty("codigo_pessoa")]
        public string CodigoPessoa { get; set; }

        [JsonProperty("cpf_cnpj")]
        public string CpfCnpj { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [JsonProperty("celular")]
        public string Celular { get; set; }

        [JsonProperty("endereco")]
        public Endereco Endereco { get; set; }

        [JsonProperty("data_criacao")]
        public string DataCriacao { get; set; }

        [JsonProperty("data_alteracao")]
        public string DataAlteracao { get; set; }

        [JsonProperty("tipo_perfil")]
        public string TipoPerfil { get; set; }
    }

    public class Endereco
    {
        [JsonProperty("logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("cidade")]
        public string Cidade { get; set; }

        [JsonProperty("uf")]
        public string Uf { get; set; }

        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("pais")]
        public string Pais { get; set; }
    }
}
