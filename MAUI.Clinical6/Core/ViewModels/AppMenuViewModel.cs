using Clinical6SDK;
using Clinical6SDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Clinical6.Core;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.Helpers;

namespace Xamarin.Forms.Clinical6.ViewModels
{
    public class AppMenuViewModel : BaseViewModel
    {
        private AppMenu _selectedItem;
        private bool _isBiometricsActive;
        private LocalAuthenticationType _biometricType;

        private readonly ICacheService _cacheService;
        private readonly IDeviceInfoService _deviceService;
        private readonly IDialogService _dialogService;
        //private readonly IAmazonPaymentService _amazonPaymentService;

        public IClinical6Instance Clinical6 { get; private set; }

        public ActionTracker LoadDataAction { get; }
        public ActionTracker LogoutAction { get; }
        public ActionTracker AboutAction { get; }
        public ActionTracker PolicyAction { get; }
        public ActionTracker BiometricsAction { get; }
        public ActionTracker RedeemPaymentAction { get; }

        public Action RefreshElements { get; set; }

        bool _isMenuPresented;
        public bool IsMenuPresented
        {
            get { return _isMenuPresented; }
            set
            {
                _isMenuPresented = value;
                OnPropertyChanged(nameof(IsMenuPresented));
            }
        }

        public async void RefreshScreen()
        {
            //var canAuthenticate = await MainService.BiometricsServiceInstance.CanAuthenticate(false);

            //if (!canAuthenticate)
            //{
            //    _isBiometricsActive = false;
            //    _biometricType = LocalAuthenticationType.None;
            //}
            //else
            //{
            //    _biometricType = MainService.BiometricsServiceInstance.CanAuthenticationWith();
            //    _isBiometricsActive = Settings.GetBoleanProperty(Settings.IsBiometricsActive);
            //}

            OnPropertyChanged(nameof(IsBiometricsActive));
            OnPropertyChanged(nameof(BiometricsTitle));
            OnPropertyChanged(nameof(BiometricsImage));
            OnPropertyChanged(nameof(StudyLogo));
            OnPropertyChanged(nameof(IsBiometricsAvailable));

            RefreshElements?.Invoke();
        }

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

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await LoadDataAction.Run();
                });
            }
        }

        public ICommand RedeemPaymentsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //await Navigation.NavigatePage<AmazonRedeemPaymentViewModel, AmazonPayment>(AmazonPayment, fromMenu: true);
                });
            }
        }

        public AppMenuViewModel(INavigationService navigationService, ICacheService cacheService,
            IDeviceInfoService deviceService, IClinical6Instance clinical6Instance, IDialogService dialogService) : base(navigationService)
        {
            _cacheService = cacheService;
            _deviceService = deviceService;
            Clinical6 = clinical6Instance;
            _dialogService = dialogService;

            LogoutAction = new ActionTracker(LogoutTask, null, ActionTracker.IgnoreErrors);
            LoadDataAction = new ActionTracker(LoadDataActionTask, null, ActionTracker.IgnoreErrors);
            AboutAction = new ActionTracker(AboutTask, null, ActionTracker.IgnoreErrors);
            BiometricsAction = new ActionTracker(BiometricsTask, null, ActionTracker.IgnoreErrors);

            IsBiometricsActive = Settings.GetBoleanProperty(Settings.IsBiometricsActive);
            //_amazonPaymentService = AppContainer.Current.Resolve<IAmazonPaymentService>();
        }

        public string Version
        {
            get
            {
                return string.Format("VersionFormatText".Localized(), _deviceService.AppVersion);
            }
        }

        public AppMenu SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == null)
                {
                    _selectedItem = null;
                }

                if (_selectedItem != value && value != null)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));

                    ViewResourcesTask(_selectedItem);
                }
            }
        }

        public bool IsAmazonPaymentAvailable
        {
            get { return MainService.Instance.IsAmazonPaymentsEnabled; }
        }

        private AmazonPayment _amazonPayment;
        public AmazonPayment AmazonPayment
        {
            get { return _amazonPayment; }
            set
            {
                _amazonPayment = value;
                OnPropertyChanged(nameof(AmazonPayment));
            }
        }

        public bool IsBiometricsAvailable
        {
            get
            {
                if (_biometricType == LocalAuthenticationType.None)
                    return false;

                return MainService.Instance.IsBiometricsEnabled;
            }
        }

        public bool IsBiometricsActive
        {
            get { return _isBiometricsActive; }
            set
            {
                if (_isBiometricsActive != value)
                {
                    _isBiometricsActive = value;
                    Settings.SetBoleanProperty(Settings.IsBiometricsActive, value);
                    OnPropertyChanged(nameof(IsBiometricsActive));

                    var isBiometricsAlreadySetup = false;
                    if (Settings.GetBoleanProperty(Settings.IsBiometricsAlreadySetup))
                    {
                        isBiometricsAlreadySetup = Settings.GetBoleanProperty(Settings.IsBiometricsAlreadySetup);
                    }

                    if (!isBiometricsAlreadySetup && _isBiometricsActive)
                    {
                        _ = BiometricsAction.Run();
                    }
                }
            }
        }

        public string BiometricsImage
        {
            get
            {
                switch (_biometricType)
                {
                    case LocalAuthenticationType.BiometryFaceID:
                        return "bio_face_id";
                    case LocalAuthenticationType.BiometryTouchID:
                    case LocalAuthenticationType.Pin:
                    default:
                        return "bio_fingerprint_ios";
                }
            }
        }

        private ObservableCollection<AppMenu> _menus;

        public ObservableCollection<AppMenu> Menus
        {
            get { return _menus; }
            set
            {
                if (_menus != value)
                {
                    _menus = value;
                    OnPropertyChanged(nameof(Menus));
                }
            }
        }

        private List<AppMenu> _appMenus;

        public List<AppMenu> AppMenus
        {
            get { return _appMenus; }
            set
            {
                if (_appMenus != value)
                {
                    _appMenus = value;
                    OnPropertyChanged(nameof(AppMenus));
                }
            }
        }

        public async Task LoadDataActionTask()
        {
            try
            {
                if (MainService.Instance.EnableCache && Menus?.Count > 0)
                {
                    return;
                }

                await GetAmazonPaymentsTask();

                Contract.Ensures(Contract.Result<Task>() != null);
                AppMenus = await Clinical6.Get<List<AppMenu>>();

                if (AppMenus?.Count <= 0)
                {
                    await Navigation.Pop();
                }

                if (AppMenus == null)
                {
                    //TODO check status instead...
                    await LogoutTask();
                    return;
                }

#if DEBUG
            var videoConsultation = new AppMenu();

            videoConsultation.Title = "Video Consults";
            videoConsultation.Parent = null;
            videoConsultation.Action = "videoconsults";
            videoConsultation.Image = new ImageResources() { Url = "gui_video_consult" };
            videoConsultation.Enabled = true;

            Menus = new ObservableCollection<AppMenu>(AppMenus.Where(c => c.Parent == null && c.Enabled).OrderBy(c => c.Position));

            // Menus.Insert(0,videoConsultation);

            var ProfileMenu = new AppMenu();

            ProfileMenu.Title = "profile";
            ProfileMenu.Parent = null;
            ProfileMenu.Action = "profile";
            ProfileMenu.Image = new ImageResources() { Url = "profile" };
            ProfileMenu.Enabled = true;

            // Menus = new ObservableCollection<AppMenu>(AppMenus.Where(c => c.Parent == null && c.Enabled).OrderBy(c => c.Position));

            // Menus.Insert(0, ProfileMenu);
#else
                Menus = new ObservableCollection<AppMenu>(AppMenus.Where(c => c.Parent == null && c.Enabled).OrderBy(c => c.Position));
#endif
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await LogoutTask();
                return;
            }

        }

        public void ViewResourcesTask(AppMenu currentMenu)
        {
            var children = AppMenus.Where(c => c.Parent == currentMenu).OrderBy(c => c.Position).ToList();

            if (children?.Count > 0)
            {
                this.NavigateSubMenu(currentMenu, children);
                return;
            }

            this.Navigate(currentMenu);
        }

        public async Task LogoutTask()
        {
            var isBiometricsActive = false;

            if (Settings.GetBoleanProperty(Settings.IsBiometricsActive) && MainService.Instance.IsBiometricsEnabled)
            {
                try
                {
                    var password = await SecureStorage.GetAsync(Settings.UserPin);
                    var email = await SecureStorage.GetAsync(Settings.NewSavedEmail);

                    if (!string.IsNullOrEmpty(email))
                    {
                        await SecureStorage.SetAsync(Settings.BiometricsEmail, email);
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        await SecureStorage.SetAsync(Settings.BiometricsPin, password);
                    }

                    isBiometricsActive = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            //NOTE: We need to perserve certain cached values when the user logs out.
            var currentLanguage = await _cacheService.GetCurrentLanguage();
            var supportedLanguages = await _cacheService.GetSupportedLanguages();
            var translations = await _cacheService.GetTranslations();

            await _cacheService.ClearAll();
            await _cacheService.ClearAuthToken();

#if DEBUG
           // return;
#endif

            LanguageService.Instance.SetCurrentLanguage(currentLanguage);
            await _cacheService.SaveSupportedLanguages(supportedLanguages);
            await _cacheService.SaveTranslations(translations);

            Settings.SetBoleanProperty(Settings.IsBiometricsActive, isBiometricsActive);
            Settings.SetProperty(Settings.ConsetStatus, string.Empty);
            Settings.SetProperty(Settings.LastConsentedAt, string.Empty);

            // 2020-01-02 This line resets the DashboardMasterPage
            // that fixes the refresh profile issues in core.
            //MainService.Instance.SetMainPage(null, null);

            if (MainService.Instance.IsVerificationCodeEnabled)
            {
                List<AppSettings> settings = new List<AppSettings>();

                settings.Add(new AppSettings() { Id = nameof(ClientSingleton.Instance.BaseUrl), SettingsAttributes = new SettingsAttributes() { Value = ClientSingleton.Instance.BaseUrl } });
                settings.Add(new AppSettings() { Id = nameof(EnvironmentConfig.MobileApplicationKey), SettingsAttributes = new SettingsAttributes() { Value = EnvironmentConfig.MobileApplicationKey } });

                await _cacheService.SaveAPIStudySettings(settings);
            }

            //await Navigation.StartLogin<AppInitViewModel>();
        }

        public string BiometricsTitle
        {
            get
            {
                switch (_biometricType)
                {
                    case LocalAuthenticationType.BiometryFaceID:
                        return "BiometricUseFaceId".Localized();
                    case LocalAuthenticationType.BiometryTouchID:
                        return "BiometricUseTouchId".Localized();
                    case LocalAuthenticationType.BiometryTouchID_Or_Fingerprint:
                        return "BiometricUseFingerprint".Localized();
                    case LocalAuthenticationType.Pin:
                    default:
                        return "BiometricUsePin".Localized();
                }
            }
        }

        public async Task AboutTask()
        {
            //var Params = new AboutViewModel.InitValues();
            //Params.PolicyType = AboutViewModel.PolicyEnum.Terms;

            //await Navigation.NavigatePage<AboutViewModel, AboutViewModel.InitValues>(Params, fromMenu: true);
        }

        public async Task BiometricsTask()
        {
            //var canAuthenticate = await MainService.BiometricsServiceInstance.CanAuthenticate(false);

            //if (canAuthenticate)
            //{
            //    MainService.BiometricsServiceInstance.AuthenticateUser(OnAuthComplete);
            //}
            //else
            //{
            //    await _dialogService.ShowAlert("BiometricTitle".Localized(), "BiometricNotAvailableTitle".Localized());
            //}
        }

        private void OnAuthComplete(bool sucess, string errorMessag)
        {
            Settings.SetBoleanProperty(Settings.IsBiometricsActive, sucess);

            if (sucess)
            {
                _isBiometricsActive = sucess;
                Settings.SetBoleanProperty(Settings.IsBiometricsActive, _isBiometricsActive);
                Settings.SetBoleanProperty(Settings.IsBiometricsAlreadySetup, _isBiometricsActive);
                RefreshScreen();
                OnPropertyChanged(nameof(IsBiometricsActive));
            }
        }

        protected override async Task InitImplementation()
        {
            //_biometricType = MainService.BiometricsServiceInstance.CanAuthenticationWith();
            RefreshScreen();
            await LoadDataAction.Run();
        }

        private async Task GetAmazonPaymentsTask()
        {
            //AmazonPayment = await _amazonPaymentService.GetAmazonPayments(Clinical6.User.Id.Value.ToString());
        }

        private async Task ShowPoliciesAsync()
        {
            //var Params = new AboutViewModel.InitValues();
            //Params.PolicyType = AboutViewModel.PolicyEnum.All;
            //await Navigation.PushModal<AboutViewModel, AboutViewModel.InitValues>(Params);
        }
    }
}