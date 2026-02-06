using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models
{
    public class VendaFiltro : FiltroBase
    {
        [QueryParameter("campo_ordenado_ascendente")]
        public string CampoOrdenadoAscendente { get; set; }

        [QueryParameter("campo_ordenado_descendente")]
        public string CampoOrdenadoDescendente { get; set; }

        [QueryParameter("termo_busca")]
        public string TermoBusca { get; set; }

        [QueryParameter("data_inicio")]
        public string DataInicio { get; set; }

        [QueryParameter("data_fim")]
        public string DataFim { get; set; }

        [QueryParameter("data_criacao_de")]
        public string DataCriacaoDe { get; set; }

        [QueryParameter("data_criacao_ate")]
        public string DataCriacaoAte { get; set; }

        [QueryParameter("data_alteracao_de")]
        public string DataAlteracaoDe { get; set; }

        [QueryParameter("data_alteracao_ate")]
        public string DataAlteracaoAte { get; set; }

        [QueryParameter("ids_vendedores")]
        public string IdsVendedores { get; set; }

        [QueryParameter("ids_clientes")]
        public string IdsClientes { get; set; }

        [QueryParameter("ids_natureza_operacao")]
        public string IdsNaturezaOperacao { get; set; }

        [QueryParameter("situacoes")]
        public string Situacoes { get; set; }

        [QueryParameter("tipos")]
        public string Tipos { get; set; }

        [QueryParameter("origens")]
        public string Origens { get; set; }

        [QueryParameter("numeros")]
        public int? Numeros { get; set; }

        [QueryParameter("ids_categorias")]
        public string IdsCategorias { get; set; }

        [QueryParameter("ids_produtos")]
        public string IdsProdutos { get; set; }

        [QueryParameter("pendente")]
        public bool? Pendente { get; set; }

        [QueryParameter("totais")]
        public string Totais { get; set; }

        [QueryParameter("ids_legado_donos")]
        public int? IdsLegadoDonos { get; set; }

        [QueryParameter("ids_legado_clientes")]
        public int? IdsLegadoClientes { get; set; }

        [QueryParameter("ids_legado_produtos")]
        public int? IdsLegadoProdutos { get; set; }

        [QueryParameter("ids_legado_categorias")]
        public int? IdsLegadoCategorias { get; set; }
    }
}

