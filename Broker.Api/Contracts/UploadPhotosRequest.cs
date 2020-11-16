using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class UploadPhotosRequest : AuthenticatedRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("files")]
        public IEnumerable<string> Files { get; set; }
    }
}
