using System;
using Clinical6SDK.Models;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
	internal class CreateContentRequest
	{
		[JsonProperty ("content")]
		public ContentModel Content { get; set; }
	}
}

