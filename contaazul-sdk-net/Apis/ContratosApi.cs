using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.Contratos;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Contratos (vendas agendadas/recorrentes) do ContaAzul.
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Contratos"/>.
    /// </summary>
    public class ContratosApi
    {
        private const string ContratosEndpoint = "/v1/contratos";

        private readonly IContaAzulApiClient _client;

        internal ContratosApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Lista os contratos de acordo com os filtros informados.
        /// <para>
        /// <see cref="ContratoFiltro.DataInicio"/> e <see cref="ContratoFiltro.DataFim"/>
        /// são obrigatórios pela API.
        /// </para>
        /// </summary>
        /// <param name="filtro">Filtros de busca. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta paginada com a lista de contratos.</returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="filtro"/> é nulo ou quando
        /// <see cref="ContratoFiltro.DataInicio"/>/<see cref="ContratoFiltro.DataFim"/> não foram informados.
        /// </exception>
        public async Task<ContratosFiltroResposta> ListarContratosAsync(ContratoFiltro filtro, CancellationToken cancellationToken = default)
        {
            if (filtro == null)
            {
                throw new ArgumentNullException(nameof(filtro));
            }

            if (string.IsNullOrWhiteSpace(filtro.DataInicio))
            {
                throw new ArgumentNullException(nameof(filtro), "DataInicio é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(filtro.DataFim))
            {
                throw new ArgumentNullException(nameof(filtro), "DataFim é obrigatório.");
            }

            var endpoint = QueryStringBuilder.BuildEndpoint(ContratosEndpoint, filtro);

            return await _client.GetAsync<ContratosFiltroResposta>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera os detalhes de um contrato específico por ID.
        /// </summary>
        /// <param name="id">ID do contrato (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Detalhes do contrato.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<ContratoResumo> ObterContratoPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<ContratoResumo>(
                $"{ContratosEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retorna o próximo número de contrato disponível a ser utilizado na criação.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>O próximo número de contrato.</returns>
        public async Task<int> ObterProximoNumeroAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAsync<int>(
                $"{ContratosEndpoint}/proximo-numero", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria um novo contrato (venda agendada/recorrente).
        /// </summary>
        /// <param name="contrato">Dados do contrato a ser criado. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resumo do contrato criado (IDs gerados).</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="contrato"/> é nulo.</exception>
        public async Task<ResumoCriacaoContrato> CriarContratoAsync(CriarContrato contrato, CancellationToken cancellationToken = default)
        {
            if (contrato == null)
            {
                throw new ArgumentNullException(nameof(contrato));
            }

            return await _client.PostAsync<CriarContrato, ResumoCriacaoContrato>(
                ContratosEndpoint, contrato, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove permanentemente um contrato, cancelando todas as vendas associadas
        /// (agendadas e efetivadas). Contratos em reajuste de valor não podem ser removidos.
        /// </summary>
        /// <param name="id">ID do contrato (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task RemoverContratoAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _client.DeleteAsync(
                $"{ContratosEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Encerra um contrato ativo. O contrato é desativado e não gera novas cobranças.
        /// Contratos em reajuste de valor não podem ser encerrados.
        /// </summary>
        /// <param name="id">ID do contrato (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task EncerrarContratoAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _client.PostAsync(
                $"{ContratosEndpoint}/{Uri.EscapeDataString(id)}/encerrar", cancellationToken).ConfigureAwait(false);
        }
    }
}
