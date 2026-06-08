using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.Vendas;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Vendas do ContaAzul.
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Vendas"/>.
    /// </summary>
    public class VendasApi
    {
        private const string VendasEndpoint = "/v1/venda";

        private readonly IContaAzulApiClient _client;

        internal VendasApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Retorna a lista de vendedores cadastrados na Conta Azul.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Lista de vendedores.</returns>
        public async Task<List<Vendedor>> GetVendedoresAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAsync<List<Vendedor>>(
                $"{VendasEndpoint}/vendedores", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Busca as vendas de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtro">Filtros de busca. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta paginada com a lista de vendas.</returns>
        public async Task<VendaListResponse> GetVendasAsync(VendaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint($"{VendasEndpoint}/busca", filtro);

            return await _client.GetAsync<VendaListResponse>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera os detalhes de uma venda específica por ID (UUID ou id legado).
        /// </summary>
        /// <param name="id">ID (UUID) ou id legado da venda. Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Detalhes da venda.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<ObterVendaResponse> GetVendaByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<ObterVendaResponse>(
                $"{VendasEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria uma nova venda.
        /// </summary>
        /// <param name="venda">Dados da venda a ser criada. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta da criação da venda.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="venda"/> é nulo.</exception>
        public async Task<CriacaoVendaResponse> CriarVendaAsync(CriacaoVendaRequest venda, CancellationToken cancellationToken = default)
        {
            if (venda == null)
            {
                throw new ArgumentNullException(nameof(venda));
            }

            return await _client.PostAsync<CriacaoVendaRequest, CriacaoVendaResponse>(
                VendasEndpoint, venda, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Atualiza uma venda existente por ID.
        /// </summary>
        /// <param name="id">ID (UUID) da venda a ser editada. Não pode ser nulo ou vazio.</param>
        /// <param name="venda">Dados da venda para edição. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta da edição da venda.</returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="id"/> é nulo/vazio ou <paramref name="venda"/> é nulo.
        /// </exception>
        public async Task<VendaEditadaResponse> AtualizarVendaAsync(string id, VendaParaEdicaoRequest venda, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (venda == null)
            {
                throw new ArgumentNullException(nameof(venda));
            }

            return await _client.PutAsync<VendaParaEdicaoRequest, VendaEditadaResponse>(
                $"{VendasEndpoint}/{Uri.EscapeDataString(id)}", venda, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gera e retorna o PDF de uma venda.
        /// </summary>
        /// <param name="id">ID (UUID) ou id legado da venda. Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Os bytes do PDF da venda.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<byte[]> ImprimirVendaAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetBytesAsync(
                $"{VendasEndpoint}/{Uri.EscapeDataString(id)}/imprimir", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Exclui várias vendas de uma só vez (lote de 1 a 10 IDs).
        /// </summary>
        /// <param name="exclusao">Lista de IDs das vendas a serem excluídas. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resumo com as quantidades excluídas e ignoradas.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="exclusao"/> é nulo.</exception>
        public async Task<ExclusaoResponse> ExcluirVendasEmLoteAsync(ExclusaoLote exclusao, CancellationToken cancellationToken = default)
        {
            if (exclusao == null)
            {
                throw new ArgumentNullException(nameof(exclusao));
            }

            return await _client.PostAsync<ExclusaoLote, ExclusaoResponse>(
                $"{VendasEndpoint}/exclusao-lote", exclusao, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retorna os itens de uma venda, paginados.
        /// </summary>
        /// <param name="idVenda">ID (UUID) da venda. Não pode ser nulo ou vazio.</param>
        /// <param name="pagina">Número da página (padrão 1).</param>
        /// <param name="tamanhoPagina">Tamanho da página (padrão 10). Valores aceitos: 10, 20, 50, 100, 200, 500, 1000.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta paginada com os itens da venda.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="idVenda"/> é nulo ou vazio.</exception>
        public async Task<ItensPaginados> GetItensVendaAsync(string idVenda, int pagina = 1, int tamanhoPagina = 10, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(idVenda))
            {
                throw new ArgumentNullException(nameof(idVenda));
            }

            var filtro = new PaginacaoFiltro { Pagina = pagina, TamanhoPagina = tamanhoPagina };
            var endpoint = QueryStringBuilder.BuildEndpoint(
                $"{VendasEndpoint}/{Uri.EscapeDataString(idVenda)}/itens", filtro);

            return await _client.GetAsync<ItensPaginados>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retorna o próximo número de venda disponível segundo a numeração do ERP.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>O próximo número de venda, ou <c>null</c> quando indisponível.</returns>
        public async Task<long?> GetProximoNumeroAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAsync<long?>(
                $"{VendasEndpoint}/proximo-numero", cancellationToken).ConfigureAwait(false);
        }
    }
}
