using System;
using Newtonsoft.Json;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services.Requests
{
    internal class SendConfirmationRequest
    {
        [JsonProperty("data")]
        public SendConfirmationRequestData Data { get; set; }
    }

    internal class SendConfirmationRequestData
    {
        [JsonProperty("type")]
        public string Type { get => "confirmations"; }

        [JsonProperty("attributes")]
        public string Attributes { get; set; }
    }
}
