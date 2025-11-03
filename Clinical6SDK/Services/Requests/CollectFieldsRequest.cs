using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    public class CollectFieldsRequest
    {
        public Dictionary<int, string> Attributes { get; set;  }
    }

    public class CollectFieldsRelationship
    {
        [JsonProperty("creator")]
        public Relationship<UserelationshipData> CreatorFlowProcess { get; set; }

        [JsonProperty("flow_process")]
        public Relationship<FlowProcessRelationshipData> FlowProcess { get; set; }

        [JsonProperty("owner")]
        public Relationship<UserelationshipData> OwnerFlowProcess { get; set; }
    }
    public class FlowProcessRelationshipData : RelationshipData
    {
        public override string Type { get => "data_collection__flow_processes"; }
    }
    public class UserelationshipData : RelationshipData
    {
        public override string Type { get => "mobile_users"; }
    }
}
