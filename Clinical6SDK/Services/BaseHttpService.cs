using Clinical6SDK.Common.Exceptions;
using Clinical6SDK.Services.Requests;
using Clinical6SDK.Services.Responses;
using Clinical6SDK.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Clinical6SDK.Services
{
    public abstract class BaseHttpService
    {
        public virtual string BaseUrl
        {
            get => ClientSingleton.Instance.BaseUrl;
            set => ClientSingleton.Instance.BaseUrl = value;
        }

        public virtual string AuthToken
        {
            get => ClientSingleton.Instance.AuthToken;
            set => ClientSingleton.Instance.AuthToken = value;
        }

        public HttpMessageHandler HttpMessageHandler
        {
            get => ClientSingleton.Instance.HttpMessageHandler;
            set => ClientSingleton.Instance.HttpMessageHandler = value;
        }

        public HttpClient HttpClient => ClientSingleton.Instance.HttpClient;

        public abstract T DeserializeResponse<T>(string content);

        public abstract T DeserializeResponseError<T>(string content);

        public bool HasToken => !string.IsNullOrWhiteSpace(AuthToken);


        /// <summary>
        /// Gets an object (or list of objects)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(Options options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options is required");
            if (string.IsNullOrEmpty(options.Url))
                throw new Exception("Options.Url is required");

            return await Send<T>(options.Url);
        }

        /// <summary>
        /// Deletes an object (non-json api)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> Delete<T>(object obj, Options options)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options is required");
            if (string.IsNullOrEmpty(options.Url))
                throw new Exception("Options.Url is required");

            string aux = obj is string ? (string)obj : JsonConvert.SerializeObject(obj, new JsonSerializerSettings());
            JObject json = JsonConvert.DeserializeObject<JObject>(aux, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            System.Diagnostics.Debug.WriteLine("Final json:" + json);

            return await Send<T>(options.Url, httpMethod: HttpMethod.Delete, requestData: json);
        }

        /// <summary>
        /// Inserts an object (non-json api)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> Insert<T>(object obj, Options options)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options is required");
            if (string.IsNullOrEmpty(options.Url))
                throw new Exception("Options.Url is required");

            string aux = obj is string ? (string)obj : JsonConvert.SerializeObject(obj, new JsonSerializerSettings());
            JObject json = JsonConvert.DeserializeObject<JObject>(aux, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            System.Diagnostics.Debug.WriteLine("Final json:" + json);

            return await Send<T>(options.Url, httpMethod: HttpMethod.Post, requestData: json);
        }

        /// <summary>
        /// Updates an object (non-json api)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> Update<T>(object obj, Options options)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options is required");
            if (string.IsNullOrEmpty(options.Url))
                throw new Exception("Options.Url is required");

            string aux = obj is string ? (string)obj : JsonConvert.SerializeObject(obj, new JsonSerializerSettings());
            JObject json = JsonConvert.DeserializeObject<JObject>(aux, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            System.Diagnostics.Debug.WriteLine("Final json:" + json);

            return await Send<T>(options.Url, httpMethod: new HttpMethod("PATCH"), requestData: json);
        }

        /// <summary>
        /// Official way to send data to the server.  Removing other calls.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="httpMethod"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task<T> Send<T>(
            string url,
            IDictionary<string, string> headers = null,
            HttpMethod httpMethod = null,
            object requestData = null)
        {
            // Default to Get
            var method = httpMethod ?? HttpMethod.Get;

            // Serialize request data
            var data = requestData == null
                ? null
                : (requestData is string
                   ? requestData.ToString()
                   : JsonConvert.SerializeObject(requestData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            System.Diagnostics.Debug.WriteLine("Final URL:" + url);

            using (var request = new HttpRequestMessage(method, new Uri(string.Format($"{BaseUrl}{url}"))))
            {

                // Allow for default post calls
                if (method == HttpMethod.Get && data != null)
                    request.Method = HttpMethod.Post;

                if (data != null)
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");

                if (!request.Headers.Contains("Authorization") && !string.IsNullOrWhiteSpace(AuthToken))
                    request.Headers.TryAddWithoutValidation("Authorization", string.Format("Token token={0}", AuthToken));

                // Add headers to request
                if (headers != null)
                    foreach (var h in headers)
                        request.Headers.TryAddWithoutValidation(h.Key, h.Value);

                // Get response
                return await SendAsync<T>(request);
            }
        }

        /// <summary>
        /// Sends but enables custom serializer and deserializers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="url"></param>
        /// <param name="successResponseDeserializer"></param>
        /// <param name="failureResponseDeserializer"></param>
        /// <param name="httpMethod"></param>
        /// <param name="headers"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task<Response<T, TError>> Send<T, TError>(
            string url,
            Func<string, T> successResponseDeserializer,
            Func<string, TError> failureResponseDeserializer,
            HttpMethod httpMethod = null,
            IDictionary<string, string> headers = null,
            object requestData = null)
        {
            // Default to GET
            var method = httpMethod ?? HttpMethod.Get;

            // Serialize request data
            var data = requestData == null
                ? null
                : (requestData is string ? requestData.ToString() : JsonConvert.SerializeObject(requestData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            string _url = string.Format($"{BaseUrl}{url}");
            System.Diagnostics.Debug.WriteLine("Final URL:" + url);

            using (var request = new HttpRequestMessage(method, new Uri(_url)))
            {
                // Add request data to request
                if (data != null)
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");

                if (!request.Headers.Contains("Authorization") && !string.IsNullOrWhiteSpace(AuthToken))
                    request.Headers.TryAddWithoutValidation("Authorization", string.Format("Token token={0}", AuthToken));

                // Add headers to request
                if (headers != null)
                    foreach (var h in headers)
                        request.Headers.Add(h.Key, h.Value);

                // Get response
                var response = await SendAsync(
                    request,
                    successResponseDeserializer,
                    failureResponseDeserializer);

                return response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await HttpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<T> SendAsync<T>(HttpRequestMessage request)
        {
            T result;
            var response = await SendAsync(request);
            var content = response.Content == null ? null : await response.Content.ReadAsStringAsync();
#if DEBUG
            HttpLogger.Output(request.ToString(), content);
#endif
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Response: " + content);
                    result = DeserializeResponse<T>(content);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    System.Diagnostics.Debug.WriteLine(content);
                    throw new Clinical6ServerException(ex.ToString());
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(content);
                var errorDic = JsonConvert.DeserializeObject<ErrorResponse>(content);
                throw new Clinical6ServerException("Server Error, please try again!", errorDic);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="request"></param>
        /// <param name="successResponseDeserializer"></param>
        /// <param name="failureResponseDeserializer"></param>
        /// <returns></returns>
        /// @Deprecated
        async Task<Response<T, TError>> SendAsync<T, TError>(
            HttpRequestMessage request,
            Func<string, T> successResponseDeserializer,
            Func<string, TError> failureResponseDeserializer)
        {
            var result = new Response<T, TError>
            {
                IsResponseSuccessful = false,
                Data = default(T),
                Error = default(TError)
            };
            try
            {
                var response = await SendAsync(request);
                var content = response.Content == null ? null : await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    result.Data = successResponseDeserializer(content);
                    result.IsResponseSuccessful = true;
                    result.ResponseStatusCode = response.StatusCode.ToString();
                }
                else
                {
                    result.Error = failureResponseDeserializer(content);
                    result.IsResponseSuccessful = false;
                    result.ResponseStatusCode = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return result;
        }

        /// <summary>
        /// Converts a dictionary to a valid URI Query based on the valid keys
        /// </summary>
        /// <param name="dict">Dictionary to use for uri query parameters</param>
        /// <param name="keys">Valid Keys for the uri query, default are "filters", "page", "sort", "search"</param>
        /// <returns></returns>
        public string GetURIQuery(
            IDictionary<string, object> dict,
            string[] keys = null)
        {
            string[] validKeys = { "filters", "page", "sort", "search" };

            Dictionary<string, object> validQuery = new Dictionary<string, object>();
            foreach (string key in keys ?? validKeys)
                if (dict.ContainsKey(key))
                    validQuery.Add(key, dict[key]);

            string uriQuery = FormatUtility.ObjectToURIQuery(validQuery);
            return string.IsNullOrEmpty(uriQuery) ? "" : "?" + uriQuery;
        }
    }
}