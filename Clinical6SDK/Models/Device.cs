using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
	public class DeviceModel :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "devices";

        [JsonProperty("mobile_application_key")]
        public string MobileApplicationKey { get; set; }

        [JsonProperty("udid")]
        public string Udid { get; set; }

        [JsonProperty("technology")]
		public string Technology { get; set; }

		[JsonProperty("access_token")]
        public string AccessToken { get; set; }

		[JsonProperty("push_id")]
		public string PushId { get; set; }

		[JsonProperty("app_version")]
		public string AppVersion { get; set; }
        
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }

		[JsonProperty("created_at")]
		public string CreatedAt { get; set; }

		[JsonProperty("updated_at")]
		public string UpdatedAt { get; set; }
	}
}
