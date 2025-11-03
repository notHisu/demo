using System;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class InvitationValidationModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "invitation_validations";

        [JsonProperty("invitation_token", NullValueHandling = NullValueHandling.Ignore)]
        public string InvitationToken { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
    }
}
