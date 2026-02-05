using System;
using System.Threading;
using System.Threading.Tasks;
using contaazul_dotnet.Helpers;
using contaazul_dotnet.Models;

namespace contaazul_dotnet
{
    public class VendaApi
    {
        private const string VendasEndpoint = "/v1/venda";
        
        private readonly ContaAzulApiClient _client;

        internal VendaApi(ContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<VendaListResponse> GetVendasAsync(VendaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var queryString = QueryStringBuilder.BuildQueryString(filtro);
            var endpoint = string.IsNullOrEmpty(queryString) 
                ? VendasEndpoint 
                : $"{VendasEndpoint}?{queryString}";

            return await _client.GetAsync<VendaListResponse>($"{endpoint}/busca", cancellationToken).ConfigureAwait(false);
        }
    }
}
