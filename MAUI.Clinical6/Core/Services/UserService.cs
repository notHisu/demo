//#define HOTZONE_API
#define NEWSDK_API

using System;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Core.Models.Requests;
using Xamarin.Forms.Clinical6.Core.Models.Responses;
using Xamarin.Forms;
using Clinical6SDK.Services;
using Newtonsoft.Json;
using JsonApiSerializer;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Resources;
using Clinical6SDK.Models;
using JsonApiSerializer.JsonApi;
using Clinical6SDK;
using Clinical6SDK.Helpers;
using Clinical6SDK.Common.Exceptions;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    /// <summary>
    ///     Responsible for user authentication and profile info
    /// </summary>
    public interface IUserService
    {
        // No Api methods
        Task<string> GetRegisteredEmail();
        /// <summary>
        /// can we get rid of this? Combine with this.GetRegistrationStatus() and this.VerifyEmail()
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task SaveRegisteredEmail(string email);
        Task<bool> GetHasPin();
        Task SaveHasPin(bool hasPin);
        Task<int?> GetUserId();
        Task<bool> GetTermsOfUseAccepted();
        Task SaveTermsOfUseAccepted(bool accepted);
        Task<bool> HasAccessToken();
        Task<bool> IsLoggedIn();

        Task<string> UpdateUserProfileWithErrorResponse<T>(User updatedProfile);
        Task<T> UpdateUserProfileTimezoneWithErrorResponse<T>(User user, string reasonForChanges);
        Task<bool> refreshToken();
        Task<bool> UpdateUserAvatar(Models.Photo photo);
        Task GetSites();
        Task<bool> TransitionUserStatus();

        // User - Login
        Task<bool> GetAccessToken();
        Task<Models.RegistrationStatus> GetRegistrationStatus(string emailAddress);
        Task<bool> CreatePin(string pin, string pinConfirmation);
        Task<(bool Success, string ErrorMessage, string ErrorResponse)> CreatePinWithErrorResponse(string pin, string pinConfirmation);

        Task<bool> ValidateEmailAndPin(string emailAddress, string password);
        Task<string> ValidateEmailAndPinWitErrorMessage(string emailAddress, string password);
        Task<(bool Success, string ErrorMessage, string ErrorResponse)> ValidateEmailAndPinWitErrorResponse(string emailAddress, string password);

        Task<bool> VerifyEmail(string invitationToken, string email = "");
        Task<string> VerifyEmaiWithError(string invitationToken, string email = "");
        Task<(bool Success, string ErrorMessage, string ErrorResponse)> VerifyEmailWithErrorResponse(string invitationToken, string email = "");

        Task<string> ResetPin(string confirmationCode, string pin, string pinConfirmation);
        Task<(bool Success, string ErrorMessage, string ErrorResponse)> ResetPinWithErrorResponse(string confirmationCode, string pin, string pinConfirmation);

        Task<bool> SendPinResetActivationCode(string emailAddress);
        Task<(bool Success, string ErrorMessage, string ErrorResponse)> SendPinResetActivationCodeWithErrorResponse(string emailAddress);


        Task<List<SsoOptions>> GetSSOProvidersAsync();
    }

    /// <summary>
    /// User service.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IHttpHandler _http;
        //private readonly IApiModelFactory _apiModelFactory;

        public IClinical6Instance clinical6 { get; private set; }

        private readonly ICacheService _cacheService;
        IClinical6MobileUserService _mobileUserService;
        IClinical6SSOOptionsService _SSOOptionsService;
        private readonly IApiRequestFactory _apiRequestFactory;
        private string _authToken = "";

        /// <summary>
        /// Gets or sets the mobile user service.
        /// </summary>
        /// <value>The mobile user service.</value>
        public IClinical6MobileUserService MobileUserService
        {
            get
            {
                if (_mobileUserService == null)
                {
                    _mobileUserService = new Clinical6MobileUserService
                    {
                        BaseUrl = EnvironmentConfig.ApiRoot,
                        AuthToken = _authToken
                    };
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(_mobileUserService.BaseUrl))
                        _mobileUserService.BaseUrl = EnvironmentConfig.ApiRoot;
                    if (string.IsNullOrWhiteSpace(_mobileUserService.AuthToken))
                        _mobileUserService.AuthToken = _authToken;
                }

                return _mobileUserService;
            }
            set
            {
                _mobileUserService = value;

                if (_mobileUserService != null)
                {
                    _mobileUserService.BaseUrl = EnvironmentConfig.ApiRoot;
                    _mobileUserService.AuthToken = _authToken;
                }
            }
        }

        /// <summary>
        /// Gets or sets the SSOO ptions service.
        /// </summary>
        /// <value>The SSOO ptions service.</value>
        public IClinical6SSOOptionsService SSOOptionsService
        {
            get
            {
                if (_SSOOptionsService == null)
                {
                    _SSOOptionsService = new Clinical6SSOOptionsService
                    {
                        BaseUrl = EnvironmentConfig.ApiRoot,
                        AuthToken = _authToken
                    };
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(_SSOOptionsService.BaseUrl))
                        _SSOOptionsService.BaseUrl = EnvironmentConfig.ApiRoot;
                    if (string.IsNullOrWhiteSpace(_SSOOptionsService.AuthToken))
                        _SSOOptionsService.AuthToken = _authToken;
                }

                return _SSOOptionsService;
            }
            set
            {
                _SSOOptionsService = value;

                if (_SSOOptionsService != null)
                {
                    _SSOOptionsService.BaseUrl = EnvironmentConfig.ApiRoot;
                    _SSOOptionsService.AuthToken = _authToken;
                }
            }
        }

        public UserService(
            IHttpHandler httpHandler,
            //IApiModelFactory apiModelFactory,
            ICacheService cacheService,
            IApiRequestFactory apiRequestFactory,
            IClinical6Instance clinical6Instance
        )
        {

            _cacheService = cacheService;
            //_apiModelFactory = apiModelFactory;
            clinical6 = clinical6Instance;

            _http = httpHandler;

            _apiRequestFactory = apiRequestFactory;

            _mobileUserService = new Clinical6MobileUserService
            {
                BaseUrl = EnvironmentConfig.ApiRoot,
                AuthToken = _authToken
            };
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>The access token.</returns>
        public async Task<bool> GetAccessToken()
        {
            try
            {
                var pushId = string.Empty;//await _pushTokenStore.GetPushToken();

                var device = await clinical6.Insert<Clinical6SDK.Helpers.Device>(_apiRequestFactory.GetNewDevice());

                if (device != null)
                {
                    if (!string.IsNullOrWhiteSpace(device.AccessToken))
                    {
                        await _cacheService.SaveUploadedPushToken(pushId);
                        await _cacheService.SaveAuthToken(device.AccessToken);
                        await _cacheService.SaveDeviceId((int)device.Id);
                        clinical6.AuthToken = device.AccessToken;
                        clinical6.Device = device;
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return false;
        }

        /// <summary>
        /// Gets the registration status.
        /// In Use
        /// </summary>
        /// <returns>The registration status.</returns>
        /// <param name="emailAddress">Email address.</param>
        public async Task<Models.RegistrationStatus> GetRegistrationStatus(string emailAddress)
        {
            try
            {
                var request = (RequestModel<Models.Requests.Account>)
                _apiRequestFactory.GetRegistrationStatusRequest(emailAddress);

                var response = await MobileUserService.GetRegistrationStatusWithResponse<JsonApiSerializer.JsonApi.DocumentRoot<ResgistrationStatusResponse>>(request);

                try
                {
                    var meta = response.Meta;

                    var passwordset = meta.GetOrDefault("password_set", false).ToString().ToLower();

                    Console.WriteLine(passwordset);

                    if (passwordset.Equals("false"))
                    {
                        if (response.Data.Value == "consent" || response.Data.Value == "pre_screening" || response.Data.Value == "screening" ||
                            response.Data.Value == "consenting" || response.Data.Value == "follow_up")
                        {
                            return Models.RegistrationStatus.Consent;
                        }
                        return Models.RegistrationStatus.New;
                    }
                }
                catch
                {
                }


                if (response.Data.Value == "new")
                {
                    return Models.RegistrationStatus.New;
                }
                else if (response.Data.Value == "active")
                {
                    return Models.RegistrationStatus.Existing;
                }
                else if (response.Data.Value == "disabled" || response.Data.Value == "off_study" || response.Data.Value == "rejected_screening")
                {
                    return Models.RegistrationStatus.Disabled;
                }
                else if (response.Data.Value == "withdrawn")
                {
                    return Models.RegistrationStatus.Withdrawn;
                }
                else if (response.Data.Value == "consent" || response.Data.Value == "pre_screening" || response.Data.Value == "screening" ||
                         response.Data.Value == "consenting" || response.Data.Value == "follow_up" || response.Data.Value == "washout" ||
                         response.Data.Value == "baseline" || response.Data.Value == "dose_optimization" || response.Data.Value == "early_discontinuation")
                {
                    return Models.RegistrationStatus.Consent;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            // TODO: Get this from the API
            return Models.RegistrationStatus.Invalid;
        }

        /// <summary>
        /// Creates the pin.
        /// </summary>
        /// <returns>The pin.</returns>
        /// <param name="pin">Pin.</param>
        /// <param name="pinConfirmation">Pin confirmation.</param>
        public async Task<bool> CreatePin(string pin, string pinConfirmation)
        {
            try
            {
                var resp = await MobileUserService.SetPin(pin, pinConfirmation);

                if (resp != null)
                {
                    return resp.IsResponseSuccessful;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return false;
        }

        /// <summary>
        /// Creates the pin with error response.
        /// </summary>
        /// <returns>The pin with error response.</returns>
        /// <param name="pin">Pin.</param>
        /// <param name="pinConfirmation">Pin confirmation.</param>
        public async Task<(bool Success, string ErrorMessage, string ErrorResponse)> CreatePinWithErrorResponse(string pin, string pinConfirmation)
        {
            try
            {
                var verificationCode = await _cacheService.GetVerificationCode();

                var req = await _apiRequestFactory.CreatePinRequest(verificationCode, pin, pinConfirmation);

                MobileUserService.AuthToken = await _cacheService.GetAuthToken();

                try
                {
                    var user = await MobileUserService.AcceptInvitation(req);
                    await _cacheService.SaveUserId(user.Id);
                    await _cacheService.SaveSiteMemberId(user.SiteMember.Id);

                    if (user.Devices.Count > 0)
                    {
                        var deviceQuery = user.Devices.Where(d => d.AccessToken != null).ToList();
                        var newToken = deviceQuery.LastOrDefault().AccessToken;
                        await _cacheService.SaveAuthToken(newToken);

                        Settings.SetBoleanProperty(Settings.IsLoginIn, true);
                        Settings.SetProperty(Settings.LoginDate, DateTime.UtcNow.Ticks.ToString());
                        try
                        {
                            if (user.SiteMember != null && user.SiteMember.ConsentStatus != null)
                            {
                                Settings.SetProperty(Settings.ConsetStatus, user.SiteMember.ConsentStatus);

                                if (user.SiteMember.LastConsentedAt != null)
                                {
                                    Settings.SetProperty(Settings.LastConsentedAt, user.SiteMember.LastConsentedAt.ToString());
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    var response = await TransitionUserStatus();
                    return (true, string.Empty, null);

                }
                catch (Clinical6ServerException ex)
                {
                    return (false, ex.Message, null);
                }
                catch (Exception ex)
                {
                    return (false, ex.Message, null);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return (false, string.Empty, string.Empty);
        }

        /// <summary>
        /// Validates the email and pin.
        /// </summary>
        /// <returns>The email and pin.</returns>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="password">Password.</param>
        public async Task<bool> ValidateEmailAndPin(string emailAddress, string password)
        {
            try
            {
                var hasToken = await HasAccessToken();
                if (!hasToken)
                {
                    _authToken = await _cacheService.GetAuthToken();
                }

                var par = await _apiRequestFactory.ValidateEmailAndPinRequest(emailAddress, password);

                Session session = await MobileUserService.SignIn<User>(par);


                if (session != null)
                {
                    await _cacheService.SaveUserId(session.User.Id);
                    await _cacheService.SaveSiteMemberId(session.User.SiteMember.Id);
                    return session.User.Id > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Validates the email and pin wit error message.
        /// </summary>
        /// <returns>The email and pin wit error message.</returns>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="password">Password.</param>
        public async Task<string> ValidateEmailAndPinWitErrorMessage(string emailAddress, string password)
        {
            try
            {
                var hasToken = await HasAccessToken();
                if (!hasToken)
                {
                    _authToken = await _cacheService.GetAuthToken();
                }

                Session session = await MobileUserService.SignIn<User>(new Session
                {
                    Email = emailAddress,
                    Password = password,
                    Device = (clinical6.Device != null && clinical6.Device.Id != null) ? clinical6.Device : new Clinical6SDK.Helpers.Device
                    {
                        Id = await _cacheService.GetDeviceId()
                    }
                });


                if (session != null)
                {
                    await _cacheService.SaveUserId(session.User.Id);
                    await _cacheService.SaveSiteMemberId(session.User.SiteMember.Id);

                    return session.User.Id > 0 ? String.Empty : " ";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return "";
        }

        /// <summary>
        /// Generates the new auth token.
        /// </summary>
        /// <returns>The new auth token.</returns>
        private async Task<bool> GenerateNewAuthToken()
        {
            var tokenSuccess = await GetAccessToken();
            if (!tokenSuccess)
            {

                var hasAccesstoken = await HasAccessToken();
                if (!hasAccesstoken)
                {
                    tokenSuccess = await GetAccessToken();

                }
                hasAccesstoken = await HasAccessToken();
                if (!hasAccesstoken)
                {
                    hasAccesstoken = await GetAccessToken();

                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// Resets the token.
        /// </summary>
        private async Task<bool> ResetToken()
        {
            await GenerateNewAuthToken();

            var hasToken = await HasAccessToken();
            if (!hasToken)
            {
                _authToken = await _cacheService.GetAuthToken();

                if (_authToken == null)
                {
                    var successfullNewAuthToken = await GenerateNewAuthToken();
                    if (!successfullNewAuthToken)
                    {
                        return false;
                    }
                    else
                    {
                        _authToken = await _cacheService.GetAuthToken();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the email and pin wit error response.
        /// </summary>
        /// <returns>The email and pin wit error response.</returns>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="password">Password.</param>
        public async Task<(bool Success, string ErrorMessage, string ErrorResponse)> ValidateEmailAndPinWitErrorResponse(string emailAddress, string password)
        {
            try
            {
                try
                {
                    await ResetToken();

                    Session session = await clinical6.SignIn<User>(new Session
                    {
                        Email = emailAddress,
                        Password = password,
                        Device = new Clinical6SDK.Helpers.Device { Id = await _cacheService.GetDeviceId() }
                    });

                    if (session.User?.HomeLocation != null)
                    {
                        Settings.SetProperty(Settings.HomeLocation, session.User?.HomeLocation.Id.ToString());
                    }

                    Settings.SetProperty(Settings.CurrentSeesionDeviceId, clinical6.Device.Id.ToString());
                    await _cacheService.SaveUserId(clinical6.User.Id);
                    await _cacheService.SaveSiteMemberId(clinical6.User.SiteMember.Id);
                    await _cacheService.SaveAuthToken(clinical6.Device.AccessToken);

                    Settings.SetBoleanProperty(Settings.IsLoginIn, true);
                    Settings.SetProperty(Settings.LoginDate, DateTime.UtcNow.Ticks.ToString());

                    if (clinical6.User.SiteMember != null)
                    {
                        if (clinical6.User.SiteMember.ConsentStatus != null)
                        {
                            Settings.SetProperty(Settings.ConsetStatus, clinical6.User.SiteMember.ConsentStatus);
                        }
                        if (clinical6.User.SiteMember.LastConsentedAt != null)
                        {
                            Settings.SetProperty(Settings.LastConsentedAt, clinical6.User.SiteMember.LastConsentedAt.Value.Ticks.ToString());
                        }
                    }
                    return (true, string.Empty, null);
                }
                catch (Clinical6ServerException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return (false, ex.ErrorResponse.Errors[0].Detail, JsonConvert.SerializeObject(ex.ErrorResponse));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                return (false, "Error", null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return (false, "", null);
        }

        /// <summary>
        /// Verifies the email.
        /// </summary>
        /// <returns>The email.</returns>
        /// <param name="invitationToken">Invitation token.</param>
        /// <param name="email">Email.</param>
        public async Task<bool> VerifyEmail(string invitationToken, string email = "")
        {
            try
            {
                var hasToken = await HasAccessToken();
                if (!hasToken)
                {
                    _authToken = await _cacheService.GetAuthToken();
                }
                Invitation invitation = await _apiRequestFactory.VerifyEmailRequest(invitationToken, email);

                User user = await MobileUserService.AcceptInvitation(invitation);

                if (user != null)
                {
                    await _cacheService.SaveUserId(user.Id);
                    await _cacheService.SaveSiteMemberId(user.SiteMember.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Verifies the emai with error.
        /// </summary>
        /// <returns>The emai with error.</returns>
        /// <param name="invitationToken">Invitation token.</param>
        /// <param name="email">Email.</param>
        public async Task<string> VerifyEmaiWithError(string invitationToken, string email = "")
        {
            try
            {
                var hasToken = await HasAccessToken();
                if (!hasToken)
                {
                    _authToken = await _cacheService.GetAuthToken();
                }

                Invitation req = await _apiRequestFactory.VerifyEmailRequest(invitationToken, email);

                try
                {
                    User user = await MobileUserService.AcceptInvitation(req);
                    await _cacheService.SaveUserId(user.Id);
                    await _cacheService.SaveSiteMemberId(user.SiteMember.Id);
                    return string.Empty;
                }
                catch (Clinical6ServerException ex)
                {
                    return ex.ErrorResponse.Errors[0].Detail;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return "Error";
        }

        /// <summary>
        /// Verifies the email with error response.
        /// </summary>
        /// <returns>The email with error response.</returns>
        /// <param name="invitationToken">Invitation token.</param>
        /// <param name="email">Email.</param>
        public async Task<(bool Success, string ErrorMessage, string ErrorResponse)> VerifyEmailWithErrorResponse(string invitationToken, string email = "")
        {
            try
            {
                var hasToken = await HasAccessToken();
                if (!hasToken)
                {
                    _authToken = await _cacheService.GetAuthToken();
                }

                var req = await _apiRequestFactory.VerifyEmailRequest(invitationToken, email);

                MobileUserService.AuthToken = await _cacheService.GetAuthToken();


                try
                {
                    InvitationValidation validation = await MobileUserService.GetInvitationStatus(req);
                    if (validation.Status.Equals("acceptable"))
                    {
                        await _cacheService.SaveVerificationCode(invitationToken);
                        return (true, string.Empty, null);
                    }
                    else
                    {
                        return (false, "The 8 digit verification code entered does not match your registered email. Please check your email to find the correct code.", JsonConvert.SerializeObject(validation));
                    }



                }
                catch (Clinical6ServerException ex)
                {
                    return (false, ex.ErrorResponse.Errors[0].Detail, JsonConvert.SerializeObject(ex.ErrorResponse));
                }
                catch
                {
                    return (false, "Error", null);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return (false, "Error", null);
        }

        /// <summary>
        /// Resets the pin.
        /// </summary>
        /// <returns>The pin.</returns>
        /// <param name="confirmationCode">Confirmation code.</param>
        /// <param name="pin">Pin.</param>
        /// <param name="pinConfirmation">Pin confirmation.</param>
        public async Task<string> ResetPin(string confirmationCode, string pin, string pinConfirmation)
        {
            if (pin != pinConfirmation)
            {
                return " ";
            }

            string usermail = string.Empty;

            //if (Application.Current.Properties.ContainsKey(Settings.PinResetEmail))
            if (Preferences.ContainsKey(Settings.PinResetEmail))
            {
                //usermail = Application.Current.Properties[Settings.PinResetEmail].ToString();
                usermail = Preferences.Get(Settings.PinResetEmail, string.Empty);

                if (string.IsNullOrEmpty(usermail))
                {
                    usermail = await GetRegisteredEmail();
                }
            }
            else
            {
                usermail = await GetRegisteredEmail();
            }

            try
            {
                var device = _apiRequestFactory.GetNewDevice();
                await device.Save();

                User user = await MobileUserService.ResetPassword(new PasswordResetModel
                {
                    Email = usermail,
                    ResetPasswordToken = confirmationCode,
                    Password = pin,
                    Device = device
                });

                try
                {
                    await _cacheService.SaveUserId(user.Id);
                    await _cacheService.SaveSiteMemberId(user.SiteMember.Id);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }


                //will this work?
                await _cacheService.SaveAuthToken(device.AccessToken);
                await _cacheService.SaveDeviceId((int)device.Id);
            }
            catch (Clinical6ServerException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ErrorResponse.Errors[0].Detail);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// Resets the pin with error response.
        /// </summary>
        /// <returns>The pin with error response.</returns>
        /// <param name="confirmationCode">Confirmation code.</param>
        /// <param name="pin">Pin.</param>
        /// <param name="pinConfirmation">Pin confirmation.</param>
        public async Task<(bool Success, string ErrorMessage, string ErrorResponse)> ResetPinWithErrorResponse(string confirmationCode, string pin, string pinConfirmation)
        {
            if (pin != pinConfirmation)
            {
                return (false, "pin != pinConfirmation", null);
            }

            string usermail = string.Empty;

            //if (Application.Current.Properties.ContainsKey(Settings.PinResetEmail))
            if (Preferences.ContainsKey(Settings.PinResetEmail))
            {
                //usermail = Application.Current.Properties[Settings.PinResetEmail].ToString();
                usermail = Preferences.Get(Settings.PinResetEmail, string.Empty);

                if (string.IsNullOrEmpty(usermail))
                {
                    usermail = await GetRegisteredEmail();
                }
            }
            else
            {
                usermail = await GetRegisteredEmail();
            }

            try
            {
                Clinical6SDK.Helpers.Device device = _apiRequestFactory.GetNewDevice();

                device = await device.Save();

                var respToken = device.AccessToken;


                if (respToken == null)
                {
                    return (false, "Token is Null", null);
                }

                MobileUserService.AuthToken = await _cacheService.GetAuthToken();
                try
                {
                    User user = await MobileUserService.ResetPassword(new PasswordResetModel
                    {
                        Email = usermail,
                        ResetPasswordToken = confirmationCode,
                        Password = pin,
                        Device = device
                    });
                    try
                    {
                        await _cacheService.SaveUserId(user.Id);
                        await _cacheService.SaveSiteMemberId(user.SiteMember.Id);
                        await _cacheService.SaveVerificationCode(confirmationCode);
                        if (user.Devices != null && user.Devices.Count > 0)
                        {
                            var deviceQuery = user.Devices.Where(d => d.AccessToken != null).ToList();
                            var foundDevice = deviceQuery.LastOrDefault();

                            await _cacheService.SaveAuthToken(foundDevice.AccessToken);
                            await _cacheService.SaveDeviceId((int)foundDevice.Id);

                            Settings.SetBoleanProperty(Settings.IsLoginIn, true);
                            Settings.SetProperty(Settings.LoginDate, DateTime.UtcNow.Ticks.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                    return (true, string.Empty, null);
                }
                catch (Clinical6ServerException ex)
                {
                    return (false, ex.ErrorResponse.Errors[0].Detail, JsonConvert.SerializeObject(ex.ErrorResponse));
                }



            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return (false, "Error", null);
        }

        /// <summary>
        /// Sends the pin reset activation code.
        /// </summary>
        /// <returns>The pin reset activation code.</returns>
        /// <param name="emailAddress">Email address.</param>
        public async Task<bool> SendPinResetActivationCode(string emailAddress)
        {
            try
            {
                User user = await MobileUserService.RequestPasswordReset(new PasswordResetModel { Email = emailAddress });

                return user != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Sends the pin reset activation code with error response.
        /// </summary>
        /// <returns>The pin reset activation code with error response.</returns>
        /// <param name="emailAddress">Email address.</param>
        public async Task<(bool Success, string ErrorMessage, string ErrorResponse)> SendPinResetActivationCodeWithErrorResponse(string emailAddress)
        {
            try
            {
                MobileUserService.AuthToken = await _cacheService.GetAuthToken();
                User user = await MobileUserService.RequestPasswordReset(new PasswordResetModel { Email = emailAddress });
                return (true, string.Empty, null);
            }
            catch (Clinical6ServerException ex)
            {
                return (false, ex.ErrorResponse.Errors[0].Detail, JsonConvert.SerializeObject(ex.ErrorResponse));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return (false, "Error", null);
        }

        /// <summary>
        /// Sends the pin reset activation code with error message.
        /// </summary>
        /// <returns>The pin reset activation code with error message.</returns>
        /// <param name="emailAddress">Email address.</param>
        public async Task<string> SendPinResetActivationCodeWithErrorMessage(string emailAddress)
        {
            try
            {
                User user = await MobileUserService.RequestPasswordReset(new PasswordResetModel { Email = emailAddress });
            }
            catch (Clinical6ServerException ex)
            {
                return ex.ErrorResponse.Errors[0].Detail;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// Updates the user profile with error response.
        /// </summary>
        /// <returns>The user profile with error response.</returns>
        /// <param name="updatedProfile">Updated profile.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<string> UpdateUserProfileWithErrorResponse<T>(User updatedProfile)
        {
            MobileUserService.AuthToken = await _cacheService.GetAuthToken();
            try
            {
                updatedProfile.Id = (int)await _cacheService.GetUserId();

                var jsonRequest = string.Empty;

                jsonRequest = JsonConvert.SerializeObject(updatedProfile);

                var resp = await MobileUserService.Update(updatedProfile);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return string.Empty;
        }


        public async Task<T> UpdateUserProfileTimezoneWithErrorResponse<T>(User user, string reasonForChanges)
        {
            MobileUserService.AuthToken = await _cacheService.GetAuthToken();
            user.Id = (int)await _cacheService.GetUserId();

            var jsonRequest = string.Empty;

            var req = new RequestModelUser<UpdateTimeZone>
            {
                Data = new Models.Requests.RequestUserChangeDataModel<UpdateTimeZone>
                {
                    Type = "profiles",
                    Id = (int)user.Id,
                    Attributes = new UpdateTimeZone()
                    {

                        TimeZone = user.TimeZone
                    }
                },
                Meta = new ReasonForChanges()
                {
                    Value = new ReasonForChangesTimeZone()
                    {
                        Reason = reasonForChanges
                    }
                }
            };

            jsonRequest = JsonConvert.SerializeObject(req);

            Console.WriteLine(jsonRequest);


            //var resp = await MobileUserService.UpdateProfileWithResponse<T>(null, Json: jsonRequest, patiendId: updatedProfile.Id);
            var service = new JsonApiHttpService();
            return await service.Send<T>(string.Format(
                "/v3/mobile_users/{0}/profile", user.Id),
                null,
                new System.Net.Http.HttpMethod("PATCH"),
                req
            );
        }


        /// <summary>
        /// Updates the user avatar.
        /// </summary>
        /// <returns>The user avatar.</returns>
        /// <param name="photo">Photo.</param>
        public async Task<bool> UpdateUserAvatar(Models.Photo photo)
        {
            //var fileuploadRequest = await _apiModelFactory.SaveAvatarForUser(photo);

            //var uploadResponse = await _http.GetResult(fileuploadRequest);
            //return uploadResponse.IsResponseSuccessful;
            return true;
        }

        /// <summary>
        /// Gets the sites.
        /// </summary>
        /// <returns>The sites.</returns>
        public async Task GetSites()
        {
            try
            {
                var sites = await clinical6.GetChildren<List<Clinical6SDK.Models.Site>>(clinical6.User, "sites");

                var site = sites.FirstOrDefault();
                if (site != null)
                {
                    Preferences.Set(Settings.SiteId, site.Id.ToString());
                    Preferences.Set(Settings.SiteDisplayId, site.SiteId);
                    Preferences.Set(Settings.SiteName, site.Name);
                }

                //Application.Current.Properties[Settings.SiteId] = sites.FirstOrDefault()?.Id;
                //Application.Current.Properties[Settings.SiteDisplayId] = sites.FirstOrDefault()?.SiteId;
                //Application.Current.Properties[Settings.SiteName] = sites.FirstOrDefault()?.Name;

                //await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Preferences.Set(Settings.SiteName, Settings.NotAvailable);
                //Application.Current.Properties[Settings.SiteName] = Settings.NotAvailable;

                //await Application.Current.SavePropertiesAsync();

                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Transitions the user status.
        /// </summary>
        /// <returns>The user status.</returns>
        public async Task<bool> TransitionUserStatus()
        {
            //var transitionRequest = await _apiModelFactory.TransitionUserStatus();
            //var response = await _http.GetResult(transitionRequest);
            //return response.IsResponseSuccessful;
            return true;
        }

        /// <summary>
        /// Gets the registered email.
        /// </summary>
        /// <returns>The registered email.</returns>
        public Task<string> GetRegisteredEmail() => _cacheService.GetRegisteredEmail();

        /// <summary>
        /// Saves the registered email.
        /// </summary>
        /// <returns>The registered email.</returns>
        /// <param name="email">Email.</param>
        public Task SaveRegisteredEmail(string email) => _cacheService.SaveRegisteredEmail(email);

        // This should be handled in the API instead, and based on the user's email
        public Task<bool> GetHasPin() => _cacheService.GetHasPin();

        /// <summary>
        /// Saves the has pin.
        /// </summary>
        /// <returns>The has pin.</returns>
        /// <param name="hasPin">If set to <c>true</c> has pin.</param>
        public Task SaveHasPin(bool hasPin) => _cacheService.SaveHasPin(hasPin);

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns>The user identifier.</returns>
        public Task<int?> GetUserId() => _cacheService.GetUserId();

        /// <summary>
        /// Gets the terms of use accepted.
        /// </summary>
        /// <returns>The terms of use accepted.</returns>
        public Task<bool> GetTermsOfUseAccepted() => _cacheService.GetTermsOfUseAccepted();

        /// <summary>
        /// Saves the terms of use accepted.
        /// </summary>
        /// <returns>The terms of use accepted.</returns>
        /// <param name="accepted">If set to <c>true</c> accepted.</param>
        public Task SaveTermsOfUseAccepted(bool accepted) => _cacheService.SaveTermsOfUseAccepted(accepted);

        /// <summary>
        /// Hases the access token.
        /// </summary>
        /// <returns>The access token.</returns>
        public async Task<bool> HasAccessToken()
        {
            var token = await _cacheService.GetAuthToken();
            return token != null;
        }

        /// <summary>
        /// Ises the logged in.
        /// </summary>
        /// <returns>The logged in.</returns>
        public async Task<bool> IsLoggedIn()
        {
            return await HasAccessToken() && !string.IsNullOrEmpty(await GetRegisteredEmail());
        }

        /// <summary>
        /// Refreses the token.
        /// </summary>
        /// <returns>The token.</returns>
        public async Task<bool> refreshToken()
        {
            var email = await _cacheService.GetRegisteredEmail();
            var pin = await SecureStorage.GetAsync(Settings.AppPin);

            await _cacheService.ClearAll();
            await _cacheService.ClearAuthToken();

            await _cacheService.SaveRegisteredEmail(email);

            var responseServer = await ValidateEmailAndPinWitErrorResponse(email, pin);

            return responseServer.Success;
        }

        /// <summary>
        /// Gets the SSOP roviders async.
        /// </summary>
        /// <returns>The SSOP roviders async.</returns>
        public async Task<List<SsoOptions>> GetSSOProvidersAsync()
        {
            return await SSOOptionsService.GetSSOProvidersAsync();
        }
    }
}