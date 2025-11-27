using Newtonsoft.Json;

namespace Xamarin.Forms.Clinical6.Core.Models
{
    /// <summary>
    /// The Type T will set this.Data.Attributes's type to T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonModel<T>
    {
        [JsonProperty("data")]
        public DataModel<T> Data { get; set; }
    }

    public class CommonDataModel<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class CommonListDataModel<T>
    {
        [JsonProperty("data")]
        public T[] Data { get; set; }
    }

    public class CommonListModel<T>
    {
        [JsonProperty("data")]
        public DataModel<T>[] Data { get; set; }
    }

    public class CommonListIncludedModel<T1, T2>
    {
        [JsonProperty("data")]
        public T1[] Data { get; set; }

        [JsonProperty("included")]
        public T2[] Included { get; set; }
    }

    /// <summary>
    /// Base class for DataModel that can be used to pass in multiple types if necessary
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class BaseDataModel<T1, T2>
    {
        [JsonProperty("attributes")]
        public T1 Attributes { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string TypeName { get; set; }

        [JsonProperty("relationships")]
        public T2 Relationships { get; set; }
    }

    /// <summary>
    /// Current default data model. Use inherited BaseDataModel<T1, T2> if relationship needs a type other than object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataModel<T> : BaseDataModel<T, object> { }

    public class RequestAccessTokenAttribute
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

    public class UpdatePushTokenAttribute
    {
        [JsonProperty("technology")]
        public string Technology { get; set; }

        [JsonProperty("push_id")]
        public string PushId { get; set; }
    }

    public class Account
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class DevicesResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

    public class Validation
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class ProfileAttributes
    {
        [JsonProperty("icf_sign_date")]
        public string IcfSignDate { get; set; }

        [JsonProperty("icf_signed_by")]
        public string IcfSignedBy { get; set; }

        [JsonProperty("location_id")]
        public string LocationId { get; set; }
    }

    public class UpdateResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class UserTransitionRequest
    {
        [JsonProperty("status")]
        public UserTransitionRequestStatus Status { get; set; }
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

    public class UserTransitionResponse
    {
        [JsonProperty("status")]
        public UserTransitionResponseStatus Status { get; set; }
    }

    public class UserTransitionResponseStatus
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class EmailInvitation
    {
        [JsonProperty("invitation_token")]
        public string InvitationToken { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class PinRequest
    {
        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("pin_confirmation")]
        public string PinConfirmation { get; set; }
    }
}