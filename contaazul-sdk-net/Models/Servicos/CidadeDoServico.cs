using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>Cidade (município) associada ao cenário de tributação do serviço.</summary>
    public sealed class CidadeDoServico
    {
        /// <summary>Código do município (IBGE).</summary>
        [JsonPropertyName("codigo")]
        public int? Codigo { get; set; }

        /// <summary>Nome do município.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        /// <summary>Sigla do estado (UF).</summary>
        [JsonPropertyName("uf")]
        public string Uf { get; set; }
    }
}
