using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Dados cadastrais da empresa vinculada ao token de autenticação (conta conectada).
    /// </summary>
    public sealed class Empresa
    {
        /// <summary>Razão social da empresa.</summary>
        [JsonPropertyName("razao_social")]
        public string RazaoSocial { get; set; }

        /// <summary>Nome fantasia da empresa.</summary>
        [JsonPropertyName("nome_fantasia")]
        public string NomeFantasia { get; set; }

        /// <summary>Documento (CNPJ) da empresa.</summary>
        [JsonPropertyName("documento")]
        public string Documento { get; set; }

        /// <summary>Email da empresa.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>Data de fundação da empresa.</summary>
        [JsonPropertyName("data_fundacao")]
        public string DataFundacao { get; set; }
    }
}
