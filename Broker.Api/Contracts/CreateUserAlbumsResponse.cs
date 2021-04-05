using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PhotoBank.Photo.Contracts;

namespace PhotoBank.Broker.Api.Contracts
{
    public class CreateUserAlbumsResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("albums")]
        public IEnumerable<Album> Albums { get; set; }
    }
}
