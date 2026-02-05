using System;
using System.Threading;
using System.Threading.Tasks;
using contaazul_dotnet.Helpers;
using contaazul_dotnet.Models;

namespace contaazul_dotnet
{
    public class PessoaApi
    {
        private const string PessoasEndpoint = "/v1/pessoas";
        
        private readonly ContaAzulApiClient _client;

        internal PessoaApi(ContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<PessoaListResponse> GetPessoasAsync(PessoaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var queryString = QueryStringBuilder.BuildQueryString(filtro);
            var endpoint = string.IsNullOrEmpty(queryString) 
                ? PessoasEndpoint 
                : $"{PessoasEndpoint}?{queryString}";

            return await _client.GetAsync<PessoaListResponse>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Pessoa> GetPessoaByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<Pessoa>($"{PessoasEndpoint}/{id}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<Pessoa> CreatePessoaAsync(Pessoa pessoa, CancellationToken cancellationToken = default)
        {
            if (pessoa == null)
            {
                throw new ArgumentNullException(nameof(pessoa));
            }

            return await _client.PostAsync<Pessoa, Pessoa>(PessoasEndpoint, pessoa, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Pessoa> UpdatePessoaAsync(string id, Pessoa pessoa, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (pessoa == null)
            {
                throw new ArgumentNullException(nameof(pessoa));
            }

            return await _client.PutAsync<Pessoa, Pessoa>($"{PessoasEndpoint}/{id}", pessoa, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeletePessoaAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _client.DeleteAsync($"{PessoasEndpoint}/{id}", cancellationToken).ConfigureAwait(false);
        }
    }
}

