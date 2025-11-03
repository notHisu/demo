using System;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class ConsultationParticipantModel : JsonApiModel
    {
        [JsonProperty("consultation", NullValueHandling = NullValueHandling.Ignore)]
        public Consultation Consultation { get; set; }

        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }
    }
}
