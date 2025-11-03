using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
    public class FlowSettings
    {
        [JsonProperty("transition")]
        public string Transition { get; set; }  // Either 'auto' or 'manual', defaults to 'auto'

        internal static class TrasitionValues
        {
            public const string AUTO = "auto";
            public const string MANUAL = "manual";
        }
    }
}
