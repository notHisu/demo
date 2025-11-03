using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
	public class MobileMenu :  JsonApiModel
	{
		public string Action { get;	set; }

		[JsonProperty ("action_object")]
		public string ActionObject { get; set; }

		[JsonProperty ("featured")]
		public bool IsFeatured { get; set; }

		public int Depth { get; set; }

		public Tag[] Tags { get; set; }

		[JsonProperty ("content_id")]
		public int ContentId { get; set; }

		public MobileMenuImage Image { get; set; }

		[JsonProperty ("subcategories")]
		public IList<MobileMenu> SubCategories { get; set; }
	}

	public class MobileMenuImage
	{
		public ImageResources Image { get; set; }
	}
}

