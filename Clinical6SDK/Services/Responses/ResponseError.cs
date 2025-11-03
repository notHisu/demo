using Newtonsoft.Json;

namespace Clinical6SDK.Services.Responses
{
	internal class ResponseError
	{
		[JsonProperty ("friendly_error")]
		public string FriendlyError { get; set; }

		[JsonProperty ("internal_code")]
		public int InternalCode { get; set; }
	}
}

