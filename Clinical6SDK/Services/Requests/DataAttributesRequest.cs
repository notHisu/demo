using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
	internal class DataAttributesRequest<T>
	{
		[JsonProperty("data")]
		public DataAttributes<T> DataAttributes { get; set; }
	}

	internal class DataAttributesRequest<T, TRelationships>
		: DataAttributesRequest<T>
	{
		[JsonProperty("data")]
		public new DataAttributes<T, TRelationships> DataAttributes { get; set; }
	}

    internal class DataAttributes<T>
    {
        [JsonProperty("attributes")]
        public T Attributes { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

	internal class DataAttributes<T, TRelationships>
		: DataAttributes<T>
	{
		[JsonProperty("relationships")]
		public TRelationships Relationships { get; set; }
	}
}
