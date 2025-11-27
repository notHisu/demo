using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ConsultationJoinModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "video_consultation_join";

        [JsonProperty("video_consultation", NullValueHandling = NullValueHandling.Ignore)]
        public Consultation Consultation { get; set; }

        [JsonProperty("video_consultation_participant", NullValueHandling = NullValueHandling.Ignore)]
        public ConsultationParticipant Participant { get; set; }
    }
}
