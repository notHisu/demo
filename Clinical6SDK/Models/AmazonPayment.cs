using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class AmazonPayment : JsonApiModel
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "payment__accounts";

        [JsonProperty("attributes")]
        public AmazonPaymentsAttributes AmazonPaymentsAttributes { get; set; }

        public double Balance
        {
            get
            {
                return AmazonPaymentsAttributes == null ? 0
                       : double.Parse(AmazonPaymentsAttributes.Balance);
            }
        }
    }

    public class AmazonPaymentsAttributes
    {
        [JsonProperty("balance")]
        public string Balance { get; set; }
    }
}
