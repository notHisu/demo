using Newtonsoft.Json;
using Clinical6SDK.Models;
using Clinical6SDK.Helpers;

namespace Clinical6SDK.Services.Requests
{
	public class BaseSignInRequest
	{
		[JsonProperty ("device")]
		public SignInRequestDevice Device { get; set; }
	}

	public class SignInRequest : BaseSignInRequest
	{
		[JsonProperty ("mobile_user")]
		public SignInRequestMobileUser MobileUser { get; set; }
	}

	public class GuestSignInRequest : BaseSignInRequest
	{
	}

	public class SignUpRequest : SignInRequest
	{
		[JsonProperty ("profile")]
		public Profile Profile { get; set; }
	}

	public class SignInRequestDevice
	{
		[JsonProperty ("udid")]
		public string Udid { get; set; }

		// TODO: Use enum for OS/platform 
		[JsonProperty ("technology")]
		public string Technology { get; set; }
	}

	public class SignInRequestMobileUser
	{
		[JsonProperty ("account_name")]
		public string AccountName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

		[JsonProperty ("password")]
		public string Password { get; set; }
	}
}

