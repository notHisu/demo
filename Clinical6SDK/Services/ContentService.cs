using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Clinical6SDK.Common.Converters;
using Clinical6SDK.Models;
using Clinical6SDK.Helpers;
using Clinical6SDK.Services.Requests;
using JsonApiSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Clinical6SDK.Services
{
    public class ContentService : JsonApiHttpService, IContentService
    {
        /// <summary>
        /// Overrides DeserializeResponse to use JsonApi Settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public override T DeserializeResponse<T>(string content)
        {
            T _return = (T)Activator.CreateInstance(typeof(T));

            if (_return is Content)
            {
                JObject o = JsonConvert.DeserializeObject<JObject>(content, new JsonApiSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> { new FormatNumbersAsStringConverter() }
                });
                Content _content = JsonConvert.DeserializeObject<Content>(content, new JsonApiSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> { new FormatNumbersAsStringConverter() }
                });
                foreach(var attribute in (JObject) o["data"]["attributes"])
                {
                    _content[attribute.Key] = attribute.Value;
                }
                _return = (T)Convert.ChangeType(_content, typeof(T));
            } else if (_return is List<Content>)
            {
                var olist = JsonConvert.DeserializeObject<JObject>(content, new JsonApiSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> { new FormatNumbersAsStringConverter() }
                });
                List<Content> _contentList = JsonConvert.DeserializeObject<List<Content>>(content, new JsonApiSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> { new FormatNumbersAsStringConverter() }
                });

                // assuming they sort the same way, which the should
                for(int i = 0; i < _contentList.Count; ++i)
                {
                    Content _content = _contentList[i];
                    foreach (var attribute in (JObject)olist["data"][i]["attributes"])
                    {
                        _content[attribute.Key] = attribute.Value;
                    }
                }
                _return = (T)Convert.ChangeType(_contentList, typeof(T));
            } else
            {
                _return = base.DeserializeResponse<T>(content);
            }
            return _return;
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
        /// Overrides the conversion to a jobject to use content dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override JObject ModelToJObject(IJsonApiModel obj)
        {
            ((Content) obj).SyncJObject();
            return ((Content) obj).AsJObject;
        }


        /// <summary>
        /// Get from a simple content type string
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<List<Content>> Get(string contentType)
        {
            return await base.Get<List<Content>>(options: new Options
            {
                UriQuery = new
                {
                    filters = new
                    {
                        content_type = new
                        {
                            permanent_link = contentType
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Get Public Contents (same thing but just with a different url)
        /// </summary>
        /// <returns></returns>
        public async Task<List<Content>> GetPublicContents(string locale)
        {
            string url = $"/v3/dynamic_content/public_contents?locale={locale}";
            return await Get<List<Content>>(options: new Options { Url = url });
        }

    }
}
