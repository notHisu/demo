using Newtonsoft.Json;
using System.Collections.Generic;
using Clinical6SDK.Services.Requests;
using Clinical6SDK.Helpers;

namespace Clinical6SDK.Models
{
    public class FlowDataGroup : JsonApiModel
    {
        [JsonProperty("owner_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerType { get; set; }

        [JsonProperty("submitted_at", NullValueHandling = NullValueHandling.Ignore)]
        public string SubmittedAt { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonIgnore]
        public string GetType
        {
            get
            {
                return "data_collection__captured_value_groups";
            }
        }

        [JsonProperty("type")]
        public override string Type { get; set; } = "data_collection__captured_value_groups";

        public IList<FlowData> CapturedValues { get; set; }

        public IList<FlowData> FlowData { get; set; }

        public User Creator { get; set; }

        public Flow FlowProcess { get; set; } // Flow

        public User Owner { get; set; }

        [JsonProperty("final_submission")]
        public bool FinalSubmission { get; set; }

        [JsonIgnore]
        private CollectFieldsRelationship _Relationships;
        [JsonIgnore]
        public CollectFieldsRelationship Relationships
        {
            get
            {
                GetRelationships();
                return _Relationships;
            }
        }
        private void GetRelationships()
        {
            _Relationships = new CollectFieldsRelationship();
            if (Creator != null)
            {
                _Relationships.CreatorFlowProcess = new Relationship<UserelationshipData>
                {
                    Data = new UserelationshipData
                    {
                        Id = Owner.Id
                    }
                };
            }
            if (FlowProcess != null)
            {
                _Relationships.FlowProcess = new Relationship<FlowProcessRelationshipData>
                {
                    Data = new FlowProcessRelationshipData
                    {
                        Id = FlowProcess.Id
                    }
                };
            }
            if (Owner != null)
            {
                _Relationships.OwnerFlowProcess = new Relationship<UserelationshipData>
                {
                    Data = new UserelationshipData
                    {
                        Id = Owner.Id
                    }
                };
            }
        }
    }
}
