using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class BadgeModel :  JsonApiModel
    {
        [JsonProperty("type")]
        public string Type  { get; set; }

        [JsonProperty("based_on")]
        public string BasedOn { get; set; }

        [JsonProperty("cache_token")]
        public string CacheToken { get; set; }

        //[JsonProperty("disabled_image")]
        public string DisabledImage { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("featured")]
        public string Featured { get; set; }

        //[JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("redemption_points")]
        public string RedemptionPoints { get; set; }

        [JsonProperty("start_at")]
        public string StartAt { get; set; }

        [JsonProperty("start_point")]
        public string StartPoint { get; set; }

        [JsonProperty("threshold")]
        public int Threshold { get; set; }

        [JsonProperty("time_unit")]
        public string TimeUnit { get; set; }
    }
}
