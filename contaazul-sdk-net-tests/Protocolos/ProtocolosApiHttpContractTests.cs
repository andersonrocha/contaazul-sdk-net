using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Models.Protocolos;

namespace ContaAzul.Sdk.Net.Tests.Protocolos;

[TestFixture]
public class ProtocolosApiHttpContractTests
{
    [Test]
    public async Task ObterProtocoloPorId_FazGetNoEndpointDoProtocolo()
    {
        var handler = new CapturingHttpHandler(HttpStatusCode.OK,
            "{\"id\":\"123e4567-e89b-12d3-a456-426614174000\",\"resposta\":\"Operação realizada com sucesso.\"," +
            "\"status\":\"SUCCESS\",\"evento_financeiro_id\":\"223e4567-e89b-12d3-a456-426614174000\"}");
        using var client = TestClientFactory.Build(handler);

        var protocolo = await client.Protocolos.ObterProtocoloPorIdAsync("123e4567-e89b-12d3-a456-426614174000");

        Assert.Multiple(() =>
        {
            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/protocolo/123e4567-e89b-12d3-a456-426614174000"));
            Assert.That(protocolo.Status, Is.EqualTo("SUCCESS"));
            Assert.That(protocolo.EventoFinanceiroId, Is.EqualTo("223e4567-e89b-12d3-a456-426614174000"));
        });
    }

    [Test]
    public void ObterProtocoloPorId_ComIdVazioLancaArgumentNullException()
    {
        using var client = TestClientFactory.Build(new CapturingHttpHandler());
        Assert.ThrowsAsync<ArgumentNullException>(async () => await client.Protocolos.ObterProtocoloPorIdAsync(" "));
    }
}
