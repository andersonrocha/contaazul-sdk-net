using System;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models.Produtos;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Produtos (inventário) do ContaAzul: catálogo, categorias e tabelas fiscais.
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Produtos"/>.
    /// </summary>
    public class ProdutosApi
    {
        private const string ProdutosEndpoint = "/v1/produtos";

        private readonly IContaAzulApiClient _client;

        internal ProdutosApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Lista os produtos cadastrados de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtro">Filtros de busca. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resumo dos produtos com os itens e o total de itens.</returns>
        public async Task<ResumoDeProdutos> ObterProdutosAsync(ProdutoFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(ProdutosEndpoint, filtro);

            return await _client.GetAsync<ResumoDeProdutos>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera os detalhes completos de um produto específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do produto (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Detalhe completo do produto.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<Produto> ObterProdutoPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<Produto>(
                $"{ProdutosEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="produto">Dados do produto a ser criado. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Produto criado.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="produto"/> é nulo.</exception>
        public async Task<Produto> CriarProdutoAsync(CriacaoProduto produto, CancellationToken cancellationToken = default)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            return await _client.PostAsync<CriacaoProduto, Produto>(
                ProdutosEndpoint, produto, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Atualiza parcialmente um produto existente (PATCH). Apenas os campos informados são alterados.
        /// </summary>
        /// <param name="id">ID do produto (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="produto">Campos a serem atualizados. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="id"/> é nulo/vazio ou <paramref name="produto"/> é nulo.
        /// </exception>
        public async Task AtualizarParcialmenteProdutoAsync(string id, AtualizacaoParcialProduto produto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            await _client.PatchAsync(
                $"{ProdutosEndpoint}/{Uri.EscapeDataString(id)}", produto, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Exclui um produto pelo seu ID.
        /// </summary>
        /// <param name="id">ID do produto (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task DeletarProdutoPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _client.DeleteAsync(
                $"{ProdutosEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista as categorias de produtos cadastradas (<c>GET /v1/produtos/categorias</c>).
        /// </summary>
        /// <param name="filtro">Filtros de paginação e busca textual. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        public async Task<CategoriasDeProduto> ObterCategoriasAsync(BuscaTextualFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint($"{ProdutosEndpoint}/categorias", filtro);

            return await _client.GetAsync<CategoriasDeProduto>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista os CESTs (Código Especificador da Substituição Tributária) (<c>GET /v1/produtos/cest</c>).
        /// </summary>
        /// <param name="filtro">Filtros de paginação e busca textual. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        public async Task<CESTsDeProduto> ObterCestsAsync(BuscaTextualFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint($"{ProdutosEndpoint}/cest", filtro);

            return await _client.GetAsync<CESTsDeProduto>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista os NCMs (Nomenclatura Comum do Mercosul) (<c>GET /v1/produtos/ncm</c>).
        /// </summary>
        /// <param name="filtro">Filtros de paginação e busca textual. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        public async Task<NCMsDeProduto> ObterNcmsAsync(BuscaTextualFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint($"{ProdutosEndpoint}/ncm", filtro);

            return await _client.GetAsync<NCMsDeProduto>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista as unidades de medida disponíveis para produtos (<c>GET /v1/produtos/unidades-medida</c>).
        /// </summary>
        /// <param name="filtro">Filtros de paginação e busca textual. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        public async Task<UnidadesDeMedidaDeProduto> ObterUnidadesMedidaAsync(BuscaTextualFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint($"{ProdutosEndpoint}/unidades-medida", filtro);

            return await _client.GetAsync<UnidadesDeMedidaDeProduto>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista as marcas de e-commerce (<c>GET /v1/produtos/ecommerce-marcas</c>).
        /// <para>
        /// A API real exige um <c>busca_textual</c> não vazio no filtro; sem ele o endpoint
        /// retorna HTTP 400 ("filtros inválidos").
        /// </para>
        /// </summary>
        /// <param name="filtro">Filtros de paginação, ordenação e busca textual. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        public async Task<MarcaDeEcommerce> ObterMarcasEcommerceAsync(MarcaEcommerceFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint($"{ProdutosEndpoint}/ecommerce-marcas", filtro);

            return await _client.GetAsync<MarcaDeEcommerce>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista as categorias de e-commerce (<c>GET /v1/produtos/ecommerce-categorias</c>).
        /// Este endpoint aceita apenas busca textual (sem paginação) e a API real exige um termo
        /// não vazio — uma chamada sem <paramref name="buscaTextual"/> retorna HTTP 400.
        /// </summary>
        /// <param name="buscaTextual">Busca textual pela descrição da categoria. Obrigatório.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="buscaTextual"/> é nulo ou vazio.</exception>
        public async Task<ProdutoEcommerceCategoria> ObterCategoriasEcommerceAsync(string buscaTextual, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(buscaTextual))
            {
                throw new ArgumentNullException(nameof(buscaTextual));
            }

            var endpoint = $"{ProdutosEndpoint}/ecommerce-categorias?busca_textual={Uri.EscapeDataString(buscaTextual)}";

            return await _client.GetAsync<ProdutoEcommerceCategoria>(endpoint, cancellationToken).ConfigureAwait(false);
        }
    }
}
