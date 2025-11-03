using Newtonsoft.Json;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services.Requests
{
    public class RegistrationStatusRequest
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}