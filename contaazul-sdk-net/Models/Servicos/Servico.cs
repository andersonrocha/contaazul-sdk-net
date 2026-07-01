using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>
    /// Detalhe completo de um serviço (<c>GET /v1/servicos/{id}</c> e resposta de
    /// <c>POST /v1/servicos</c>).
    /// </summary>
    public sealed class Servico
    {
        /// <summary>Código interno do serviço.</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Código CNAE do serviço.</summary>
        [JsonPropertyName("codigo_cnae")]
        public string CodigoCnae { get; set; }

        /// <summary>Código do município onde o serviço é prestado.</summary>
        [JsonPropertyName("codigo_municipio_servico")]
        public string CodigoMunicipioServico { get; set; }

        /// <summary>Custo do serviço.</summary>
        [JsonPropertyName("custo")]
        public decimal? Custo { get; set; }

        /// <summary>Descrição detalhada do serviço.</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>ID (UUID) do serviço.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>ID externo do serviço.</summary>
        [JsonPropertyName("id_externo")]
        public string IdExterno { get; set; }

        /// <summary>ID do serviço no sistema legado.</summary>
        [JsonPropertyName("id_servico")]
        public int? IdServico { get; set; }

        /// <summary>Lei 116 associada ao serviço.</summary>
        [JsonPropertyName("lei_116")]
        public string Lei116 { get; set; }

        /// <summary>Lista de cenários de tributação do serviço.</summary>
        [JsonPropertyName("lista_cenario_tributario")]
        public List<CenarioDeTributacaoDoServico> ListaCenarioTributario { get; set; }

        /// <summary>Natureza operacional do serviço.</summary>
        [JsonPropertyName("natureza_operacional")]
        public NaturezaOperacionalDoServico NaturezaOperacional { get; set; }

        /// <summary>Preço de venda do serviço.</summary>
        [JsonPropertyName("preco")]
        public decimal? Preco { get; set; }

        /// <summary>Status atual do serviço (<c>ATIVO</c> ou <c>INATIVO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Tipo do serviço (<c>PRESTADO</c>, <c>TOMADO</c> ou <c>AMBOS</c>).</summary>
        [JsonPropertyName("tipo_servico")]
        public string TipoServico { get; set; }
    }
}
