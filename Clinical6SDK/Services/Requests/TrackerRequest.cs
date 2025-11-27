using System;
using Newtonsoft.Json;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services.Requests
{
	internal class TrackerRequest
	{
		[JsonProperty ("tracker")]
		public EventAction Tracker { get; set; }
	}
}

