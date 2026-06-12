using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Baixas do ContaAzul (conciliação de pagamentos de parcelas).
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Baixas"/>.
    /// </summary>
    public class BaixasApi
    {
        private const string ParcelasEndpoint = "/v1/financeiro/eventos-financeiros/parcelas";

        private readonly IContaAzulApiClient _client;

        internal BaixasApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>Cria uma nova baixa vinculada a uma parcela.</summary>
        /// <param name="parcelaId">ID da parcela (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="baixa">Dados da baixa. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        public async Task<BaixaCriacaoResponse> CriarBaixaAsync(string parcelaId, BaixaCriacaoRequest baixa, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(parcelaId))
            {
                throw new ArgumentNullException(nameof(parcelaId));
            }

            if (baixa == null)
            {
                throw new ArgumentNullException(nameof(baixa));
            }

            return await _client.PostAsync<BaixaCriacaoRequest, BaixaCriacaoResponse>(
                $"{ParcelasEndpoint}/{Uri.EscapeDataString(parcelaId)}/baixa", baixa, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Lista as baixas associadas a uma parcela.</summary>
        /// <param name="parcelaId">ID da parcela (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        public async Task<List<BaixaResponse>> ListarBaixasPorParcelaAsync(string parcelaId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(parcelaId))
            {
                throw new ArgumentNullException(nameof(parcelaId));
            }

            return await _client.GetAsync<List<BaixaResponse>>(
                $"{ParcelasEndpoint}/{Uri.EscapeDataString(parcelaId)}/baixa", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Consulta uma baixa específica pelo seu identificador.</summary>
        /// <param name="baixaId">ID da baixa (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        public async Task<BaixaResponse> ObterBaixaPorIdAsync(string baixaId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(baixaId))
            {
                throw new ArgumentNullException(nameof(baixaId));
            }

            return await _client.GetAsync<BaixaResponse>(
                $"{ParcelasEndpoint}/baixa/{Uri.EscapeDataString(baixaId)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Atualiza parcialmente uma baixa.</summary>
        /// <param name="baixaId">ID da baixa (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="baixa">Campos a atualizar (<see cref="BaixaAtualizacaoRequest.Versao"/> obrigatório). Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        public async Task<BaixaCriacaoResponse> AtualizarBaixaAsync(string baixaId, BaixaAtualizacaoRequest baixa, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(baixaId))
            {
                throw new ArgumentNullException(nameof(baixaId));
            }

            if (baixa == null)
            {
                throw new ArgumentNullException(nameof(baixa));
            }

            return await _client.PatchAsync<BaixaAtualizacaoRequest, BaixaCriacaoResponse>(
                $"{ParcelasEndpoint}/baixa/{Uri.EscapeDataString(baixaId)}", baixa, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Exclui uma baixa pelo seu identificador.</summary>
        /// <param name="baixaId">ID da baixa (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        public async Task DeletarBaixaAsync(string baixaId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(baixaId))
            {
                throw new ArgumentNullException(nameof(baixaId));
            }

            await _client.DeleteAsync(
                $"{ParcelasEndpoint}/baixa/{Uri.EscapeDataString(baixaId)}", cancellationToken).ConfigureAwait(false);
        }
    }
}
