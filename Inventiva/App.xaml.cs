using Clinical6SDK.Services;
//using Microsoft.AppCenter;
//using Microsoft.AppCenter.Analytics;
//using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6;
using Xamarin.Forms.Clinical6.Core;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Views;
using Xamarin.Forms.Clinical6.Views;
using Microsoft.Maui.ApplicationModel;
using Xamarin.Forms.Clinical6.Login;
//using Device = Xamarin.Forms.Device;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Inventiva
{
    public partial class App : Application
    {
        // Caution: Do not change formatting of these const strings below,
        // as these values are searched and replaced by a script in AppCenter.
        //const string APPCENTER_EDIT_ENDPOINT_NAME = "core-qa";
        //const string APPCENTER_EDIT_ENDPOINT_KEY = "b09f75ff9c0a8ef1e35b6949209da46c";
        const string APPCENTER_EDIT_ENDPOINT_NAME = "vertex-cf-qls-qa";
        const string APPCENTER_EDIT_ENDPOINT_KEY = "c13886a239a83820631d87cabdaaec26";

        public const string NotificationKey = "NotificationKey";

        public App()
        {
            InitializeComponent();

            var apiRoot = Settings.GetProperty(Settings.ApiRoot);
            var mobileApplicationKey = Settings.GetProperty(Settings.MobileApplicationKey);
#if DEBUG
            if (apiRoot != null && mobileApplicationKey != null)
            {
                MainService.CreateInstance(apiRoot, mobileApplicationKey);
            }
            else
            {
                MainService.CreateInstance("https://inv-hnas337-uat.clinical6.com", "ea9248173e30c2401d841b29ee5275cd");
                //MainService.CreateInstance("https://inv-hnas337-development.clinical6.com", "ea9248173e30c2401d841b29ee5275cd");
                //MainService.CreateInstance("https://inv-hnas337-qa.clinical6.com", "ea9248173e30c2401d841b29ee5275cd");
                //MainService.CreateInstance("https://vertex-cf-qls-development.clinical6.com//", "c13886a239a83820631d87cabdaaec26");
            }
#else

            //Set Mapping for Environment Email Routing
            Settings.SetDictProperty(Settings.EnvironmentMappedLogins, new Dictionary<string, string>()
            {
                 {"hughesdanny@prahs.com","inv-hnas337-uat" },
                 {"pratestuser1+applesubmissiontest@gmail.com","inv-hnas337-uat" }
            });

                        //string endpoint = $"https://{APPCENTER_EDIT_ENDPOINT_NAME}.clinical6.com/";
            //string appkey = APPCENTER_EDIT_ENDPOINT_KEY;
#if DEBUG
                const string ApiUrl = "https://inv-hnas337-uat.clinical6.com";
                //const string ApiUrl = "https://inv-hnas337-qa.clinical6.com";
                //const string ApiUrl = "https://inv-hnas337-development.clinical6.com";
#else
                const string ApiUrl = "https://inv-hnas337.clinical6.com";
#endif

            MainService.CreateInstance(ApiUrl, "ea9248173e30c2401d841b29ee5275cd");
            //MainService.CreateInstance("https://inv-hnas337-qa.clinical6.com", "ea9248173e30c2401d841b29ee5275cd");
            //MainService.CreateInstance("https://inv-hnas337-uat.clinical6.com", "ea9248173e30c2401d841b29ee5275cd");
            //MainService.CreateInstance("https://inv-hnas337-development.clinical6.com", "ea9248173e30c2401d841b29ee5275cd");
            //MainService.CreateInstance("https://inv-hnas337.clinical6.com", "ea9248173e30c2401d841b29ee5275cd");

            /*if (apiRoot != null && mobileApplicationKey != null)

                MainService.CreateInstance(apiRoot, mobileApplicationKey);
            }
            else
            {
                MainService.CreateInstance(endpoint, appkey);
            }*/
#endif
            MainService.Instance.IsBiometricsEnabled = true;
            Settings.SetProperty(Settings.SleepTime, DateTime.UtcNow.Ticks.ToString());
            Settings.SetBoleanProperty(Settings.UseNameForActivityHistory, true);
            Settings.SetBoleanProperty(Settings.UseNameForFlow, true);
            Settings.SetIntProperty(Settings.NumericPrecision, 1);
            AppContainer.Current.Initialize(DependencyConfig.Configure);
            AppContainer.Current.Resolve<IAppSpecificConfig>().ProfileStudyStartDateShown = false;
            AppContainer.Current.Resolve<IAppSpecificConfig>().ProfileFirstNameShown = false;
            AppContainer.Current.Resolve<IAppSpecificConfig>().ProfileLastNameShown = false;
            AppContainer.Current.Resolve<IAppSpecificConfig>().InitialAccountName = true;

            CreateMainPage();
        }

        public void CreateMainPage()
        {
            MainService.Instance.IsLoginPMA = true;
            MainService.Instance.IsCheckCosentEnabled = false;
            MainService.Instance.TimeOutSession = 15;
            MainService.Instance.IsBiometricsEnabled = true;
            MainService.Instance.IsAmazonPaymentsEnabled = false;
            MainService.Instance.AppResourceManager = Inventiva.Resources.AppResources.ResourceManager; // Setup client translations.

            var dashboardMasterPage = new DashboardMasterPage();

            // Store reference using Lazy<Page> for later access
            MainService.HomePage = new Lazy<Page>(() => dashboardMasterPage);

            // Set FlyoutPage as MainPage
            //MainPage = dashboardMasterPage;
            MainPage = new EmailPMAPage();
        }

        public void SetLoggingConfig(string config)
        {
            //AppContainer.Current.Resolve<ILoggingService>().SetConfig(config);
        }

        protected override void OnStart()
        {

#if !DEBUG
            //NOTE: Only sets up AppCenter crash reporting for Release builds
            //AppCenter.Start("ios=5e6c87cd-20eb-4274-85c6-bed0793c0a15;android=683cea43-f61f-4cea-9133-39e8a0d5fecd", typeof(Analytics), typeof(Crashes));
#endif
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static async void UpdateDevice()
        {
            try
            {
                var pushDevicetoken = string.IsNullOrEmpty(Settings.GetProperty(Settings.PushDevicetoken)) ? string.Empty : Settings.GetProperty(Settings.PushDevicetoken);

                var result = await UpdateDeviceTokenAsync(pushDevicetoken);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<object> UpdateDeviceTokenAsync(string deviceTokenString)
        {
            if (string.IsNullOrEmpty(deviceTokenString))
            {
                return null;
            }

            string technologyString = null;
            if (OperatingSystem.IsAndroid())
            {
                technologyString = "android";
            }
            else if (OperatingSystem.IsIOS())
            {
                technologyString = "ios";
            }

            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    technologyString = "android";
            //}
            //else if (Device.RuntimePlatform == Device.iOS)
            //{
            //    technologyString = "ios";
            //}

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
                AppVersion = "1.0",
                PushId = deviceTokenString
            };

            var result = await service.Update<Clinical6SDK.Helpers.Device>(device);
            return result;
        }

        public void ShowMessage(string title, string message, string idAlert = "")
        {
            //Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainPage?.DisplayAlert(title, message, "OkTitle".Localized());
            });
        }

        public void ShowNotifications()
        {
            if (!Settings.GetBoleanProperty(Settings.IsLoginIn))
            {
                return;
            }

            Xamarin.Forms.Clinical6.Core.Helpers.Settings.SetBoleanProperty(App.NotificationKey, false);

            var app = Application.Current;

            NavigationPage navigationPage = (NavigationPage)app.MainPage;

            //MasterDetailPage homePage = (MasterDetailPage)navigationPage.CurrentPage;
            FlyoutPage homePage = (FlyoutPage)navigationPage.CurrentPage;

            //Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (homePage.Detail is NavigationPage)
                    {
                        var nav = homePage.Detail as NavigationPage;

                        if (nav.CurrentPage is DashboardTabPage)
                        {
                            var tab = nav.CurrentPage as DashboardTabPage;
                            var navTab = tab.CurrentPage.Navigation;
                            //tab.CurrentPage.TabIndex = 0;
                            tab.CurrentPage = tab.Children[0];
                            (tab.CurrentPage as MyTasksPage).ViewModel.Navigation.Push<AlertsViewModel>();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
            });
        }

        public void CloseRemoteConsent()
        {
            //if (MainPage is NavigationPage && (MainPage as NavigationPage).CurrentPage is RemoteConsentPage)
            //{
            //    var viewmodel = ((MainPage as NavigationPage).CurrentPage as RemoteConsentPage).ViewModel;
            //    viewmodel.Navigation.Push<RemoteConsentCompletedViewModel>();
            //}
            //else
            //{
            //    MainService.CurrentNavigationService.NavigatePage<RemoteConsentCompletedViewModel>();
            //}
        }

    }
}