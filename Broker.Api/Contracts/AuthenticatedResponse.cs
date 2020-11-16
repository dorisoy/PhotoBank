using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class AuthenticatedResponse
    {
        [JsonPropertyName("isAuthenticated")]
        public bool IsAuthenticated { get; set; }
    }
}
