using System.Threading.Tasks;
using ContaAzul.Sdk.Net.Exceptions;

namespace ContaAzul.Sdk.Net.Tests.Integration;

[TestFixture]
[Explicit("Teste de integração ao vivo: requer credenciais reais do ContaAzul.")]
[Category("Integration")]
public class ProtocolosIntegrationTests : IntegrationTestBase
{
    [Test]
    public void ObterProtocoloPorId_ComIdInexistente_LancaContaAzulApiException()
    {
        // Não há endpoint de listagem de protocolos; um UUID inexistente deve resultar em erro
        // (404/400), confirmando que o endpoint está acessível e mapeado corretamente.
        Assert.ThrowsAsync<ContaAzulApiException>(async () =>
            await Client.Protocolos.ObterProtocoloPorIdAsync("00000000-0000-0000-0000-000000000000"));
    }
}
