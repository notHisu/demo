using System;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;
using Newtonsoft.Json;

namespace Xamarin.Forms.Clinical6.Core.Models.Responses
{
    public class ResponseModel<T>
    {
        [JsonProperty("data")]
        public DataModel<T> Data { get; set; }
        
        [JsonProperty("included")]
        public DataModel<DevicesResponse>[] Included { get; set; }
    }
    
     public class ResponseProfileModel<T>
    {
        [JsonProperty("data")]
        public DataModel<T> Data { get; set; }
        
        [JsonProperty("included")]
        public DataModel<User>[] Included { get; set; }
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

    public class DataModel<T>
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
        public string TypeName { get; set; }

        [JsonProperty("relationships")]
        public object Relationships { get; set; }
    }

    public class ResgistrationStatus
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
    
    public class ResgistrationStatusResponse
    {
        public string id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "statuses";
        [JsonProperty("value")]
        public string Value { get; set; }
    }
    /*
    public class MobileUser
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("account_name")]
        public string Account_Name { get; set; }

        public string _AccountName;
        public string AccountName
        {
            get
            {
                if (string.IsNullOrEmpty(Account_Name))
                    _AccountName = UserName;

                if (string.IsNullOrEmpty(_AccountName))
                    _AccountName = string.Empty;

                return _AccountName;
            }
            set
            {
                _AccountName = value;
            }
        }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("encryption_key")]
        public string EncryptionKey { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("verified_at")]
        public DateTime? VerifiedAt { get; set; }

        [JsonProperty("invitation_sent_at")]
        public DateTime? InvitationSentAt { get; set; }

        [JsonProperty("invitation_accepted_at")]
        public DateTime? InvitationAcceptedAt { get; set; }
    }
    */
    public class DevicesResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("consent_status")]
        public string ConsentStatus { get; set; }
        
        [JsonProperty("last_consented_at")]
        public DateTime? LastConsentedAt { get; set; }
    }

    /* API v2 */
    public class UpdateResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }


}
