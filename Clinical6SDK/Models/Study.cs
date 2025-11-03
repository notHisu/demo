using System;
using System.Collections.Generic;
using Clinical6SDK.Common.Converters;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class StudyDefinition : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__definitions";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("definition_versions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StudyDefinitionVersion> DefinitionVersions { get; set; }
    }

    public class StudyDefinitionVersion : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__definition_versions";

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("version_name")]
        public string Name { get; set; }

        [JsonProperty("definition", NullValueHandling = NullValueHandling.Ignore)]
        public StudyDefinition Definition { get; set; }

        [JsonProperty("step_definitions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StudyStepDefinition> StepDefinitions { get; set; }
    }

    public class StudyStep : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__steps";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public int Order { get; set; }

        [JsonProperty("definition_version", NullValueHandling = NullValueHandling.Ignore)]
        public StudyDefinitionVersion DefinitionVersion { get; set; }

        [JsonProperty("tasks", NullValueHandling = NullValueHandling.Ignore)]
        public List<StudyTask> Tasks { get; set; }
    }

    /// <summary>
    /// Study step progress.  This is what is used to determine the name and
    /// information about the current step that the patient is on.
    /// </summary>
    /// <example>
    /// // Get the current study step - it's where LeftAt is null
    /// var result = await _service.GetChildren<List<StudyStepProgress>>(CurrentSiteMember, new StudyStepProgress().Type);
    /// StudySteps = (result.Success)? result.Response : StudySteps;
    /// CurrentStudyStep = StudySteps.Find(x => x.LeftAt == null);
    /// </example>
    public class StudyStepProgress : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__step_progresses";

        [JsonProperty("entered_at")]
        public DateTime? EnteredAt { get; set; }

        [JsonProperty("left_at")]
        public DateTime? LeftAt { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("site_member", NullValueHandling = NullValueHandling.Ignore)]
        public SiteMember SiteMember { get; set; }

        [JsonProperty("step_definition", NullValueHandling = NullValueHandling.Ignore)]
        public StudyStepDefinition StepDefinition { get; set; }

        [JsonProperty("previous_step_progress", NullValueHandling = NullValueHandling.Ignore)]
        public StudyStepProgress PreviousStepProgress { get; set; }
    }

    public class StudyStepDefinition : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__step_definitions";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int? Order { get; set; }

        [JsonProperty("definition_version", NullValueHandling = NullValueHandling.Ignore)]
        public StudyDefinitionVersion DefinitionVersion { get; set; }

        [JsonProperty("task_definitions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StudyTaskDefinition> TaskDefinitions { get; set; }
    }

    public class StudyTask : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__tasks";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("step", NullValueHandling = NullValueHandling.Ignore)]
        public StudyStep Step { get; set; }

        [JsonProperty("task_actions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StudyTaskAction> TaskActions { get; set; }
    }

    public class StudyTaskAction : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__task_actions";

        [JsonProperty("task", NullValueHandling = NullValueHandling.Ignore)]
        public StudyTask Task { get; set; }
    }

    public class StudyTaskDefinition : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__task_definitions";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("step_definition", NullValueHandling = NullValueHandling.Ignore)]
        public StudyStepDefinition StepDefinition { get; set; }

        [JsonProperty("sub_task_definitions", NullValueHandling = NullValueHandling.Ignore)]
        public List<StudySubTaskDefinition> SubTaskDefinitions { get; set; }
    }

    public class StudyTaskAssignment : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__task_assignments";

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status which can be completed, initial, in progress, skipped.</value>
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("site_member", NullValueHandling = NullValueHandling.Ignore)]
        public SiteMember SiteMember { get; set; }

        [JsonProperty("task_definition", NullValueHandling = NullValueHandling.Ignore)]
        public StudyTaskDefinition TaskDefinition { get; set; }

        [JsonProperty("finished_step_progress", NullValueHandling = NullValueHandling.Ignore)]
        public StudyStepProgress FinishedStepProgress { get; set; }

        [JsonProperty("sub_task_occurrences", NullValueHandling = NullValueHandling.Ignore)]
        public List<StudySubTaskOccurrence> SubTaskOccurrences { get; set; }
    }

    public class StudySubTaskDefinition : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__sub_task_definitions";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        [JsonProperty("button", NullValueHandling = NullValueHandling.Ignore)]
        public string Button { get; set; }

        [JsonProperty("resume_button", NullValueHandling = NullValueHandling.Ignore)]
        public string ResumeButton { get; set; }

        [JsonProperty("image")]
        public ImageResources Image { get; set; }

        [JsonProperty("task_definition", NullValueHandling = NullValueHandling.Ignore)]
        public StudyTaskDefinition TaskDefinition { get; set; }

        [JsonProperty("taskable", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(MyTypeDeterminingResourceObjectConvertor))]
        public ITaskable Taskable { get; set; }
    }

    /// <summary>
    /// Study sub task occurrence.  This is what is displayed on the dashboard
    /// as it has all the task information that is active and ready for the
    /// patient to do at the current step they are on.
    /// </summary>
    public class StudySubTaskOccurrenceModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__sub_task_occurrences";

        [JsonProperty("resolved_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ResolvedAt { get; set; }

        [JsonProperty("started_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartedAt { get; set; }

        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status which can be completed, initial, in progress, skipped.</value>
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("site_member", NullValueHandling = NullValueHandling.Ignore)]
        public SiteMember SiteMember { get; set; }

        [JsonProperty("sub_task_definition", NullValueHandling = NullValueHandling.Ignore)]
        public StudySubTaskDefinition SubTaskDefinition { get; set; }

        // From PATCH notes
        [JsonProperty("captured_value_group", NullValueHandling = NullValueHandling.Ignore)]
        public FlowDataGroup FlowDataGroup { get; set; }

        [JsonProperty("sub_task_draft", NullValueHandling = NullValueHandling.Ignore)]
        public SubTaskDraft SubTaskDraft { get; set; }

        [JsonProperty("taskable", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(MyTypeDeterminingResourceObjectConvertor))]
        public ITaskable Taskable { get; set; }
    }

    public class StudySubTaskResult : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__sub_task_results";

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }
    }

    public class SubTaskDraft : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "study__sub_task_drafts";

        [JsonProperty("sub_task_occurrence", NullValueHandling = NullValueHandling.Ignore)]
        public StudySubTaskOccurrence SubTaskOccurrence { get; set; }

        [JsonProperty("taskable_progress", NullValueHandling = NullValueHandling.Ignore)]
        public FlowDataGroup TaskableProgress { get; set; }
    }

    public class DataCollectionFlowProcessValues : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "data_collection__flow_process_values";

        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<object, object> Attributes { get; set; }
    }
}
