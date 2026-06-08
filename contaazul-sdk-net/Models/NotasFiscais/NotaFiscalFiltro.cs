using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models.NotasFiscais
{
    /// <summary>
    /// Filtros para consulta de notas fiscais de produto (NF-e) — <c>GET /v1/notas-fiscais</c>.
    /// <para>
    /// <see cref="DataInicial"/> e <see cref="DataFinal"/> são obrigatórios pela API.
    /// </para>
    /// </summary>
    public class NotaFiscalFiltro : FiltroBase
    {
        /// <summary>Data inicial no formato <c>YYYY-MM-DD</c>. Obrigatório.</summary>
        [QueryParameter("data_inicial")]
        public string DataInicial { get; set; }

        /// <summary>Data final no formato <c>YYYY-MM-DD</c>. Obrigatório.</summary>
        [QueryParameter("data_final")]
        public string DataFinal { get; set; }

        /// <summary>Documento do tomador (ex.: CPF/CNPJ).</summary>
        [QueryParameter("documento_tomador")]
        public string DocumentoTomador { get; set; }

        /// <summary>Número da nota fiscal.</summary>
        [QueryParameter("numero_nota")]
        public string NumeroNota { get; set; }

        /// <summary>ID da venda (UUID).</summary>
        [QueryParameter("id_venda")]
        public string IdVenda { get; set; }
    }
}
