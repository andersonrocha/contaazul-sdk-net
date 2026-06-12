using ContaAzul.Sdk.Net.Models.Financeiro;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class FinanceiroRoundTripTests
{
    // --- Cobranças ---

    [Test]
    public void GerarCobrancaRequest_RoundTrips() => JsonRoundTrip.Verify<GerarCobrancaRequest>(@"{
      ""conta_bancaria"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""descricao_fatura"": ""Pagamento da fatura #1234"",
      ""id_parcela"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""data_vencimento"": ""2023-10-10"",
      ""tipo"": ""LINK_PAGAMENTO"",
      ""atributos"": { ""desconto_antecipado"": { ""percentual"": 10, ""dias_antes_vencer"": 10 } },
      ""maximo_parcelas"": 3
    }");

    [Test]
    public void GerarCobrancaResponse_RoundTrips() => JsonRoundTrip.Verify<GerarCobrancaResponse>(@"{
      ""id"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""url"": ""http://www.exemplo.com.br"",
      ""status"": ""AGUARDANDO_CONFIRMACAO""
    }");

    // --- Baixas ---

    [Test]
    public void BaixaCriacaoRequest_RoundTrips() => JsonRoundTrip.Verify<BaixaCriacaoRequest>(@"{
      ""data_pagamento"": ""2023-10-01"",
      ""composicao_valor"": { ""multa"": 5, ""juros"": 2.5, ""valor_bruto"": 150, ""desconto"": 10, ""taxa"": 3.75 },
      ""conta_financeira"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""metodo_pagamento"": ""CARTAO_CREDITO"",
      ""observacao"": ""Pagamento referente à fatura #1234."",
      ""nsu"": ""1234567890""
    }");

    [Test]
    public void BaixaCriacaoResponse_RoundTrips() => JsonRoundTrip.Verify<BaixaCriacaoResponse>(@"{
      ""id"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""versao"": 1,
      ""data_pagamento"": ""2023-10-01"",
      ""composicao_valor"": { ""valor_bruto"": 150 },
      ""conta_financeira"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""metodo_pagamento"": ""CARTAO_CREDITO"",
      ""observacao"": ""obs"",
      ""nsu"": ""1234567890""
    }");

    [Test]
    public void BaixaResponse_RoundTrips() => JsonRoundTrip.Verify<BaixaResponse>(@"{
      ""id"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""versao"": 1,
      ""data_pagamento"": ""2023-10-01"",
      ""valor_composicao"": { ""valor_bruto"": 150, ""multa"": 5 },
      ""conta_financeira"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""id_reconciliacao"": ""r-1"",
      ""id_parcela"": ""p-1"",
      ""observacao"": ""obs"",
      ""metodo_pagamento"": ""CARTAO_CREDITO"",
      ""origem"": ""SALDO_CONTA_BANCARIA"",
      ""tipo_evento_financeiro"": ""RECEITA"",
      ""nsu"": ""123"",
      ""id_referencia"": ""REF1234"",
      ""atualizado_em"": ""2023-10-01T12:00:00Z"",
      ""anexos"": [ { ""id"": ""a-1"", ""nome"": ""recibo.pdf"", ""tipo"": ""RECIBO_DIGITAL"", ""tipo_conteudo"": ""URL"", ""url"": ""https://x/y.pdf"" } ]
    }");

    // --- Centros de custo / categorias / contas ---

    [Test]
    public void CentroDeCustoResponse_RoundTrips() => JsonRoundTrip.Verify<CentroDeCustoResponse>(@"{
      ""itens_totais"": 6,
      ""items"": [ { ""id"": ""cc-1"", ""codigo"": ""1040"", ""nome"": ""Contabilidade"", ""ativo"": true } ],
      ""totais"": { ""ativo"": 6, ""inativo"": 0, ""todos"": 6 }
    }");

    [Test]
    public void RespostaItensCategoria_RoundTrips() => JsonRoundTrip.Verify<RespostaItens<Categoria>>(@"{
      ""itens_totais"": 1,
      ""itens"": [ { ""id"": ""c-1"", ""versao"": 0, ""nome"": ""Eletrônicos"", ""tipo"": ""RECEITA"", ""entrada_dre"": ""DESPESAS_ADMINISTRATIVAS"", ""considera_custo_dre"": true } ],
      ""totais"": { ""ativo"": 6, ""inativo"": 0, ""todos"": 6 }
    }");

    [Test]
    public void RespostaItensContaFinanceira_RoundTrips() => JsonRoundTrip.Verify<RespostaItens<ContaFinanceira>>(@"{
      ""itens_totais"": 1,
      ""itens"": [ { ""id"": ""cf-1"", ""banco"": ""BANCO_BRASIL"", ""codigo_banco"": 1, ""nome"": ""Conta Corrente"", ""ativo"": true, ""tipo"": ""CONTA_CORRENTE"", ""conta_padrao"": true, ""possui_config_boleto_bancario"": false, ""agencia"": ""001"", ""numero"": ""31"" } ]
    }");

    [Test]
    public void EstruturaDre_RoundTrips() => JsonRoundTrip.Verify<EstruturaDre>(@"{
      ""itens"": [
        {
          ""id"": ""dre-1"", ""descricao"": ""Receita Operacional Bruta"", ""codigo"": ""01"", ""posicao"": 1,
          ""indica_totalizador"": true, ""representa_soma_custo_medio"": false,
          ""subitens"": [ { ""id"": ""dre-1-1"", ""descricao"": ""Vendas"", ""codigo"": ""01.01"", ""posicao"": 2 } ],
          ""categorias_financeiras"": [ { ""id"": ""cat-1"", ""codigo"": ""1"", ""nome"": ""Venda de Produtos"", ""ativo"": true } ]
        }
      ]
    }");

    [Test]
    public void SaldoAtualResponse_RoundTrips() =>
        JsonRoundTrip.Verify<SaldoAtualResponse>(@"{ ""saldo_atual"": 1000.36 }");

    // --- Contas a pagar/receber ---

    [Test]
    public void RespostaItensContaPagarReceber_RoundTrips() => JsonRoundTrip.Verify<RespostaItens<ContaPagarReceber>>(@"{
      ""itens_totais"": 1,
      ""itens"": [
        {
          ""id"": ""c-1"", ""descricao"": ""Venda de Produtos"", ""data_vencimento"": ""2027-08-15"",
          ""status"": ""OVERDUE"", ""status_traduzido"": ""ATRASADO"", ""total"": 781201.79, ""nao_pago"": 213023.79, ""pago"": 0,
          ""data_criacao"": ""2027-08-15T14:30:00Z"", ""data_alteracao"": ""2027-08-15T14:30:00Z"", ""data_competencia"": ""2018-03-16"",
          ""categorias"": [ { ""id"": ""cat-1"", ""nome"": ""Adiantamento"" } ],
          ""centros_custo"": [ { ""id"": ""cc-1"", ""nome"": ""Centro X"" } ],
          ""cliente"": { ""id"": ""cli-1"", ""nome"": ""Maria da Silva"" },
          ""renegociacao"": { ""id"": ""rn-1"", ""valor"": 25, ""id_evento"": ""ev-1"" }
        }
      ],
      ""totais"": { ""ativo"": 6, ""inativo"": 0, ""todos"": 6 }
    }");

    // --- Transferências ---

    [Test]
    public void RespostaItensTransferencia_RoundTrips() => JsonRoundTrip.Verify<RespostaItens<TransferenciaContaFinanceira>>(@"{
      ""itens_totais"": 1,
      ""itens"": [
        {
          ""id"": ""t-1"", ""descricao"": ""Transferência"", ""valor"": 1500.5, ""data"": ""2026-02-15"",
          ""origem"": { ""data"": ""2026-02-15"", ""composicao_valor"": { ""valor_bruto"": 1500.5, ""valor_liquido"": 1500.5 }, ""conta_financeira"": { ""id"": ""o-1"", ""nome"": ""Conta Corrente"", ""instituicao_bancaria"": { ""codigo"": 1, ""nome"": ""Banco do Brasil"" } } },
          ""destino"": { ""data"": ""2026-02-15"", ""composicao_valor"": { ""valor_bruto"": 1500.5, ""valor_liquido"": 1500.5 }, ""conta_financeira"": { ""id"": ""d-1"", ""nome"": ""Conta Poupança"", ""instituicao_bancaria"": { ""codigo"": 1, ""nome"": ""Banco do Brasil"" } } }
        }
      ]
    }");

    // --- Eventos / parcelas ---

    [Test]
    public void EventoFinanceiroRequest_RoundTrips() => JsonRoundTrip.Verify<EventoFinanceiroRequest>(@"{
      ""data_competencia"": ""2024-07-15"",
      ""valor"": 100,
      ""observacao"": ""obs"",
      ""descricao"": ""Prestação de serviço"",
      ""contato"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""conta_financeira"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""rateio"": [ { ""id_categoria"": ""cat-1"", ""valor"": 100, ""rateio_centro_custo"": [ { ""id_centro_custo"": ""cc-1"", ""valor"": 100 } ] } ],
      ""condicao_pagamento"": { ""parcelas"": [ { ""descricao"": ""Mensalidade (2/6)"", ""data_vencimento"": ""2024-07-15"", ""nota"": ""PIX"", ""conta_financeira"": ""cf-1"", ""detalhe_valor"": { ""valor_bruto"": 100 }, ""metodo_pagamento"": ""PIX_PAGAMENTO_INSTANTANEO"" } ] }
    }");

    [Test]
    public void ProtocoloResponse_RoundTrips() => JsonRoundTrip.Verify<ProtocoloResponse>(@"{
      ""protocolId"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"",
      ""status"": ""SUCCESS"",
      ""createdAt"": ""2024-10-22T14:30:00Z""
    }");

    [Test]
    public void ParcelaFinanceira_RoundTrips() => JsonRoundTrip.Verify<ParcelaFinanceira>(@"{
      ""evento"": {
        ""data_competencia"": ""2025-06-11"", ""id"": ""ev-1"",
        ""condicao_pagamento"": { ""quantidade_parcelas"": 10, ""montante_fixo"": false },
        ""referencia"": { ""id"": ""r-1"", ""revisao"": ""1"", ""origem"": ""LANCAMENTO_FINANCEIRO"" },
        ""agendado"": true, ""tipo"": ""RECEITA"", ""codigo_referencia"": ""123456"",
        ""rateio"": [ { ""id_categoria"": ""cat-1"", ""nome_categoria"": ""Eletrônicos"", ""valor"": 20.5, ""valor_bruto"": 20.5, ""rateio_centro_custo"": [ { ""id_centro_custo"": ""cc-1"", ""nome_centro_custo"": ""Contabilidade"", ""valor"": 30.5 } ] } ]
      },
      ""id"": ""p-1"", ""versao"": 1, ""referencia"": ""ref-1"", ""indice"": 1, ""conciliado"": true, ""status"": ""PENDENTE"",
      ""valor_pago"": 10, ""perda"": { ""data"": ""2024-07-15"", ""valor"": 1.99 }, ""nao_pago"": 5,
      ""data_vencimento"": ""2025-09-05"", ""data_pagamento_previsto"": ""2025-09-05"", ""descricao"": ""Parcela"", ""nota"": ""nota"",
      ""conta_financeira"": { ""id"": ""cf-1"", ""nome"": ""Conta Corrente"", ""tipo"": ""CONTA_CORRENTE"" },
      ""id_conta_financeira"": ""cf-1"", ""valor_composicao"": { ""valor_bruto"": 20, ""valor_liquido"": 9 }, ""metodo_pagamento"": ""DEPOSITO_BANCARIO"",
      ""nsu"": ""000215783210"", ""baixa_agendada"": false,
      ""baixas"": [ { ""id"": ""b-1"", ""versao"": 1, ""data_pagamento"": ""2025-05-20"", ""valor_composicao"": { ""valor_bruto"": 20 }, ""conta_financeira"": { ""id"": ""cf-1"", ""nome"": ""Conta Corrente"" }, ""metodo_pagamento"": ""DEPOSITO_BANCARIO"", ""origem"": ""LANCAMENTO_FINANCEIRO"", ""tipo_evento_financeiro"": ""RECEITA"", ""atualizado_em"": ""2025-08-05T08:37:05"" } ],
      ""anexos"": [ { ""id"": ""an-1"", ""versao"": 1, ""descricao"": ""Boleto"", ""nome"": ""boleto.pdf"", ""url"": ""www.x.com"", ""tipo_conteudo"": ""URL"", ""tipo_anexo"": ""BOLETO_BANCARIO"" } ],
      ""solicitacoes_cobrancas"": [ { ""id"": ""sc-1"", ""versao"": 1, ""status_solicitacao_cobranca"": ""REGISTRADO"", ""valor_composicao"": { ""valor_bruto"": 20 }, ""data_vencimento"": ""2025-09-05"", ""tipo_solicitacao_cobranca"": ""BOLETO"", ""url"": ""www.x.com"", ""notificacao_cobranca"": { ""id"": ""nc-1"", ""solicitacao_cobranca_ids"": [ ""sc-1"" ], ""versao"": 1, ""itens_notificacao_cobranca"": [ { ""id"": ""inc-1"", ""versao"": 1, ""email"": ""x@y.com"", ""status_entrega"": ""ENVIADO"" } ], ""assunto"": ""Fatura"" } } ],
      ""fatura"": { ""numero"": 123, ""rps"": 1, ""tipo_fatura"": ""NFE"" },
      ""data_alteracao"": ""2025-10-22T08:37:05"", ""valor_total_liquido"": 10,
      ""renegociacao"": { ""id"": ""rn-1"", ""valor"": 25 }
    }");

    [Test]
    public void ParcelaAtualizacaoResponse_RoundTrips() => JsonRoundTrip.Verify<ParcelaAtualizacaoResponse>(@"{
      ""nota"": ""Pgto 3/5"", ""descricao"": ""Serviço"", ""vencimento"": ""2024-07-15"",
      ""composicao_valor"": { ""valor_bruto"": 275.99, ""valor_liquido"": 250.33 }, ""versao"": 1,
      ""data_pagamento_esperado"": ""2024-07-15"", ""metodo_pagamento"": ""CARTAO_CREDITO"",
      ""perda"": { ""data"": ""2024-07-15"", ""valor"": 1.99 }, ""nsu"": ""1029384756"", ""pagamento_agendado"": false,
      ""conta_financeira"": { ""id"": ""cf-1"", ""versao"": 1, ""nome"": ""Conta Corrente"", ""agencia"": ""001"", ""numero"": ""123"", ""tipo"": ""OUTROS"", ""banco"": ""OUTROS"" }
    }");

    [Test]
    public void RespostaItensSaldoInicial_RoundTrips() => JsonRoundTrip.Verify<RespostaItens<SaldoInicialContaFinanceira>>(@"{
      ""itens_totais"": 1,
      ""itens"": [ { ""tipo"": ""RECEITA"", ""id_conta_financeira"": ""cf-1"", ""data_competencia"": ""2018-03-16"", ""saldo_inicial"": 2500 } ]
    }");

    [Test]
    public void RespostaItensEventoFinanceiroId_RoundTrips() => JsonRoundTrip.Verify<RespostaItens<EventoFinanceiroId>>(@"{
      ""itens_totais"": 1,
      ""itens"": [ { ""id"": ""35473eec-4e74-11ee-b500-9f61de8a8b8b"" } ]
    }");
}
