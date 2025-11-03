using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

using JsonApiSerializer;
using Clinical6SDK.Common.Converters;
using Clinical6SDK.Services;
using System;
using Clinical6SDK.Models;

namespace Clinical6SDK.Helpers
{
    /// <summary>
    /// A dynamic indexed model to support Dynamic Content
    /// </summary>
    /// <example>
    /// <code>
    /// var service = new ContentService();
    /// List{Content} content = await service.Get("cars");
    /// var myCar = content[0];
    /// myCar = await myCar.Save();
    ///
    /// var sameCar = await service.Get{Content}(myCar);
    ///
    /// Console.WriteLine(myCar["make"]);
    /// myCar["make"] = "Toyota";
    /// myCar["model"] = "Corolla";
    /// Console.WriteLine(myCar["make"]);
    /// Console.WriteLine(myCar["model"]);
    /// </code>
    /// </example>
    public class Content : JsonApiModel, IComparable
    {
        public override string Type { get; set; } = "dynamic_content__contents";

        [JsonIgnore]
        public JObject Attributes { get; set; } = new JObject();


        private ContentTypeModel contentType;

        [JsonProperty("content_type", NullValueHandling = NullValueHandling.Ignore)]
        public ContentTypeModel ContentType {
            get => contentType;
            set { contentType = value; SyncJObject(); }
        }

        [JsonIgnore]
        public JObject AsJObject => _jobject;

        [JsonIgnore]
        private JObject _jobject;

        [JsonIgnore]
        virtual public object this[string key]
        {
            get
            {
                if (Attributes != null && Attributes[key] != null)
                {
                    Type valueType;
                    if (Attributes[key] is JArray)
                    {
                        return Attributes[key];
                    }
                    else if (((JValue)Attributes[key]).Value != null)
                    {
                        valueType = ((JValue)Attributes[key]).Value.GetType();
                        return Attributes[key].ToObject(valueType);
                    }
                }
                return null;
            }
            set
            {
                if(key == "id")
                {
                    Id = (int?)value;
                    Attributes.Add(new JProperty("id", value));
                }
                else
                {
                    if (Attributes == null)
                        Attributes = new JObject();
                    Attributes[key] = JToken.FromObject(value);
                    SyncJObject();
                }
            }
        }

        public void SyncJObject()
        {
            var aux = JsonConvert.SerializeObject(this, new JsonApiSerializerSettings());

            _jobject = JsonConvert.DeserializeObject<JObject>(aux, new JsonApiSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new FormatNumbersAsStringConverter() }
            });
            if (_jobject["data"] == null)
                _jobject["data"] = new JObject();
            _jobject["data"]["attributes"] = Attributes;
        }

        public async Task<Content> Save()
        {
            var service = new ContentService();
            return (this.Id != null || this.Id >= 0)
                ? await service.Update<Content>(this)
                : await service.Insert<Content>(this);
        }

        public int CompareTo(object obj)
        {
            Content c = (Content)obj;
            return string.Compare(this["title"].ToString(), c["title"].ToString());
        }

        //public override string ToString()
        //{
        //    return AsJObject.ToString();
        //}

        //public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        //{
        //    return (IEnumerator<KeyValuePair<string, object>>)AsJObject.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this.GetEnumerator();
        //}

    }
}

