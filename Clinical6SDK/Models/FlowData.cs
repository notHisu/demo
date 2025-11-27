using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
    public class FlowData :  JsonApiModel
    {
        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }

        [JsonProperty("submitted_at")]
        public string SubmittedAt { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
