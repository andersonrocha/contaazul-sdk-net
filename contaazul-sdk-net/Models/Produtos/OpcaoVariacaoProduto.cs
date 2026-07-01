using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Opção de variação de um produto (descrição + id). Usada tanto nas opções do produto
    /// com variação quanto nas opções do tipo de variação.
    /// </summary>
    public sealed class OpcaoVariacaoProduto
    {
        /// <summary>Descrição da opção de variação.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID da opção de variação.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
