using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PhotoBank.Broker.Api.Localization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetLocalizationResponse
    {
        [JsonPropertyName("locale")]
        public Dictionary<string, string> Locale { get; set; }
    }
}
