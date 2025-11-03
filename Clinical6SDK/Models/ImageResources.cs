using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
	public class ImageResources
	{
		public string Url { get; set; }

		public ImageResource Thumb { get; set; }

		public ImageResource Small { get; set; }

		[JsonProperty ("small_hd")]
		public ImageResource SmallHD { get; set; }

		public ImageResource Fullscreen { get; set; }

		[JsonProperty ("fullscreen_hd")]
		public ImageResource FullscreenHD { get; set; }

		public ImageResource Main { get; set; }

		[JsonProperty ("main_hd")]
		public ImageResource MainHD { get; set; }

		[JsonProperty ("iphone_4")]
		public ImageResource iPhone4 { get; set; }

		[JsonProperty ("iphone_5")]
		public ImageResource iPhone5 { get; set; }

		[JsonProperty ("iphone_6")]
		public ImageResource iPhone6 { get; set; }

		[JsonProperty ("iphone_6_plus")]
		public ImageResource iPhone6Plus { get; set; }

		[JsonProperty ("ipad_non_retina")]
		public ImageResource iPadNonRetina { get; set; }

		[JsonProperty ("ipad_retina")]
		public ImageResource iPadRetina { get; set; }

		[JsonProperty ("galaxy_s3")]
		public ImageResource GalaxyS3 { get; set; }

		[JsonProperty ("galaxy_s4")]
		public ImageResource GalaxyS4 { get; set; }

		[JsonProperty ("nexus_7_2012")]
		public ImageResource Nexus72012 { get; set; }

		[JsonProperty ("nexus_7_2013")]
		public ImageResource Nexus72013 { get; set; }

		[JsonProperty ("nexus_10_2013")]
		public ImageResource Nexus102013 { get; set; }
	}

	public class ImageResource
	{
		public string Url { get; set; }
	}
}

