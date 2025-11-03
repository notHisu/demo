using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class FlowConnection :  JsonApiModel, ITaskable
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "c6__flow_connections";

        [JsonProperty("flow_process", NullValueHandling = NullValueHandling.Ignore)]
        public Flow Flow { get; set; }
    }
}
