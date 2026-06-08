using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Resumo da configuração de recorrência do contrato.
    /// </summary>
    public sealed class ConfiguracaoRecorrenciaResumo
    {
        /// <summary>Vigência restante do contrato.</summary>
        [JsonPropertyName("vigencia_restante")]
        public int VigenciaRestante { get; set; }

        /// <summary>Vigência total do contrato.</summary>
        [JsonPropertyName("vigencia_total")]
        public int VigenciaTotal { get; set; }
    }
}
