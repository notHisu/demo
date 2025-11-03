using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    public class AddAwardedBadgeRequest
    {
        [JsonProperty("invitation_token")]
        public string InvitationToken { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
