using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Models
{
    public abstract class FiltroBase
    {
        private int _pagina = 1;
        private int _tamanhoPagina = 10;

        [QueryParameter("pagina")]
        public int? Pagina
        {
            get => _pagina;
            set => _pagina = value ?? 1;
        }

        [QueryParameter("tamanho_pagina")]
        public int? TamanhoPagina
        {
            get => _tamanhoPagina;
            set => _tamanhoPagina = value ?? 10;
        }
    }
}
