using Newtonsoft.Json;
using System;

namespace Clinical6SDK.Models
{
    public class AwardedBadge :  JsonApiModel
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("earned_on_date")]
        public DateTime EarnedOnDate { get; set; }
    }
}
