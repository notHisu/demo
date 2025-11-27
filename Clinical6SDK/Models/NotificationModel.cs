using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class NotificationModel :  JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "notification__deliveries";

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action { get; set; }

        [JsonProperty("actionObject", NullValueHandling = NullValueHandling.Ignore)]
        public string ActionObject { get; set; }

        [JsonProperty("read", NullValueHandling = NullValueHandling.Ignore)]
        public bool Read { get; set; }

        [JsonProperty("archive", NullValueHandling = NullValueHandling.Ignore)]
        public bool Archive { get; set; }

        [JsonProperty("channels", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Channels { get; set; }

        [JsonProperty("channel_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ChannelType { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("opts", NullValueHandling = NullValueHandling.Ignore)]
        public NotificationOptsModel Opts { get; set; }

        [JsonProperty("archived_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ArchivedAt { get; set; }

        [JsonProperty("read_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ReadAt { get; set; }
    }

    public class NotificationOptsModel :  JsonApiModel
    {
        //// This doesn't look like it should be in the 
        //[JsonProperty("triggerer", NullValueHandling = NullValueHandling.Ignore)]
        //public string Triggerer { get; set; }

        [JsonProperty("web_app_url", NullValueHandling = NullValueHandling.Ignore)]
        public string WebAppUrl { get; set; }

        [JsonProperty("current_date", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentDate { get; set; }

        [JsonProperty("locale", NullValueHandling = NullValueHandling.Ignore)]
        public string Locale { get; set; }

        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action { get; set; }

        [JsonProperty("action_object", NullValueHandling = NullValueHandling.Ignore)]
        public string ActionObject { get; set; }
    }
}
