using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class NewAlbum
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
