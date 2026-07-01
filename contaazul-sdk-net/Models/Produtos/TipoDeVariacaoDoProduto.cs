using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Tipo de variação do produto (ex.: tamanho, cor) e suas opções.</summary>
    public sealed class TipoDeVariacaoDoProduto
    {
        /// <summary>Descrição do tipo de variação.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID do tipo de variação.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Lista de opções do tipo de variação.</summary>
        [JsonPropertyName("opcoes")]
        public List<OpcaoVariacaoProduto> Opcoes { get; set; }
    }
}
