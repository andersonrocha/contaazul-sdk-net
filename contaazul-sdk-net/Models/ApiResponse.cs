using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class ApiResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("errors")]
        public List<ApiError> Errors { get; set; }

        public bool Success => Errors == null || Errors.Count == 0;
    }
}
