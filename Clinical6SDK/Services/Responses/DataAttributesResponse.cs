using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Responses
{
	internal class DataAttributesResponse<TData>
		where TData : Models.JsonApiModel
	{
		[JsonProperty("data")]
		public DataAttributes<TData> DataAttributes { get; set; }
	}

	internal class DataAttributesResponse<TData, TIncluded>
		: DataAttributesResponse<TData>
		where TData : Models.JsonApiModel
        where TIncluded : Models.JsonApiModel
	{
		[JsonProperty("included")]
		public DataAttributes<TIncluded>[] IncludedAttributes { get; set; }
	}

	internal class DataAttributes<T>
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public object Relationships { get; set; }
		public T Attributes { get; set; }
	}
}
