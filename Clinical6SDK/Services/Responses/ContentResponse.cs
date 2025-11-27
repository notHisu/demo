using System;
using Clinical6SDK.Models;
using System.Collections.Generic;

namespace Clinical6SDK.Services.Responses
{
    public class ContentResponse<T> 
		where T : ContentModel
	{
		public T Content { get; set; }
	}

    public class EnumerableContentResponse<T>
		where T : ContentModel
	{
		public IEnumerable<T> Content { get; set; }
	}

    public class CreateContentResponse
	{
		public int Id { get; set; }
		public string Messages { get; set; }
	}
}

