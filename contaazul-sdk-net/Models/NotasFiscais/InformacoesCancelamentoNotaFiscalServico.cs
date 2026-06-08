using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>Informações de cancelamento da nota fiscal de serviço.</summary>
    public sealed class InformacoesCancelamentoNotaFiscalServico
    {
        /// <summary>Motivo do cancelamento.</summary>
        [JsonPropertyName("motivo")]
        public string Motivo { get; set; }

        /// <summary>Usuário que cancelou.</summary>
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; }
    }
}
