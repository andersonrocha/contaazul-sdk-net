using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Informações de variação do produto na criação.</summary>
    public sealed class CriacaoVariacaoProduto
    {
        /// <summary>Lista de produtos associados à variação.</summary>
        [JsonPropertyName("produtos")]
        public List<CriacaoProdutoVariacaoProduto> Produtos { get; set; }

        /// <summary>Lista de tipos de variação do produto.</summary>
        [JsonPropertyName("tipos")]
        public List<CriacaoTipoVariacaoProduto> Tipos { get; set; }
    }
}
