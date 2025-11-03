using System;
using Clinical6SDK.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Clinical6SDK.Models;
using System.Linq;
using Clinical6SDK.Common.Exceptions;
using Clinical6SDK.Services.Requests;
using Clinical6SDK.Services.Responses;
using Clinical6SDK.Helpers;
using Device = Clinical6SDK.Helpers.Device;

namespace Clinical6SDK
{
    /// <summary>
	/// Main class that gives basic authentication and CRUD commands.
	/// </summary>
    public class Clinical6Instance : IClinical6Instance
    {
        private readonly string _udid, _technology;
        private readonly ClientSingleton _client = ClientSingleton.Instance;
        private string _mobileApplicationKey;

        public string AuthToken
        {
            get
            {
                return _client.AuthToken;
            }
            set
            {
                _client.AuthToken = value;
            }
        }

        public string BaseUrl
        {
            get
            {
                return _client.BaseUrl;
            }
            set
            {
                _client.BaseUrl = value;
            }
        }

        public Device Device
        {
            get
            {
                return _client.Device;
            }
            set
            {
                _client.Device = value;
            }
        }

        public string MobileApplicationKey
        {
            get
            {
                return _mobileApplicationKey;
            }
            set
            {
                _mobileApplicationKey = value;
            }
        }

        public User User
        {
            get
            {
                return _client.User;
            }
            set
            {
                _client.User = value;
            }
        }

        IClinical6MobileUserService _mobileUserService;
        public IClinical6MobileUserService MobileUserService
        {
            get
            {
                if (_mobileUserService == null)
                {
                    _mobileUserService = new Clinical6MobileUserService
                    {
                        BaseUrl = BaseUrl,
                        AuthToken = AuthToken
                    };

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(_mobileUserService.BaseUrl))
                        _mobileUserService.BaseUrl = BaseUrl;
                    if (string.IsNullOrWhiteSpace(_mobileUserService.AuthToken))
                        _mobileUserService.AuthToken = AuthToken;
                }

                return _mobileUserService;
            }
            set
            {
                _mobileUserService = value;

                if (_mobileUserService != null)
                {
                    _mobileUserService.BaseUrl = BaseUrl;
                    _mobileUserService.AuthToken = AuthToken;
                }
            }
        }

        JsonApiHttpService _jsonApiHttpService;
        internal JsonApiHttpService JsonApiHttpService
        {
            get
            {
                if (_jsonApiHttpService == null)
                {
                    _jsonApiHttpService = new JsonApiHttpService
                    {
                        BaseUrl = BaseUrl,
                        AuthToken = AuthToken
                    };
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(_jsonApiHttpService.BaseUrl))
                        _jsonApiHttpService.BaseUrl = BaseUrl;
                    if (string.IsNullOrWhiteSpace(_jsonApiHttpService.AuthToken))
                        _jsonApiHttpService.AuthToken = AuthToken;
                }

                return _jsonApiHttpService;
            }
            set
            {
                _jsonApiHttpService = value;

                if (_jsonApiHttpService != null)
                {
                    _jsonApiHttpService.BaseUrl = BaseUrl;
                    _jsonApiHttpService.AuthToken = AuthToken;
                }
            }
        }


        /// <summary>
		/// Determines if the user is signed in (AuthToken exists)
		/// </summary>
        public bool IsSignedIn
        {
            get { return !string.IsNullOrWhiteSpace(AuthToken); }
        }


        /// <summary>
		/// Simple Constructor
		/// </summary>
        public Clinical6Instance() { }


        /// <summary>
        /// Initializes a new instance of the Captive Reach SDK. 
        /// </summary>
        /// <param name="serviceUrl">Your Captive Reach service URL.</param>
        /// <param name="udid">The UDID of the device using the SDK.</param>
        /// <param name="technology">"ios" or "android"</param>
        public Clinical6Instance(string serviceUrl, string udid, string technology) : this(serviceUrl, udid, technology, null) { }


        /// <summary>
        /// Initializes a new instance of the Captive Reach SDK with an existing authentication token. Use this initialization method 
        /// if you have a persisted authentication token to avoid having to call one of the sign in methods again.
        /// </summary>
        /// <param name="serviceUrl">Your Captive Reach service URL.</param>
        /// <param name="udid">The UDID of the device using the SDK.</param>
        /// <param name="technology">"ios" or "android"</param>
        /// <param name="authToken">Token from an pre-existing authenticated session.</param>
        public Clinical6Instance(string serviceUrl, string udid, string technology, string authToken)
        {
            BaseUrl = serviceUrl;
            _udid = udid;
            _technology = technology;
            _client.AuthToken = authToken;
        }


        /// <summary>
        /// Makes a call to delete a JsonApi object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<T> Delete<T>(IJsonApiModel obj, Options options = null) where T : IJsonApiModel
        {
            return await JsonApiHttpService.Delete<T>(obj, options);
        }


        /// <summary>
        /// Makes a call to get a JsonApi object or list of JsonApi objects from the platform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var clinical6 = new Clinical6Instance(); // will need url and authtoken
        /// var devices = await clinical6.Get{List{Device}}();
        /// </code>
        /// Alternatively, if the object already exists with an id, a specific instance can be returned
        /// <code>
        /// var myDevice = await clinical6.Get{Device}(new Device { Id = 15 });
        /// </code>
        /// </example>
        public async Task<T> Get<T>(IJsonApiModel obj = null, Options options = null) where T : new()
        {
            return await JsonApiHttpService.Get<T>(obj, options);
        }

        /// <summary>
        /// Makes a call to get the children of a JsonApi object from the platform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="childType"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <example>
        /// This is an example on how to get a list of devices from a user
        /// <code>
        /// var clinical6 = new Clinical6Instance(); // will need url and authtoken
        /// var devices = await clinical6.GetChildrenList{Device}}(clinical6.User);
        /// </code>
        /// Alternatively, the childType can be set directly.
        /// <code>
        /// var devices = await clinical6.GetChildrenList{Device}}(clinical6.User, "devices");
        /// </code>
        /// </example>
        public async Task<T> GetChildren<T>(IJsonApiModel obj, string childType, Options options = null) where T : new()
        {
            return await JsonApiHttpService.GetChildren<T>(obj, childType, options);
        }

        /// <summary>
        /// Makes a call to get the profile of the logged in user from the platform
        /// </summary>
        /// <returns>The logged in user with a profile</returns>
        /// <example>
        /// <code>
        /// var clinical6 = new Clinical6Instance(); // will need url and authtoken
        /// clinical6.User = await clinical6.getProfile();
        /// </code>
        /// </example>
        public async Task<User> GetProfile()
        {
            return await new Clinical6MobileUserService().GetProfile();
        }

        /// <summary>
        /// Makes a call to insert a JsonApi object to the platform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var clinical6 = new Clinical6Instance(); // will need url and authtoken
        /// var mySite = new Site();
        /// var site = clinical6.Insert{Site}(mySite);
        /// </code>
        /// </example>
        public async Task<T> Insert<T>(IJsonApiModel obj, Options options = null) where T : IJsonApiModel
        {
            return await JsonApiHttpService.Insert<T>(obj, options);
        }

        public async Task<User> Register<T>(IParams parameters = null, string Json = "", Options options = null) where T : IJsonApiModel
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.Register<T>(parameters, Json, options);
        }

        /// <summary>
        /// Makes a call to update a JsonApi object on the platform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// var clinical6 = new Clinical6Instance(); // will need url and authtoken
        /// var mySite = new Site();
        /// // some change here
        /// var site = clinical6.Update{Site}(mySite);
        /// </code>
        /// </example>
        public async Task<T> Update<T>(IJsonApiModel obj, Options options = null) where T : IJsonApiModel
        {
            return await JsonApiHttpService.Update<T>(obj, options);
        }

        /// <summary>
        /// Creates a temporary user and signs that user into the Captive Reach service.
        /// </summary>
        /// <returns>True if the guest sign in was successful; False otherwise.</returns>
        public async Task<bool> SignInGuestAsync()
        {
            var result = await MobileUserService.SignInGuest(_udid, _technology);

            if (result == null || string.IsNullOrWhiteSpace(result.AuthToken))
            {
                ResetInstance();
                return false;
            }

            AuthToken = result.AuthToken;

            return IsSignedIn;
        }

        /// <summary>
        /// Creates a new user and signs that user into the Captive Reach service.
        /// </summary>
        /// <returns>True if the sign up was successful; False otherwise.</returns>
        /// <param name="accountName">Unique account name for the new user.</param>
        /// <param name="password">Password for the new user.</param>
        /// <param name="profile">Profile object with details about the new user.</param>
        public async Task<bool> SignUpAsync(string accountName, string password, Profile profile)
        {


            if (string.IsNullOrWhiteSpace(accountName))

                throw new ArgumentException("accountName");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("password");

            if (profile == null)
                throw new ArgumentException("profile");

            var result = await MobileUserService.SignUp(accountName, password, _udid, _technology, profile);

            if (result == null || string.IsNullOrWhiteSpace(result.AuthToken))
                return false;

            // TODO: Handle API returned errors (e.g., account already exists)

            AuthToken = result.AuthToken;

            return IsSignedIn;
        }

        /// <summary>
        /// Validates the given user credentials and signs that user into the Captive Reach service.
        /// </summary>
        /// <returns>True if the sign in was successful; False otherwise.</returns>
        /// <param name="accountName">Unique account name for the user signing in.</param>
        /// <param name="password">Password for the user signing in.</param>
        public async Task<bool> SignInAsync(string accountName, string password)
        {
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("accountName");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("password");

            var request = new SignInRequestMobileUser();

            request.AccountName = accountName;

            request.Password = password;

            var result = await MobileUserService.SignIn(request, _udid, _technology);

            if (result == null || string.IsNullOrWhiteSpace(result.AuthToken))
                return false;

            // TODO: Handle API returned errors (e.g., account already exists)

            AuthToken = result.AuthToken;

            return IsSignedIn;
        }

        /// <summary>
        /// Signs the in email async.
        /// </summary>
        /// <returns>The in email async.</returns>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        public async Task<bool> SignInEmailAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("accountName");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("password");

            var request = new SignInRequestMobileUser();

            request.Email = email;
            request.AccountName = email;
            request.Password = password;

            var result = await MobileUserService.SignIn(request, _udid, _technology);

            if (result == null || string.IsNullOrWhiteSpace(result.AuthToken))
                return false;

            // TODO: Handle API returned errors (e.g., account already exists)

            AuthToken = result.AuthToken;

            return IsSignedIn;
        }

        /// <summary>
		/// Signs the user out and resets data.
		/// </summary>
		/// <returns></returns>
        public async Task SignOutAsync()
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException("The user is already signed out.");

            await MobileUserService.SignOut();

            ResetInstance();
        }

        /// <summary>
        /// Sends the user password instructions via email.
        /// </summary>
        /// <param name="email">The user's registered email address that will be used to look up the user's account and password.</param>
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("email");

            var result = await MobileUserService.ForgotPassword(email);

            if (result == null)
                return false;

            // TODO: Handle API return errors (e.g., not found)

            return true;
        }

        /// <summary>
        /// Sends the user's account name via email.
        /// </summary>
        /// <param name="email">The user's registered email address that will be used to look up the user's account.</param>
        public async Task<bool> ForgotAccountNameAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("email");

            var result = await MobileUserService.ForgotAccountName(email);

            if (result == null)
                return false;

            // TODO: Handle API return errors (e.g., not found)

            return true;
        }

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <returns>True if the reset was successful; False otherwise.</returns>
        /// <param name="oldPassword">Current / old password.</param>
        /// <param name="newPassword">New password.</param>
        /// <param name="newPasswordConfirmation">New password confirmation.</param>
        public async Task<bool> ResetPasswordAsync(string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new ArgumentException("oldPassword");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("newPassword");

            if (string.IsNullOrWhiteSpace(newPasswordConfirmation))
                throw new ArgumentException("newPasswordConfirmation");

            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            var result = await MobileUserService.ResetPassword(oldPassword, newPassword, newPasswordConfirmation);

            if (result == null)
                return false;

            // TODO: Handle API returned errors (e.g., password mismatch)

            return true;
        }

        /// <summary>
        /// Disables the user's account.  All access to the Captive Reach service will be revoked.
        /// NOTE: This only disables the account, this does NOT delete any user data.
        /// </summary>
        public async Task DisableAccountAsync()
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            await MobileUserService.Disable();

            ResetInstance();
        }

        /// <summary>
        /// Removes the user's account from the Captive Reach service. 
        /// Caution: This will remove all the data associated with the user. This action cannot be undone.
        /// </summary>
        public async Task DeleteAccountAsync()
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            await MobileUserService.Delete();

            ResetInstance();
        }

        /// <summary>
        /// Verifies the user's account using the provided verification code. 
        /// </summary>
        /// <returns>True if the verification process was successful; False otherwise.</returns>
        /// <param name="code">Verification code.</param>
        public async Task<bool> VerifyAccountAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("code", "code");

            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            var result = await MobileUserService.Verify(code);

            if (result == null)
                return false;

            return true;
        }

        /// <summary>
        /// Sets up the PIN for the user's account.  This method can only be used for users that have not had their PINs set
        /// already (e.g., new invited users).
        /// </summary>
        /// <returns>True if setting the PIN was successful; False otherwise.</returns>
        /// <param name="pin">PIN.</param>
        /// <param name="pinConfirmation">PIN confirmation.</param>
        public async Task<bool> SetPinAsync(int pin, int pinConfirmation)
        {
            if (pin < 0)
                throw new ArgumentOutOfRangeException("pin");

            if (pinConfirmation < 0)
                throw new ArgumentOutOfRangeException("pinConfirmation");

            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            var result = await MobileUserService.SetPin(pin, pinConfirmation);

            if (result == null)
                return false;

            return true;
        }




        public async Task<User> AcceptInvitation<T>(Invitation invitation) where T : IJsonApiModel
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.AcceptInvitation(invitation);
        }
        public async Task<T> Confirm<T>(string token)
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.Confirm<T>(token);
        }

        public async Task<T> GetRegistrationStatus<T>(IParams parameters = null, string Json = "", Options options = null)
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.GetRegistrationStatus<T>(parameters, Json, options);
        }

        public async Task<T> GetSession<T>(string token)
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.GetSession<T>(token);
        }

        public async Task<User> Invite(Invitation invitation)
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.Invite(invitation);
        }
        public async Task<User> RequestPasswordReset(PasswordResetModel reset, Options options = null)
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.RequestPasswordReset(reset, options);
        }
        public async Task<User> ResetPassword(PasswordResetModel reset, Options options = null)
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.ResetPassword(reset, options);
        }
        public async Task<T> SendConfirmation<T>(string email)
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.SendConfirmation<T>(email);
        }
        public async Task<Session> SignIn<T>(Session session, Options options = null) where T : IJsonApiModel
        {
            if (!IsSignedIn)
                throw new Clinical6UnauthorizedException();

            return await MobileUserService.SignIn<T>(session, options);
        }

        void ResetInstance()
        {
			// Reset auth token
			_client.AuthToken = null;
			_client.User = null;

			// Reset all services
			MobileUserService = null;
        }
    }
}

