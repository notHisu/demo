using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class AppMenu : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "navigation__app_menus";

        [JsonProperty("image")]
        public ImageResources Image { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("action_object")]
        public string ActionObject { get; set; }

        [JsonProperty("action_detail")]
        public DynamicContentAttribute ActionDetail { get; set; }

        [JsonProperty("parent")]
        public AppMenu Parent { get; set; }


        [JsonProperty("dynamic_attributes")]
        public DynamicContentAttribute ContentAttribute
        {
            get;
            set;
        }

        public string ImageRoot { get; set; } = new Clinical6Instance().BaseUrl;

        public string ImageUrl
        {
            get
            {
                var root = ImageRoot;

                var image = Image?.Url;

                if (string.IsNullOrEmpty(image))
                {
                    return string.Empty;
                }

                if (Uri.IsWellFormedUriString(image, UriKind.Absolute))
                {
                    return image;
                }
                //else if (Uri.IsWellFormedUriString(root + image, UriKind.Absolute))
                //{
                //    return string.Format("{0}{1}", root, image);
                //}

                return null;
            }
        }
    }

    public class DynamicContentAttributeKeys
    {
        [JsonProperty("type")]
        public string type { get; set; } = "dynamic_content__attribute_keys";

        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }
    }

    public class DynamicContentAttribute
    {
        [JsonProperty("type")]
        public string type { get; set; } = "dynamic_content__content_types";

        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty("dynamic_attributes")]
        public List<DynamicContentAttributeKeys> DynamicAttributes { get; set; }

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("baseline")]
        public bool Baseline { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }
    }
}
