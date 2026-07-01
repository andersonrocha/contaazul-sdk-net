using ContaAzul.Sdk.Net.Models.Orcamentos;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class OrcamentosRoundTripTests
{
    [Test]
    public void Orcamento_RoundTrips() => JsonRoundTrip.Verify<Orcamento>(@"{
      ""composicao_de_valor"": { ""desconto"": { ""tipo"": ""VALOR"", ""valor"": 10 }, ""frete"": 5 },
      ""data_orcamento"": ""2026-05-01"",
      ""data_validade"": ""2026-05-15"",
      ""descricao"": ""Proposta comercial"",
      ""id"": ""aff32f2a-2904-4918-a18b-96fa39ac435c"",
      ""id_cliente"": ""72f07482-bfda-44b0-a2e7-d8817bf950fa"",
      ""id_vendedor"": ""8cc4ff03-e8c6-4d7e-8c41-4245f55f8612"",
      ""itens"": [ { ""custo"": 8, ""descricao"": ""Manutencao"", ""id"": ""9a1960f7-87e6-48c7-b30d-0ae0f8d6292e"", ""nome"": ""Produto 01"", ""quantidade"": 1, ""tipo"": ""PRODUTO"", ""valor"": 10 } ],
      ""numero"": 1,
      ""observacoes"": ""Entrega Gratis"",
      ""observacoes_pagamento"": ""Pagamento a vista"",
      ""previsao_entrega"": ""A combinar"",
      ""situacao"": ""ORCAMENTO"",
      ""versao"": 1
    }");

    [Test]
    public void ListagemOrcamentosPorFiltro_RoundTrips() => JsonRoundTrip.Verify<ListagemOrcamentosPorFiltro>(@"{
      ""itens"": [ {
        ""cliente"": { ""email"": ""exemplo@email.com"", ""id"": ""c-1"", ""nome"": ""Joao"" },
        ""data_alteracao"": ""2025-10-17T02:00:08.841"",
        ""data_criacao"": ""2025-05-16T17:51:04.63"",
        ""data_orcamento"": ""2023-12-31"",
        ""id"": ""123e4567-e89b-12d3-a456-426614174000"",
        ""id_contrato"": ""223e4567-e89b-12d3-a456-426614174000"",
        ""itens"": ""PRODUTO"",
        ""numero"": 1001,
        ""origem"": ""NFE"",
        ""situacao"": ""ORCAMENTO"",
        ""total"": 1000,
        ""versao"": 1
      } ],
      ""total_itens"": 10
    }");

    [Test]
    public void CriarOrcamento_RoundTrips() => JsonRoundTrip.Verify<CriarOrcamento>(@"{
      ""composicao_de_valor"": { ""desconto"": { ""tipo"": ""VALOR"", ""valor"": 10 }, ""frete"": 5 },
      ""data_orcamento"": ""2026-05-01"",
      ""data_validade"": ""2026-05-15"",
      ""descricao"": ""Proposta comercial"",
      ""id_cliente"": ""72f07482-bfda-44b0-a2e7-d8817bf950fa"",
      ""id_vendedor"": ""8cc4ff03-e8c6-4d7e-8c41-4245f55f8612"",
      ""itens"": [ { ""id"": ""623ef303-54df-4df6-b816-69416f29e093"", ""quantidade"": 1, ""valor"": 10, ""valor_custo"": 8 } ],
      ""observacoes"": ""Entrega rapida"",
      ""observacoes_pagamento"": ""30 dias"",
      ""previsao_entrega"": ""10 dias uteis""
    }");

    [Test]
    public void ResumoCriacaoOrcamento_RoundTrips() => JsonRoundTrip.Verify<ResumoCriacaoOrcamento>(@"{
      ""id"": ""cae40e8a-8330-4469-9bf7-51cbf3e8e2cd""
    }");

    [Test]
    public void ExclusaoLoteOrcamento_RoundTrips() => JsonRoundTrip.Verify<ExclusaoLoteOrcamento>(@"{
      ""ids"": [ ""7d7c9d4a-27aa-457e-b981-2df4c81970f7"", ""c44e254d-0040-46e2-bccf-6898d0981201"" ]
    }");
}
