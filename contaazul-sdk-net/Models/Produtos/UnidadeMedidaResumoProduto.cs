using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Unidade de medida resumida (descrição + id). Usada nas informações fiscais do produto
    /// e como unidade de entrada nas conversões de unidade de medida.
    /// </summary>
    public sealed class UnidadeMedidaResumoProduto
    {
        /// <summary>Descrição da unidade de medida.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID da unidade de medida.</summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
