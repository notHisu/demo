using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Clinical6SDK.Services;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ConsentStrategyModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "consent__strategies";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("strategy_type")]
        public string StrategyType { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }

        [JsonProperty("cohort")]
        public Cohorts Cohorts { get; set; }

        [JsonProperty("mobile_users")]
        public List<User> Users { get; set; }

        [JsonProperty("consent__strategy_assignments")]
        public List<ConsentStrategyAssignmentModel> Assignments { get; set; }

        [JsonProperty("forms")]
        public List<ConsentForms> Forms { get; set; }
        //
        //Cohorts
    }

    public class ConsentStrategyAssignmentModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "consent__strategy_assignments";

        [JsonProperty("strategy")]
        public ConsentStrategyModel Strategy { get; set; }

        [JsonProperty("mobile_user")]
        public User User { get; set; }

    }

    public class ConsentGrants :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "consent__grants";

        [JsonProperty("signing_password")]
        public string SigningPassword { get; set; }

        [JsonProperty("progress")]
        public string Progress { get; set; }

        [JsonProperty("sign_url")]
        public string SignUrl { get; set; }

        [JsonProperty("consented_at")]
        public DateTime? ConsentedAt { get; set; }

        [JsonProperty("granted_for")]
        public User GrantedFor { get; set; }

        [JsonProperty("strategy")]
        public ConsentStrategyModel Strategy { get; set; }

        [JsonProperty("form")]
        public ConsentForms Form { get; set; }

        [JsonProperty("request_type")]
        public string RequestType { get; set; }

        [JsonIgnore]
        public ErrorResponse ErrorResponse { get; set; }
    }

    public class Cohorts :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "cohorts";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("cohort_type")]
        public string CohortType { get; set; }
        //
    }

    public class ConsentForms :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "consent__forms";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("strategy")]
        public ConsentStrategyModel Strategy { get; set; }
    }

    public class ConsentFormVersions :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "consent__form_versions";
    }

    public class ConsentAvailableStrategyModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "consent__available_strategies";

        [JsonProperty("strategy")]
        public ConsentStrategyModel Strategy { get; set; }

        [JsonProperty("forms")]
        public List<ConsentForms> Forms { get; set; }
    }
}
