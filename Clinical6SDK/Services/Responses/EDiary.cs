namespace Clinical6SDK.Services.Responses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class EDiary
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("included")]
        public List<Included> Included { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public DataAttributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public DataRelationships Relationships { get; set; }
    }

    public partial class DataAttributes
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }
    }

    public partial class DataRelationships
    {
        [JsonProperty("entry_group")]
        public EntryGroup EntryGroup { get; set; }

        [JsonProperty("template")]
        public EntryGroup Template { get; set; }

        [JsonProperty("captured_value_group")]
        public CapturedValueGroup CapturedValueGroup { get; set; }

        [JsonProperty("owner")]
        public EntryGroup Owner { get; set; }

        [JsonProperty("flow_process")]
        public EntryGroup FlowProcess { get; set; }

        [JsonProperty("status")]
        public CapturedValueGroup Status { get; set; }
    }

    public partial class CapturedValueGroup
    {
        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public partial class EntryGroup
    {
        [JsonProperty("data")]
        public Dat Data { get; set; }
    }

    public partial class Dat
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Included
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public IncludedAttributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public IncludedRelationships Relationships { get; set; }
    }

    public partial class IncludedAttributes
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("permanent_link", NullValueHandling = NullValueHandling.Ignore)]
        public string PermanentLink { get; set; }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public long? Position { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }

        [JsonProperty("uuid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uuid { get; set; }

        [JsonProperty("account_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountName { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("invitation_sent_at", NullValueHandling = NullValueHandling.Ignore)]
        public object InvitationSentAt { get; set; }

        [JsonProperty("invitation_accepted_at", NullValueHandling = NullValueHandling.Ignore)]
        public object InvitationAcceptedAt { get; set; }

        [JsonProperty("disabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Disabled { get; set; }

        [JsonProperty("disabled_at")]
        public object DisabledAt { get; set; }

        [JsonProperty("password_expired_at", NullValueHandling = NullValueHandling.Ignore)]
        public string PasswordExpiredAt { get; set; }

        [JsonProperty("consent_credentials")]
        public object ConsentCredentials { get; set; }

        [JsonProperty("owner_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerType { get; set; }

        [JsonProperty("published_at", NullValueHandling = NullValueHandling.Ignore)]
        public object PublishedAt { get; set; }

        [JsonProperty("conditional_paths")]
        public object ConditionalPaths { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public partial class IncludedRelationships
    {
        [JsonProperty("entry_templates", NullValueHandling = NullValueHandling.Ignore)]
        public EntryTemplates EntryTemplates { get; set; }

        [JsonProperty("child_entry_groups", NullValueHandling = NullValueHandling.Ignore)]
        public ChildEntryGroups ChildEntryGroups { get; set; }

        [JsonProperty("parent_entry_group", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup ParentEntryGroup { get; set; }

        [JsonProperty("entry_group", NullValueHandling = NullValueHandling.Ignore)]
        public EntryGroup EntryGroup { get; set; }

        [JsonProperty("flow_process", NullValueHandling = NullValueHandling.Ignore)]
        public EntryGroup FlowProcess { get; set; }

        [JsonProperty("devices", NullValueHandling = NullValueHandling.Ignore)]
        public ChildEntryGroups Devices { get; set; }

        [JsonProperty("user_role", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup Userole { get; set; }

        [JsonProperty("profile", NullValueHandling = NullValueHandling.Ignore)]
        public EntryGroup Profile { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public EntryGroup Language { get; set; }

        [JsonProperty("patient", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup Patient { get; set; }

        [JsonProperty("threads", NullValueHandling = NullValueHandling.Ignore)]
        public ChildEntryGroups Threads { get; set; }

        [JsonProperty("site_member", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup SiteMember { get; set; }

        [JsonProperty("overall_status", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup OverallStatus { get; set; }

        [JsonProperty("cohort_assignments", NullValueHandling = NullValueHandling.Ignore)]
        public ChildEntryGroups CohortAssignments { get; set; }

        [JsonProperty("linked_steps", NullValueHandling = NullValueHandling.Ignore)]
        public ChildEntryGroups LinkedSteps { get; set; }

        [JsonProperty("published", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup Published { get; set; }

        [JsonProperty("draft", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup Draft { get; set; }

        [JsonProperty("initial_step", NullValueHandling = NullValueHandling.Ignore)]
        public CapturedValueGroup InitialStep { get; set; }
    }

    public partial class ChildEntryGroups
    {
        [JsonProperty("data")]
        public List<object> Data { get; set; }
    }

    public partial class EntryTemplates
    {
        [JsonProperty("data")]
        public List<Dat> Data { get; set; }
    }

    public partial class EDiary
    {
        public static EDiary FromJson(string json) => JsonConvert.DeserializeObject<EDiary>(json);
    }

    public static class Serialize
    {
        public static string ToJson(this EDiary self) => JsonConvert.SerializeObject(self);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}