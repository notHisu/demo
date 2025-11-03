namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    /// <summary>
    /// Type values returned from BE for SubTaskResultsAttributes answers
    /// </summary>
    public class ResultType
    {
        public const string BoolValue = "boolean_value";
        public const string IntValue = "int_value";
        public const string StringValue = "string_value";
        public const string MultiValue = "multi_value";
        public const string DateValue = "date_value";
        public const string DateTimeValue = "datetime_value";

        //NOTE: Not currently implemented in Core BE
        public const string TimeValue = "time_value";
    }
}