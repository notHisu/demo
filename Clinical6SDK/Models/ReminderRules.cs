using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ReminderRules : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "reminder__rules";

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
