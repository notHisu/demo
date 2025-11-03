using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Clinical6.Core.Helpers;

namespace Xamarin.Forms.Clinical6.Core.Models
{
    public class SubTaskSummary
    {
        public SubTaskSummary(BaseDataModel<SubTaskAttribute, SubTaskRelationships> task, DataModel<IncludedAttributes> includedData, DataModel<SubTaskResultsAttributes>[] subTasks)
        {
            Task = task;
            IncludedData = includedData;
            SubTasks = subTasks;
        }

        public BaseDataModel<SubTaskAttribute, SubTaskRelationships> Task { get; set; }
        public DataModel<IncludedAttributes> IncludedData { get; set; }
        public DataModel<SubTaskResultsAttributes>[] SubTasks { get; set; }
    }

    public class SubTaskRelationships
    {
        [JsonProperty("site_member")]
        public CommonDataModel<TypeAndId> SiteMember { get; set; }

        [JsonProperty("sub_task_definition")]
        public CommonDataModel<TypeAndId> SubTaskDefinition { get; set; }

        [JsonProperty("task_assignment")]
        public CommonDataModel<TypeAndId> TaskAssignment { get; set; }

        [JsonProperty("sub_task_draft")]
        public CommonDataModel<TypeAndId> SubTaskDraft { get; set; }

        [JsonProperty("taskable")]
        public CommonDataModel<TypeAndId> Taskable { get; set; }
    }

    [JsonConverter(typeof(SubTaskResultsAttributesConverter))]
    public class SubTaskResultsAttributes
    {
        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("result_type")]
        public string AnswerResultType { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
    }

    public class IncludedAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("button")]
        public string Button { get; set; }

        [JsonProperty("resume_button")]
        public string ResumeButton { get; set; }

        [JsonProperty("image")]
        public ImageUrls Image { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("site_user_executed")]
        public bool SiteUserExecuted { get; set; }

        [JsonProperty("system_executed")]
        public bool SystemExecuted { get; set; }
    }

    public class UrlHolder
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class ImageUrls
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("small")]
        public UrlHolder Small { get; set; }

        [JsonProperty("small_hd")]
        public UrlHolder SmallHd { get; set; }

        [JsonProperty("fullscreen")]
        public UrlHolder Fullscreen { get; set; }

        [JsonProperty("fullscreen_hd")]
        public UrlHolder FullscreenHd { get; set; }

        [JsonProperty("main")]
        public UrlHolder Main { get; set; }

        [JsonProperty("main_hd")]
        public UrlHolder MainHd { get; set; }
    }

    public class SubTaskAttribute
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("started_at")]
        public string StartedAt { get; set; }

        [JsonProperty("resolved_at")]
        public string ResolvedAt { get; set; }
    }

    public class FlowProcessSummary : IHavePermanentLink
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("initial_step_id")]
        public int IntialStep { get; set; }

        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Not set from Json, set manually after converting Json to this
        /// </summary>
        public int Id { get; set; }
    }

    public class FlowProcessContainer
    {
        //"uuid":null,
        //"name":"Adverse Event",
        //"permanent_link":"adverse_event",
        //"flow_processes":[
        //{
        //"position":1,
        //"id":18,
        //"name":"Adverse Event",
        //"permanent_link":"adverse_event",
        //"owner_type":"MobileUser",
        //"initial_step_id":97,
        //"published":true
        //}
        //],
        //"created_at":"2017-06-21T01:28:25Z",
        //"updated_at":"2017-06-21T01:28:25Z"
        [JsonProperty("flow_processes")]
        public FlowProcessSummary[] FlowProcesses { get; set; }
    }

    public interface IHavePermanentLink
    {
        string PermanentLink { get; set; }
        int Id { get; set; }
    }

    public class FlowProcessDto
    {
        [JsonProperty("steps")]
        public FlowStepDto[] Steps { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("show_summary")]
        public bool ShowConfirmation { get; set; }

        [JsonProperty("first_step")]
        public int FirstStep { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class FlowStepDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Id of the flow process
        /// </summary>
        [JsonProperty("flow_process_id")]
        public int FlowProcessId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        /// <summary>
        /// Mostly null. If populated contains encoded html
        /// </summary>
        /// <example>\u003cdiv\u003e\u0026nbsp;\u003c/div\u003e</example>
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public FlowImage FlowImage { get; set; }

        [JsonProperty("progress_bar")]
        public bool ShowProgressBar { get; set; }

        [JsonProperty("footer")]
        public string Footer { get; set; }

        [JsonProperty("slider_orientation")]
        public string SliderOrientation { get; set; }

        [JsonProperty("inputs")]
        public FlowInputDto[] InputsDto { get; set; }

        [JsonProperty("paths")]
        public FlowPathDto[] PathsDto { get; set; }

        /// <example>
        /// You must review and accept the use terms and privacy policy before proceeding.
        /// </example>
        [JsonProperty("help_text")]
        public string HelpText { get; set; }

        [JsonIgnore]
        public Func<object> GetFunc { get; set; }
    }

    public class FlowInputDto
    {
        /// <example>
        /// agreement,input,single_choice,date,numeric
        /// </example>
        [JsonProperty("question_type")]
        public string QuestionType { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("storage_attribute")]
        public string StorageAttribute { get; set; }

        [JsonProperty("max")]
        public string Max { get; set; }

        [JsonProperty("max_label")]
        public string MaxLabel { get; set; }

        [JsonProperty("min")]
        public string Min { get; set; }

        [JsonProperty("min_label")]
        public string MinLabel { get; set; }

        [JsonProperty("mid")]
        public string Mid { get; set; }

        [JsonProperty("mid_label")]
        public string MidLabel { get; set; }

        [JsonProperty("interval")]
        public double Interval { get; set; }

        [JsonProperty("tick_label_interval", NullValueHandling = NullValueHandling.Ignore)]
        public double TickLabelInterval { get; set; } = 1; // Default to listing numbers across bottom of slider

        [JsonProperty("tick_major_interval", NullValueHandling = NullValueHandling.Ignore)]
        public double TickMajorInterval { get; set; }

        [JsonProperty("tick_minor_interval", NullValueHandling = NullValueHandling.Ignore)]
        public double TickMinorInterval { get; set; }

        [JsonProperty("validation_expression", NullValueHandling = NullValueHandling.Ignore)]
        public string ValidationExpression { get; set; }

        [JsonProperty("validation_details", NullValueHandling = NullValueHandling.Ignore)]
        public FlowValidation ValidationDetails { get; set; }
        /// <example>
        /// variation in the question type. 
        /// QuestionTypes with styles underneath
        ///-----agreement-----
        ///checkbox
        ///-----input-----
        ///text
        ///zip_code
        ///text_area
        ///-----single_choice-----
        ///radio_buttons
        ///-----date-----
        ///spinner
        ///-----numeric-----
        ///text_box
        ///range
        /// </example>
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        /// <example>
        /// I have read and agree to the statement.
        /// </example>
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("choice_list")]
        public FlowInputChoiceDto[] ChoicesDto { get; set; }

        //[JsonProperty("locked")]
        //public object[] Locked { get; set; }
    }


    public class FlowPathDto
    {
        [JsonProperty("button_name")]
        public string ButtonName { get; set; }

        [JsonProperty("capture")]
        public bool Capture { get; set; }

        [JsonProperty("last")]
        public bool Last { get; set; }

        /// <summary>
        /// If NextSteps.Length == 0; Last is also true
        /// </summary>
        [JsonProperty("steps")]
        public FlowPathNextDto[] NextDtoSteps { get; set; }
    }

    public class NextConditionDto
    {
        /// <summary>
        /// Id of the field to compare
        /// </summary>
        [JsonProperty("operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Right now always '='
        /// </summary>
        [JsonProperty("criteria")]
        public string Criteria { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        /// <remarks>always null</remarks>>
        [JsonProperty("source")]
        public string Source { get; set; }
    }

    public class FlowPathNextDto
    {
        [JsonProperty("step")]
        public int Step { get; set; }

        /// <summary>
        /// Conduction.Length == 0; always follow to this step
        /// </summary>
        [JsonProperty("conditions")]
        public NextConditionDto[] Condition { get; set; }
    }

    public class FlowInputChoiceDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class FlowImage
    {
        [JsonProperty("type")]
        public string ImageType { get; set; }

        [JsonProperty("original")]
        public string Original { get; set; }
    }

    public class FlowValidation
    {
        public Date Min
        {
            get
            {
                try
                {
                    if (MinValue is JObject)
                    {
                        var jObject = MinValue as JObject;
                        Date date = jObject.ToObject<Date>();
                        return date;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                return null;
            }
        }

        public Date Max
        {
            get
            {
                try
                {
                    if (MaxValue is JObject)
                    {
                        var jObject = MaxValue as JObject;
                        Date date = jObject.ToObject<Date>();
                        return date;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                return null;
            }
        }

        public double MaxInput
        {
            get
            {
                try
                {
                    if (MaxValue is double)
                    {
                        return Convert.ToDouble(MaxValue);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                return 0;
            }
        }

        [JsonProperty("min")]
        public object MinValue { get; set; }
        [JsonProperty("max")]
        public object MaxValue { get; set; }

        [JsonProperty("Celsius")]
        public FlowValidationValue Celsius { get; set; }
        [JsonProperty("Fahrenheit")]
        public FlowValidationValue Fahrenheit { get; set; }
    }

    public class FlowValidationValue
    {
        [JsonProperty("min")]
        public float Min { get; set; }
        [JsonProperty("max")]
        public float Max { get; set; }
    }

    public class Date
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("attribute_name")]
        public string AttributeName { get; set; }
        [JsonProperty("add")]
        public DayMonthYear Add { get; set; }
        [JsonProperty("subtract")]
        public DayMonthYear Subtract { get; set; }
    }

    public class DayMonthYear
    {
        [JsonProperty("day")]
        public int Day { get; set; }
        [JsonProperty("month")]
        public int Month { get; set; }
        [JsonProperty("year")]
        public int Year { get; set; }
    }
}