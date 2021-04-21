using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetLocalizationRequest
    {
        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}
