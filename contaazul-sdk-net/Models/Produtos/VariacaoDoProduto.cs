using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Informações de variação do produto (detalhe).</summary>
    public sealed class VariacaoDoProduto
    {
        /// <summary>Lista de produtos com variações.</summary>
        [JsonPropertyName("produtos")]
        public List<ProdutosComVariacao> Produtos { get; set; }

        /// <summary>Lista de tipos de variação do produto.</summary>
        [JsonPropertyName("tipos")]
        public List<TipoDeVariacaoDoProduto> Tipos { get; set; }
    }
}
