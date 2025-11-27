using System;
using Clinical6SDK.Services;

namespace Clinical6SDK.Common.Exceptions
{
    public class Clinical6ServerException : Exception
    {
        public ErrorResponse ErrorResponse { get; private set; }

        public Clinical6ServerException()
        {
        }

        public Clinical6ServerException (string message) : base(message)
        {
        }

        public Clinical6ServerException(string message, ErrorResponse errorDict) : base(message)
        {
            ErrorResponse = errorDict;
        }
    }
}
