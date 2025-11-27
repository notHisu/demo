using System;
using Clinical6SDK.Helpers;
using Newtonsoft.Json;
using Device = Clinical6SDK.Helpers.Device;

namespace Clinical6SDK.Models
{
    public class SessionModel : JsonApiModel
    {
        public override string Type { get; set; } = "sessions";

        [JsonProperty("sign_in_count", NullValueHandling = NullValueHandling.Ignore)]
        public string SignInCount { get; set; }

        [JsonProperty("last_sign_in_at", NullValueHandling = NullValueHandling.Ignore)]
        public string LastSignInAt { get; set; }

        [JsonProperty("failed_attempts", NullValueHandling = NullValueHandling.Ignore)]
        public string FailedAttempts { get; set; }

        [JsonProperty("locked_at", NullValueHandling = NullValueHandling.Ignore)]
        public string LockedAt { get; set; }

        // v4
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        // v4
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        [JsonProperty("account_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountName { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("authentication_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthenticationToken { get; set; }

        [JsonProperty("confirmed_at", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfirmedAt { get; set; }

        [JsonProperty("invitation_sent_at", NullValueHandling = NullValueHandling.Ignore)]
        public string InvitationSentAt { get; set; }

        [JsonProperty("invitation_accepted_at", NullValueHandling = NullValueHandling.Ignore)]
        public string InvitationAcceptedAt { get; set; }

        [JsonProperty("password_changed_at", NullValueHandling = NullValueHandling.Ignore)]
        public string PasswordChangedAt { get; set; }

        [JsonProperty("disabled_at", NullValueHandling = NullValueHandling.Ignore)]
        public string DisabledAt { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }

        [JsonProperty("devices", NullValueHandling = NullValueHandling.Ignore)]
        public Device Device { get; set; }

        // v4
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }

        // v4
        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }

        // v4
        [JsonProperty("id_token", NullValueHandling = NullValueHandling.Ignore)]
        public string IdToken { get; set; }

        // v4
        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenType { get; set; }

        // v4
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public string ExpiresIn { get; set; }

    }
}
