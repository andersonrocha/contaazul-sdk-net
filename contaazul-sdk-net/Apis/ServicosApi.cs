using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models.Servicos;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Serviços do ContaAzul: catálogo de serviços da empresa.
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Servicos"/>.
    /// </summary>
    public class ServicosApi
    {
        private const string ServicosEndpoint = "/v1/servicos";

        private readonly IContaAzulApiClient _client;

        internal ServicosApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Lista os serviços cadastrados de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtro">Filtros de busca. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta paginada com os serviços.</returns>
        public async Task<ServicosPorFiltro> ObterServicosAsync(ServicoFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(ServicosEndpoint, filtro);

            return await _client.GetAsync<ServicosPorFiltro>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera os detalhes de um serviço específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do serviço (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Detalhe completo do serviço.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<Servico> ObterServicoPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<Servico>(
                $"{ServicosEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria um novo serviço.
        /// </summary>
        /// <param name="servico">Dados do serviço a ser criado. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Serviço criado.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="servico"/> é nulo.</exception>
        public async Task<Servico> CriarServicoAsync(CriarServico servico, CancellationToken cancellationToken = default)
        {
            if (servico == null)
            {
                throw new ArgumentNullException(nameof(servico));
            }

            return await _client.PostAsync<CriarServico, Servico>(
                ServicosEndpoint, servico, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Atualiza parcialmente um serviço existente (PATCH). Apenas os campos informados são alterados.
        /// </summary>
        /// <param name="id">ID do serviço (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="servico">Campos a serem atualizados. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="id"/> é nulo/vazio ou <paramref name="servico"/> é nulo.
        /// </exception>
        public async Task AtualizarParcialmenteServicoAsync(string id, AtualizacaoParcialServico servico, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (servico == null)
            {
                throw new ArgumentNullException(nameof(servico));
            }

            await _client.PatchAsync(
                $"{ServicosEndpoint}/{Uri.EscapeDataString(id)}", servico, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Exclui serviços em lote a partir de seus identificadores (legado).
        /// </summary>
        /// <param name="parametros">IDs dos serviços a serem excluídos. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="parametros"/> é nulo.</exception>
        /// <exception cref="ArgumentException">Lançada quando a lista de IDs está vazia.</exception>
        public async Task DeletarServicosEmLoteAsync(ParametrosParaDeletarServicosEmLote parametros, CancellationToken cancellationToken = default)
        {
            if (parametros == null)
            {
                throw new ArgumentNullException(nameof(parametros));
            }

            if (parametros.Ids == null || parametros.Ids.Count == 0)
            {
                throw new ArgumentException("Ids é obrigatório.", nameof(parametros));
            }

            await _client.DeleteAsync(ServicosEndpoint, parametros, cancellationToken).ConfigureAwait(false);
        }
    }
}
