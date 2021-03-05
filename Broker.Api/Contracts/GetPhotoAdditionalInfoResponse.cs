using System.Text.Json.Serialization;
using PhotoBank.Photo.Contracts;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotoAdditionalInfoResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("photoId")]
        public int PhotoId { get; set; }

        [JsonPropertyName("additionalInfo")]
        public PhotoAdditionalInfo AdditionalInfo { get; set; }
    }
}
