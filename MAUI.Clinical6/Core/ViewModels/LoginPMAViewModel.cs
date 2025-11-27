using Clinical6SDK;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;
using Clinical6SDK.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Clinical6.Core;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.Helpers;
using Xamarin.Forms.Clinical6.Services;
using Xamarin.Forms.Clinical6.Views;

namespace Xamarin.Forms.Clinical6.ViewModels
{
    /// <summary>
    /// Login PMAV iew model.
    /// </summary>
    public class LoginPMAViewModel : BaseViewModel
    {
        // parameters
        public readonly IUserService _userService;
        public readonly IDialogService _dialogService;
        public readonly ICacheService _cacheService;
        public readonly IDeviceInfoService _deviceService;
        private readonly ILanguageService _languageService;
        private readonly IPolicyVerificationService _policyVerificationService;
        private readonly IBiometricsService _biometricsService;
        
        // constructed
        public Clinical6Instance _clinical6;
        public readonly IClinical6MobileUserService _mobileUserService;

        public ActionTracker LoginAction { get; }
        public ActionTracker ForgotPinAction { get; }
        public ActionTracker LoadSSOProvidersAction { get; }
        public ActionTracker BiometricsAction { get; }

        //public override BusyTracker PageBusy => LoginAction;

        public Regex ValidPinPattern = new Regex(@"^[a-zA-Z0-9@#$%*():;""""'/?!+=_-]{8,}$");

        private string _emailAddress;
        private string _password;

        public bool _emailError = false;
        public bool _passError = false;

        private string _emailErrorMessage = "LoginPMAViewModelEmailReqText".Localized();
        private List<SsoOptions> _optionsSSO;
        private int _invalidCredentials = 3;
        private int _tappedCount = 0;
        private bool canBiometricsAuthenticate;

        public LocalAuthenticationType biometricType;

        public ICommand OnEndpointChangeCommand => new Command(async () => await EndpointChangeTapped());

        public new ICommand OnPolicyCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ShowPoliciesAsync();
                });
            }
        }

        public new string AppVersion
        {
            get { return string.Format("VersionFormatText".Localized(), _deviceService.AppVersion); }
        }

        public Action CallBackDisplayEndpoints { get; set; }

        public LoginPMAViewModel(INavigationService navigationService, IUserService userService, IDialogService dialogService,
            ICacheService cacheService, ILanguageService languageService, IDeviceInfoService deviceService, IPolicyVerificationService policyVerificationService, IBiometricsService biometricsService) : base(navigationService)
        {
            _userService = userService;
            _dialogService = dialogService;
            _cacheService = cacheService;
            _deviceService = deviceService;
            _languageService = languageService;
            _policyVerificationService = policyVerificationService;
            _biometricsService = biometricsService;

            _clinical6 = new Clinical6Instance();
            _mobileUserService = new Clinical6MobileUserService();

            LoginAction = new ActionTracker(TryLogin, CanLogin2, null);

            ForgotPinAction = new ActionTracker(ForgotPinTask, onError: ActionTracker.IgnoreErrors);
            LoadSSOProvidersAction = new ActionTracker(LoadSSOProvidersTask, onError: ActionTracker.IgnoreErrors);
            BiometricsAction = new ActionTracker(BiometricsTask, onError: ActionTracker.IgnoreErrors);

            //biometricType = _biometricsService.CanAuthenticationWith();

            //Set Mapping for Environment Email Routing
            Settings.SetDictProperty(Settings.EnvironmentMappedLogins, new Dictionary<string, string>()
            {
                 {"hughesdanny@prahs.com","inv-hnas337-uat" },
                 {"pratestuser1+applesubmissiontest@gmail.com","inv-hnas337-uat" }
            });
        }

        public async Task InitializeAsync()
        {
            biometricType = _biometricsService.CanAuthenticationWith();
            // Raise property changed if using INotifyPropertyChanged
        }

        public virtual async Task RefreshScreen()
        {
            //canBiometricsAuthenticate = await MainService.BiometricsServiceInstance.CanAuthenticate(false);
            canBiometricsAuthenticate = await _biometricsService.CanAuthenticate(false);

            //biometricType = MainService.BiometricsServiceInstance.CanAuthenticationWith();
            biometricType = _biometricsService.CanAuthenticationWith();

            if (!canBiometricsAuthenticate)
            {
                biometricType = LocalAuthenticationType.None;
            }

            OnPropertyChanged(nameof(IsBiometricsIconAvailable));
            OnPropertyChanged(nameof(ImageBiometrics));
        }

        //public virtual async Task RefreshScreen()
        //{
        //    if (MainService?.BiometricsServiceInstance == null)
        //    {
        //        canBiometricsAuthenticate = false;
        //        biometricType = LocalAuthenticationType.None;
        //        return;
        //    }

        //    canBiometricsAuthenticate = await MainService.BiometricsServiceInstance.CanAuthenticate(false);

        //    biometricType = MainService.BiometricsServiceInstance.CanAuthenticationWith();

        //    if (!canBiometricsAuthenticate)
        //    {
        //        biometricType = LocalAuthenticationType.None;
        //    }

        //    OnPropertyChanged(nameof(IsBiometricsIconAvailable));
        //    OnPropertyChanged(nameof(ImageBiometrics));
        //}


        /// <summary>
        /// Inits the implementation.
        /// </summary>
        /// <returns>The implementation.</returns>
        protected override async Task InitImplementation()
        {
            EmailAddress = await _userService.GetRegisteredEmail();
            PassError = false;
            EmailError = false;

#if DEBUG
            Password = "Rakesh1!";
            EmailAddress = "rakesh.kumar+p565@deosi.com";

            //EmailAddress = "thenickpeppers+1@gmail.com";
            //Password = "Prahs2019!";
#endif

            await RefreshScreen();
            await LoadSSOProvidersAction.Run();
        }

        ////////////////////////////////////////////////// Email / Password BEGIN

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The email address.</value>
        public virtual string EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (value == _emailAddress)
                    return;

                _emailAddress = value;
                CheckEmailError();
                LoginAction.Command.ChangeCanExecute();
                OnPropertyChanged(nameof(EmailAddress));
            }
        }

        public virtual string Password
        {
            get => _password;
            set
            {
                if (value == _password)
                    return;

                _password = value;
                CheckPasswordError();
                LoginAction.Command.ChangeCanExecute();
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(Opacity));
            }
        }


        public virtual string ImageBiometrics
        {
            get
            {
                var isBiometricsActive = false;

                if (Settings.GetBoleanProperty(Settings.IsBiometricsActive))
                {
                    isBiometricsActive = Settings.GetBoleanProperty(Settings.IsBiometricsActive);
                }

                if (MainService.Instance.IsBiometricsEnabled && isBiometricsActive && biometricType != LocalAuthenticationType.None)
                {
                    return biometricType == LocalAuthenticationType.BiometryFaceID ? "bio_face_id_entry" : biometricType == LocalAuthenticationType.BiometryTouchID_Or_Fingerprint ? "bio_fingerprint_entry_android" : "bio_fingerprint_entry_ios";
                }

                return string.Empty;
            }
        }

        public virtual bool IsBiometricsIconAvailable
        {
            get
            {


                var isBiometricsActive = false;

                if (Settings.GetBoleanProperty(Settings.IsBiometricsActive))
                {
                    isBiometricsActive = Settings.GetBoleanProperty(Settings.IsBiometricsActive);
                }


                if (canBiometricsAuthenticate && isBiometricsActive)
                {
                    return true;
                }

                return false;
            }
        }

        public string Opacity
        {
            get
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    return "1.0";
                }

                return "0.5";
            }
        }

        public virtual string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set
            {
                if (value == _emailErrorMessage) return;

                _emailErrorMessage = value;

                OnPropertyChanged(nameof(EmailErrorMessage));
            }
        }

        public virtual bool EmailError
        {
            get => _emailError;
            set
            {
                _emailError = value;
                OnPropertyChanged(nameof(EmailError));
            }
        }

        public virtual bool PassError
        {
            get => _passError;
            set
            {
                _passError = value;
                OnPropertyChanged(nameof(PassError));
            }
        }

        public virtual string EmailErrorMatBorderColor
        {
            get
            {
                var key = !EmailError ? "NormalBorderColor" : "ErrorBorderColor";
                return AppHelpers.GetResource(key).ToString();
            }
        }

        public virtual string PasWordErrorMatBorderColor
        {
            get
            {
                var key = !PassError ? "NormalBorderColor" : "ErrorBorderColor";
                return AppHelpers.GetResource(key).ToString();
            }
        }

        public virtual string CanLoginErrorText
        {
            get => CanLogin ? "SignInReadyHelpText".Localized() : "SignInErrorHelpText".Localized();
        }

        public virtual bool CanLogin
        {
            get => !(EmailError || PassError || string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(Password));
        }

        public virtual bool CanLogin2()
        {
            return CanLogin;
        }

        // This gets called with every change to email field.
        public void CheckEmailError()
        {
            if (!VerifyEmail(EmailAddress))
            {
                EmailError = true;
                EmailErrorMessage = string.IsNullOrEmpty(EmailAddress) ? "LoginPMAViewModelEmailReqText".Localized() : "LoginPMAViewModelInvalidEmailText".Localized();
            }
            else
            {
                EmailError = false;
                EmailErrorMessage = "";
            }
            OnPropertyChanged(nameof(EmailErrorMatBorderColor));
            OnPropertyChanged(nameof(CanLogin));
            OnPropertyChanged(nameof(CanLoginErrorText));
        }

        // This gets called with every change to password field.
        // NOTE: More stringent password requirements could be added here.
        public void CheckPasswordError()
        {
            if (string.IsNullOrEmpty(Password))
            {
                PassError = true;
            }
            else
            {
                PassError = false;
            }

            OnPropertyChanged(nameof(PasWordErrorMatBorderColor));
            OnPropertyChanged(nameof(CanLogin));
            OnPropertyChanged(nameof(CanLoginErrorText));
        }

        /// <summary>
        /// Verifies the email.
        /// </summary>
        /// <returns><c>true</c>, if email was verifyed, <c>false</c> otherwise.</returns>
        /// <param name="email">Email.</param>
        //public bool VerifyEmail(string email)
        //{
        //    var lowerEmail = email.ToLower();
        //    Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        //    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
        //    Match match = regex.Match(lowerEmail);

        //    return match.Success;
        //}

        public bool VerifyEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var lowerEmail = email.ToLowerInvariant();
            Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                                    RegexOptions.IgnoreCase);

            return regex.IsMatch(lowerEmail);
        }


        ////////////////////////////////////////////////// Email / Password END


        public List<SsoOptions> OptionsSSO
        {
            get => _optionsSSO;
            set
            {
                if (value == _optionsSSO)
                    return;

                _optionsSSO = value;
                OnPropertyChanged(nameof(OptionsSSO));
            }
        }

        /// <summary>
        /// Forgots the pin task.
        /// </summary>
        /// <returns>The pin task.</returns>
        private async Task ForgotPinTask()
        {
            //await Navigation.Push<ForgotPinEnterEmailViewModel>();
        }

        private async Task BiometricsTask()
        {
            var isBiometricsActive = false;

            if (Settings.GetBoleanProperty(Settings.IsBiometricsActive))
            {
                isBiometricsActive = Settings.GetBoleanProperty(Settings.IsBiometricsActive);
            }

            if (MainService.Instance.IsBiometricsEnabled && isBiometricsActive)
            {
                //var canAuthenticate = await MainService.BiometricsServiceInstance.CanAuthenticate(false);
                var canAuthenticate = await _biometricsService.CanAuthenticate(false);

                if (canAuthenticate)
                {
                    _biometricsService.AuthenticateUser(OnAuthComplete);
                    //MainService.BiometricsServiceInstance.AuthenticateUser(OnAuthComplete);
                }
            }
        }

        private async void OnAuthComplete(bool sucess, string errorMessage)
        {
            Settings.SetBoleanProperty(Settings.AutomaticLogin, sucess);

            if (sucess)
            {
                try
                {
                    EmailAddress = await SecureStorage.GetAsync(Settings.BiometricsEmail);
                    Password = await SecureStorage.GetAsync(Settings.BiometricsPin);

                    if (!string.IsNullOrEmpty(EmailAddress) && !string.IsNullOrEmpty(Password))
                    {
                        await LoginAction.Run();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "InvalidCredentialsBody".Localized();
            }

            return;
        }

        /// <summary>
        /// Loads the SSOP roviders task.
        /// </summary>
        /// <returns>The SSOP roviders task.</returns>
        private async Task LoadSSOProvidersTask()
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            var SsoOptionsList = await _userService.GetSSOProvidersAsync();

            var optionsSSO = SsoOptionsList.Where(c => c.UserType == "MobileUser");

            if (optionsSSO != null)
            {
                OptionsSSO = optionsSSO.OrderBy(c => c.Name).ToList();
            }

            Console.WriteLine(SsoOptionsList.ToString());
        }

        /// <summary>
        /// Checks the email.
        /// </summary>
        /// <returns>The email.</returns>
        public virtual async Task TryLogin()
        {
            try
            {
                var email = EmailAddress;

                switch (EmailAddress)
                {
                    case "userhasaccess@parallel6.com":
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        Settings.SetBoleanProperty(Settings.IsLoginIn, true);
                        Settings.SetProperty(Settings.LoginDate, DateTime.UtcNow.Ticks.ToString());
                        await Navigation.StartMainApp(MainService.HomePage.Value);
                        return;
                    case "usermustreset@parallel6.com":
                        //await Navigation.Push<PasswordExpiredViewModel>();
                        return;
                    case "userlocked@parallel6.com":
                        //await Navigation.Push<LockedViewModel>();
                        return;
                    case "userpasswordinvalid@parallel6.com":
                        {
                            _invalidCredentials--;

                            if (_invalidCredentials == 0)
                            {
                                //await Navigation.Push<LockedViewModel>();
                                return;
                            }

                            await _dialogService.ShowAlert("LoginPMAViewModelAlertHeaderText".Localized(),
                                                           string.Format("{0} {1}", "LoginPMAViewModelRemainingAtemptText".Localized(),
                                                           _invalidCredentials));
                        }
                        return;
                    case "useraccountdisabled@parallel6.com":
                        //await Navigation.Push<DisabledAccountViewModel>();
                        return;
                    case "useraccountwithdrawn@parallel6.com":
                        //await Navigation.Push<WithdrawnAccountViewModel>();
                        return;
                }

                var access = Connectivity.NetworkAccess;
                if (access != NetworkAccess.Internet && access != NetworkAccess.ConstrainedInternet)
                {
                    //await Navigation.Push<NoInternetViewModel>();
                    //await Navigation.Push<AppInitViewModel>();
                    return;
                }

                // Apps can provide a dictionary of environment mapped logins.  These are email addresses that, when used for login,
                // will force the app to use a specific environment.  This is useful for providing "dummy" logins to apps that normally
                // point to production, but that aren't real users and should not trigger any data collection on the prod environment.
                Dictionary<string, string> environmentMappedLogins = Settings.GetDictProperty(Settings.EnvironmentMappedLogins);

                if (environmentMappedLogins != null && environmentMappedLogins.ContainsKey(email))
                {
                    string mappedEnv = environmentMappedLogins[email];
                    string apiRoot = $"https://{mappedEnv}.clinical6.com/";
                    await UpdateEndPointAsync(apiRoot);
                }

                var status = await _userService.GetRegistrationStatus(email);
                //status = RegistrationStatus.Existing;

                //TODO this should be removed after BE guy return
                if (status == RegistrationStatus.New)
                {
                    //if (Application.Current.Properties.ContainsKey(Settings.PinResetEmail))
                    if (!string.IsNullOrEmpty(Preferences.Get(Settings.PinResetEmail, null)))
                    {
                        //var usermail = Application.Current.Properties[Settings.PinResetEmail].ToString();
                        var usermail = Preferences.Get(Settings.PinResetEmail, string.Empty);

                        if (email == usermail)
                        {
                            status = RegistrationStatus.Existing;
                        }
                    }
                }

                switch (status)
                {
                    case RegistrationStatus.Withdrawn:
                        //await Navigation.Push<WithdrawnAccountViewModel>();
                        return;
                    case RegistrationStatus.Disabled:
                        //await Navigation.Push<DisabledAccountViewModel>();
                        return;
                    case RegistrationStatus.Invalid:
                        await _dialogService.ShowAlert("EmailEntryInvalidEmailTitle".Localized(), "EmailEntryInvalidEmailMessage".Localized());
                        return;
                    case RegistrationStatus.New:
                        await _dialogService.ShowAlert("InvalidCredentialsTitle".Localized(), "InvalidCredentialsBody".Localized());
                        return;
                    case RegistrationStatus.Existing:
                    case RegistrationStatus.Consent:
                        await _userService.SaveRegisteredEmail(email);
                        await _userService.SaveTermsOfUseAccepted(true);
                        await _userService.SaveHasPin(true);
                        await Login();
                        return;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"TryLogin Exception: {exc}");
                //await Navigation.StartLogin<AppInitViewModel>();
            }
        }

        /// <summary>
        /// Login this instance.
        /// </summary>
        /// <returns>The login.</returns>
        public virtual async Task Login()
        {
            var email = EmailAddress;
            var pin = Password;

            var responseServer = await _userService.ValidateEmailAndPinWitErrorResponse(email, pin);

            if (responseServer.Success)
            {
                try
                {
                    await SaveEmailToSecureStorage(email);
                    await SecureStorage.SetAsync(Settings.AppPin, pin);
                    await SecureStorage.SetAsync(Settings.BiometricsEmail, EmailAddress);
                    await SecureStorage.SetAsync(Settings.BiometricsPin, pin);
                    await SecureStorage.SetAsync(Settings.UserPin, pin);
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"Error saving to secure storage: {exc}");
                }

                var result = await GetUserProfile();

                try
                {
                    _clinical6.User = await _mobileUserService.GetProfile(_clinical6.User);
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"_mobileUserService.GetProfile Exception: {exc}");
                }

                if (result != null)
                {
                    var consentStatus = string.IsNullOrEmpty(Settings.GetProperty(Settings.ConsetStatus)) ? "consent_not_complete" : Settings.GetProperty(Settings.ConsetStatus);
                    var counterSigned = consentStatus.ToLower() == "consent_complete" ? true : false;

                    Preferences.Set(Settings.SiteMember, _clinical6.User?.SiteMember?.Id ?? 0);
                    //Application.Current.Properties[Settings.SiteMember] = _clinical6.User?.SiteMember?.Id;

                    if (!MainService.Instance.IsCheckCosentEnabled)
                    {
                        counterSigned = true;
                    }

                    var mobileUser = result.User;
                    await UpdateLanguage(mobileUser?.Profile);

                    var isPolicyDocumentsValid = true;
#if DEBUG
                    isPolicyDocumentsValid = await _policyVerificationService.VerifyPolicyDocuments(mobileUser);
#else
                    isPolicyDocumentsValid = await _policyVerificationService.VerifyPolicyDocuments(mobileUser);
#endif

                    if (!isPolicyDocumentsValid)
                    {
                        //await Navigation.Push<PoliciesViewModel>();

                        UpdateDevice();
                    }
                    else if (!counterSigned)
                    {
                        // This is no longer used.
                        //await Navigation.Push<ConsentStatusViewModel, string>(consentStatus);
                    }
                    else
                    {
                        await UpdateTimeZone(result.User);

                        UpdateDevice();

                        Settings.SetProperty(Settings.SleepTime, DateTime.UtcNow.Ticks.ToString());

                        var master = MainService.HomePage.Value as DashboardMasterPage;
                        if (master != null)
                        {
                            master.IsPresented = false;
                        }

                        //if (Device.RuntimePlatform == Device.Android)
                        if (DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            Settings.SetBoleanProperty(Settings.AutomaticLogin, true);
                        }

                        var isBiometricsActive = false;

                        if (Settings.GetBoleanProperty(Settings.IsBiometricsActive))
                        {
                            isBiometricsActive = Settings.GetBoleanProperty(Settings.IsBiometricsActive);
                        }

                        //var canAuthenticate = await MainService.BiometricsServiceInstance.CanAuthenticate(false);
                        var canAuthenticate = await _biometricsService.CanAuthenticate(false);

                        var isBiometricsAlreadySetup = false;
                        if (Settings.GetBoleanProperty(Settings.IsBiometricsAlreadySetup))
                        {
                            isBiometricsAlreadySetup = Settings.GetBoleanProperty(Settings.IsBiometricsAlreadySetup);
                        }

                        Debug.WriteLine("MainService.Instance.IsBiometricsEnabled:>>" + MainService.Instance.IsBiometricsEnabled);
                        Debug.WriteLine("!isBiometricsActive:>>" + !isBiometricsActive);
                        Debug.WriteLine("canAuthenticate:>>" + canAuthenticate);
                        Debug.WriteLine("!isBiometricsAlreadySetup:>>" + !isBiometricsAlreadySetup);

                        if (MainService.Instance.IsBiometricsEnabled && !isBiometricsActive && canAuthenticate && !isBiometricsAlreadySetup)
                        {
                            //var initSettings = new BiometricsSettingsInitValues();

                            //initSettings.CallBackOnNotNow += async delegate
                            //{
                            //    initSettings.CallBackOnNotNow = null;
                            //    initSettings.CallBackBiometricsSucess = null;
                            //    await Navigation.StartMainApp(MainService.HomePage.Value);
                            //};

                            //initSettings.CallBackBiometricsSucess += async delegate (bool sucess)
                            //{
                            //    await Navigation.StartMainApp(MainService.HomePage.Value);
                            //    initSettings.CallBackOnNotNow = null;
                            //    initSettings.CallBackBiometricsSucess = null;
                            //};

                            //await Navigation.PushModal<BiometricsSettingsViewModel, BiometricsSettingsInitValues>(initSettings);
                        }
                        else
                        {
                            await Navigation.StartMainApp(MainService.HomePage.Value);
                        }
                    }
                }
                else
                {
                    UpdateDevice();

                    var master = MainService.HomePage.Value as DashboardMasterPage;

                    master.IsPresented = false;

                    //if (Device.RuntimePlatform == Device.Android)
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        Settings.SetBoleanProperty(Settings.AutomaticLogin, true);
                    }

                    var isBiometricsActive = false;

                    if (Settings.GetBoleanProperty(Settings.IsBiometricsActive))
                    {
                        isBiometricsActive = Settings.GetBoleanProperty(Settings.IsBiometricsActive);
                    }

                    var isBiometricsAlreadySetup = false;
                    if (Settings.GetBoleanProperty(Settings.IsBiometricsAlreadySetup))
                    {
                        isBiometricsAlreadySetup = Settings.GetBoleanProperty(Settings.IsBiometricsAlreadySetup);
                    }

                    if (MainService.Instance.IsBiometricsEnabled && !isBiometricsActive && !isBiometricsAlreadySetup)
                    {
                        //var initSettings = new BiometricsSettingsInitValues();

                        //initSettings.CallBackOnNotNow += async delegate
                        //{
                        //    initSettings.CallBackOnNotNow = null;
                        //    initSettings.CallBackBiometricsSucess = null;
                        //    await Navigation.StartMainApp(MainService.HomePage.Value);
                        //};

                        //initSettings.CallBackBiometricsSucess += async delegate (bool sucess)
                        //{
                        //    await Navigation.StartMainApp(MainService.HomePage.Value);
                        //    initSettings.CallBackOnNotNow = null;
                        //    initSettings.CallBackBiometricsSucess = null;
                        //};

                        //await Navigation.PushModal<BiometricsSettingsViewModel, BiometricsSettingsInitValues>(initSettings);
                    }
                    else
                    {
                        await Navigation.StartMainApp(MainService.HomePage.Value);
                    }
                }
            }
            else
            {
                if (checkMessageIfUserMustResetPassword(responseServer.ErrorMessage))
                {
                    //await Navigation.Push<PasswordExpiredViewModel>();
                }
                else if (checkMessageIfPasswordInvalid(responseServer.ErrorMessage))
                {
                    _invalidCredentials--;

                    if (_invalidCredentials == 0)
                    {
                        //await Navigation.Push<LockedViewModel>();
                        return;
                    }

                    await _dialogService.ShowAlert("LoginPMAViewModelAlertHeaderText".Localized(),
                                                   "LoginPMAViewModelRemainingAtemptTextBody".Localized());
                }
                else if (checkMessageIfAccountIsLocked(responseServer.ErrorMessage))
                {
                    //await Navigation.Push<LockedViewModel>();
                }
                else if (checkMessageIfAccountIsDisabled(responseServer.ErrorMessage))
                {
                    //await Navigation.Push<DisabledAccountViewModel>();
                }
                else if (checkMessageIfUserWithdrawnFromStudy(responseServer.ErrorMessage))
                {
                    //await Navigation.Push<WithdrawnAccountViewModel>();
                }
                else
                {
                    await _dialogService.ShowAlert("InvalidCredentials".Localized(), responseServer.ErrorMessage);
                }
            }
        }

        public async Task UpdateTimeZone(User profile)
        {
            try
            {
                if (profile != null)
                {
                    var userProfile = profile;

                    //var timeZoneId = DependencyService.Get<ITimeZoneManager>().GetTimeZoneId();//Foundation.NSTimeZone.LocalTimeZone.Name;
                    //var currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                    //if (currentTimeZone == null)
                    //{
                    //    currentTimeZone = TimeZoneInfo.GetSystemTimeZones().Where(c => c.BaseUtcOffset == TimeZoneInfo.Local.BaseUtcOffset
                    //                      && c.StandardName == TimeZoneInfo.Local.StandardName
                    //                      && c.DaylightName == TimeZoneInfo.Local.DaylightName).FirstOrDefault();
                    //}

                    //userProfile.TimeZone = currentTimeZone.Id;

                    try
                    {
                        await _userService.UpdateUserProfileTimezoneWithErrorResponse<User>(userProfile, "Time Zone Updated");
                        Console.WriteLine("<<<< Time Zone Updated! >>>>");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("<<<< " + ex.Message + " >>>>");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        async Task UpdateLanguage(Profile profile)
        {
            try
            {
                if (profile != null)
                {
                    var currentLanguageIso = (await _cacheService.GetCurrentLanguage())?.Iso.ToLower();
                    if (profile?.Language?.Iso.ToLower() != currentLanguageIso)
                    {
                        await _cacheService.SaveCurrentLanguage(profile?.Language);
                        await _languageService.GetTranslations(profile?.Language);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Checks the message if password invalid.
        /// </summary>
        /// <returns><c>true</c>, if message if password invalid was checked, <c>false</c> otherwise.</returns>
        /// <param name="errorMessage">Error message.</param>
        private bool checkMessageIfPasswordInvalid(string errorMessage)
        {
            if (errorMessage.IndexOf("Please review your credentials", StringComparison.Ordinal) >= 0)
            {
                return true;
            }
            else if (errorMessage.IndexOf("You have one more attempt before your account is locked", StringComparison.Ordinal) >= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks the message if account is locked.
        /// </summary>
        /// <returns><c>true</c>, if message if account is locked was checked, <c>false</c> otherwise.</returns>
        /// <param name="errorMessage">Error message.</param>
        private bool checkMessageIfAccountIsLocked(string errorMessage)
        {
            if (errorMessage.IndexOf("locked", StringComparison.Ordinal) >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Checks the message if account is disabled.
        /// </summary>
        /// <returns><c>true</c>, if message if account is disabled was checked, <c>false</c> otherwise.</returns>
        /// <param name="errorMessage">Error message.</param>
        private bool checkMessageIfAccountIsDisabled(string errorMessage)
        {
            if (errorMessage.IndexOf("disabled", StringComparison.Ordinal) >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Checks the message if user must reset password.
        /// </summary>
        /// <returns><c>true</c>, if message if user must reset password was checked, <c>false</c> otherwise.</returns>
        /// <param name="errorMessage">Error message.</param>
        private bool checkMessageIfUserMustResetPassword(string errorMessage)
        {
            if (errorMessage.IndexOf("Your password expired", StringComparison.Ordinal) >= 0)
                return true;

            if (errorMessage.IndexOf("expired", StringComparison.Ordinal) >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Checks the user been withdrawn from study.
        /// </summary>
        /// <returns></returns>
        /// <param name="errorMessage">Error message.</param>
        private bool checkMessageIfUserWithdrawnFromStudy(string errorMessage)
        {
            if (errorMessage.IndexOf("You have been withdrawn from the study", StringComparison.Ordinal) >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Updates the device.
        /// </summary>
        public static async void UpdateDevice()
        {
            try
            {
                var pushDevicetoken = string.IsNullOrEmpty(Settings.GetProperty(Settings.PushDevicetoken)) ? String.Empty : Settings.GetProperty(Settings.PushDevicetoken);

                var result = await UpdateDeviceTokenAsync(pushDevicetoken);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Updates the device token async.
        /// </summary>
        /// <returns>The device token async.</returns>
        /// <param name="deviceTokenString">Device token string.</param>
        private static async Task<object> UpdateDeviceTokenAsync(string deviceTokenString)
        {
            if (string.IsNullOrEmpty(deviceTokenString))
                return null;

            string technologyString = null;

            //if (Device.RuntimePlatform == Device.Android)
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                technologyString = "android";
            }
            //else if (Device.RuntimePlatform == Device.iOS)
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                technologyString = "ios";
            }

            var service = new JsonApiHttpService
            {
                BaseUrl = EnvironmentConfig.ApiRoot,
                AuthToken = await new CacheService().GetAuthToken()
            };

            var device = new Clinical6SDK.Helpers.Device
            {
                Id = await new CacheService().GetDeviceId(),
                MobileApplicationKey = EnvironmentConfig.MobileApplicationKey,
                Technology = technologyString,
                AppVersion = VersionTracking.CurrentVersion,
                PushId = deviceTokenString
            };

            var result = await service.Update<Clinical6SDK.Helpers.Device>(device);
            return result;
        }

        /// <summary>
        /// Ons the updatedterms complete.
        /// </summary>
        /// <returns>The updatedterms complete.</returns>
        protected async Task OnUpdatedtermsComplete()
        {
            _clinical6.User = await _mobileUserService.GetProfile(_clinical6.User);

            var dateTime = DateTime.UtcNow;
            _clinical6.User.PrivacyPolicyAcceptedAt = dateTime;
            _clinical6.User.TermsOfUseAcceptedAt = dateTime;
            _clinical6.User.AntiSpamPolicyAcceptedAt = dateTime;
            _clinical6.User.TimeZone = TimeZoneInfo.Local.StandardName;

            await _clinical6.User.Save();
            await _userService.SaveTermsOfUseAccepted(true);
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <returns>The user profile.</returns>
        private async Task<Profile> GetUserProfile()
        {
            try
            {
                _clinical6.User = await _clinical6.GetProfile();
                return _clinical6.User?.Profile;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"GetUserProfile Exception: {exc}");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ShowPoliciesAsync()
        {
            //var Params = new AboutViewModel.InitValues();
            //Params.PolicyType = AboutViewModel.PolicyEnum.All;
            //await Navigation.PushModal<AboutViewModel, AboutViewModel.InitValues>(Params);
        }

        public List<string> _endpoints;

        public List<string> Endpoints
        {
            get { return _endpoints; }
            set
            {
                _endpoints = value;
                OnPropertyChanged(nameof(Endpoints));
            }
        }

        public string _selectedEndpoint;

        public string SelectedEndpoint
        {
            get { return _selectedEndpoint; }
            set
            {
                _selectedEndpoint = value;
                OnPropertyChanged(nameof(SelectedEndpoint));
                ResetEndPointAsyn();
            }
        }

        private async Task ResetEndPointAsyn()
        {

            var urlEndpoint = EnvironmentConfig.ApiRoot.Replace("-development", "-{0}")
                                .Replace("-qa", "-{0}").Replace("-training", "-{0}")
                                .Replace("-uat", "-{0}");

            //We use the same aproch of VSS to sent the End Point
            await UpdateEndPointAsync(string.Format(urlEndpoint, SelectedEndpoint.ToLower()));

            //await Navigation.StartLogin<AppInitViewModel>();
        }

        private async Task UpdateEndPointAsync(string urlEndpoint)
        {
            Settings.SetProperty(Settings.ApiRoot, urlEndpoint);

            List<AppSettings> settings = new List<AppSettings>();

            EnvironmentConfig.ApiRoot = urlEndpoint;
            ClientSingleton.Instance.BaseUrl = urlEndpoint;

            settings.Add(new AppSettings() { Id = nameof(ClientSingleton.Instance.BaseUrl), SettingsAttributes = new SettingsAttributes() { Value = ClientSingleton.Instance.BaseUrl } });
            settings.Add(new AppSettings() { Id = nameof(EnvironmentConfig.MobileApplicationKey), SettingsAttributes = new SettingsAttributes() { Value = EnvironmentConfig.MobileApplicationKey } });

            await _cacheService.SaveAPIStudySettings(settings);
            ClientSingleton.Instance.VerificationCodeUrl = urlEndpoint;
        }

        private async Task EndpointChangeTapped()
        {
            //If url is on production we cant change the url
            if (EnvironmentConfig.ApiRoot.IndexOf("development") > 0
                || EnvironmentConfig.ApiRoot.IndexOf("qa") > 0
                || EnvironmentConfig.ApiRoot.IndexOf("training") > 0
                || EnvironmentConfig.ApiRoot.IndexOf("uat") > 0)
            {
                _endpoints = new List<string>();

                var urlEndpoint = EnvironmentConfig.ApiRoot.Replace("-development", string.Empty).Replace("-qa", string.Empty).Replace("-training", string.Empty).Replace("-uat", string.Empty);

                _endpoints.Add("Development");
                _endpoints.Add("QA");
                _endpoints.Add("Training");
                _endpoints.Add("UAT");

                OnPropertyChanged(nameof(Endpoints));

                CallBackDisplayEndpoints?.Invoke();
            }
        }

        private async Task SaveEmailToSecureStorage(string email)
        {
            try
            {
                await SecureStorage.SetAsync(Settings.NewSavedEmail, email);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Secure storage set exception: {exc}");
            }
        }
    }
}
