using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.Pessoas;

namespace ContaAzul.Sdk.Net.Apis
{
    public class PessoasApi
    {
        private const string PessoasEndpoint = "/v1/pessoas";

        private readonly IContaAzulApiClient _client;

        internal PessoasApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<PessoaListResponse> GetPessoasAsync(PessoaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(PessoasEndpoint, filtro);

            return await _client.GetAsync<PessoaListResponse>(endpoint, cancellationToken).ConfigureAwait(false);
        }
    }
}

