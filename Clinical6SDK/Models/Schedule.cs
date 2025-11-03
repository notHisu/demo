using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Clinical6SDK.Models
{
    public class Schedule: JsonApiModel
    {
        [JsonProperty("active_time_start")]
        public DateTime ActiveTimeStart { get; set; }

        [JsonProperty("active_time_end")]
        public DateTime ActiveTimeEnd { get; set; }

        [JsonProperty("days_of_week")]
        public string DaysOfWeek { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("exception_days")]
        public string ExceptionDays { get; set; }

        [JsonProperty("relative_schedule")]
        public string RelativeSchedule { get; set; }

        [JsonProperty("relative_unit")]
        public TimeUnit RelativeUnit { get; set; }

        [JsonProperty("skip_exceptions")]
        public bool SkipExceptions { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("start_date_offset")]
        public int StartDateOffset { get; set; }

        [JsonProperty("start_date_offset_unit")]
        public TimeUnit StartDateOffsetUnit { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }

    public enum DaysOfWeek
    {
        Monday = 0,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public enum TimeUnit
    {
        Days,
        Weeks,
        Months
    }
}
