using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.Services;
using Xamarin.Forms.Clinical6.UI.Views;
using Xamarin.Forms.Clinical6.Views;

namespace Xamarin.Forms.Clinical6
{
    /// <summary>
    /// Main service.
    /// </summary>
    public class MainService : IMainService
    {
        //IFlowProcessService _flowProcessService;

        public event EventHandler<bool> SessionTimedOutEvent;

        /// <summary>
        /// The abstract navigation object
        /// </summary>
        /// <remarks>
        /// This should be set by the parent page (or view model), but in some cases it may not be.
        /// </remarks>
        private INavigationService _navigation;
        private int _patientId = 1;
        private IHavePermanentLink _flowSummary;
        private FileUploadParams _fileUploadParams;
        //private FlowProcessInitValues InitParam;

        public static Lazy<Page> HomePage;

        public bool IsPublicSettingsStudyLogoEnabled { get; set; }

        public bool IsLoginPMA { get; set; }

        public bool EnableCache { get; set; } = false;

        public bool TimeOutDisabled { get; set; }

        public bool IsCheckCosentEnabled { get; set; }

        public bool IsBiometricsEnabled { get; set; }

        public System.Resources.ResourceManager AppResourceManager { get; set; }

        public bool IsAmazonPaymentsEnabled { get; set; }

        //Forces US Currency for AmazonPayments label
        //on slideout menu
        public bool IsUSOnlyCulture { get; set; }

        public bool IsVerificationCodeEnabled => ClientSingleton.Instance.VerificationCodeUrl != null;

        public ImageSource StudyLogo { get; set; }

        public byte[] StudyLogoBytes { get; set; }

        public bool HideAlertsSection { get; set; }

        public int TimeOutSession { get; set; }

        public static MainService Instance { get; private set; }

        public static INavigationService CurrentNavigationService { get; set; }

        //private static IBiometricsService _instance;

        //public static IBiometricsService BiometricsServiceInstance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = DependencyService.Get<IBiometricsService>();
        //        return _instance;
        //    }
        //}

        //public static IBiometricsService BiometricsServiceInstance
        //    => IPlatformApplication.Current.Services.GetService<IBiometricsService>();

        public INavigationService Navigation
        {
            // If the parent has not set the navigation service, initialize one with no nav capabilities
            get => _navigation ?? (_navigation = AppContainer.Current.Resolve<INavigationService>());
            set => _navigation = value;
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="apiRoot">API root.</param>
        /// <param name="mobileApplicationKey">Mobile application key.</param>
        public static MainService CreateInstance(string apiRoot, string mobileApplicationKey)
        {
            EnvironmentConfig.ApiRoot = apiRoot;
            EnvironmentConfig.MobileApplicationKey = mobileApplicationKey;

            ClientSingleton.Instance.BaseUrl = EnvironmentConfig.ApiRoot;

            Instance = new MainService();
            return Instance;
        }

        /// <summary>
        /// Sets the main page.
        /// </summary>
        /// <returns>The main page.</returns>
        /// <param name="MainPage">Main page.</param>
        /// <param name="DashBoardPage">Dash board page.</param>
        /// <param name="processBeforeStart">If set to <c>true</c> process before start.</param>
        //public AppInitPage SetMainPage(Page MainPage, Page DashBoardPage = null, bool processBeforeStart = false)
        //{
        //    if (Instance.TimeOutSession <= 0)
        //    {
        //        Instance.TimeOutSession = EnvironmentConfig.TimeOutSession;
        //    }

        //    var appInitPage = MainPage as AppInitPage;
        //    if (appInitPage == null)
        //    {
        //        appInitPage = new AppInitPage();
        //        if (DashBoardPage != null)
        //        {
        //            HomePage = new Lazy<Page>(() => DashBoardPage);
        //        }
        //        else
        //        {
        //            HomePage = new Lazy<Page>(() => new DashboardMasterPage());
        //        }

        //        appInitPage.ViewModel.ProcessBeforeStart = processBeforeStart;
        //        appInitPage.ViewModel.DashBoardPage = HomePage;
        //    }

        //    return appInitPage;
        //}

        public void TriggerSessionTimeOutEvent(bool sessionExpired)
        {
            if (SessionTimedOutEvent == null)
                return;

            SessionTimedOutEvent.Invoke(null, sessionExpired);
        }

        /// <summary>
        /// Checks the expirtion token.
        /// </summary>
        /// <returns><c>true</c>, if expirtion token was checked, <c>false</c> otherwise.</returns>
        public bool CheckExpirtionToken()
        {
            var isLogin = Settings.GetBoleanProperty(Settings.IsLoginIn);

            if (isLogin)
            {
                var lastTimeActive = Settings.GetProperty(Settings.SleepTime);

                if (!string.IsNullOrEmpty(lastTimeActive))
                {
                    var datelastTimeActive = new DateTime(long.Parse(lastTimeActive));

                    var currenttiem = DateTime.UtcNow - datelastTimeActive;

                    if (currenttiem.TotalMinutes >= Instance.TimeOutSession)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Starts the flow process.
        /// </summary>
        /// <returns>The flow process.</returns>
        /// <param name="flowProccessId">Flow proccess identifier.</param>
        //public async Task StartFlowProcess(int flowProccessId)
        //{
        //    var crfs = (await _flowProcessService?.GetFlowProcesses()).OrderBy(SortFlowProcess);
        //    var item = new CrfListItem(crfs.FirstOrDefault());

        //    int _patientId = 100;

        //    var initValues = new FlowProcessInitValues(item, item.Name, _patientId);
        //    await Navigation.Push<FlowProcessSplashViewModel, FlowProcessInitValues>(initValues);
        //}

        /// <summary>
        /// Sorts the flow process.
        /// </summary>
        /// <returns>The flow process.</returns>
        /// <param name="model">Model.</param>
        private int SortFlowProcess(FlowProcessSummary model)
        {
            //book 1
            if (model.PermanentLink == "essential_information_published") //essential information
            {
                return 1;
            }

            if (model.PermanentLink == "antibiotic_treatment_published") //Antiobotic Treatment
            {
                return 2;
            }

            if (model.PermanentLink == "follow_up_published") //Follow Up
            {
                return 3;
            }

            if (model.PermanentLink == "laboratory_data_published") //Laboratory Data
            {
                return 4;
            }

            //book 2
            if (model.PermanentLink == "medical_history_published") //medical treatment
            {
                return 1;
            }
            if (model.PermanentLink == "prior_and_concomitant_published") //Prio and Conmitant
            {
                return 2;
            }
            if (model.PermanentLink == "gepotidacin_treatment_published") //Gepotindacin Treatment
            {
                return 3;
            }
            if (model.PermanentLink == "vitals_published") //Vitals
            {
                return 4;
            }

            if (model.PermanentLink == "y_pestis_lab_published") //Y. pestis Laboratory Data
            {
                return 5;
            }
            if (model.PermanentLink == "ecg_published") //ECG
            {
                return 6;
            }
            if (model.PermanentLink == "adverse_event_published") //Adverse Events
            {
                return 7;
            }
            if (model.PermanentLink == "death_information_published") //Death Information
            {
                return 8;
            }
            if (model.PermanentLink == "assessments_narrative_published") //Assessments/Narrative
            {
                return 9;
            }

            return 10;
        }

        /// <summary>
        /// Loads the completed flow process.
        /// </summary>
        /// <returns>The completed flow process.</returns>
        /// <param name="crf">Crf.</param>
        //private async Task LoadCompletedFlowProcess(PreviousListItem crf)
        //{
        //    _fileUploadParams = await _flowProcessService?.GetUploadFlowProcess(_flowSummary, _patientId);

        //    var initValues = new CompletedFlowProcessViewModel.InitValues
        //    {
        //        FlowTitle = crf.PreviousAnswer.FlowProcessName,
        //        CompletedFlowId = crf.PreviousAnswerId,
        //        PatientId = _patientId,
        //        FlowSummary = crf.IsUpload ? _fileUploadParams?.FlowProcess : _flowSummary
        //    };

        //    if (crf.IsUpload)
        //    {
        //        /* Legacy
        //        await Navigation.Push<CompleteUploadFlowProcessViewModel, CompletedFlowProcessViewModel.InitValues>(initValues);
        //        */
        //    }
        //    else
        //    {
        //        await Navigation.Push<CompletedFlowProcessViewModel, CompletedFlowProcessViewModel.InitValues>(initValues);
        //    }
        //}

        /// <summary>
        /// Gotos the upload flow process.
        /// </summary>
        /// <returns>The upload flow process.</returns>
        //private async Task GotoUploadFlowProcess()
        //{
        //    _fileUploadParams = await _flowProcessService?.GetUploadFlowProcess(_flowSummary, _patientId);

        //    if (_fileUploadParams == null)
        //    {
        //        //still loading or loading failed. Should never happen
        //        return;
        //    }
        //    /* Legacy
        //    await Navigation.Push<UploadFlowProcessViewModel, FileUploadParams>(_fileUploadParams);
        //    */
        //}

        /// <summary>
        /// Gotos the create flow process.
        /// </summary>
        /// <returns>The create flow process.</returns>
        private async Task GotoCreateFlowProcess()
        {
            //await Navigation.Push<FlowProcessViewModel, FlowProcessInitValues>(InitParam);
        }

        /// <summary>
        /// Completeds the flow process.
        /// </summary>
        /// <returns>The flow process.</returns>
        public async Task CompletedFlowProcess()
        {
            //InitParam = new FlowProcessInitValues(_flowSummary, "flowtitle", _patientId);
            await GotoCreateFlowProcess();
        }

        /// <summary>
        /// Completeds the upload flow process.
        /// </summary>
        /// <returns>The upload flow process.</returns>
        public async Task CompletedUploadFlowProcess()
        {
            _patientId = 1;
            // _flowSummary = 
            //await GotoUploadFlowProcess();
        }

        public Task StartFlowProcess(int flowProccessId)
        {
            throw new NotImplementedException();
        }
    }
}