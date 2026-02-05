using System.Collections.Generic;
using Newtonsoft.Json;

namespace contaazul_dotnet.Models
{
    public class ApiResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("errors")]
        public List<ApiError> Errors { get; set; }

        public bool Success => Errors == null || Errors.Count == 0;
    }

    public class ApiError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }
}
