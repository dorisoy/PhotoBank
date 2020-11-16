using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotosResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("photoIds")]
        public IEnumerable<int> PhotoIds { get; set; }
    }
}
