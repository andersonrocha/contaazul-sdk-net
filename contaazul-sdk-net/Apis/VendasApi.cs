using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;

namespace ContaAzul.Sdk.Net.Apis
{
    public class VendasApi
    {
        private const string VendasEndpoint = "/v1/venda";

        private readonly IContaAzulApiClient _client;

        internal VendasApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<VendaListResponse> GetVendasAsync(VendaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(VendasEndpoint, filtro);

            return await _client.GetAsync<VendaListResponse>($"{endpoint}/busca", cancellationToken).ConfigureAwait(false);
        }
    }
}
