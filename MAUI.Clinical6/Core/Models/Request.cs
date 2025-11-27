using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms.Clinical6.Core.Helpers;

namespace Xamarin.Forms.Clinical6.Core.Models
{
    public class Request<T, TError>
    {
        private readonly bool _includeNullFieldsInRequest;
        private const string AuthorizationKey = "Authorization";

        public Request(Uri uri, object requestData = null, Method method = Method.Get, Func<string, T> onSuccess = null, Func<string, TError> onError = null,
            IDictionary<string, string> headers = null, bool includeNullFieldsInRequest = true)
        {
            _includeNullFieldsInRequest = includeNullFieldsInRequest;
            RequestData = requestData;
            OnSuccess = onSuccess ?? DefaultResponseDeserializer;
            OnError = onError ?? DefaultErrorDeserializer;
            Headers = headers ?? new Dictionary<string, string>();
            Method = method;
            Uri = uri;
        }

        /// <summary>
        /// Simple method so you don't have to pass in a dictionary using the constructor
        /// </summary>
        /// <param name="authToken">the actual auth token.</param>
        public void AddAuthToken(string authToken)
        {
            //Headers.Add(AuthorizationKey, $"Token token=\"{authToken}\"");
            Headers.Add(AuthorizationKey, string.Format("Token token={0}", authToken));
        }

        protected T DefaultResponseDeserializer(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(content,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Converters = new List<JsonConverter>
                        {
                        new SubTaskResultsAttributesConverter()
                        }
                    });
            }
            catch(Exception ex)
            {
                // Add extra info about specifically a deserialization issue.
                string message = "Deserialization Problem: " + ex.Message;
                Exception serializationEx = new Exception(message);
                throw serializationEx;
                //return default(T);
            }
        }

        protected TError DefaultErrorDeserializer(string content)
        {
            return JsonConvert.DeserializeObject<TError>(content);
        }

        public Uri Uri { get; }

        public IDictionary<string, string> Headers { get; }
        public Method Method { get; }

        /// <summary>
        /// Public setter for integration test purposes.
        /// </summary>
        public Func<string, T> OnSuccess { get; set; }

        public Func<string, TError> OnError { get; }

        /// <remarks>
        /// Could makr this private
        /// </remarks>
        private object RequestData { get; }


        public virtual HttpContent BuildContent()
        {
            var data = RequestData == null
                ? null
                : JsonConvert.SerializeObject(RequestData, new JsonSerializerSettings
                {
                    NullValueHandling = _includeNullFieldsInRequest ? NullValueHandling.Include : NullValueHandling.Ignore
                });

            Console.WriteLine(data);

            return data == null ? null : new StringContent(data, Encoding.UTF8, "application/json");
        }
    }
}