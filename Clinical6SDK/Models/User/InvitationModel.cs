using System;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;
using Device = Clinical6SDK.Helpers.Device;

namespace Clinical6SDK.Models
{
    public class InvitationModel : JsonApiModel
	{
        [JsonProperty("type")]
        public override string Type { get; set; } = "invitations";

        [JsonProperty("invitation_token", NullValueHandling = NullValueHandling.Ignore)]
        public string InvitationToken { get; set; }

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        [JsonProperty("terms_of_use_accepted", NullValueHandling = NullValueHandling.Ignore)]
        public bool TermsOfUseAccepted { get; set; }

        [JsonProperty("privacy_policy_accepted", NullValueHandling = NullValueHandling.Ignore)]
        public bool PrivacyPolicyAccepted { get; set; }

        [JsonProperty("anti_spam_accepted", NullValueHandling = NullValueHandling.Ignore)]
        public bool AntiSpamAccepted { get; set; }

        [JsonProperty("firstname", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("dob", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime dob { get; set; }

        [JsonProperty("account_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountName { get; set; }

        [JsonProperty("devices", NullValueHandling = NullValueHandling.Ignore)]
        public Device Device { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public Language language { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public Role Role { get; set; }

        [JsonProperty("site", NullValueHandling = NullValueHandling.Ignore)]
        public Site Site { get; set; }

        [JsonProperty("strategy", NullValueHandling = NullValueHandling.Ignore)]
        public ConsentStrategyModel Strategy { get; set; }

        //[JsonProperty("cohort", NullValueHandling = NullValueHandling.Ignore)]
        //public Cohort Cohort { get; set; }
    }
}
