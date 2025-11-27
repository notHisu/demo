using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
	public class Tag :  JsonApiModel
	{
		[JsonProperty ("name")]
		public string Name { get; set; }
	}
}

