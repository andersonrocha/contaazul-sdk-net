using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>Conversão de unidade de medida do produto na criação.</summary>
    public sealed class CriacaoConversaoUnidadeMedidaProduto
    {
        /// <summary>Fator de conversão entre unidades de medida.</summary>
        [JsonPropertyName("fator")]
        public decimal? Fator { get; set; }

        /// <summary>Lista de IDs de fornecedores associados à conversão.</summary>
        [JsonPropertyName("id_fornecedor")]
        public List<string> IdFornecedor { get; set; }

        /// <summary>Unidade de medida de entrada para conversão.</summary>
        [JsonPropertyName("unidade_medida")]
        public ReferenciaIdInteiroProduto UnidadeMedida { get; set; }
    }
}
