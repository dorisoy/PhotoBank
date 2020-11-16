using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotoResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("fileBase64Content")]
        public string FileBase64Content { get; set; }
    }
}
