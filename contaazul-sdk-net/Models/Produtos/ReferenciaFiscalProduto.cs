using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Referência fiscal com código, descrição e id. Usada para CEST e NCM, tanto nas
    /// listagens (<c>GET /v1/produtos/cest</c> e <c>/ncm</c>) quanto nas informações
    /// fiscais do produto (<see cref="FiscalDoProduto.Cest"/> e <see cref="FiscalDoProduto.Ncm"/>).
    /// </summary>
    public sealed class ReferenciaFiscalProduto
    {
        /// <summary>Código (ex.: código do CEST ou do NCM).</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Descrição.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID interno da referência fiscal.</summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
