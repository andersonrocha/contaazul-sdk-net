using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Cobranças do ContaAzul (contas a receber).
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Cobrancas"/>.
    /// </summary>
    public class CobrancasApi
    {
        private const string Endpoint = "/v1/financeiro/eventos-financeiros/contas-a-receber";

        private readonly IContaAzulApiClient _client;

        internal CobrancasApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>Gera uma nova cobrança.</summary>
        /// <param name="cobranca">Dados da cobrança. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <exception cref="ArgumentNullException">Quando <paramref name="cobranca"/> é nulo.</exception>
        public async Task<GerarCobrancaResponse> CriarCobrancaAsync(GerarCobrancaRequest cobranca, CancellationToken cancellationToken = default)
        {
            if (cobranca == null)
            {
                throw new ArgumentNullException(nameof(cobranca));
            }

            return await _client.PostAsync<GerarCobrancaRequest, GerarCobrancaResponse>(
                $"{Endpoint}/gerar-cobranca", cobranca, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Consulta uma cobrança pelo seu identificador.</summary>
        /// <param name="idCobranca">Identificador da cobrança (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <exception cref="ArgumentNullException">Quando <paramref name="idCobranca"/> é nulo ou vazio.</exception>
        public async Task<GerarCobrancaResponse> ObterCobrancaPorIdAsync(string idCobranca, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(idCobranca))
            {
                throw new ArgumentNullException(nameof(idCobranca));
            }

            return await _client.GetAsync<GerarCobrancaResponse>(
                $"{Endpoint}/cobranca/{Uri.EscapeDataString(idCobranca)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Cancela/deleta uma cobrança pelo seu identificador.</summary>
        /// <param name="idCobranca">Identificador da cobrança (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <exception cref="ArgumentNullException">Quando <paramref name="idCobranca"/> é nulo ou vazio.</exception>
        public async Task DeletarCobrancaPorIdAsync(string idCobranca, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(idCobranca))
            {
                throw new ArgumentNullException(nameof(idCobranca));
            }

            await _client.DeleteAsync(
                $"{Endpoint}/cobranca/{Uri.EscapeDataString(idCobranca)}", cancellationToken).ConfigureAwait(false);
        }
    }
}
