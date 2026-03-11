using System.Text.Json.Serialization;

namespace ContaAzul.Sdk.Net.Models
{
    public sealed class ApiError
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }
}
