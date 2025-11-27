using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    internal class GetInputDataByFlowRequest
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }

        [JsonProperty("existing_id")]
        public string ExistingId { get; set; }

        [JsonProperty("captured_value_group_id")]
        public string CapturedValueGroupId { get; set; }
    }
}
