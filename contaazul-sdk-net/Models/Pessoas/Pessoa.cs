using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Cadastro completo de uma pessoa (cliente, fornecedor ou transportadora).
    /// Retornado por <c>GET /v1/pessoas/{id}</c> e <c>GET /v1/pessoas/legado/{id}</c>.
    /// </summary>
    public sealed class Pessoa
    {
        /// <summary>ID da pessoa.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome da pessoa.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Nome da empresa (nome fantasia).</summary>
        [JsonPropertyName("nome_empresa")]
        public string NomeEmpresa { get; set; }

        /// <summary>Tipo de pessoa (ex.: <c>FISICA</c>, <c>Jurídica</c>, <c>Estrangeira</c>).</summary>
        [JsonPropertyName("tipo_pessoa")]
        public string TipoPessoa { get; set; }

        /// <summary>Código da pessoa.</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Documento da pessoa (CPF/CNPJ).</summary>
        [JsonPropertyName("documento")]
        public string Documento { get; set; }

        /// <summary>RG da pessoa.</summary>
        [JsonPropertyName("rg")]
        public string Rg { get; set; }

        /// <summary>Data de nascimento da pessoa.</summary>
        [JsonPropertyName("data_nascimento")]
        public string DataNascimento { get; set; }

        /// <summary>Emails da pessoa separados por vírgula.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>Telefone celular da pessoa.</summary>
        [JsonPropertyName("telefone_celular")]
        public string TelefoneCelular { get; set; }

        /// <summary>Telefone comercial da pessoa.</summary>
        [JsonPropertyName("telefone_comercial")]
        public string TelefoneComercial { get; set; }

        /// <summary>Indica se a pessoa está ativa.</summary>
        [JsonPropertyName("ativo")]
        public bool? Ativo { get; set; }

        /// <summary>Indica se a pessoa é optante pelo Simples Nacional.</summary>
        [JsonPropertyName("optante_simples_nacional")]
        public bool? OptanteSimplesNacional { get; set; }

        /// <summary>Indica se a pessoa é um órgão público.</summary>
        [JsonPropertyName("orgao_publico")]
        public bool? OrgaoPublico { get; set; }

        /// <summary>Observações gerais da pessoa.</summary>
        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        /// <summary>Data de criação da pessoa (ISO 8601, São Paulo/GMT-3).</summary>
        [JsonPropertyName("criado_em")]
        public string CriadoEm { get; set; }

        /// <summary>Data da última alteração da pessoa (ISO 8601, São Paulo/GMT-3).</summary>
        [JsonPropertyName("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }

        /// <summary>Atrasos nos pagamentos.</summary>
        [JsonPropertyName("atrasos_pagamentos")]
        public decimal? AtrasosPagamentos { get; set; }

        /// <summary>Atrasos nos recebimentos.</summary>
        [JsonPropertyName("atrasos_recebimentos")]
        public decimal? AtrasosRecebimentos { get; set; }

        /// <summary>Pagamentos do mês atual.</summary>
        [JsonPropertyName("pagamentos_mes_atual")]
        public decimal? PagamentosMesAtual { get; set; }

        /// <summary>Recebimentos do mês atual.</summary>
        [JsonPropertyName("recebimentos_mes_atual")]
        public decimal? RecebimentosMesAtual { get; set; }

        /// <summary>Contato para cobrança e faturamento.</summary>
        [JsonPropertyName("contato_cobranca_faturamento")]
        public ContatoCobrancaFaturamento ContatoCobrancaFaturamento { get; set; }

        /// <summary>Mensagens de pagamentos em aberto.</summary>
        [JsonPropertyName("mensagem_pagamentos_abertos")]
        public MensagensPagamentosAbertos MensagemPagamentosAbertos { get; set; }

        /// <summary>Endereços da pessoa.</summary>
        [JsonPropertyName("enderecos")]
        public List<EnderecoPessoa> Enderecos { get; set; }

        /// <summary>Inscrições estaduais/municipais (informações fiscais).</summary>
        [JsonPropertyName("inscricoes")]
        public List<InscricaoPessoa> Inscricoes { get; set; }

        /// <summary>Lembretes de vencimento.</summary>
        [JsonPropertyName("lembretes_vencimento")]
        public List<LembreteVencimento> LembretesVencimento { get; set; }

        /// <summary>Outros contatos da pessoa.</summary>
        [JsonPropertyName("outros_contatos")]
        public List<OutroContatoPessoa> OutrosContatos { get; set; }

        /// <summary>Perfis associados à pessoa.</summary>
        [JsonPropertyName("perfis")]
        public List<PerfilPessoa> Perfis { get; set; }

        /// <summary>Vínculos com registros legados (API V1).</summary>
        [JsonPropertyName("pessoas_legado")]
        public List<PessoaLegada> PessoasLegado { get; set; }
    }
}
