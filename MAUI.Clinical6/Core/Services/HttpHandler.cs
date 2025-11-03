using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Models;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface IHttpHandler
    {
        Task<Result<T, TError>> GetResult<T, TError>(Request<T, TError> request);
    }

    public class HttpHandler : IHttpHandler
    {
        HttpClient _client;

        public async Task<Result<T, TError>> GetResult<T, TError>(Request<T, TError> request)
        {
            var method = ConvertToHttpEnum(request.Method);

            // Serialize request data
            ///AppContainer.Current.Resolve<ILoggingService>().Debug(request.ToString());

            using (var httpRequest = new HttpRequestMessage(method, request.Uri))
            {
                // Add request data to request
                var content = request.BuildContent();
                if (content != null)
                {
                    httpRequest.Content = content;
                }
                // Add headers to request
                foreach (var h in request.Headers)
                {
                    httpRequest.Headers.Add(h.Key, h.Value);
                }

                try
                {
                    // Get response
                    var response = await GetResponseAsync(httpRequest, request.OnSuccess, request.OnError);
                    return response;
                }
                catch (Exception e)
                {
                    return Result<T, TError>.ExceptionGettingResponse(request, e);
                }
            }
        }

        private HttpMethod ConvertToHttpEnum(Method method)
        {
            switch (method)
            {
                case Method.Get:
                    return HttpMethod.Get;
                case Method.Post:
                    return HttpMethod.Post;
                case Method.Delete:
                    return HttpMethod.Delete;
                case Method.Put:
                    return HttpMethod.Put;
                case Method.Patch:
                    return new HttpMethod("PATCH");
            }
            //TODO: wire in switch statement or ability to convert
            return HttpMethod.Post;
        }

        private async Task<Result<T, TError>> GetResponseAsync<T, TError>(
            HttpRequestMessage request,
            Func<string, T> successResponseDeserializer,
            Func<string, TError> failureResponseDeserializer)
        {
            if (_client == null)
            {
                _client = new HttpClient(new HttpClientHandler());
            }

            Console.WriteLine(string.Format("GetResponseAsync {0}", request.RequestUri.AbsoluteUri));

            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
            {
                var content = response.Content == null ? null : await response.Content.ReadAsStringAsync();
                try
                {
                    Console.WriteLine("Response: " + content);

#if DEBUG
                    Clinical6SDK.Utilities.HttpLogger.Output(request.ToString(), content);
#endif
                    if (response.IsSuccessStatusCode)
                    {
                        return Result<T, TError>.IsSuccess(response.StatusCode.ToString(), successResponseDeserializer(content));
                    }

                    return Result<T, TError>.IsFailure(response.StatusCode.ToString(), failureResponseDeserializer(content));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return Result<T, TError>.ProblemParsingResponse(response.StatusCode.ToString(), content, e);
                }
            }
        }
    }
}