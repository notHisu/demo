using System;
using Clinical6SDK.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clinical6SDK.Services.Responses
{
    public class CollectFieldsResponse
    {
        public bool IsResponseSuccessful { get; set; }

        [JsonProperty("process_status")]
        public string ProcessStatus { get; set; }
    }
}
