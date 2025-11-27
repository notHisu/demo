using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ReminderRule :  JsonApiModel
    {
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("end_date")]
        public string EndDate { get; set; }

        [JsonProperty("enable")]
        public string Enable { get; set; }

        [JsonProperty("active_time_start")]
        public string ActiveTimeStart { get; set; }

        [JsonProperty("active_time_end")]
        public string ActiveTimeEnd { get; set; }
    }
}
