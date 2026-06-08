using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Json
{
    /// <summary>
    /// Conversor de leitura tolerante para <see cref="Nullable{DateTime}"/>. A API do ContaAzul
    /// devolve datas não preenchidas como <b>string vazia</b> (<c>""</c>), o que faz o parser
    /// padrão do <see cref="System.Text.Json"/> lançar <see cref="JsonException"/>. Este conversor
    /// trata string vazia/em branco como <c>null</c> e aceita os formatos de data/data-hora usuais.
    /// </summary>
    public sealed class FlexibleNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                if (DateTime.TryParse(value, CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind | DateTimeStyles.AllowWhiteSpaces, out var parsed))
                {
                    return parsed;
                }

                throw new JsonException($"Formato de data/hora não suportado: '{value}'.");
            }

            throw new JsonException($"Token inesperado {reader.TokenType} ao ler uma data/hora.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
