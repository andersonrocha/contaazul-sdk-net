using ContaAzul.Sdk.Net.Models;
using System.Text.Json;

namespace ContaAzul.Sdk.Net.Tests;

[TestFixture]
public class VendaListResponseTests
{
    [Test]
    public void WhenDeserializeVendaListResponseWithNewStructureThenMapsPropertiesCorrectly()
    {
        var json = @"{
  ""itens"": [],
  ""totais"": {
    ""total"": 1077.4600000000,
    ""aprovado"": 1077.4600000000,
    ""cancelado"": 0,
    ""esperando_aprovacao"": 0
  },
  ""quantidades"": {
    ""total"": 2,
    ""aprovado"": 2,
    ""cancelado"": 0,
    ""esperando_aprovacao"": 0
  },
  ""total_itens"": 2
}";

        var result = JsonSerializer.Deserialize<VendaListResponse>(json);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Itens, Is.Not.Null);
            Assert.That(result.Itens, Has.Count.EqualTo(0));

            Assert.That(result.Totais, Is.Not.Null);
            Assert.That(result.Totais.Total, Is.EqualTo(1077.46m));
            Assert.That(result.Totais.Aprovado, Is.EqualTo(1077.46m));
            Assert.That(result.Totais.Cancelado, Is.EqualTo(0m));
            Assert.That(result.Totais.EsperandoAprovacao, Is.EqualTo(0m));

            Assert.That(result.Quantidades, Is.Not.Null);
            Assert.That(result.Quantidades.Total, Is.EqualTo(2));
            Assert.That(result.Quantidades.Aprovado, Is.EqualTo(2));
            Assert.That(result.Quantidades.Cancelado, Is.EqualTo(0));
            Assert.That(result.Quantidades.EsperandoAprovacao, Is.EqualTo(0));

            Assert.That(result.TotalItens, Is.EqualTo(2));
        });
    }
}
