using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ConsultationModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "video_consultation__consultations";

        [JsonProperty("closed_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ClosedAt { get; set; }

        [JsonProperty("deleted_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DeletedAt { get; set; }

        [JsonProperty("elapsed_seconds", NullValueHandling = NullValueHandling.Ignore)]
        public int? ElapsedSeconds { get; set; }

        [JsonProperty("session_identity", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionIdentity { get; set; }

        [JsonProperty("started_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartedAt { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("scheduled_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ScheduledAt { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("participants", NullValueHandling = NullValueHandling.Ignore)]
        public List<ConsultationParticipant> Participants { get; set; }

        [JsonProperty("scheduled_by", NullValueHandling = NullValueHandling.Ignore)]
        public User ScheduledBy { get; set; }
    }
}
