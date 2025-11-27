using System;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
	public class AuthorizationTokenModel :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "authorization_tokens";

        [JsonProperty("service", NullValueHandling = NullValueHandling.Ignore)]
        public string Service { get; set; }

        [JsonProperty("authorize_url", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorizeUrl { get; set; }

        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public bool HasRefreshToken { get; set; }

        [JsonProperty("authorizable", NullValueHandling = NullValueHandling.Ignore)]
        public User Authorizable { get; set; }
	}
}
