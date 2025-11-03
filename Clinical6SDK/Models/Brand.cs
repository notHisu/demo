
namespace Clinical6SDK.Models
{
	public class Brand : JsonApiModel
	{
		public object Ancestry { get; set; }

		public string Name { get; set; }

		public ImageResources Logo { get; set; }
	}
}

