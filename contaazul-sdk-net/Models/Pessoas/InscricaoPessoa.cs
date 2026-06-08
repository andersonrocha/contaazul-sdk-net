using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Pessoas
{
    /// <summary>
    /// Inscrição estadual/municipal de uma pessoa. Modelo unificado usado tanto na leitura
    /// quanto na criação/atualização — campos não informados são omitidos na serialização.
    /// </summary>
    public sealed class InscricaoPessoa
    {
        /// <summary>ID da inscrição (presente apenas em respostas).</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Indicador de inscrição estadual. Valores possíveis:
        /// <c>NAO CONTRIBUINTE</c>, <c>CONTRIBUINTE</c>, <c>ISENTO</c>.
        /// </summary>
        [JsonPropertyName("indicador_inscricao_estadual")]
        public string IndicadorInscricaoEstadual { get; set; }

        /// <summary>Inscrição estadual.</summary>
        [JsonPropertyName("inscricao_estadual")]
        public string InscricaoEstadual { get; set; }

        /// <summary>Inscrição municipal.</summary>
        [JsonPropertyName("inscricao_municipal")]
        public string InscricaoMunicipal { get; set; }

        /// <summary>Inscrição SUFRAMA.</summary>
        [JsonPropertyName("inscricao_suframa")]
        public string InscricaoSuframa { get; set; }
    }
}
