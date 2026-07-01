using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Opção do tipo de variação do produto na criação.</summary>
    public sealed class CriacaoOpcaoTipoVariacaoProduto
    {
        /// <summary>Descrição da opção de variação do tipo de variação.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID da opção de variação do tipo de variação.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
