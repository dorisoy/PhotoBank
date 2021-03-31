using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class SetUserInfoRequest : AuthenticatedRequest
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string EMail { get; set; }

        [JsonPropertyName("about")]
        public string About { get; set; }
    }
}
