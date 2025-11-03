using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
    public class FlowOptions
    {
        [JsonProperty("existing_id")]
        public string ExistingId { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }


        public Entry Entry { get; set; }
    }
}
