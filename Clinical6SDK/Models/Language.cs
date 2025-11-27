using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class Language :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "languages";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("iso")]
        public string Iso { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        //[JsonIgnore]
        //public List<Translation> translations;
    }
}
