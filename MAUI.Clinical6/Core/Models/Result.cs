using System;

namespace Xamarin.Forms.Clinical6.Core.Models
{
    public class Result
    {
        //do we need this class or just move up to the generic?
        public bool IsResponseSuccessful { get; set; }

        public string ResponseStatusCode { get; set; }

        public string Message { get; set; }
    }

    public class Result<TSuccess, TError> : Result
    {
        public TSuccess Success { get; set; }
        public TError Error { get; set; }


        public static Result<TSuccess, TError> IsFailure(string responseStatusCode, TError error)
        {
            return new Result<TSuccess, TError>
            {
                Error = error,
                IsResponseSuccessful = false,
                ResponseStatusCode = responseStatusCode
            };
        }

        public static Result<TSuccess, TError> IsSuccess(string responseStatusCode, TSuccess data)
        {
            return new Result<TSuccess, TError>
            {
                Success = data,
                IsResponseSuccessful = true,
                ResponseStatusCode = responseStatusCode
            };
        }

        public static Result<TSuccess, TError> ProblemParsingResponse(string responseStatusCode, string content, Exception exception)
        {
            return new Result<TSuccess, TError>
            {
                IsResponseSuccessful = false,
                ResponseStatusCode = responseStatusCode,
                Message = $"HttpCode: {responseStatusCode} Problem Parsing Response: {exception.Message}. Content: {content}. "
            };
        }

        public static Result<TSuccess, TError> ExceptionGettingResponse(Request<TSuccess, TError> request, Exception exception)
        {
            return new Result<TSuccess, TError>
            {
                IsResponseSuccessful = false,
                Message = $"Exception Getting Response: {exception.Message}. Method: {request.Method}. Url: {request.Uri} "
            };
        }
    }
}