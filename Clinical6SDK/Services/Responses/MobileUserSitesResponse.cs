using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Responses
{
    public partial class MobileUserSitesResponse
    {
        [JsonProperty("data")]
        public List<MobileUserSitesReponseData> Data { get; set; }

        [JsonProperty("included")]
        public List<Included> Included { get; set; }
    }

    public partial class MobileUserSitesReponseData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public MobileUserSitesReponseAttributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public Relationships Relationships { get; set; }
    }

    public partial class MobileUserSitesReponseAttributes
    {
        [JsonProperty("site_id")]
        public string SiteId { get; set; }

        [JsonProperty("external_identifier")]
        public string ExternalIdentifier { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        public object PhoneNumber { get; set; }

        [JsonProperty("fax_number")]
        public object FaxNumber { get; set; }

        [JsonProperty("contact_name")]
        public string ContactName { get; set; }

        [JsonProperty("contact_email")]
        public string ContactEmail { get; set; }

        [JsonProperty("contact_phone")]
        public object ContactPhone { get; set; }

        [JsonProperty("contact_fax")]
        public object ContactFax { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("site_member_count")]
        public long SiteMemberCount { get; set; }
    }

    public partial class Relationships
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("agreement_templates")]
        public AgreementTemplates AgreementTemplates { get; set; }

        [JsonProperty("site_contacts")]
        public SiteContacts SiteContacts { get; set; }
    }

    public partial class AgreementTemplates
    {
        [JsonProperty("data")]
        public List<object> Data { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class SiteContacts
    {
        [JsonProperty("data")]
        public List<Dat> Data { get; set; }
    }

    public partial class IncludedAttributes
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("zip_code")]
        public object ZipCode { get; set; }

        [JsonProperty("address_line_1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("address_line_2")]
        public object AddressLine2 { get; set; }

        [JsonProperty("address_line_3")]
        public object AddressLine3 { get; set; }
    }


}
