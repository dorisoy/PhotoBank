using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotoRequest : AuthenticatedRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("photoId")]
        public int PhotoId { get; set; }
    }
}
