using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using contaazul_dotnet.Attributes;

namespace contaazul_dotnet.Helpers
{
    internal static class QueryStringBuilder
    {
        public static string BuildQueryString<T>(T filter) where T : class
        {
            if (filter == null)
            {
                return string.Empty;
            }

            var parameters = new List<string>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<QueryParameterAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                var value = property.GetValue(filter);
                if (value == null)
                {
                    continue;
                }

                var parameterName = attribute.Name;
                var parameterValue = FormatValue(value);

                if (!string.IsNullOrWhiteSpace(parameterValue))
                {
                    parameters.Add($"{parameterName}={Uri.EscapeDataString(parameterValue)}");
                }
            }

            return string.Join("&", parameters);
        }

        private static string FormatValue(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            switch (value)
            {
                case string stringValue:
                    return stringValue;
                case bool boolValue:
                    return boolValue.ToString().ToLower();
                case int intValue:
                    return intValue.ToString();
                case long longValue:
                    return longValue.ToString();
                case decimal decimalValue:
                    return decimalValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                case double doubleValue:
                    return doubleValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                case DateTime dateTimeValue:
                    return dateTimeValue.ToString("yyyy-MM-dd");
                default:
                    return value.ToString();
            }
        }
    }
}
