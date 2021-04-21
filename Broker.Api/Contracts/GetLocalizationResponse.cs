using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetLocalizationResponse
    {
        [JsonPropertyName("availableLanguages")]
        public IEnumerable<Language> AvailableLanguages { get; set; }

        [JsonPropertyName("locale")]
        public Dictionary<string, string> Locale { get; set; }
    }

    [Serializable]
    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
