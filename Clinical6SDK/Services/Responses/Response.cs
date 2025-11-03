
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Responses
{
	public class Response
	{
		public bool IsResponseSuccessful { get; set; }
		public string ResponseStatusCode { get; set; }

		public string Status { get; set; }
		public string Message { get; set; }
	}

	public class Response<TSuccess, TError> : Response
	{
		public TSuccess Data { get; set; }
		public TError Error { get; set; }

        public static Response<TSuccess, TError> IsFailure(string responseStatusCode, TError error)
        {
            return new Response<TSuccess, TError>
            {
                Error = error,
                IsResponseSuccessful = false,
                ResponseStatusCode = responseStatusCode
            };
        }

        public static Response<TSuccess, TError> IsSuccess(string responseStatusCode, TSuccess data)
        {
            return new Response<TSuccess, TError>
            {
                Data = data,
                IsResponseSuccessful = true,
                ResponseStatusCode = responseStatusCode
            };
        }

        public static Response<TSuccess, TError> ProblemParsingResponse(string responseStatusCode, string content, System.Exception exception)
        {
            return new Response<TSuccess, TError>
            {
                IsResponseSuccessful = false,
                ResponseStatusCode = responseStatusCode,
                Message = $"HttpCode: {responseStatusCode} Problem Parsing Response: {exception.Message}. Content: {content}. "
            };
        }

        public static Response<TSuccess, TError> ExceptionGettingResponse(System.Net.Http.HttpMethod method, System.Uri uri, System.Exception exception)
        {
            return new Response<TSuccess, TError>
            {
                IsResponseSuccessful = false,
                Message = $"Exception Getting Response: {exception.Message}. Method: {method}. Url: {uri} "
            };
        }

        public static TSuccess SucessDeserializer(string content)
        {
            return JsonConvert.DeserializeObject<TSuccess>(content,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static TError ErrorDeserializer(string content)
        {
            return JsonConvert.DeserializeObject<TError>(content);
        }
	}
}

