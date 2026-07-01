using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Conversão de unidade de medida do produto.</summary>
    public sealed class ConversaoDeUnidadeDeMedidaDoProduto
    {
        /// <summary>Fator de conversão entre unidades.</summary>
        [JsonPropertyName("fator")]
        public decimal? Fator { get; set; }

        /// <summary>Lista de IDs de fornecedores associados à conversão.</summary>
        [JsonPropertyName("id_fornecedor")]
        public List<string> IdFornecedor { get; set; }

        /// <summary>ID da conversão de unidade de medida.</summary>
        [JsonPropertyName("id_unidade_conversao")]
        public string IdUnidadeConversao { get; set; }

        /// <summary>Unidade de medida de entrada.</summary>
        [JsonPropertyName("unidade_medida")]
        public UnidadeMedidaResumoProduto UnidadeMedida { get; set; }
    }
}
