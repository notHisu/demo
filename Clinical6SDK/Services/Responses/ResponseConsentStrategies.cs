namespace Clinical6SDK.Services.Responses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ResponseConsentStrategies
    {
        [JsonProperty("data")]
        public List<ConsentStrategiesData> Data { get; set; }

        [JsonProperty("included")]
        public List<Included> Included { get; set; }
    }

    public partial class ConsentStrategiesData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public DatumAttributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public ConsentStrategiesDataRelationships Relationships { get; set; }
    }

    public partial class DatumAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("strategy_type")]
        public string StrategyType { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public partial class ConsentStrategiesDataRelationships
    {
        [JsonProperty("cohort")]
        public Cohort Cohort { get; set; }

        [JsonProperty("forms")]
        public Forms Forms { get; set; }
    }

    public partial class Cohort
    {
        [JsonProperty("data")]
        public Dat Data { get; set; }
    }


    public partial class Forms
    {
        [JsonProperty("data")]
        public List<Dat> Data { get; set; }
    }

    public partial class IncludedAttributes
    {

        [JsonProperty("cohort_type", NullValueHandling = NullValueHandling.Ignore)]
        public string CohortType { get; set; }

        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }
    }

    public partial class IncludedRelationships
    {
        [JsonProperty("consent_form_versions")]
        public AdditionalSignatures ConsentFormVersions { get; set; }

        [JsonProperty("additional_signatures")]
        public AdditionalSignatures AdditionalSignatures { get; set; }

        [JsonProperty("strategy")]
        public Cohort Strategy { get; set; }
    }

    public partial class AdditionalSignatures
    {
        [JsonProperty("data")]
        public List<object> Data { get; set; }
    }
}
