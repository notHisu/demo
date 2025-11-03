using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
	public class EventAction
	{
		[JsonProperty ("action")]
		public string Action { get; set; }

		[JsonProperty ("section")]
		public string Section { get; set; }
	}
}

