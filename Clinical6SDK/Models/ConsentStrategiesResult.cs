using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ConsentStrategiesResult
    {
        [JsonProperty("included")]
        public ConsentIncluded[] Included { get; set; }
    }

    public class ConsentStrategiesdataSite
    {
        [JsonProperty("id")]
        public string id { get; set; }
    }

    public class ConsentIncluded
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("attributes")]
        public Consentattributes Attributes { get; set; }
    }

    public class Consentattributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("strategy_type")]
        public string StrategyType { get; set; }
    }
}
