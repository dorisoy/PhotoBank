using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class SetUserPictureRequest : AuthenticatedRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("newPictureId")]
        public string NewPictureId { get; set; }
    }
}
