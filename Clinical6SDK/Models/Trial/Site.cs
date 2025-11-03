using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class Site : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "trials__sites";

        [JsonProperty("site_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SiteId { get; set; }

        [JsonProperty("external_identifier", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("phone_number", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }

        [JsonProperty("fax_number", NullValueHandling = NullValueHandling.Ignore)]
        public string FaxNumber { get; set; }

        [JsonProperty("contact_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactName { get; set; }

        [JsonProperty("contact_email", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactEmail { get; set; }

        [JsonProperty("contact_phone", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactPhone { get; set; }

        [JsonProperty("contact_fax", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactFax { get; set; }

        [JsonProperty("site_member_count", NullValueHandling = NullValueHandling.Ignore)]
        public int? SiteMemberCount { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public Location Location { get; set; }

        //[JsonProperty("agreement_templates")]
        //public List<AgreementTemplate> AgreementTemplates { get; set; }

        [JsonProperty("site_contacts", NullValueHandling = NullValueHandling.Ignore)]
        public List<SiteContact> SiteContacts { get; set; }
    }

    public class SiteContactModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "trials__site_contacts";

        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the type of the contact.
        /// </summary>
        /// <value>The type of the contact.</value>
        /// <example> siteContact = new SiteContact { ContactType = "Office" };</example>
        [JsonProperty("contact_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactType { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("fax", NullValueHandling = NullValueHandling.Ignore)]
        public string Fax { get; set; }

        [JsonProperty("primary_contact", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPrimaryContact { get; set; }

        [JsonProperty("site", NullValueHandling = NullValueHandling.Ignore)]
        public Site Site { get; set; }

        [JsonIgnore]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }

    public class SiteMember : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "trials__site_members";

        [JsonProperty("member_type")]
        public string MemberType { get; set; }

        [JsonProperty("consent_status")]
        public string ConsentStatus { get; set; }

        [JsonProperty("first_consented_at")]
        public DateTime? FirstConsentedAt { get; set; }

        [JsonProperty("last_consented_at")]
        public DateTime? LastConsentedAt { get; set; }

        [JsonProperty("site", NullValueHandling = NullValueHandling.Ignore)]
        public Site Site { get; set; }

        [JsonProperty("mobile_user", NullValueHandling = NullValueHandling.Ignore)]
        public User MobileUser { get; set; }

        [JsonProperty("consent_strategy", NullValueHandling = NullValueHandling.Ignore)]
        public ConsentStrategyModel ConsentStrategy { get; set; }

        [JsonProperty("active_study_definition_version", NullValueHandling = NullValueHandling.Ignore)]
        public StudyDefinitionVersion ActiveStudyDefinitionVersion { get; set; }
    }

}
