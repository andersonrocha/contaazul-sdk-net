using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.Pessoas;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Pessoas (clientes, fornecedores e transportadoras) do ContaAzul.
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Pessoas"/>.
    /// </summary>
    public class PessoasApi
    {
        private const string PessoasEndpoint = "/v1/pessoas";

        private readonly IContaAzulApiClient _client;

        internal PessoasApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Lista as pessoas cadastradas de acordo com os filtros informados.
        /// </summary>
        /// <param name="filtro">Filtros de busca. Opcional.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta com os itens resumidos e o total de itens.</returns>
        public async Task<PessoaListResponse> ObterPessoasAsync(PessoaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(PessoasEndpoint, filtro);

            return await _client.GetAsync<PessoaListResponse>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera os detalhes de uma pessoa específica pelo seu ID (UUID).
        /// </summary>
        /// <param name="id">ID da pessoa (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Cadastro completo da pessoa.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="id"/> é nulo ou vazio.</exception>
        public async Task<Pessoa> ObterPessoaPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<Pessoa>(
                $"{PessoasEndpoint}/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recupera os detalhes de uma pessoa pelo seu ID legado (API V1).
        /// </summary>
        /// <param name="idLegado">ID legado da pessoa. Não pode ser nulo ou vazio.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Cadastro completo da pessoa.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="idLegado"/> é nulo ou vazio.</exception>
        public async Task<Pessoa> ObterPessoaPorLegadoIdAsync(string idLegado, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(idLegado))
            {
                throw new ArgumentNullException(nameof(idLegado));
            }

            return await _client.GetAsync<Pessoa>(
                $"{PessoasEndpoint}/legado/{Uri.EscapeDataString(idLegado)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retorna os dados cadastrais da empresa vinculada ao token de autenticação (conta conectada).
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Dados da empresa conectada.</returns>
        public async Task<Empresa> ObterEmpresaConectadaAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAsync<Empresa>(
                $"{PessoasEndpoint}/conta-conectada", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria uma nova pessoa.
        /// </summary>
        /// <param name="pessoa">Dados da pessoa a ser criada. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resumo da pessoa criada.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="pessoa"/> é nulo.</exception>
        public async Task<ResumoPessoa> CriarPessoaAsync(PessoaRequest pessoa, CancellationToken cancellationToken = default)
        {
            if (pessoa == null)
            {
                throw new ArgumentNullException(nameof(pessoa));
            }

            return await _client.PostAsync<PessoaRequest, ResumoPessoa>(
                PessoasEndpoint, pessoa, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Atualiza integralmente o cadastro de uma pessoa existente (PUT).
        /// </summary>
        /// <param name="id">ID da pessoa (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="pessoa">Dados completos da pessoa. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resumo da pessoa atualizada.</returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="id"/> é nulo/vazio ou <paramref name="pessoa"/> é nulo.
        /// </exception>
        public async Task<ResumoPessoa> AtualizarPessoaAsync(string id, PessoaRequest pessoa, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (pessoa == null)
            {
                throw new ArgumentNullException(nameof(pessoa));
            }

            return await _client.PutAsync<PessoaRequest, ResumoPessoa>(
                $"{PessoasEndpoint}/{Uri.EscapeDataString(id)}", pessoa, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Atualiza parcialmente o cadastro de uma pessoa existente (PATCH).
        /// Apenas os campos informados são alterados.
        /// </summary>
        /// <param name="id">ID da pessoa (UUID). Não pode ser nulo ou vazio.</param>
        /// <param name="pessoa">Campos a serem atualizados. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="id"/> é nulo/vazio ou <paramref name="pessoa"/> é nulo.
        /// </exception>
        public async Task AtualizarParcialmentePessoaAsync(string id, AtualizacaoParcialPessoa pessoa, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (pessoa == null)
            {
                throw new ArgumentNullException(nameof(pessoa));
            }

            await _client.PatchAsync(
                $"{PessoasEndpoint}/{Uri.EscapeDataString(id)}", pessoa, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Ativa em lote um conjunto de pessoas (máximo de 10 IDs por requisição).
        /// </summary>
        /// <param name="pessoas">IDs das pessoas a serem ativadas. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resultado do processamento em lote.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="pessoas"/> é nulo.</exception>
        /// <exception cref="ArgumentException">Lançada quando a lista de UUIDs está vazia.</exception>
        public async Task<List<StatusPessoasEmLoteResultado>> AtivarPessoasEmLoteAsync(PessoasEmLoteRequest pessoas, CancellationToken cancellationToken = default)
        {
            ValidarLote(pessoas);

            return await _client.PostAsync<PessoasEmLoteRequest, List<StatusPessoasEmLoteResultado>>(
                $"{PessoasEndpoint}/ativar", pessoas, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Inativa em lote um conjunto de pessoas (máximo de 10 IDs por requisição).
        /// </summary>
        /// <param name="pessoas">IDs das pessoas a serem inativadas. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resultado do processamento em lote.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="pessoas"/> é nulo.</exception>
        /// <exception cref="ArgumentException">Lançada quando a lista de UUIDs está vazia.</exception>
        public async Task<List<StatusPessoasEmLoteResultado>> InativarPessoasEmLoteAsync(PessoasEmLoteRequest pessoas, CancellationToken cancellationToken = default)
        {
            ValidarLote(pessoas);

            return await _client.PostAsync<PessoasEmLoteRequest, List<StatusPessoasEmLoteResultado>>(
                $"{PessoasEndpoint}/inativar", pessoas, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Exclui pessoas em lote a partir de seus identificadores.
        /// </summary>
        /// <param name="pessoas">IDs das pessoas a serem excluídas. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="pessoas"/> é nulo.</exception>
        /// <exception cref="ArgumentException">Lançada quando a lista de UUIDs está vazia.</exception>
        public async Task ExcluirPessoasEmLoteAsync(PessoasEmLoteRequest pessoas, CancellationToken cancellationToken = default)
        {
            ValidarLote(pessoas);

            await _client.PostAsync(
                $"{PessoasEndpoint}/excluir", pessoas, cancellationToken).ConfigureAwait(false);
        }

        private static void ValidarLote(PessoasEmLoteRequest pessoas)
        {
            if (pessoas == null)
            {
                throw new ArgumentNullException(nameof(pessoas));
            }

            if (pessoas.Uuids == null || pessoas.Uuids.Count == 0)
            {
                throw new ArgumentException("Uuids é obrigatório.", nameof(pessoas));
            }
        }
    }
}
