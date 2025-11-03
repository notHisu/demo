using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    public class RegisterRequest
    {
        [JsonProperty("guest")]
        public string Guest { get; set; }
        
        [JsonProperty("account_name")]
        public string Account_name { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
    }
    
    internal class RegisterRelationship
    {
        [JsonProperty("devices")]
        public Relationship<RegisterRelationshipData> devices { get; set; }
    }

    internal class RegisterRelationshipData : RelationshipData
    {
        public override string Type => "devices";
    }
}
