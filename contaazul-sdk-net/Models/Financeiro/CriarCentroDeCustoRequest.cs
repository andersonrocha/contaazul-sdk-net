using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Dados para criar um centro de custo (<c>POST /v1/centro-de-custo</c>).</summary>
    public sealed class CriarCentroDeCustoRequest
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Nome do centro de custo. Obrigatório.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
