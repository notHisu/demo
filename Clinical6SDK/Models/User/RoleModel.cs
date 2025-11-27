using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class RoleModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "user_roles";

        [JsonProperty("can_view_pii", NullValueHandling = NullValueHandling.Ignore)]
        public bool CanViewPii { get; set; }

        [JsonProperty("is_super", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSuper { get; set; }

        [JsonProperty("is_admin", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAdmin { get; set; }

        [JsonProperty("is_mobile", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsMobile { get; set; }

        [JsonProperty("is_default", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsDefault { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("permanent_link", NullValueHandling = NullValueHandling.Ignore)]
        public string PermanentLink { get; set; }

    }
}
