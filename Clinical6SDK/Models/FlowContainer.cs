using Newtonsoft.Json;
using System.Collections.Generic;
namespace Clinical6SDK.Models
{
    public class FlowContainerV2Root
    {
        [JsonProperty("data")]
        public FlowContainerV2 Data { get; set; }
    }

    public class FlowContainerV2
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public FlowContainerV2Attributes Attributes { get; set; }
    }

    public class FlowContainerV2Attributes
    {
        [JsonProperty("uuid")]
        public string UUID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("flow_processes")]
        public List<FlowContainerV2Flow> FlowProcesses { get; set; }
    }

    public class FlowContainerV2Flow
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("published")]
        public bool IsPublished { get; set; }

        [JsonProperty("initial_step_id")]
        public int InitialStepId { get; set; }

        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }
    }
}
