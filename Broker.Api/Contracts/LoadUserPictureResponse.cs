using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotoBank.Broker.Api.Contracts
{
    public class LoadUserPictureResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("pictureBase64Content")]
        public string PictureBase64Content { get; set; }

        [JsonPropertyName("newPictureId")]
        public string NewPictureId { get; set; }
    }
}
