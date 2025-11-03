using System;

namespace Clinical6SDK.Services
{
	public interface IClinical6Service
	{
		string BaseUrl { get; set; }
		string AuthToken { get; set; }
	}
}

