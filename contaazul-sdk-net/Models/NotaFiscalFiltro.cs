using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models
{
    public class NotaFiscalFiltro : FiltroBase
    {
        [QueryParameter("data_competencia_de")]
        public string DataCompetenciaDe { get; set; }

        [QueryParameter("data_competencia_ate")]
        public string DataCompetenciaAte { get; set; }

        [QueryParameter("ids")]
        public string Ids { get; set; }

        [QueryParameter("id_cliente")]
        public string IdCliente { get; set; }

        [QueryParameter("numero_venda")]
        public int? NumeroVenda { get; set; }

        [QueryParameter("numero_nfse_inicial")]
        public int? NumeroNfseInicial { get; set; }

        [QueryParameter("numero_nfse_final")]
        public int? NumeroNfseFinal { get; set; }

        [QueryParameter("numero_rps_inicial")]
        public int? NumeroRpsInicial { get; set; }

        [QueryParameter("numero_rps_final")]
        public int? NumeroRpsFinal { get; set; }

        [QueryParameter("status")]
        public string Status { get; set; }

        [QueryParameter("tipo_negociacao")]
        public string TipoNegociacao { get; set; }
    }
}
