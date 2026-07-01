using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Protocolos;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Protocolos do ContaAzul: acompanhamento do processamento assíncrono de eventos
    /// financeiros enviados ao ERP.
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Protocolos"/>.
    /// </summary>
    public class ProtocolosApi
    {
        private const string ProtocoloEndpoint = "/v1/protocolo";

        private readonly IContaAzulApiClient _client;

        internal ProtocolosApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Consulta um protocolo pelo seu identificador, retornando o status do processamento
        /// (<c>PENDING</c>, <c>SUCCESS</c> ou <c>ERROR</c>) do evento financeiro associado.
        /// </summary>
        /// <param name="id">ID do protocolo (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Detalhe do protocolo.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<Protocolo> ObterProtocoloPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<Protocolo>(
                $"{ProtocoloEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }
    }
}
