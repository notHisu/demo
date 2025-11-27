using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class VerificationCodeModel
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("hostname", NullValueHandling = NullValueHandling.Ignore)]
        public string Hostname { get; set; }
    }
}
