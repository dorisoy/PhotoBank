using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PhotoBank.Photo.Contracts;

namespace PhotoBank.Broker.Api.Contracts
{
    public class SetPhotoAdditionalInfoRequest : AuthenticatedRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("photoId")]
        public int PhotoId { get; set; }

        [JsonPropertyName("additionalInfo")]
        public PhotoAdditionalInfo AdditionalInfo { get; set; }
    }
}
