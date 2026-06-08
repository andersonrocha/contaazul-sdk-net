using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Contratos
{
    /// <summary>
    /// Termos de vigência/recorrência do contrato.
    /// <para>
    /// O conjunto de campos preenchidos varia conforme o endpoint: a listagem
    /// (<see cref="ItemContrato.Termos"/>) retorna <see cref="DataFim"/>,
    /// <see cref="TipoExpiracao"/>, <see cref="VigenciaAtual"/> e <see cref="VigenciaTotal"/>;
    /// o detalhe (<see cref="ContratoResumo.Termos"/>) retorna <see cref="DataInicio"/>,
    /// <see cref="DataFim"/>, <see cref="DiaEmissaoVenda"/>, <see cref="IntervaloFrequencia"/>,
    /// <see cref="Numero"/>, <see cref="TipoExpiracao"/> e <see cref="TipoFrequencia"/>.
    /// </para>
    /// </summary>
    public sealed class TermosContrato
    {
        /// <summary>Data de início do contrato. (Detalhe)</summary>
        [JsonPropertyName("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>Data de término do contrato.</summary>
        [JsonPropertyName("data_fim")]
        public string DataFim { get; set; }

        /// <summary>Dia do mês para emissão da venda. (Detalhe)</summary>
        [JsonPropertyName("dia_emissao_venda")]
        public int? DiaEmissaoVenda { get; set; }

        /// <summary>Intervalo entre as cobranças (ex.: a cada 1 mês). (Detalhe)</summary>
        [JsonPropertyName("intervalo_frequencia")]
        public int? IntervaloFrequencia { get; set; }

        /// <summary>Número do contrato. (Detalhe)</summary>
        [JsonPropertyName("numero")]
        public int? Numero { get; set; }

        /// <summary>Tipo de expiração (ex.: <c>DATA</c>, <c>VEZES</c>, <c>NUNCA</c>).</summary>
        [JsonPropertyName("tipo_expiracao")]
        public string TipoExpiracao { get; set; }

        /// <summary>
        /// Tipo de frequência da cobrança (período de agendamento:
        /// <c>MENSAL</c>, <c>SEMANAL</c> ou <c>ANUAL</c>). (Detalhe)
        /// </summary>
        [JsonPropertyName("tipo_frequencia")]
        public string TipoFrequencia { get; set; }

        /// <summary>Número de cobranças já realizadas. (Listagem)</summary>
        [JsonPropertyName("vigencia_atual")]
        public int? VigenciaAtual { get; set; }

        /// <summary>Total de cobranças previstas. (Listagem)</summary>
        [JsonPropertyName("vigencia_total")]
        public int? VigenciaTotal { get; set; }
    }
}
