using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Cliente associado a uma venda (listagem).
    /// </summary>
    public sealed class Cliente
    {
        /// <summary>ID (UUID) do cliente.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Nome do cliente.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>E-mail do cliente.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>Telefone do cliente.</summary>
        [JsonPropertyName("telefone")]
        public string Telefone { get; set; }

        /// <summary>Endereço do cliente.</summary>
        [JsonPropertyName("endereco")]
        public string Endereco { get; set; }

        /// <summary>Cidade do cliente.</summary>
        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }

        /// <summary>Estado (UF) do cliente.</summary>
        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        /// <summary>País do cliente.</summary>
        [JsonPropertyName("pais")]
        public string Pais { get; set; }

        /// <summary>CEP do cliente.</summary>
        [JsonPropertyName("cep")]
        public string Cep { get; set; }
    }
}
