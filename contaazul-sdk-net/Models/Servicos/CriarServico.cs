using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>
    /// Dados para criar um novo serviço (<c>POST /v1/servicos</c>).
    /// Apenas <see cref="Descricao"/> é obrigatório.
    /// </summary>
    public sealed class CriarServico
    {
        /// <summary>Código interno do serviço (máx. 20 caracteres).</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Custo do serviço.</summary>
        [JsonPropertyName("custo")]
        public decimal? Custo { get; set; }

        /// <summary>Descrição do serviço (obrigatório, máx. 100 caracteres).</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Preço de venda do serviço.</summary>
        [JsonPropertyName("preco")]
        public decimal? Preco { get; set; }

        /// <summary>Status do serviço (<c>ATIVO</c> ou <c>INATIVO</c>).</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Tipo do serviço (<c>PRESTADO</c>, <c>TOMADO</c> ou <c>AMBOS</c>).</summary>
        [JsonPropertyName("tipo_servico")]
        public string TipoServico { get; set; }
    }
}
