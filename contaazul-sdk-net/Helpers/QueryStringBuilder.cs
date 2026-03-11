using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ContaAzul.Sdk.Net.Attributes;

namespace ContaAzul.Sdk.Net.Helpers
{
    internal static class QueryStringBuilder
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static string BuildEndpoint<T>(string endpoint, T filter) where T : class
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));
            }

            if (filter == null)
            {
                return endpoint;
            }

            var queryString = BuildQueryString(filter);
            return string.IsNullOrEmpty(queryString) 
                ? endpoint 
                : $"{endpoint}?{queryString}";
        }

        private static string BuildQueryString<T>(T filter) where T : class
        {
            if (filter == null)
            {
                return string.Empty;
            }

            var parameters = new List<string>();
            var properties = _propertyCache.GetOrAdd(
                typeof(T),
                t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));

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
