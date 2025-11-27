using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    public class RegistrationValidationModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "registration_validations";

        [JsonProperty("account_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountName { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }

}
