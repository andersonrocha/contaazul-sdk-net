using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models.Orcamentos;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Orçamentos do ContaAzul: propostas comerciais (listar, criar, detalhar e excluir em lote).
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Orcamentos"/>.
    /// </summary>
    public class OrcamentosApi
    {
        private const string OrcamentosEndpoint = "/v1/orcamentos";

        private readonly IContaAzulApiClient _client;

        internal OrcamentosApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Lista os orçamentos de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtro">Filtros de busca. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta com os orçamentos e o total de itens.</returns>
        public async Task<ListagemOrcamentosPorFiltro> ObterOrcamentosAsync(OrcamentoFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(OrcamentosEndpoint, filtro);

            return await _client.GetAsync<ListagemOrcamentosPorFiltro>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera os detalhes de um orçamento específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do orçamento (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Detalhe completo do orçamento.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<Orcamento> ObterOrcamentoPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<Orcamento>(
                $"{OrcamentosEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria um novo orçamento.
        /// </summary>
        /// <param name="orcamento">Dados do orçamento a ser criado. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resumo com o ID do orçamento criado.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="orcamento"/> é nulo.</exception>
        public async Task<ResumoCriacaoOrcamento> CriarOrcamentoAsync(CriarOrcamento orcamento, CancellationToken cancellationToken = default)
        {
            if (orcamento == null)
            {
                throw new ArgumentNullException(nameof(orcamento));
            }

            return await _client.PostAsync<CriarOrcamento, ResumoCriacaoOrcamento>(
                OrcamentosEndpoint, orcamento, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Exclui orçamentos em lote a partir de seus identificadores (máximo de 10 por requisição).
        /// </summary>
        /// <param name="exclusao">IDs dos orçamentos a serem excluídos. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="exclusao"/> é nulo.</exception>
        /// <exception cref="ArgumentException">Lançada quando a lista de IDs está vazia.</exception>
        public async Task ExcluirOrcamentosEmLoteAsync(ExclusaoLoteOrcamento exclusao, CancellationToken cancellationToken = default)
        {
            if (exclusao == null)
            {
                throw new ArgumentNullException(nameof(exclusao));
            }

            if (exclusao.Ids == null || exclusao.Ids.Count == 0)
            {
                throw new ArgumentException("Ids é obrigatório.", nameof(exclusao));
            }

            await _client.DeleteAsync(OrcamentosEndpoint, exclusao, cancellationToken).ConfigureAwait(false);
        }
    }
}
