using System;

namespace Clinical6SDK.Common.Exceptions
{
	public class Clinical6UnauthorizedException : Exception
	{
		public Clinical6UnauthorizedException ()
		{
		}

		public Clinical6UnauthorizedException (string message)
			: base(message)
		{
		}
	}
}

