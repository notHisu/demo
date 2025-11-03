using Newtonsoft.Json;
using Clinical6SDK.Models;
using System.Collections.Generic;

namespace Clinical6SDK.Services.Requests
{
    public class TransitionRequest
    {
        [JsonProperty("options")]
        public OptionsTransition OptionsTransition { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("object_type")]
        public string ObjectType { get; set; }

        [JsonProperty("transition")]
        public string Transition { get; set; }

    }

    public class OptionsTransition
    {
        public Dictionary<string, string> Options { get; set; }
    }

}
