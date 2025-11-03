namespace Clinical6SDK.Services.Responses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ResponseCapturedValuesCollection
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("captured_values")]
        public List<CapturedValue> CapturedValues { get; set; }
    }

    public partial class CapturedValue
    {
        [JsonProperty("input_id")]
        public long InputId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
