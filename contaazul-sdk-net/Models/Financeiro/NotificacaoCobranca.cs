using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Notificação de cobrança.</summary>
    public sealed class NotificacaoCobranca
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("solicitacao_cobranca_ids")]
        public List<string> SolicitacaoCobrancaIds { get; set; }

        [JsonPropertyName("versao")]
        public long? Versao { get; set; }

        [JsonPropertyName("enviado_em")]
        public string EnviadoEm { get; set; }

        [JsonPropertyName("aberto_em")]
        public string AbertoEm { get; set; }

        [JsonPropertyName("itens_notificacao_cobranca")]
        public List<ItemNotificacaoCobranca> ItensNotificacaoCobranca { get; set; }

        [JsonPropertyName("assunto")]
        public string Assunto { get; set; }

        [JsonPropertyName("corpo")]
        public string Corpo { get; set; }

        [JsonPropertyName("respondido_para")]
        public string RespondidoPara { get; set; }

        [JsonPropertyName("agendado")]
        public bool? Agendado { get; set; }

        [JsonPropertyName("auto_notificacao")]
        public bool? AutoNotificacao { get; set; }

        [JsonPropertyName("envio_instantaneo")]
        public bool? EnvioInstantaneo { get; set; }
    }
}
