using ContaAzul.Sdk.Net.Models.Protocolos;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class ProtocolosRoundTripTests
{
    [Test]
    public void Protocolo_RoundTrips() => JsonRoundTrip.Verify<Protocolo>(@"{
      ""id"": ""123e4567-e89b-12d3-a456-426614174000"",
      ""resposta"": ""Operacao realizada com sucesso."",
      ""status"": ""SUCCESS"",
      ""evento_financeiro_id"": ""223e4567-e89b-12d3-a456-426614174000""
    }");
}
