using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

using Clinical6SDK.Services.Responses;
using JsonApiSerializer;
using System.Reflection;
using Clinical6SDK.Common.Exceptions;
using Clinical6SDK.Models;
using Clinical6SDK.Utilities;
using Clinical6SDK.Common.Converters;
using System.Linq;
using Clinical6SDK.Services.Requests;

namespace Clinical6SDK.Services
{
	public class GenericJsonHttpService : BaseHttpService
	{
		public override T DeserializeResponse<T>(string content)
		{
			return JsonConvert.DeserializeObject<T> (content, 
				new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		}

		public override T DeserializeResponseError<T>(string content)
		{
			return JsonConvert.DeserializeObject<T> (content);
		}
    }
}

