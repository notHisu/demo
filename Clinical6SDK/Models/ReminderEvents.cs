using System;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ReminderEvents : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "reminder__events";

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("extras")]
        public object Extras { get; set; }

        [JsonProperty("source_type")]
        public string SourceType { get; set; }

        [JsonProperty("mobile_user")]
        public User MobileUser { get; set; }
    }
}
