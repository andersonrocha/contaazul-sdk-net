using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models.Servicos
{
    /// <summary>
    /// Dados para atualização parcial de um serviço (<c>PATCH /v1/servicos/{id}</c>).
    /// Apenas os campos informados são alterados.
    /// </summary>
    public sealed class AtualizacaoParcialServico
    {
        /// <summary>Código interno do serviço (máx. 20 caracteres).</summary>
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        /// <summary>Custo do serviço.</summary>
        [JsonPropertyName("custo")]
        public decimal? Custo { get; set; }

        /// <summary>Descrição do serviço (máx. 100 caracteres).</summary>
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        /// <summary>Preço de venda do serviço.</summary>
        [JsonPropertyName("preco")]
        public decimal? Preco { get; set; }

        /// <summary>Tipo do serviço (<c>PRESTADO</c>, <c>TOMADO</c> ou <c>AMBOS</c>).</summary>
        [JsonPropertyName("tipo_servico")]
        public string TipoServico { get; set; }
    }
}
