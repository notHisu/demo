using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Responses
{
    public class VuforiaTargetResponse : Response
    {
        [JsonProperty("vuforia_target")]
        public string VuforiaTarget { get; set; }
    }
}
