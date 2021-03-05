using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class PhotoAdditionalInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
