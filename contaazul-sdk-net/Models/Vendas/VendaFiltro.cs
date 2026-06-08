using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Vendas
{
    /// <summary>
    /// Filtros para a busca de vendas (<c>/v1/venda/busca</c>).
    /// Os parâmetros de múltiplos valores devem ser informados como valores separados por vírgula.
    /// </summary>
    public class VendaFiltro : FiltroBase
    {
        /// <summary>
        /// Campo para ordenação ascendente (<c>NUMERO</c>, <c>CLIENTE</c> ou <c>DATA</c>).
        /// Quando informado, desconsidera <see cref="CampoOrdenadoDescendente"/>.
        /// </summary>
        [QueryParameter("campo_ordenado_ascendente")]
        public string CampoOrdenadoAscendente { get; set; }

        /// <summary>
        /// Campo para ordenação descendente (<c>NUMERO</c>, <c>CLIENTE</c> ou <c>DATA</c>).
        /// Não deve ser informado junto com <see cref="CampoOrdenadoAscendente"/>.
        /// </summary>
        [QueryParameter("campo_ordenado_descendente")]
        public string CampoOrdenadoDescendente { get; set; }

        /// <summary>Termo para busca por nome, e-mail do cliente ou número da venda.</summary>
        [QueryParameter("termo_busca")]
        public string TermoBusca { get; set; }

        /// <summary>Data de início da emissão da venda (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>Data final da emissão da venda (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_fim")]
        public string DataFim { get; set; }

        /// <summary>Data de início da criação da venda (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_criacao_de")]
        public string DataCriacaoDe { get; set; }

        /// <summary>Data final da criação da venda (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_criacao_ate")]
        public string DataCriacaoAte { get; set; }

        /// <summary>Data de alteração de (ISO 8601, São Paulo/GMT-3).</summary>
        [QueryParameter("data_alteracao_de")]
        public string DataAlteracaoDe { get; set; }

        /// <summary>Data de alteração até (ISO 8601, São Paulo/GMT-3).</summary>
        [QueryParameter("data_alteracao_ate")]
        public string DataAlteracaoAte { get; set; }

        /// <summary>IDs dos vendedores, separados por vírgula.</summary>
        [QueryParameter("ids_vendedores")]
        public string IdsVendedores { get; set; }

        /// <summary>IDs dos clientes, separados por vírgula.</summary>
        [QueryParameter("ids_clientes")]
        public string IdsClientes { get; set; }

        /// <summary>IDs da natureza da operação, separados por vírgula.</summary>
        [QueryParameter("ids_natureza_operacao")]
        public string IdsNaturezaOperacao { get; set; }

        /// <summary>Situações das vendas, separadas por vírgula.</summary>
        [QueryParameter("situacoes")]
        public string Situacoes { get; set; }

        /// <summary>Tipos de vendas, separados por vírgula.</summary>
        [QueryParameter("tipos")]
        public string Tipos { get; set; }

        /// <summary>Origens das vendas, separadas por vírgula.</summary>
        [QueryParameter("origens")]
        public string Origens { get; set; }

        /// <summary>Número da venda.</summary>
        [QueryParameter("numeros")]
        public int? Numeros { get; set; }

        /// <summary>IDs das categorias, separados por vírgula.</summary>
        [QueryParameter("ids_categorias")]
        public string IdsCategorias { get; set; }

        /// <summary>IDs dos produtos, separados por vírgula.</summary>
        [QueryParameter("ids_produtos")]
        public string IdsProdutos { get; set; }

        /// <summary>Indica se a venda está pendente.</summary>
        [QueryParameter("pendente")]
        public bool? Pendente { get; set; }

        /// <summary>
        /// Tipo de total de venda. Valores possíveis: <c>WAITING_APPROVED</c>, <c>APPROVED</c>,
        /// <c>CANCELED</c>, <c>ALL</c>.
        /// </summary>
        [QueryParameter("totais")]
        public string Totais { get; set; }

        /// <summary>ID legado dos donos.</summary>
        [QueryParameter("ids_legado_donos")]
        public int? IdsLegadoDonos { get; set; }

        /// <summary>ID legado dos clientes.</summary>
        [QueryParameter("ids_legado_clientes")]
        public int? IdsLegadoClientes { get; set; }

        /// <summary>ID legado dos produtos.</summary>
        [QueryParameter("ids_legado_produtos")]
        public int? IdsLegadoProdutos { get; set; }

        /// <summary>ID legado das categorias.</summary>
        [QueryParameter("ids_legado_categorias")]
        public int? IdsLegadoCategorias { get; set; }
    }
}
