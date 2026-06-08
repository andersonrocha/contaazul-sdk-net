using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models;
using ContaAzul.Sdk.Net.Models.NotasFiscais;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Notas Fiscais do ContaAzul.
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.NotasFiscais"/>.
    /// </summary>
    public class NotasFiscaisApi
    {
        private const string NotasFiscaisEndpoint = "/v1/notas-fiscais";
        private const string NotasFiscaisServicoEndpoint = "/v1/notas-fiscais-servico";

        private readonly IContaAzulApiClient _client;

        internal NotasFiscaisApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Consulta as notas fiscais de produto (NF-e) emitidas no ERP, de acordo com os filtros
        /// informados. Retorna somente NF-e com status <c>EMITIDA</c> e <c>CORRIGIDA_SUCESSO</c>.
        /// <para>
        /// <see cref="NotaFiscalFiltro.DataInicial"/> e <see cref="NotaFiscalFiltro.DataFinal"/>
        /// são obrigatórios pela API.
        /// </para>
        /// </summary>
        /// <param name="filtro">Filtros de busca. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta paginada com a lista de notas fiscais de produto.</returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="filtro"/> é nulo ou quando
        /// <see cref="NotaFiscalFiltro.DataInicial"/>/<see cref="NotaFiscalFiltro.DataFinal"/> não foram informados.
        /// </exception>
        public async Task<RespostaPaginada<NotaFiscal>> ObterNotasFiscaisAsync(NotaFiscalFiltro filtro, CancellationToken cancellationToken = default)
        {
            if (filtro == null)
            {
                throw new ArgumentNullException(nameof(filtro));
            }

            if (string.IsNullOrWhiteSpace(filtro.DataInicial))
            {
                throw new ArgumentNullException(nameof(filtro), "DataInicial é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(filtro.DataFinal))
            {
                throw new ArgumentNullException(nameof(filtro), "DataFinal é obrigatório.");
            }

            var endpoint = QueryStringBuilder.BuildEndpoint(NotasFiscaisEndpoint, filtro);

            return await _client.GetAsync<RespostaPaginada<NotaFiscal>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Consulta as notas fiscais de serviço (NFS-e) emitidas no ERP, de acordo com os filtros
        /// informados. Retorna NFS-e com todos os status possíveis a partir da emissão.
        /// <para>
        /// <see cref="NotaFiscalServicoFiltro.DataCompetenciaDe"/> e
        /// <see cref="NotaFiscalServicoFiltro.DataCompetenciaAte"/> são obrigatórios
        /// (intervalo máximo de 15 dias).
        /// </para>
        /// </summary>
        /// <param name="filtro">Filtros de busca. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>Resposta paginada com a lista de notas fiscais de serviço.</returns>
        /// <exception cref="ArgumentNullException">
        /// Lançada quando <paramref name="filtro"/> é nulo ou quando
        /// <see cref="NotaFiscalServicoFiltro.DataCompetenciaDe"/>/<see cref="NotaFiscalServicoFiltro.DataCompetenciaAte"/>
        /// não foram informados.
        /// </exception>
        public async Task<RespostaPaginada<NotaFiscalServico>> ObterNotasFiscaisServicoAsync(NotaFiscalServicoFiltro filtro, CancellationToken cancellationToken = default)
        {
            if (filtro == null)
            {
                throw new ArgumentNullException(nameof(filtro));
            }

            if (string.IsNullOrWhiteSpace(filtro.DataCompetenciaDe))
            {
                throw new ArgumentNullException(nameof(filtro), "DataCompetenciaDe é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(filtro.DataCompetenciaAte))
            {
                throw new ArgumentNullException(nameof(filtro), "DataCompetenciaAte é obrigatório.");
            }

            var endpoint = QueryStringBuilder.BuildEndpoint(NotasFiscaisServicoEndpoint, filtro);

            return await _client.GetAsync<RespostaPaginada<NotaFiscalServico>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Vincula uma ou mais notas fiscais (identificadas pelas chaves de acesso) a um MDF-e
        /// (Manifesto Eletrônico de Documentos Fiscais).
        /// </summary>
        /// <param name="vinculo">Dados do vínculo. Não pode ser nulo.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="vinculo"/> é nulo.</exception>
        /// <exception cref="ArgumentException">
        /// Lançada quando <see cref="LinkNotaFiscalMdfe.ChavesAcesso"/> está vazia ou
        /// <see cref="LinkNotaFiscalMdfe.Identificador"/> não foi informado.
        /// </exception>
        public async Task VincularNotaFiscalMdfeAsync(LinkNotaFiscalMdfe vinculo, CancellationToken cancellationToken = default)
        {
            if (vinculo == null)
            {
                throw new ArgumentNullException(nameof(vinculo));
            }

            if (vinculo.ChavesAcesso == null || vinculo.ChavesAcesso.Count == 0)
            {
                throw new ArgumentException("ChavesAcesso é obrigatório.", nameof(vinculo));
            }

            if (string.IsNullOrWhiteSpace(vinculo.Identificador))
            {
                throw new ArgumentException("Identificador é obrigatório.", nameof(vinculo));
            }

            await _client.PostAsync(
                $"{NotasFiscaisEndpoint}/vinculo-mdfe", vinculo, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Consulta os dados de uma nota fiscal específica pela sua chave de acesso.
        /// O conteúdo é retornado como XML.
        /// </summary>
        /// <param name="chave">Chave de acesso da nota fiscal. Não pode ser nula ou vazia.</param>
        /// <param name="cancellationToken">Token de cancelamento da operação.</param>
        /// <returns>O XML da nota fiscal como <see cref="string"/>.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando <paramref name="chave"/> é nula ou vazia.</exception>
        public async Task<string> ObterNotaFiscalPorChaveAsync(string chave, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(chave))
            {
                throw new ArgumentNullException(nameof(chave));
            }

            var bytes = await _client.GetBytesAsync(
                $"{NotasFiscaisEndpoint}/{Uri.EscapeDataString(chave)}", cancellationToken).ConfigureAwait(false);

            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }
    }
}
