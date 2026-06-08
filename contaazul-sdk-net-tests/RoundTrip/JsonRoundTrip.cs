using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

/// <summary>
/// Utilitário para testes de round-trip de payloads: desserializa um JSON no modelo,
/// re-serializa e compara semanticamente os dois JSONs (ignorando ordem de propriedades).
/// Garante que nenhum campo do payload é perdido no mapeamento do modelo.
/// <para>
/// Usa as mesmas opções de serialização do <c>HttpClientBase</c> do SDK
/// (<c>PropertyNameCaseInsensitive</c> + <c>DefaultIgnoreCondition = WhenWritingNull</c>),
/// portanto os fixtures não devem conter campos explicitamente nulos.
/// </para>
/// </summary>
internal static class JsonRoundTrip
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Desserializa <paramref name="json"/> em <typeparamref name="T"/>, re-serializa e
    /// verifica que o resultado é semanticamente igual ao JSON original.
    /// </summary>
    public static void Verify<T>(string json)
    {
        var model = JsonSerializer.Deserialize<T>(json, Options);
        var reserialized = JsonSerializer.Serialize(model, Options);

        var expected = JsonNode.Parse(json);
        var actual = JsonNode.Parse(reserialized);

        Assert.That(
            JsonNode.DeepEquals(expected, actual),
            Is.True,
            $"Round-trip de {typeof(T).Name} divergiu.\n" +
            $"Esperado: {expected?.ToJsonString()}\n" +
            $"Obtido:   {actual?.ToJsonString()}");
    }
}
