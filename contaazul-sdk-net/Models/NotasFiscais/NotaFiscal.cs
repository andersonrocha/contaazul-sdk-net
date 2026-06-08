using System;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>
    /// Nota fiscal de produto (NF-e) emitida no ERP.
    /// Retornada por <c>GET /v1/notas-fiscais</c>.
    /// </summary>
    public sealed class NotaFiscal
    {
        /// <summary>Chave de acesso da nota fiscal.</summary>
        [JsonPropertyName("chave_acesso")]
        public string ChaveAcesso { get; set; }

        /// <summary>Data de emissão da nota fiscal.</summary>
        [JsonPropertyName("data_emissao")]
        public DateTime? DataEmissao { get; set; }

        /// <summary>Nome do destinatário.</summary>
        [JsonPropertyName("nome_destinatario")]
        public string NomeDestinatario { get; set; }

        /// <summary>Número da nota fiscal.</summary>
        [JsonPropertyName("numero_nota")]
        public int? NumeroNota { get; set; }

        /// <summary>
        /// Status da nota fiscal. Valores possíveis: <c>EMITIDA</c>, <c>CORRIGIDA_SUCESSO</c>.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
