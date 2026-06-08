using System;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>Informações de transmissão da nota fiscal de serviço.</summary>
    public sealed class InformacaoTransmissaoNotaFiscalServico
    {
        /// <summary>Data de início do cancelamento.</summary>
        [JsonPropertyName("data_inicio_cancelamento")]
        public DateTime? DataInicioCancelamento { get; set; }

        /// <summary>Data de início da emissão.</summary>
        [JsonPropertyName("data_inicio_emissao")]
        public DateTime? DataInicioEmissao { get; set; }
    }
}
