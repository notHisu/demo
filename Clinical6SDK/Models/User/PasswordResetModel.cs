using System;
using System.Collections.Generic;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;
using Device = Clinical6SDK.Helpers.Device;

namespace Clinical6SDK.Models
{
    public class PasswordResetModel : JsonApiModel
	{
        [JsonProperty("type")]
        public override string Type { get; set; } = "password_resets";

        [JsonProperty("reset_password_token", NullValueHandling = NullValueHandling.Ignore)]
        public string ResetPasswordToken { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        [JsonProperty("devices", NullValueHandling = NullValueHandling.Ignore)]
        public Device Device { get; set; }
    }
}
