using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
	public class DevicesRequest
	{
		[JsonProperty("mobile_application_key")]
		public string MobileApplicationKey { get; set; }

		[JsonProperty("technology")]
		public string Technology { get; set; }
	}
}
