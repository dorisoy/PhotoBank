using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PhotoBank.Photo.Contracts;

namespace PhotoBank.Broker.Api.Contracts
{
    public class CreateUserAlbumsRequest : AuthenticatedRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("newAlbums")]
        public IEnumerable<NewAlbum> NewAlbums { get; set; }
    }
}
