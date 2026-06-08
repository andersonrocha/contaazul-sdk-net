using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Termos de recorrência para a criação de um contrato.
    /// </summary>
    public sealed class CriarTermosContrato
    {
        /// <summary>
        /// Data de fim da recorrência no formato <c>YYYY-MM-DD</c>; não pode ser anterior
        /// à data de início. <b>Obrigatório.</b>
        /// </summary>
        [JsonPropertyName("data_fim")]
        public string DataFim { get; set; }

        /// <summary>
        /// Data de início da recorrência no formato <c>YYYY-MM-DD</c>. <b>Obrigatório.</b>
        /// </summary>
        [JsonPropertyName("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>Dia do mês em que a venda será emitida (1-31). <b>Obrigatório.</b></summary>
        [JsonPropertyName("dia_emissao_venda")]
        public int DiaEmissaoVenda { get; set; }

        /// <summary>Intervalo de frequência entre as recorrências (1-60). <b>Obrigatório.</b></summary>
        [JsonPropertyName("intervalo_frequencia")]
        public int IntervaloFrequencia { get; set; }

        /// <summary>Número de ocorrências (mínimo 1). <b>Obrigatório.</b></summary>
        [JsonPropertyName("numero")]
        public int Numero { get; set; }

        /// <summary>
        /// Tipo de expiração da recorrência: <c>DATA</c> ou <c>NUNCA</c>. <b>Obrigatório.</b>
        /// </summary>
        [JsonPropertyName("tipo_expiracao")]
        public string TipoExpiracao { get; set; }

        /// <summary>
        /// Tipo de frequência da recorrência: <c>MENSAL</c> ou <c>ANUAL</c>. <b>Obrigatório.</b>
        /// </summary>
        [JsonPropertyName("tipo_frequencia")]
        public string TipoFrequencia { get; set; }
    }
}
