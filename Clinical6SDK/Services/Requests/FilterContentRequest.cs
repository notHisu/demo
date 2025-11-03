using System;
using Clinical6SDK.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Requests
{
	public class FilterContentRequest
	{
		[JsonProperty ("filters")]
		public IEnumerable<IContentFilter> Filters { get; set; }

        public string OrderBy { get; set; }

        public bool Minimal { get; set; }

        public bool BrandRelative { get; set; }

        public string ReferenceModule { get; set; }

        public int Page { get; set; }

        public int PerPage { get; set; }

	}
}

