using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
	public class AcceptInvitationRequest
	{
		[JsonProperty("invitation_token")]
		public string InvitationToken { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
	}

	public class InvitationDevicesRelationship
	{
		[JsonProperty("devices")]
		public Relationship<DevicesRelationshipData> Devices { get; set; }
	}

	public class DevicesRelationshipData : RelationshipData
	{
		public override string Type => "devices";
	}
}
