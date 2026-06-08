using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Item resumido de pessoa retornado na listagem por filtro (<c>GET /v1/pessoas</c>).
    /// </summary>
    public sealed class ItemPessoaResumo
    {
        /// <summary>ID da pessoa.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID legado da pessoa.</summary>
        [JsonPropertyName("id_legado")]
        public int? IdLegado { get; set; }

        /// <summary>UUID legado da pessoa.</summary>
        [JsonPropertyName("uuid_legado")]
        public string UuidLegado { get; set; }

        /// <summary>Nome da pessoa (física, jurídica ou estrangeira).</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Tipo de pessoa (ex.: <c>FISICA</c>).</summary>
        [JsonPropertyName("tipo_pessoa")]
        public string TipoPessoa { get; set; }

        /// <summary>Documento da pessoa (CPF/CNPJ).</summary>
        [JsonPropertyName("documento")]
        public string Documento { get; set; }

        /// <summary>Email da pessoa.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>Telefone da pessoa.</summary>
        [JsonPropertyName("telefone")]
        public string Telefone { get; set; }

        /// <summary>Indica se a pessoa está ativa ou inativa.</summary>
        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }

        /// <summary>Observações gerais sobre a pessoa.</summary>
        [JsonPropertyName("observacoes_gerais")]
        public string ObservacoesGerais { get; set; }

        /// <summary>Perfis associados à pessoa (ex.: <c>CLIENTE</c>, <c>FORNECEDOR</c>, <c>TRANSPORTADORA</c>).</summary>
        [JsonPropertyName("perfis")]
        public List<string> Perfis { get; set; }

        /// <summary>Endereço da pessoa.</summary>
        [JsonPropertyName("endereco")]
        public EnderecoPessoa Endereco { get; set; }

        /// <summary>Data/hora de criação.</summary>
        [JsonPropertyName("data_criacao")]
        public DateTime? DataCriacao { get; set; }

        /// <summary>Data/hora da última alteração.</summary>
        [JsonPropertyName("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }
    }
}
