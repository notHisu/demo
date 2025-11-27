using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    public class ResetPasswordRequest
    {
        [JsonProperty("reset_password_token")]
        public string Reset_Password_Token { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
    }
    
    internal class ResetPasswordRelationship
    {
        [JsonProperty("devices")]
        public Relationship<ResetPasswordRelationshipData> devices { get; set; }
    }

    internal class ResetPasswordRelationshipData : RelationshipData
    {
        public override string Type => "devices";
    }
}
