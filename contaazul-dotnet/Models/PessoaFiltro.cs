using contaazul_dotnet.Attributes;

namespace contaazul_dotnet.Models
{
    public class PessoaFiltro : FiltroBase
    {
        [QueryParameter("tipo_ordenacao")]
        public string TipoOrdenacao { get; set; }

        [QueryParameter("ordem_ordenacao")]
        public string OrdemOrdenacao { get; set; }

        [QueryParameter("busca")]
        public string Busca { get; set; }

        [QueryParameter("ids")]
        public string Ids { get; set; }

        [QueryParameter("documentos")]
        public string Documentos { get; set; }

        [QueryParameter("paises")]
        public string Paises { get; set; }

        [QueryParameter("cidades")]
        public string Cidades { get; set; }

        [QueryParameter("ufs")]
        public string Ufs { get; set; }

        [QueryParameter("codigos_pessoa")]
        public string CodigosPessoa { get; set; }

        [QueryParameter("emails")]
        public string Emails { get; set; }

        [QueryParameter("tipos_pessoa")]
        public string TiposPessoa { get; set; }

        [QueryParameter("nomes")]
        public string Nomes { get; set; }

        [QueryParameter("telefones")]
        public string Telefones { get; set; }

        [QueryParameter("data_criacao_inicio")]
        public string DataCriacaoInicio { get; set; }

        [QueryParameter("data_criacao_fim")]
        public string DataCriacaoFim { get; set; }

        [QueryParameter("data_alteracao_de")]
        public string DataAlteracaoDe { get; set; }

        [QueryParameter("data_alteracao_ate")]
        public string DataAlteracaoAte { get; set; }

        [QueryParameter("tipo_perfil")]
        public string TipoPerfil { get; set; }

        [QueryParameter("com_endereco")]
        public bool? ComEndereco { get; set; }
    }
}


