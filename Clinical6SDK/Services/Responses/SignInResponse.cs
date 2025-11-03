using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Responses
{
	public class SignInResponse : Response
	{
		[JsonProperty ("auth_token")]
		public string AuthToken { get; set; }

		[JsonProperty ("encryption_key")]
		public string EncryptionKey { get; set; }

		[JsonProperty ("verification_status")]
		public string VerificationStatus { get; set; }
	}
}

