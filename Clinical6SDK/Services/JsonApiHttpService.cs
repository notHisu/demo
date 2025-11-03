using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonApiSerializer;
using Clinical6SDK.Models;
using Clinical6SDK.Utilities;
using Clinical6SDK.Common.Converters;
using Clinical6SDK.Services.Requests;

namespace Clinical6SDK.Services
{
    public class JsonApiHttpService : BaseHttpService
    {

        /// <summary>
        /// Overrides DeserializeResponse to use JsonApi Settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public override T DeserializeResponse<T>(string content)
        {
            System.Diagnostics.Debug.WriteLine("Json:" + content);
            return JsonConvert.DeserializeObject<T>(content, new JsonApiSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new FormatNumbersAsStringConverter() }
            });
        }

        /// <summary>
        /// Response Errors are not handled by this for json api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public override T DeserializeResponseError<T>(string content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a JsonApiModel to a JObject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual JObject ModelToJObject(IJsonApiModel obj)
        {
            var aux = JsonConvert.SerializeObject(obj, new JsonApiSerializerSettings());
            var json = JsonConvert.DeserializeObject<JObject>((string)aux, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            System.Diagnostics.Debug.WriteLine("Final json:" + json);
            return json;
        }

        /// <summary>
        /// Returns a url used for the v3 endpoint
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GenerateUrl(IJsonApiModel obj, string childType = "", Options options = null)
        {
            Options _options = options == null ? new Options() : options;

            if (!string.IsNullOrWhiteSpace(_options.Url))
                return _options.Url;

            // Allow for another property to be the key
            // This is useful for Languages (for example) to use it's ISO as the url key
            string idString = obj.Id != null && obj.Id > 0 ? string.Format("/{0}", obj.GetType().GetRuntimeProperty(_options.Key).GetValue(obj, null)) : "";

            // Generate type string
            string typeString = obj.Type.Replace("__", "/");

            // Allow for a list of child properties
            string childTypeString = childType.Length > 0 ? string.Format($"/{childType}").Replace("__", "/") : "";

            // Generate url
            string url = string.Format($"/v3/{typeString}{idString}{childTypeString}");

            // Add URI Query to url
            if (options != null && options.UriQuery != null)
                url += "?" + FormatUtility.ObjectToURIQuery(options.UriQuery);

            return url;
        }

        public bool AreParamsValid(
            IList<string> paramsRequired,
            IParams parameters = null,
            string Json = "")
        {
            bool res = false;
            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            if (paramsRequired != null)
            {
                foreach (var p in paramsRequired)
                {
                    res = (_parameters.ContainsKey(p) && _parameters[p] != null);
                    if (!res)
                        throw new ArgumentException(string.Format("Param {0} is required", p)); // break
                }
            }

            return res;
        }

        public IDictionary<string, object> ConvertToDictionary(IEnumerable<JToken> list)
        {
            var dic = new Dictionary<string, object>();
            foreach (var p in list)
            {
                if (p.Type.ToString() == "Property")
                {
                    var prop = p as JProperty;
                    if (dic.ContainsKey(prop.Name))
                    {
                        int x = 1;
                        while (dic.ContainsKey(prop.Name + x.ToString()))
                        {
                            x++;
                        }
                        dic.Add(prop.Name + x.ToString(), prop.Value);
                    }
                    else
                    {
                        dic.Add(prop.Name, prop.Value);
                    }
                }
            }
            return dic;
        }


        /// <summary>
        /// Invokes a Delete call to the server.
        /// </summary>
        /// <returns>An object with Success, Response and Error</returns>
        /// <param name="obj">Object.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> Delete<T>(
            IJsonApiModel obj,
            Options options = null
        ) where T : IJsonApiModel
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");

            return await Send<T>(
                GenerateUrl(obj, options: options),
                httpMethod: HttpMethod.Delete,
                requestData: ModelToJObject(obj));
        }


        /// <summary>
        /// Invokes a Get call to the server.
        /// </summary>
        /// <returns>A IJsonApiModel object</returns>
        /// <param name="obj">Object.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> Get<T>(
            IJsonApiModel obj = null,
            Options options = null
        ) where T : new()
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.Type))
            {
                T t = new T();

                Type type = t is IEnumerable ? t.GetType().GenericTypeArguments[0] : t.GetType();

                // If the type isn't IJsonApiModel, then throw an exception
                if (!type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IJsonApiModel)))
                    throw new Exception("Type Param must implement the IJsonApiModel interface");

                obj = (IJsonApiModel)Activator.CreateInstance(type);
            }

            return await Send<T>(GenerateUrl(obj, options: options));
        }


        /// <summary>
        /// Invokes a Get call to the server to get children of an object.
        /// </summary>
        /// <returns>An object with Success, Response and Error</returns>
        /// <param name="obj">Object.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> GetChildren<T>(
            IJsonApiModel obj,
            string childType = null,
            Options options = null
        ) where T : new()
        {
            if (childType == null || string.IsNullOrWhiteSpace(childType))
            {
                T t = new T();

                Type type = t is IEnumerable ? t.GetType().GenericTypeArguments[0] : t.GetType();

                // If the type isn't IJsonApiModel, then throw an exception
                if (!type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IJsonApiModel)))
                    throw new Exception("Type Param must implement the IJsonApiModel interface");

                obj = (IJsonApiModel)Activator.CreateInstance(type);
            }
            return await Send<T>(GenerateUrl(obj, childType, options));
        }


        /// <summary>
        /// Invokes an Insert(POST) call to the server.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> Insert<T>(
            IJsonApiModel obj,
            Options options = null
        ) where T : IJsonApiModel
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");

            return await Send<T>(
                GenerateUrl(obj, options: options),
                httpMethod: HttpMethod.Post,
                requestData: ModelToJObject(obj));
        }


        /// <summary>
        /// Invokes an Update(PATCH) call to the server.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> Update<T>(
            IJsonApiModel obj,
            Options options = null
        ) where T : IJsonApiModel
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");

            return await Send<T>(
                GenerateUrl(obj, options: options),
                httpMethod: new HttpMethod("PATCH"),
                requestData: ModelToJObject(obj));
        }
    }
}
