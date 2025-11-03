using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    public class AddEntryRequest
    {
    }
    
    internal class EntryRelationship
    {
        [JsonProperty("template")]
        public Relationship<EntrysRelationshipData> template { get; set; }
    }

    internal class EntrysRelationshipData : RelationshipData
    {
        public override string Type => "ediary__entry_templates";
    }
}
