using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Helpers;
using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Apis
{
    /// <summary>
    /// API de Financeiro do ContaAzul (centros de custo, categorias, contas financeiras,
    /// parcelas, contas a pagar/receber, transferências e saldos).
    /// Acessada através da propriedade <see cref="ContaAzulApiClient.Financeiro"/>.
    /// </summary>
    public class FinanceiroApi
    {
        private const string CentroDeCustoEndpoint = "/v1/centro-de-custo";
        private const string CategoriasEndpoint = "/v1/categorias";
        private const string ContaFinanceiraEndpoint = "/v1/conta-financeira";
        private const string EventosEndpoint = "/v1/financeiro/eventos-financeiros";

        private readonly IContaAzulApiClient _client;

        internal FinanceiroApi(IContaAzulApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        // --- Centros de custo ---

        /// <summary>Lista os centros de custo de acordo com os filtros informados.</summary>
        public async Task<CentroDeCustoResponse> ObterCentrosDeCustoAsync(CentroDeCustoFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(CentroDeCustoEndpoint, filtro);
            return await _client.GetAsync<CentroDeCustoResponse>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Cria um novo centro de custo.</summary>
        public async Task<CentroDeCusto> CriarCentroDeCustoAsync(CriarCentroDeCustoRequest centroDeCusto, CancellationToken cancellationToken = default)
        {
            if (centroDeCusto == null)
            {
                throw new ArgumentNullException(nameof(centroDeCusto));
            }

            return await _client.PostAsync<CriarCentroDeCustoRequest, CentroDeCusto>(
                CentroDeCustoEndpoint, centroDeCusto, cancellationToken).ConfigureAwait(false);
        }

        // --- Categorias ---

        /// <summary>Lista as categorias de acordo com os filtros informados.</summary>
        public async Task<RespostaItens<Categoria>> ObterCategoriasAsync(CategoriaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(CategoriasEndpoint, filtro);
            return await _client.GetAsync<RespostaItens<Categoria>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Retorna a estrutura de categorias DRE.</summary>
        public async Task<EstruturaDre> ObterCategoriasDreAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAsync<EstruturaDre>(
                "/v1/financeiro/categorias-dre", cancellationToken).ConfigureAwait(false);
        }

        // --- Contas financeiras ---

        /// <summary>Lista as contas financeiras de acordo com os filtros informados.</summary>
        public async Task<RespostaItens<ContaFinanceira>> ObterContasFinanceirasAsync(ContaFinanceiraFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint(ContaFinanceiraEndpoint, filtro);
            return await _client.GetAsync<RespostaItens<ContaFinanceira>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Retorna o saldo atual de uma conta financeira.</summary>
        public async Task<SaldoAtualResponse> ObterSaldoAtualAsync(string idContaFinanceira, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(idContaFinanceira))
            {
                throw new ArgumentNullException(nameof(idContaFinanceira));
            }

            return await _client.GetAsync<SaldoAtualResponse>(
                $"{ContaFinanceiraEndpoint}/{Uri.EscapeDataString(idContaFinanceira)}/saldo-atual", cancellationToken).ConfigureAwait(false);
        }

        // --- Transferências ---

        /// <summary>Lista as transferências entre contas financeiras.</summary>
        public async Task<RespostaItens<TransferenciaContaFinanceira>> ObterTransferenciasAsync(TransferenciaFiltro filtro = null, CancellationToken cancellationToken = default)
        {
            var endpoint = QueryStringBuilder.BuildEndpoint("/v1/financeiro/transferencias", filtro);
            return await _client.GetAsync<RespostaItens<TransferenciaContaFinanceira>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        // --- Eventos financeiros (contas a receber / pagar) ---

        /// <summary>Cria um evento financeiro de contas a receber.</summary>
        public async Task<ProtocoloResponse> CriarContaAReceberAsync(EventoFinanceiroRequest evento, CancellationToken cancellationToken = default)
        {
            if (evento == null)
            {
                throw new ArgumentNullException(nameof(evento));
            }

            return await _client.PostAsync<EventoFinanceiroRequest, ProtocoloResponse>(
                $"{EventosEndpoint}/contas-a-receber", evento, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Cria um evento financeiro de contas a pagar.</summary>
        public async Task<ProtocoloResponse> CriarContaAPagarAsync(EventoFinanceiroRequest evento, CancellationToken cancellationToken = default)
        {
            if (evento == null)
            {
                throw new ArgumentNullException(nameof(evento));
            }

            return await _client.PostAsync<EventoFinanceiroRequest, ProtocoloResponse>(
                $"{EventosEndpoint}/contas-a-pagar", evento, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista as parcelas de contas a receber.
        /// <see cref="MovimentacaoFinanceiraFiltro.DataVencimentoDe"/>/<see cref="MovimentacaoFinanceiraFiltro.DataVencimentoAte"/>
        /// são obrigatórios.
        /// </summary>
        public async Task<RespostaItens<ContaPagarReceber>> ObterContasAReceberAsync(MovimentacaoFinanceiraFiltro filtro, CancellationToken cancellationToken = default)
        {
            ValidarVencimento(filtro);
            var endpoint = QueryStringBuilder.BuildEndpoint($"{EventosEndpoint}/contas-a-receber/buscar", filtro);
            return await _client.GetAsync<RespostaItens<ContaPagarReceber>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista as parcelas de contas a pagar.
        /// <see cref="MovimentacaoFinanceiraFiltro.DataVencimentoDe"/>/<see cref="MovimentacaoFinanceiraFiltro.DataVencimentoAte"/>
        /// são obrigatórios.
        /// </summary>
        public async Task<RespostaItens<ContaPagarReceber>> ObterContasAPagarAsync(MovimentacaoFinanceiraFiltro filtro, CancellationToken cancellationToken = default)
        {
            ValidarVencimento(filtro);
            var endpoint = QueryStringBuilder.BuildEndpoint($"{EventosEndpoint}/contas-a-pagar/buscar", filtro);
            return await _client.GetAsync<RespostaItens<ContaPagarReceber>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        // --- Parcelas ---

        /// <summary>Lista as parcelas vinculadas a um evento financeiro.</summary>
        public async Task<List<ParcelaFinanceira>> ObterParcelasPorEventoAsync(string idEvento, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(idEvento))
            {
                throw new ArgumentNullException(nameof(idEvento));
            }

            return await _client.GetAsync<List<ParcelaFinanceira>>(
                $"{EventosEndpoint}/{Uri.EscapeDataString(idEvento)}/parcelas", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Consulta uma parcela pelo seu identificador.</summary>
        public async Task<ParcelaFinanceira> ObterParcelaPorIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _client.GetAsync<ParcelaFinanceira>(
                $"{EventosEndpoint}/parcelas/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Atualiza parcialmente uma parcela.</summary>
        public async Task<ParcelaAtualizacaoResponse> AtualizarParcelaAsync(string id, ParcelaAtualizacaoRequest parcela, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (parcela == null)
            {
                throw new ArgumentNullException(nameof(parcela));
            }

            return await _client.PatchAsync<ParcelaAtualizacaoRequest, ParcelaAtualizacaoResponse>(
                $"{EventosEndpoint}/parcelas/{Uri.EscapeDataString(id)}", parcela, cancellationToken).ConfigureAwait(false);
        }

        // --- Eventos alterados / saldos iniciais ---

        /// <summary>Retorna os IDs dos eventos financeiros alterados em um período. <c>DataInicio</c>/<c>DataFim</c> obrigatórios.</summary>
        public async Task<RespostaItens<EventoFinanceiroId>> ObterEventosAlteradosAsync(PeriodoFinanceiroFiltro filtro, CancellationToken cancellationToken = default)
        {
            ValidarPeriodo(filtro);
            var endpoint = QueryStringBuilder.BuildEndpoint($"{EventosEndpoint}/alteracoes", filtro);
            return await _client.GetAsync<RespostaItens<EventoFinanceiroId>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>Retorna os saldos iniciais das contas financeiras em um período. <c>DataInicio</c>/<c>DataFim</c> obrigatórios.</summary>
        public async Task<RespostaItens<SaldoInicialContaFinanceira>> ObterSaldosIniciaisAsync(PeriodoFinanceiroFiltro filtro, CancellationToken cancellationToken = default)
        {
            ValidarPeriodo(filtro);
            var endpoint = QueryStringBuilder.BuildEndpoint($"{EventosEndpoint}/saldo-inicial", filtro);
            return await _client.GetAsync<RespostaItens<SaldoInicialContaFinanceira>>(endpoint, cancellationToken).ConfigureAwait(false);
        }

        private static void ValidarVencimento(MovimentacaoFinanceiraFiltro filtro)
        {
            if (filtro == null)
            {
                throw new ArgumentNullException(nameof(filtro));
            }

            if (string.IsNullOrWhiteSpace(filtro.DataVencimentoDe) || string.IsNullOrWhiteSpace(filtro.DataVencimentoAte))
            {
                throw new ArgumentException("DataVencimentoDe e DataVencimentoAte são obrigatórios.", nameof(filtro));
            }
        }

        private static void ValidarPeriodo(PeriodoFinanceiroFiltro filtro)
        {
            if (filtro == null)
            {
                throw new ArgumentNullException(nameof(filtro));
            }

            if (string.IsNullOrWhiteSpace(filtro.DataInicio) || string.IsNullOrWhiteSpace(filtro.DataFim))
            {
                throw new ArgumentException("DataInicio e DataFim são obrigatórios.", nameof(filtro));
            }
        }
    }
}
