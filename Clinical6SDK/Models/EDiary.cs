using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    /// <summary>
    /// EDiary Entry used to collect data for the day (or period).  This will have a flow to help with the collection process.
    /// </summary>
    public class Entry : JsonApiModel
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public override string Type { get; set; } = "ediary__entries";

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Clinical6SDK.Models.Entry"/> is locked.
        /// </summary>
        /// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
        [JsonProperty("locked", NullValueHandling = NullValueHandling.Ignore)]
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the entry group.
        /// </summary>
        /// <value>The entry group.</value>
        [JsonProperty("entry_group", NullValueHandling = NullValueHandling.Ignore)]
        public EntryGroup EntryGroup { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore)]
        public EntryTemplate Template { get; set; }

        /// <summary>
        /// Gets or sets the captured value group.
        /// </summary>
        /// <value>The captured value group.</value>
        [JsonProperty("captured_value_group", NullValueHandling = NullValueHandling.Ignore)]
        public object CapturedValueGroup { get; set; }

        /// <summary>
        /// Gets or sets the flow.
        /// </summary>
        /// <value>The flow.</value>
        [JsonProperty("flow_process", NullValueHandling = NullValueHandling.Ignore)]
        public DataCollectionFlowProcesses Flow { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public User Owner { get; set; }
    }

    /// <summary>
    /// EDiary entry group used to collect several entries.
    /// </summary>
    public class EntryGroup : JsonApiModel
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public override string Type { get; set; } = "ediary__entry_groups";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permanent link.
        /// </summary>
        /// <value>The permanent link.</value>
        [JsonProperty("permanent_link", NullValueHandling = NullValueHandling.Ignore)]
        public string PermanentLink { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public object Category { get; set; }

        /// <summary>
        /// Gets or sets the templates.
        /// </summary>
        /// <value>The templates.</value>
        [JsonProperty("entry_templates", NullValueHandling = NullValueHandling.Ignore)]
        public List<EntryTemplate> Templates { get; set; }

        /// <summary>
        /// Gets or sets the child entry groups.
        /// </summary>
        /// <value>The child entry groups.</value>
        [JsonProperty("child_entry_groups", NullValueHandling = NullValueHandling.Ignore)]
        public List<EntryGroup> ChildEntryGroups { get; set; }

        /// <summary>
        /// Gets or sets the parent entry group.
        /// </summary>
        /// <value>The parent entry group.</value>
        [JsonProperty("parent_entry_group", NullValueHandling = NullValueHandling.Ignore)]
        public List<EntryGroup> ParentEntryGroup { get; set; }
    }

    /// <summary>
    /// EDiary Entry Template.
    /// </summary>
    public class EntryTemplate : JsonApiModel
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public override string Type { get; set; } = "ediary__entry_templates";

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the entry group.
        /// </summary>
        /// <value>The entry group.</value>
        [JsonProperty("entry_group", NullValueHandling = NullValueHandling.Ignore)]
        public EntryGroup EntryGroup { get; set; }

        /// <summary>
        /// Gets or sets the flow.
        /// </summary>
        /// <value>The flow.</value>
        [JsonProperty("flow_process", NullValueHandling = NullValueHandling.Ignore)]
        public DataCollectionFlowProcesses Flow { get; set; }
    }

    /// <summary>
    /// Data collection flow processes.
    /// </summary>
    public class DataCollectionFlowProcesses : JsonApiModel
    {

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public override string Type { get; set; } = "data_collection__flow_processes";

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public object Category { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permanent link.
        /// </summary>
        /// <value>The permanent link.</value>
        [JsonProperty("permanent_link", NullValueHandling = NullValueHandling.Ignore)]
        public string PermanentLink { get; set; }

        /// <summary>
        /// Gets or sets the consent credentials.
        /// </summary>
        /// <value>The consent credentials.</value>
        [JsonProperty("consent_credentials", NullValueHandling = NullValueHandling.Ignore)]
        public string ConsentCredentials { get; set; }

        /// <summary>
        /// Gets or sets the type of the owner.
        /// </summary>
        /// <value>The type of the owner.</value>
        [JsonProperty("owner_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerType { get; set; }

        /// <summary>
        /// Gets or sets the conditional paths.
        /// </summary>
        /// <value>The conditional paths.</value>
        [JsonProperty("conditional_paths", NullValueHandling = NullValueHandling.Ignore)]
        public string ConditionalPaths { get; set; }
    }

}
