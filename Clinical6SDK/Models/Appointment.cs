using System;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class Appointment : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "appointments";

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("notes")]
        public object Notes { get; set; }

        [JsonProperty("mobile_user")]
        public User MobileUser { get; set; }
    }
}
