using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;
using Device = Clinical6SDK.Helpers.Device;

namespace Clinical6SDK.Models
{
    public class UserModel : JsonApiModel
	{
        [JsonProperty("type")]
        public override string Type { get; set; } = "mobile_users";

        [JsonProperty("account_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountName { get; set; }

        [JsonProperty("initial_account_name", NullValueHandling = NullValueHandling.Ignore)]
        public string InitialAccountName { get; set; }

        [JsonProperty("subject_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SubjectId { get; set; }

        [JsonProperty("firstname", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("encryption_key", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptionKey { get; set; }

        [JsonProperty("Uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("verified_at", NullValueHandling = NullValueHandling.Ignore)]
        public string VerifiedAt { get; set; }

        [JsonProperty("invitation_sent_at", NullValueHandling = NullValueHandling.Ignore)]
        public string InvitationSentAt { get; set; }

        [JsonProperty("invitation_accepted_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime InvitationAcceptedAt { get; set; }

        [JsonProperty("guest", NullValueHandling = NullValueHandling.Ignore)]
        public string Guest { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        [JsonProperty("terms_of_use_accepted_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TermsOfUseAcceptedAt { get; set; }

        [JsonProperty("privacy_policy_accepted_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PrivacyPolicyAcceptedAt { get; set; }

        [JsonProperty("anti_spam_policy_accepted_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? AntiSpamPolicyAcceptedAt { get; set; }

        [JsonProperty("timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeZone { get; set; }

        [JsonProperty("avatar", NullValueHandling = NullValueHandling.Ignore)]
        public ImageResources Avatar { get; set; }

        [JsonProperty("devices", NullValueHandling = NullValueHandling.Ignore)]
        public List<Device> Devices { get; set; }

        [JsonProperty("profile", NullValueHandling = NullValueHandling.Ignore)]
        public Profile Profile { get; set; }

        [JsonProperty("site_member", NullValueHandling = NullValueHandling.Ignore)]
        public SiteMember SiteMember { get; set; }

        [JsonProperty("eligibility_status", NullValueHandling = NullValueHandling.Ignore)]
        public string EligibilityStatus { get; set; }

        [JsonProperty("home_location", NullValueHandling = NullValueHandling.Ignore)]
        public Location HomeLocation { get; set; }
    }
}
