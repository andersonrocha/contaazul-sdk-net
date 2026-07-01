using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Produtos
{
    /// <summary>
    /// Unidade de medida retornada na listagem <c>GET /v1/produtos/unidades-medida</c>.
    /// </summary>
    public sealed class UnidadeMedidaProduto
    {
        /// <summary>Abreviação da unidade de medida (ex.: <c>Kg</c>).</summary>
        [JsonPropertyName("abreviacao")]
        public string Abreviacao { get; set; }

        /// <summary>Descrição da unidade de medida (ex.: <c>Kilograma</c>).</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Indica se a unidade de medida está em uso.</summary>
        [JsonPropertyName("em_uso")]
        public bool? EmUso { get; set; }

        /// <summary>ID da unidade de medida.</summary>
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
