using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Tipo de variação do produto na criação.</summary>
    public sealed class CriacaoTipoVariacaoProduto
    {
        /// <summary>Descrição do tipo de variação.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Lista de opções do tipo de variação.</summary>
        [JsonPropertyName("opcoes")]
        public List<CriacaoOpcaoTipoVariacaoProduto> Opcoes { get; set; }
    }
}
