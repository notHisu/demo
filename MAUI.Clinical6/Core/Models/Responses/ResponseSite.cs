using System;
using Newtonsoft.Json;

namespace Xamarin.Forms.Clinical6.Core.Models.Responses
{
    public class ResponseSiteModel
    {

        [JsonProperty("data")]
        public dataSite[] Data { get; set; }

        [JsonProperty("included")]
        public locations[] Included { get; set; }
    }

    public class locations
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("attributes")]
        public Site Sites { get; set; }
    }

    public class dataSite
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("attributes")]
        public Site Sites { get; set; }
    }

    public class Site
    {
        public string site_id { get; set; }
        public string external_identifier { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("primary_contact")]
        public bool PrimaryContact { get; set; }


        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

    }
}
