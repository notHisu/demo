using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
    public class FlowInput :  JsonApiModel
    {
		public string Attribute { get; set; }

		public string Body { get; set; }

		[JsonProperty("choice_list")]
		public IList<FlowChoice> ChoiceList { get; set; }

		public string Instructions { get; set; }

		public IList<bool> Locked { get; set; } // documentation says array??

		[JsonProperty("question_type")]
		public string QuestionType { get; set; }

		public bool Required { get; set; }

		[JsonProperty("storage_attribute")]
		public string StorageAttribute { get; set; }

		public string Style { get; set; }

		[JsonProperty("validation_details")]
		public FlowValidation ValidationDetails { get; set; }

		[JsonProperty("validation_expression")]
		public string ValidationExpression { get; set; }

        [JsonProperty("min_label")]
        public string MinLabel { get; set; }

        [JsonProperty("max_label")]
        public string MaxLabel { get; set; }


        public FlowChoice FindChoiceByValue(string v)
        {
            FlowChoice ret = null;
            if (!string.IsNullOrWhiteSpace(v))
            {
                foreach (var choice in ChoiceList)
                {
                    if (!string.IsNullOrWhiteSpace(choice.Body) &&
                       choice.Body.ToLowerInvariant() == v.ToLowerInvariant())
                    {
                        ret = choice;
                    }
                }
            }
            return ret;
        }

        public string GetData()
        {
            return new Clinical6SDK.Services.Clinical6FlowService().GetInputDataByIdAsync(this.Id.ToString()).Result;
        }

        public static class QuestionTypes
        {
            public const string INPUT = "input";
            public const string SINGLE_CHOICE = "single_choice";
            public const string MULTIPLE_CHOICE= "multiple_choice";
            public const string PRE_POPULATED = "pre_populated";
            public const string SEARCH = "search";
            public const string SEARCH_AND_UPLOAD = "search_and_upload";
            public const string AUTOCOMPLETE = "autocomplete";
            public const string DATE = "date";
            public const string NUMERIC = "numeric";
            public const string TIME = "time";
            public const string FILE_UPLOAD = "file_upload";
            public const string RANK_ORDER = "rank_order";
        }
	}
}
