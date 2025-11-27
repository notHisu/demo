using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Clinical6SDK.Models
{
    public class ProfileModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "profiles";

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("avatar")]
        public ImageResources Avatar { get; set; }

        [JsonProperty("gender")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("ethnicity")]
        public string Ethnicity { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("occupation")]
        public string Occupation { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("dob")]
        public string Dob { get; set; }

        [JsonProperty("timezone")]
        public string TimeZone { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("middle_initial")]
        public string MiddleInitial { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("screening_at", NullValueHandling = NullValueHandling.Ignore)]
        public string ScreeningAt { get; set; }

        [JsonProperty("nodes")]
        public string Nodes { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("randomized_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? RandomizedAt { get; set; }

        [JsonProperty("screened_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ScreenedAt { get; set; }

        [JsonProperty("profileable")]
        public User User { get; set; }

        [JsonProperty("language")]
        public Language Language { get; set; }

        [JsonProperty("shipment_delivery_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ShipmentDeliveryAt { get; set; }

        [JsonProperty("first_dose_taken_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FirstDoseTakenAt { get; set; }

        [JsonProperty("first_insertion_attempt_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FirstInsertionAttemptAt { get; set; }

        [JsonProperty("second_insertion_attempt_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? SecondInsertionAttemptAt { get; set; }

        [JsonProperty("insertion_status", NullValueHandling = NullValueHandling.Ignore)]
        public string InsertionStatus { get; set; }

        [JsonProperty("unblinded_treatment")]
        public string UnblindedTreatment { get; set; }

        [JsonProperty("day_one_at")]
        public string DayOneAt { get; set; }

        [JsonProperty("enrolled_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EnrolledAt { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        unknown
    }
}
