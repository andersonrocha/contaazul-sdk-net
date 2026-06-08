using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Resumo do local de prestação de serviço do contrato.
    /// </summary>
    public sealed class LocalPrestacaoServicoResumo
    {
        /// <summary>Nome do local de prestação de serviço.</summary>
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
    }
}
