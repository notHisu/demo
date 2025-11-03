using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
    public class SettingsData : JsonApiModel
    {
        [JsonProperty("type")]
        public List<AppSettings> Settings { get; set; }
    }

    public class AppSettings
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = "settings";

        [JsonProperty("attributes")]
        public SettingsAttributes SettingsAttributes { get; set; }
    }

    public class SettingsAttributes
    {
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}