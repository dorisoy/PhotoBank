using System;
using System.Text.Json.Serialization;

namespace PhotoBank.Broker.Api.Contracts
{
    public class GetPhotoResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("photoId")]
        public int PhotoId { get; set; }

        [JsonPropertyName("fileBase64Content")]
        public string FileBase64Content { get; set; }

        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }
    }
}
