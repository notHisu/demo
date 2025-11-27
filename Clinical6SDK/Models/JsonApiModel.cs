using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
	public abstract class JsonApiModel : IJsonApiModel
	{
        [JsonIgnore]
        public int? Id
        {
            get { return string.IsNullOrWhiteSpace(sId) ? null : int.Parse(sId) as int?; }
            set { sId = value.ToString(); }
        }

        [JsonProperty("type")]
        public virtual string Type { get; set; }

        /// <summary>
        /// json:api only works if id is a string
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        internal string sId { get; set; }

        [JsonProperty ("created_at", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? CreatedAt { get; set; }

        [JsonProperty ("updated_at", NullValueHandling = NullValueHandling.Ignore)]
		public DateTime? UpdatedAt { get; set; }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
		public int? Position { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
		public string Title { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
		public string Description { get; set; }
    }
}

