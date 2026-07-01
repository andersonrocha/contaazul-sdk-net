using ContaAzul.Sdk.Net.Models.Servicos;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class ServicosRoundTripTests
{
    [Test]
    public void Servico_RoundTrips() => JsonRoundTrip.Verify<Servico>(@"{
      ""codigo"": ""SERV001"",
      ""codigo_cnae"": ""6201500"",
      ""codigo_municipio_servico"": ""3550308"",
      ""custo"": 250,
      ""descricao"": ""Servico de consultoria tecnica"",
      ""id"": ""e7090261-9978-4d37-86f8-0800851da683"",
      ""id_externo"": ""EXT123"",
      ""id_servico"": 73233,
      ""lei_116"": ""116/03"",
      ""lista_cenario_tributario"": [
        { ""id"": ""cenario_001"", ""inss_aliquota"": 11, ""iss_aliquota"": 5, ""iss_retido"": false, ""municipio"": { ""codigo"": 3550308, ""nome"": ""Sao Paulo"", ""uf"": ""SP"" }, ""nome_usuario"": ""Joao Silva"", ""ultima_atualizacao"": ""2024-01-15T10:30:00Z"" }
      ],
      ""natureza_operacional"": { ""id"": ""natureza_001"" },
      ""preco"": 500,
      ""status"": ""ATIVO"",
      ""tipo_servico"": ""PRESTADO""
    }");

    [Test]
    public void ServicosPorFiltro_RoundTrips() => JsonRoundTrip.Verify<ServicosPorFiltro>(@"{
      ""itens"": [ { ""id"": ""s-1"", ""descricao"": ""Consultoria"", ""preco"": 500, ""status"": ""ATIVO"", ""tipo_servico"": ""PRESTADO"" } ],
      ""paginacao"": { ""pagina_atual"": 1, ""tamanho_pagina"": 10, ""total_itens"": 1, ""total_paginas"": 1 }
    }");

    [Test]
    public void CriarServico_RoundTrips() => JsonRoundTrip.Verify<CriarServico>(@"{
      ""codigo"": ""SERV001"",
      ""custo"": 250,
      ""descricao"": ""Servico de consultoria tecnica"",
      ""preco"": 500,
      ""status"": ""ATIVO"",
      ""tipo_servico"": ""PRESTADO""
    }");

    [Test]
    public void AtualizacaoParcialServico_RoundTrips() => JsonRoundTrip.Verify<AtualizacaoParcialServico>(@"{
      ""codigo"": ""SERV001"",
      ""custo"": 50,
      ""descricao"": ""Servico de consultoria"",
      ""preco"": 100,
      ""tipo_servico"": ""PRESTADO""
    }");

    [Test]
    public void ParametrosParaDeletarServicosEmLote_RoundTrips() => JsonRoundTrip.Verify<ParametrosParaDeletarServicosEmLote>(@"{
      ""ids"": [ 73233, 73234, 73235 ]
    }");
}
