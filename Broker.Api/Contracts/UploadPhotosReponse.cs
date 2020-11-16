using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class UploadPhotosReponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("photoId")]
        public int PhotoId { get; set; }
    }
}
