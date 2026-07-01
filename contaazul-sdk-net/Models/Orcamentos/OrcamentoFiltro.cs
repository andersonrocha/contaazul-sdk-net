using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.Orcamentos
{
    /// <summary>
    /// Filtros para a listagem de orçamentos (<c>GET /v1/orcamentos</c>).
    /// Os parâmetros de múltiplos valores devem ser informados como valores separados por vírgula.
    /// </summary>
    public class OrcamentoFiltro : FiltroBase
    {
        /// <summary>
        /// Campo para ordenação ascendente (<c>DATA</c>, <c>NUMERO</c> ou <c>CLIENTE</c>).
        /// Quando informado, desconsidera <see cref="CampoOrdenadoDescendente"/>.
        /// </summary>
        [QueryParameter("campo_ordenado_ascendente")]
        public string CampoOrdenadoAscendente { get; set; }

        /// <summary>
        /// Campo para ordenação descendente (<c>DATA</c>, <c>NUMERO</c> ou <c>CLIENTE</c>).
        /// Não deve ser informado junto com <see cref="CampoOrdenadoAscendente"/>.
        /// </summary>
        [QueryParameter("campo_ordenado_descendente")]
        public string CampoOrdenadoDescendente { get; set; }

        /// <summary>Termo de busca.</summary>
        [QueryParameter("termo_busca")]
        public string TermoBusca { get; set; }

        /// <summary>Data inicial da emissão do orçamento (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_inicio")]
        public string DataInicio { get; set; }

        /// <summary>Data final da emissão do orçamento (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_fim")]
        public string DataFim { get; set; }

        /// <summary>Data de criação inicial (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_criacao_de")]
        public string DataCriacaoDe { get; set; }

        /// <summary>Data de criação final (<c>YYYY-MM-DD</c>).</summary>
        [QueryParameter("data_criacao_ate")]
        public string DataCriacaoAte { get; set; }

        /// <summary>Data de alteração inicial (<c>YYYY-MM-DDThh:mm:ss</c>).</summary>
        [QueryParameter("data_alteracao_de")]
        public string DataAlteracaoDe { get; set; }

        /// <summary>Data de alteração final (<c>YYYY-MM-DDThh:mm:ss</c>).</summary>
        [QueryParameter("data_alteracao_ate")]
        public string DataAlteracaoAte { get; set; }

        /// <summary>IDs dos vendedores (UUID), separados por vírgula.</summary>
        [QueryParameter("ids_vendedores")]
        public string IdsVendedores { get; set; }

        /// <summary>IDs dos clientes (UUID), separados por vírgula.</summary>
        [QueryParameter("ids_clientes")]
        public string IdsClientes { get; set; }

        /// <summary>IDs das naturezas de operação (UUID), separados por vírgula.</summary>
        [QueryParameter("ids_natureza_operacao")]
        public string IdsNaturezaOperacao { get; set; }

        /// <summary>IDs das categorias (UUID), separados por vírgula.</summary>
        [QueryParameter("ids_categorias")]
        public string IdsCategorias { get; set; }

        /// <summary>IDs dos produtos (UUID), separados por vírgula.</summary>
        [QueryParameter("ids_produtos")]
        public string IdsProdutos { get; set; }

        /// <summary>
        /// Situações dos orçamentos, separadas por vírgula
        /// (<c>ORCAMENTO</c>, <c>ORCAMENTO_ACEITO</c>, <c>ORCAMENTO_RECUSADO</c>).
        /// </summary>
        [QueryParameter("situacoes")]
        public string Situacoes { get; set; }

        /// <summary>Origens dos orçamentos, separadas por vírgula.</summary>
        [QueryParameter("origens")]
        public string Origens { get; set; }

        /// <summary>Números dos orçamentos, separados por vírgula.</summary>
        [QueryParameter("numeros")]
        public string Numeros { get; set; }

        /// <summary>IDs legados dos donos, separados por vírgula.</summary>
        [QueryParameter("ids_legado_donos")]
        public string IdsLegadoDonos { get; set; }

        /// <summary>IDs legados dos clientes, separados por vírgula.</summary>
        [QueryParameter("ids_legado_clientes")]
        public string IdsLegadoClientes { get; set; }

        /// <summary>IDs legados dos produtos, separados por vírgula.</summary>
        [QueryParameter("ids_legado_produtos")]
        public string IdsLegadoProdutos { get; set; }
    }
}
