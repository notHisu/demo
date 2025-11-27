using Autofac;
using Clinical6SDK;
using Clinical6SDK.Models;
using Clinical6SDK.Services;

//using FormsToolkit;
//using MAUI.Clinical6.Core.ViewModels;
using Microsoft.Maui.Controls;
//using Xamarin.Forms.Clinical6.Biometrics;


//using Xamarin.Forms.Clinical6.Biometrics;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.Login;

//using Xamarin.Forms.Clinical6.Core.ViewModels.VideoConsult;
//using Xamarin.Forms.Clinical6.FlowProcess;
//using Xamarin.Forms.Clinical6.Login;
using Xamarin.Forms.Clinical6.Services;


//using Xamarin.Forms.Clinical6.FlowProcess;
//using Xamarin.Forms.Clinical6.Login;
using Xamarin.Forms.Clinical6.UI.Services;
using Xamarin.Forms.Clinical6.UI.Views;
//using Xamarin.Forms.Clinical6.UI.Views.AmazonPayments;
//using Xamarin.Forms.Clinical6.UI.Views.VideoConsult;

//using Xamarin.Forms.Clinical6.UI.Views.AmazonPayments;
//using Xamarin.Forms.Clinical6.UI.Views.VideoConsult;
using Xamarin.Forms.Clinical6.ViewModels;
using Xamarin.Forms.Clinical6.Views;

#if ANDROID
using MAUI.Clinical6.Android.Services;
#endif

namespace Xamarin.Forms.Clinical6.UI
{
    /// <summary>
    /// Dependency config.
    /// </summary>
    public static class DependencyConfig
    {
        /// <summary>
        /// Wires up common dependencies
        /// </summary>
        public static void Configure(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterViewModels(builder);
        }

        public static void RegisterRenderViewModel<TVM, TPage>(this ContainerBuilder builder)
        where TVM : BaseViewModel
            where TPage : Page, IViewModelPage<TVM>
        {
            builder.RegisterViewModel<TVM, TPage>();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            //builder.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();

            // Third-party plugin abstractions
            //builder.Register(_ => MessagingService.Current).As<IMessagingService>();
            //builder.Register(_ => CrossPushNotification.Current).As<IPushNotification>();

            //builder.RegisterType<ApiModelFactory>().As<IApiModelFactory>().SingleInstance();
            builder.RegisterType<ApiRequestFactory>().As<IApiRequestFactory>().SingleInstance();
            builder.RegisterType<Clinical6LanguageService>().As<IClinical6LanguageService>().SingleInstance();
            builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<LanguageService>().As<ILanguageService>().SingleInstance();
            //builder.RegisterType<CommunicationService>().As<ICommunicationService>().SingleInstance();
            builder.RegisterType<DeviceInfoService>().As<IDeviceInfoService>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<Clinical6EDiaryService>().As<IClinical6EDiaryService>().SingleInstance();
            //builder.RegisterType<FlowProcessService>().As<IFlowProcessService>().SingleInstance();
            //builder.RegisterType<PatientService>().As<IPatientService>().SingleInstance();
            builder.RegisterType<HttpHandler>().As<IHttpHandler>().SingleInstance();
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
            builder.RegisterType<Clinical6Instance>().As<IClinical6Instance>().SingleInstance();
            //builder.RegisterType<ZXingBarcodeService>().As<IBarcodeService>().SingleInstance();
            //builder.RegisterType<ContentService>().As<IContentService>().SingleInstance();
            //builder.RegisterType<PermissionService>().As<IPermissionService>().SingleInstance();
            //builder.RegisterType<PhotoService>().As<IPhotoService>().SingleInstance();
            builder.RegisterType<PolicyVerificationService>().As<IPolicyVerificationService>().SingleInstance();

            // NOTE: This is purposely not a singleton, as each ViewModel needs its own
            builder.RegisterType<NavigationService>().As<INavigationService>();
            //builder.RegisterType<AmazonPaymentService>().As<IAmazonPaymentService>().SingleInstance();
            builder.RegisterType<Clinical6MobileUserService>().As<IClinical6MobileUserService>().SingleInstance();
            builder.RegisterType<AppSpecificConfig>().As<IAppSpecificConfig>().SingleInstance();

            //builder.RegisterType<BiometricsService>().As<IBiometricsService>().SingleInstance();
            //var biometricsService = DependencyService.Get<IBiometricsService>();
            //builder.RegisterInstance(biometricsService).As<IBiometricsService>();
#if ANDROID
            builder.RegisterType<BiometricsService>().As<IBiometricsService>().SingleInstance();
#endif
        }

        private static void RegisterViewModels(ContainerBuilder builder)
        {
            // App Init
            //builder.RegisterViewModel<AppInitViewModel, AppInitPage>();

            // Login
            //builder.RegisterViewModel<ForgotPinEnterEmailViewModel, ForgotPinEnterEmailPage>(); // Forgot Password
            //builder.RegisterViewModel<ForgotPinEmailSentViewModel, ForgotPinEmailSentPage>();  // Check Your Email

            // Legacy - still used?
            //builder.RegisterViewModel<EmailEntryViewModel, EmailEntryPage>();
            //builder.RegisterViewModel<PinEntryViewModel, PinEntryPage>();
            //builder.RegisterViewModel<CreatePinViewModel, CreatePinPage>();
            //builder.RegisterViewModel<ForgotPinResetViewModel, ForgotPinResetPage>();
            //builder.RegisterViewModel<TermsOfUseViewModel, TermsOfUsePage>();

            // Flow
            //builder.RegisterViewModel<FlowProcessSplashViewModel, FlowProcessSplashPage>();
            //builder.RegisterViewModel<FlowProcessViewModel, FlowProcessPage>();
            //builder.RegisterViewModel<CompletedFlowProcessViewModel, CompletedFlowProcessPage>();
            //builder.RegisterViewModel<CompletedHistoryViewModel, CompletedHistoryPage>();


            // Tasks (eDiary)
            builder.RegisterViewModel<MyTasksViewModel, MyTasksPage>();

            // LoginPMA
            builder.RegisterViewModel<LoginPMAViewModel, EmailPMAPage>();            // Participant Sign In
            //builder.RegisterViewModel<DisabledAccountViewModel, DisabledAccountPage>();     // Disabled Account
            //builder.RegisterViewModel<PasswordExpiredViewModel, PasswordExpiredPage>();     // Password Expired
            //builder.RegisterViewModel<LockedViewModel, LockedPage>();              // Locked Account
            //builder.RegisterViewModel<WithdrawnAccountViewModel, WithdrawnAccountPage>();    // Withdrawn Account
            //builder.RegisterViewModel<SessionTimeOutViewModel, SessionTimeOutPage>();      // Session Timeout
            //builder.RegisterViewModel<NoInternetViewModel, NoInternetPage>();          // No Internet Connection
            //builder.RegisterViewModel<PoliciesViewModel, PoliciesPage>();            // Accept New Privacy Policy

            // Dashboard
            builder.RegisterViewModel<AlertsViewModel, AlertsPage>();              // Alerts
            //builder.RegisterViewModel<AlertDetailViewModel, AlertDetailPage>();
            //builder.RegisterViewModel<AlertArchivedDetailViewModel, AlertArchivedDetailPage>();
            //builder.RegisterViewModel<AlertsArchiveViewModel, AlertsArchivePage>();
            builder.RegisterViewModel<AppMenuViewModel, MenuPage>();                // Sliding menu page with logo
            builder.RegisterViewModel<AppSubMenuViewModel, SubMenuPage>();             // Activity History, My Profile, Resources, etc.
            //builder.RegisterViewModel<AboutViewModel, AboutPage>();               // Privacy Link
            //builder.RegisterViewModel<EndpointChangeViewModel, EndpointChangePage>();      // Dynamic endpoint selection

            // Menu
            //builder.RegisterViewModel<ContentViewModel, DynamicContentPage>();
            //builder.RegisterViewModel<ContentListViewModel, DynamicListContentPage>();

            // Submenus
            //builder.RegisterViewModel<AboutTheStudyViewModel, AboutTheStudyPage>();               // About
            //builder.RegisterViewModel<ArticlesViewModel, ArticlesPage>();                    // Articles
            //builder.RegisterViewModel<ArticleDetailsViewModel, ArticleDetailsPage>();
            //builder.RegisterViewModel<AppointmentsViewModel, AppointmentsPage>();                // Appointments
            //builder.RegisterViewModel<FlowSurveyHistoryViewModel, FlowSurveyHistoryPage>();           // Activity History
            //builder.RegisterViewModel<FlowCompletedSurveyProcessViewModel, CompletedHistoryFlowProcessPage>();
            //builder.RegisterViewModel<ContactViewModel, ContactPage>();                     // Contacts
            //builder.RegisterViewModel<ContactDetailsViewModel, ContactDetailsPage>();
            //builder.RegisterViewModel<GenericHtmlListViewModel, GenericHtmlListPage>();             // Generic Html
            //builder.RegisterViewModel<GenericHtmlViewModel, GenericHtmlPage>();
            //builder.RegisterViewModel<ProfileViewModel, ProfilePage>();                     // My Profile
            //builder.RegisterViewModel<UserGuideViewModel, UserGuidePage>();                   // User Guide
            //builder.RegisterViewModel<VideoConsultListViewModel, VideoConsultListPage>();            // Video Consult
            //builder.RegisterViewModel<VideoConsultWaitingRoomViewModel, VideoConsultRoomPage>();
            //builder.RegisterViewModel<VideoListViewModel, VideoListPage>();                   // Videos
            //builder.RegisterViewModel<VideoPlayerViewModel, VideoPlayerPage>();

            // Processes
            //builder.RegisterViewModel<AmazonRedeemPaymentViewModel, AmazonRedeemPaymentPage>();         //  Amazon Payments
            //builder.RegisterViewModel<AmazonRedeemPaymentConfirmViewModel, AmazonRedeemPaymentConfirmPage>();
            //builder.RegisterViewModel<AmazonRedeemPaymentFinishViewModel, AmazonRedeemPaymentFinishPage>();
            //builder.RegisterViewModel<BiometricsSettingsViewModel, BiometricsSettingsPage>();
            //builder.RegisterViewModel<ExternalOAuthViewModel, ExternalOAuthPage>();               //  External OAuth
            //builder.RegisterViewModel<RemoteConsentViewModel, RemoteConsentPage>();
            //builder.RegisterViewModel<RemoteConsentCompletedViewModel, RemoteConsentCompletedPage>();
            //builder.RegisterViewModel<VerificationCodeViewModel, VerificationCodePage>();
        }

        public static void RegisterViewModel<TVM>(this ContainerBuilder builder)
            where TVM : BaseViewModel
        {
            builder.RegisterType<TVM>().AsSelf();
        }

        public static void RegisterViewModel<TVM, TPage>(this ContainerBuilder builder)
            where TVM : BaseViewModel
            where TPage : Page, IViewModelPage<TVM>
        {
            builder.RegisterViewModel<TVM>();
            NavigationService.RegisterMapping<TVM, TPage>();
        }
    }
}