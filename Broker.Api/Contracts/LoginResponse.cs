using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class LoginResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
