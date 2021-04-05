using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PhotoBank.Photo.Contracts
{
    [Serializable]
    public class Album
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
