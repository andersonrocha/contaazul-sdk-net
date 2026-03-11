using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Apis
{
    public class NotasFiscaisApi
    {
        private const string NotasFiscaisEndpoint = "/v1/notas-fiscais-servico";

        private readonly IContaAzulApiClient _client;

        internal NotasFiscaisApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<NotaFiscalListResponse> GetNotasFiscaisAsync(NotaFiscalFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(NotasFiscaisEndpoint, filtro);

            return await _client.GetAsync<NotaFiscalListResponse>(endpoint, cancellationToken).ConfigureAwait(false);
        }
    }
}
