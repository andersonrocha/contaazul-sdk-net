using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>Cenário de tributação de um serviço.</summary>
    public sealed class CenarioDeTributacaoDoServico
    {
        /// <summary>ID do cenário de tributação.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Alíquota do INSS.</summary>
        [JsonPropertyName("inss_aliquota")]
        public decimal? InssAliquota { get; set; }

        /// <summary>Alíquota do ISS.</summary>
        [JsonPropertyName("iss_aliquota")]
        public decimal? IssAliquota { get; set; }

        /// <summary>Indica se o ISS é retido.</summary>
        [JsonPropertyName("iss_retido")]
        public bool? IssRetido { get; set; }

        /// <summary>Cidade do cenário de tributação.</summary>
        [JsonPropertyName("municipio")]
        public CidadeDoServico Municipio { get; set; }

        /// <summary>Nome do usuário que criou o cenário.</summary>
        [JsonPropertyName("nome_usuario")]
        public string NomeUsuario { get; set; }

        /// <summary>Data e hora da última atualização do cenário (ISO 8601).</summary>
        [JsonPropertyName("ultima_atualizacao")]
        public string UltimaAtualizacao { get; set; }
    }
}
