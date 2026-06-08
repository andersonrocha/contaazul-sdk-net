using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Dados para criação (<c>POST /v1/pessoas</c>) ou atualização integral
    /// (<c>PUT /v1/pessoas/{id}</c>) de uma pessoa. Os dois endpoints compartilham o mesmo
    /// conjunto de campos; campos não informados são omitidos na serialização.
    /// <para>Na criação, <see cref="Nome"/> e <see cref="TipoPessoa"/> são obrigatórios.</para>
    /// </summary>
    public sealed class PessoaRequest
    {
        /// <summary>Nome da pessoa. Obrigatório na criação.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>
        /// Tipo de pessoa. Valores possíveis: <c>Física</c>, <c>Jurídica</c>, <c>Estrangeira</c>.
        /// Obrigatório na criação.
        /// </summary>
        [JsonPropertyName("tipo_pessoa")]
        public string TipoPessoa { get; set; }

        /// <summary>Nome fantasia da pessoa jurídica.</summary>
        [JsonPropertyName("nome_fantasia")]
        public string NomeFantasia { get; set; }

        /// <summary>CPF da pessoa física.</summary>
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }

        /// <summary>CNPJ da pessoa jurídica.</summary>
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        /// <summary>RG da pessoa.</summary>
        [JsonPropertyName("rg")]
        public string Rg { get; set; }

        /// <summary>Código da pessoa.</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Data de nascimento da pessoa física.</summary>
        [JsonPropertyName("data_nascimento")]
        public string DataNascimento { get; set; }

        /// <summary>Emails da pessoa separados por vírgula.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>Telefone celular.</summary>
        [JsonPropertyName("telefone_celular")]
        public string TelefoneCelular { get; set; }

        /// <summary>Telefone comercial.</summary>
        [JsonPropertyName("telefone_comercial")]
        public string TelefoneComercial { get; set; }

        /// <summary>Observações sobre a pessoa.</summary>
        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        /// <summary>Indica se a pessoa está ativa.</summary>
        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }

        /// <summary>Indica se a pessoa é optante pelo Simples Nacional.</summary>
        [JsonPropertyName("optante_simples")]
        public bool? OptanteSimples { get; set; }

        /// <summary>Indica se a pessoa é uma agência pública.</summary>
        [JsonPropertyName("agencia_publica")]
        public bool? AgenciaPublica { get; set; }

        /// <summary>Contato para cobrança e faturamento.</summary>
        [JsonPropertyName("contato_cobranca_faturamento")]
        public ContatoCobrancaFaturamento ContatoCobrancaFaturamento { get; set; }

        /// <summary>Lista de endereços.</summary>
        [JsonPropertyName("enderecos")]
        public List<EnderecoPessoa> Enderecos { get; set; }

        /// <summary>Lista de inscrições estaduais e municipais.</summary>
        [JsonPropertyName("inscricoes")]
        public List<InscricaoPessoa> Inscricoes { get; set; }

        /// <summary>Lista de outros contatos da pessoa.</summary>
        [JsonPropertyName("outros_contatos")]
        public List<OutroContatoPessoa> OutrosContatos { get; set; }

        /// <summary>Lista de perfis associados à pessoa.</summary>
        [JsonPropertyName("perfis")]
        public List<PerfilPessoa> Perfis { get; set; }
    }
}
