using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class StatusModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "statuses";

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }
    }
}
