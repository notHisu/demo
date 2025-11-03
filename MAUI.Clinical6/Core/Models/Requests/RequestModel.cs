using System;
using System.Collections.Generic;
using Clinical6SDK.Services;
using Newtonsoft.Json;

namespace Xamarin.Forms.Clinical6.Core.Models.Requests
{
    public class RequestModel<T> : IParams
    {
        [JsonProperty("mobileapplicationkey")]
        public string MobileApplicationKey { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("data")]
        public DataModel<T> Data { get; set; }
        
        public string JsonResponse { get; set; }
        
        public List<ErrorsResonse> errors { get; set; }
    }

    public class DataModel<T>
    {
        /// <summary>
        ///     Gets or sets the request access token attribute. Check if this serialized corectly
        /// </summary>
        /// <value>The request access token attribute.</value>
        [JsonProperty("attributes")]
        public T Attributes { get; set; }

        //[JsonProperty("id")]
        //public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("relationships")]
        public object Relationships { get; set; }
    }

    public class RequestModelUser<T> : IParams
    {
        [JsonProperty("mobileapplicationkey", NullValueHandling = NullValueHandling.Ignore)]
        public string MobileApplicationKey { get; set; }
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("data")]
        public RequestUserChangeDataModel<T> Data { get; set; }
        [JsonProperty("meta")]
        public ReasonForChanges Meta { get; set; }
        [JsonProperty("JsonResponse", NullValueHandling = NullValueHandling.Ignore)]
        public string JsonResponse { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<ErrorsResonse> errors { get; set; }
    }

    public class RequestUserChangeDataModel<T>
    {
        /// <summary>
        ///     Gets or sets the request access token attribute. Check if this serialized corectly
        /// </summary>
        /// <value>The request access token attribute.</value>
        [JsonProperty("attributes")]
        public T Attributes { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ReasonForChanges
    {
        [JsonProperty("reason_for_changes")]
        public ReasonForChangesTimeZone Value { get; set; }
    }


    public class UpdateTimeZone
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("timezone")]
        public string TimeZone { get; set; }
    }

    public class ReasonForChangesTimeZone
    {
        [JsonProperty("timezone")]
        public string Reason { get; set; }
    }

    public class RegistrationStatus
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Account
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class AccessTokenAttribute
    {
        [JsonProperty("mobile_application_key")]
        public string MobileApplicationKey { get; set; }

        [JsonProperty("technology")]
        public string Technology { get; set; }

        [JsonProperty("push_id")]
        public string PushId { get; set; }

        [JsonProperty("app_version")]
        public string AppVersion { get; set; }
    }

    public class EmailInvitation
    {
        [JsonProperty("invitation_token")]
        public string InvitationToken { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class RequestPasswordReset
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class ResetPasword
    {
        [JsonProperty("reset_password_token")]
        public string ResetPasswordToken { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }



    public class UserTransitionRequest : IParams
    {
        [JsonProperty("mobileapplicationkey")]
        public string MobileApplicationKey { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public UserTransitionRequestStatus Status { get; set; }
        
        public List<ErrorsResonse> errors { get; set; }
    }

    public class UserTransitionRequestStatus
    {
        [JsonProperty("object")]
        public int Object { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("owner_type")]
        public string OwnerType { get; set; }

        [JsonProperty("transition")]
        public string Transition { get; set; }

        [JsonProperty("object_type")]
        public string ObjectType { get; set; }
    }
}
