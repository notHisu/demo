using System;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class RedeemPayment : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "";

        [JsonIgnore]
        public string UserId { get; set; }

        //# Currently only supports agcod
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; } = "agcod";

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}
