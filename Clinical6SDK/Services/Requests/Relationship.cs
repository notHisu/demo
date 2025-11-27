using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
    public class Relationship<T>
		where T : RelationshipData
	{
		[JsonProperty("data")]
		public T Data { get; set; }
	}

    public class RelationshipData
	{
		[JsonProperty("type")]
        public virtual string Type { get; set; }

		[JsonProperty("id")]
		public int? Id { get; set; }
	}
}
