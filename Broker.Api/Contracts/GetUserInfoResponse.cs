using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetUserInfoResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string EMail { get; set; }

        [JsonPropertyName("about")]
        public string About { get; set; }

        [JsonPropertyName("pictureBase64Content")]
        public string PictureBase64Content { get; set; }
    }
}
