using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class SsoOptions
    {
        [JsonProperty("type")]
        public string type { get; set; } = "sso_options";

        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty("user_type")]
        public string UserType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string Title
        {
            get
            {
                return string.Format("Sign In With {0}", Name);
            }
        }

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("mobile_application_name")]
        public string MobileApplicationName { get; set; }
    }
}
