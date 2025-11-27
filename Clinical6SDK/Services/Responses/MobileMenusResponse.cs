using System.Collections.Generic;
using Newtonsoft.Json;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services.Responses
{
	public class  MobileMenusResponse
	{
		[JsonProperty ("json")]
		public MobileMenusResponseJsonObjectWrapper Response { get; set; }
	}

	public class MobileMenusResponseJsonObjectWrapper
	{
		[JsonProperty ("mobile_menus")]
		public IList<MobileMenu> MobileMenus { get; set; }
	}
}

