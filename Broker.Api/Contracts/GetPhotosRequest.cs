using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotosRequest : AuthenticatedRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("albumsId")]
        public IEnumerable<int> AlbumsId { get; set; }
    }
}
