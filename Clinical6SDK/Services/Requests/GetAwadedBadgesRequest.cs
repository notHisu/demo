using Newtonsoft.Json;
using Clinical6SDK.Models;
using System;

namespace Clinical6SDK.Services.Requests
{
    public class GetAwardedBadgesRequest
    {
        [JsonProperty("entry_group_id")]
        public int EntryGroupId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}