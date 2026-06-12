using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Financeiro
{
    /// <summary>Condição de pagamento de um evento financeiro (lista de parcelas).</summary>
    public sealed class CondicaoPagamentoEvento
    {
        [JsonPropertyName("parcelas")]
        public List<ParcelaCondicaoPagamentoRequest> Parcelas { get; set; }
    }
}
