namespace Clinical6SDK.Services.Responses
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class FlowProcessesResponse
    {
        [JsonProperty("data")]
        public List<FlowProcessesResponseData> Data { get; set; }
    }

    public partial class FlowProcessesResponseData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public FlowProcessesAttributes Attributes { get; set; }
    }

    public partial class FlowProcessesAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("published_at")]
        public object PublishedAt { get; set; }

        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }

        [JsonProperty("initial_step_id")]
        public object InitialStepId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

}