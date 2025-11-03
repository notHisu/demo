namespace Clinical6SDK.Services.Responses
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class MobileUserSessionResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("included")]
        public List<Included> Included { get; set; }
    }

    public partial class MobileUserSessionDataAttributes
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }

        [JsonProperty("invitation_sent_at", NullValueHandling = NullValueHandling.Ignore)]
        public object InvitationSentAt { get; set; }

        [JsonProperty("invitation_accepted_at", NullValueHandling = NullValueHandling.Ignore)]
        public object InvitationAcceptedAt { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("disabled_at", NullValueHandling = NullValueHandling.Ignore)]
        public object DisabledAt { get; set; }

        [JsonProperty("password_expired_at", NullValueHandling = NullValueHandling.Ignore)]
        public string PasswordExpiredAt { get; set; }
    }

    public partial class DataRelationships
    {
        [JsonProperty("devices")]
        public Devices Devices { get; set; }

        [JsonProperty("user_role")]
        public Language Userole { get; set; }

        [JsonProperty("profile")]
        public Language Profile { get; set; }

        [JsonProperty("language")]
        public Language Language { get; set; }

        [JsonProperty("patient")]
        public Language Patient { get; set; }

        [JsonProperty("threads")]
        public CohortAssignments Threads { get; set; }

        [JsonProperty("site_member")]
        public Language SiteMember { get; set; }

        [JsonProperty("overall_status")]
        public HomeLocation OverallStatus { get; set; }

        [JsonProperty("cohort_assignments")]
        public CohortAssignments CohortAssignments { get; set; }

        [JsonProperty("home_location")]
        public HomeLocation HomeLocation { get; set; }
    }

    public partial class CohortAssignments
    {
        [JsonProperty("data")]
        public List<object> Data { get; set; }
    }

    public partial class Devices
    {
        [JsonProperty("data")]
        public List<Data> Data { get; set; }
    }

    public partial class HomeLocation
    {
        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public partial class Language
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

   
    public partial class MobileUserSessionIncludedAttributes
    {
        [JsonProperty("udid", NullValueHandling = NullValueHandling.Ignore)]
        public string Udid { get; set; }

        [JsonProperty("technology", NullValueHandling = NullValueHandling.Ignore)]
        public string Technology { get; set; }

        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        [JsonProperty("push_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PushId { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }

        [JsonProperty("app_version")]
        public object AppVersion { get; set; }

        [JsonProperty("permanent_link", NullValueHandling = NullValueHandling.Ignore)]
        public string PermanentLink { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("is_super", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSuper { get; set; }

        [JsonProperty("is_admin", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAdmin { get; set; }

        [JsonProperty("is_mobile", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMobile { get; set; }

        [JsonProperty("is_default", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDefault { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("can_view_pii", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanViewPii { get; set; }

        [JsonProperty("member_type", NullValueHandling = NullValueHandling.Ignore)]
        public string MemberType { get; set; }

        [JsonProperty("consent_status")]
        public object ConsentStatus { get; set; }

        [JsonProperty("last_consented_at")]
        public object LastConsentedAt { get; set; }
    }

    public partial class IncludedRelationships
    {
        [JsonProperty("site")]
        public Language Site { get; set; }

        [JsonProperty("mobile_user")]
        public Language MobileUser { get; set; }

        [JsonProperty("consent_strategy")]
        public HomeLocation ConsentStrategy { get; set; }
    }
}
