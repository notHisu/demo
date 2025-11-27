using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    public class RequestPasswordRequest
    {
        [JsonProperty("account_name")]
        public string Account_name { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
    }
    internal class RequestPasswordRelationship
    {
        [JsonProperty("devices")]
        public Relationship<RequestPasswordRelationshipData> devices { get; set; }
    }

    internal class RequestPasswordRelationshipData : RelationshipData
    {
        public override string Type => "password_resets";
    }
    
}
