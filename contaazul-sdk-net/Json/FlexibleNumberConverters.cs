using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Json
{
    /// <summary>
    /// Conversores de leitura tolerante para campos inteiros. A API do ContaAzul às vezes
    /// devolve inteiros como números com casa decimal (ex.: <c>10.0</c>) ou como string
    /// (ex.: <c>"10"</c>); estes conversores aceitam ambos e convertem para o tipo inteiro,
    /// evitando <see cref="JsonException"/> na desserialização. A escrita permanece padrão.
    /// </summary>
    internal static class FlexibleNumber
    {
        internal static long ReadInt64(ref Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out var l))
                    {
                        return l;
                    }
                    return (long)reader.GetDouble();
                case JsonTokenType.String:
                    var s = reader.GetString();
                    if (long.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var fromStr))
                    {
                        return fromStr;
                    }
                    if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    {
                        return (long)d;
                    }
                    throw new JsonException($"Não foi possível converter '{s}' para um inteiro.");
                default:
                    throw new JsonException($"Token inesperado {reader.TokenType} ao ler um inteiro.");
            }
        }
    }

    /// <summary>Conversor tolerante para <see cref="int"/>.</summary>
    public sealed class FlexibleInt32Converter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => checked((int)FlexibleNumber.ReadInt64(ref reader));

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value);
    }

    /// <summary>Conversor tolerante para <see cref="Nullable{Int32}"/>.</summary>
    public sealed class FlexibleNullableInt32Converter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            return checked((int)FlexibleNumber.ReadInt64(ref reader));
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }

    /// <summary>Conversor tolerante para <see cref="long"/>.</summary>
    public sealed class FlexibleInt64Converter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => FlexibleNumber.ReadInt64(ref reader);

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value);
    }

    /// <summary>Conversor tolerante para <see cref="Nullable{Int64}"/>.</summary>
    public sealed class FlexibleNullableInt64Converter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            return FlexibleNumber.ReadInt64(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
