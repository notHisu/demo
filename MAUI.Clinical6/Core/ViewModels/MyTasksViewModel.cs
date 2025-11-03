using Clinical6SDK;
using Clinical6SDK.Common.Exceptions;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;
using Clinical6SDK.Services;
using Clinical6SDK.Services.Requests;
using JsonApiSerializer.JsonApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.UI.Views.Card;
using Xamarin.Forms.Clinical6.ViewModels;

namespace Xamarin.Forms.Clinical6.Core.ViewModels
{
    /// <summary>
    /// MyTasks view model.
    /// </summary>
    public class MyTasksViewModel : BaseViewModel
    {
        /***** Fields *****/
        public readonly ICacheService _cacheService;
        public readonly IUserService _userService;
        private bool _autoRefreshEnable = true;
        private bool _isInitialLoading = false;
        private bool _isRefreshing = false;
        private bool _isTaskButtonProcessing = false;
        private ObservableCollection<IMaterialCard> _TaskCards;
        private List<AppSettings> appSettings;
        private List<ConsentAvailableStrategyModel> AvailableStrategies;
        public string StudyName;
        private List<StudyStepProgress> StudySteps;
        private List<StudyTask> StudyStepTasks;
        private List<StudySubTaskOccurrence> StudySubTaskOccurrences;



        /***** Properties *****/
        public bool AutoRefreshEnable
        {
            get => _autoRefreshEnable;
            set
            {
                if (value != _autoRefreshEnable)
                {
                    SetProperty(ref _autoRefreshEnable, value);
                }
            }
        }
        public BaseCard ConsentCard { get; private set; }
        public Clinical6Instance clinical6 { get; private set; } = new Clinical6Instance();
        public Site CurrentSite { get; set; }
        public SiteMember CurrentSiteMember { get; set; }
        private StudyStepProgress CurrentStudyStep;
        public Profile CurrentUserProfile => clinical6.User?.Profile;

        public string GreetingsText
        {
            get
            {
                var headerTitle = string.Empty;

                try
                {
                    headerTitle = (CurrentUserProfile?.FirstName == null)
            ? string.Empty
            : string.Format("MyTasksHeaderGreetingText".Localized(), CurrentUserProfile.FirstName.ToUpper());
                }
                catch (Exception ex)
                {
                    headerTitle = CurrentUserProfile.FirstName;
                    Console.WriteLine(ex);
                }

                return headerTitle;
            }
        }

        // Whether there are any tasks to do.
        public bool HasTasks => TaskCards != null && TaskCards.Count != 0;
        public string HeaderTitleText =>
            (CurrentStudyStep == null
            || CurrentSiteMember == null
            || IsRefreshing
            || new[] { "consent_in_progress", "consent_complete" }.Contains(CurrentSiteMember.ConsentStatus))
            ? " "
            : CurrentStudyStep.StepDefinition.Name;

        public string WeekTitle
        {
            get { return string.Format("HomeWeek".Localized(), WeekNumber, WeekTotal); }
        }

        private DateTime CleanDatetime(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }

        private int _weekTitle;
        public int WeekNumber
        {
            get { return _weekTitle; }
            set
            {
                if (_weekTitle != value)
                {
                    _weekTitle = value;
                    OnPropertyChanged(nameof(WeekNumber));
                    OnPropertyChanged(nameof(WeekTitle));
                }
            }
        }

        private int WeekTotal
        {
            get
            {
                if (Settings.GetIntProperty(Settings.StudyWeekTotal) is int WeekTotal)
                    return WeekTotal;

                return 52;
            }
        }

        public string HeaderTitleAccessibleText
        {
            get
            {
                var header = string.Format("{0}, {1}, {2}", GreetingsText, HeaderTitleText, RemainingActivitiesText);
                if (StatusCard != null)
                {
                    if (StatusCard.Title != null)
                    {
                        header = string.Format("{0}. {1}", header, StatusCard.Title);
                    }
                    if (StatusCard.Body != null)
                    {
                        header = string.Format("{0}. {1}", header, StatusCard.Body);
                    }
                    if (StatusCard.AdditionalInfo != null)
                    {
                        header = string.Format("{0}. {1}", header, StatusCard.AdditionalInfo);
                    }
                }
                return header;
            }
        }
        public bool IsDemoFlag { get; private set; } = false;
        public bool IsBannerRequired => (CurrentSiteMember != null) && CurrentSiteMember.FirstConsentedAt != null && new[] { "consent_initial", "initial" }.Contains(CurrentSiteMember.ConsentStatus);
        public bool IsInitialLoading { get => _isInitialLoading; set => SetProperty(ref _isInitialLoading, value); }
        // Whether the participant is currently considered reconsenting.
        public bool IsReconsentState =>
            CurrentSiteMember?.FirstConsentedAt != null && new[] { "consent_initial", "initial" }.Contains(CurrentSiteMember.ConsentStatus)
            || (CurrentSiteMember?.LastConsentedAt != null && CurrentSiteMember.ConsentStatus == "consent_in_progress");
        public bool IsRefreshing { get => _isRefreshing && !IsInitialLoading; set => SetProperty(ref _isRefreshing, value); }
        public bool IsTaskButtonProcessing { get => _isTaskButtonProcessing; set => SetProperty(ref _isTaskButtonProcessing, value); }
        public ActionTracker LoadAutoRefreshAction { get; }
        public ActionTracker LoadSiteAction { get; }
        public ActionTracker LoadSiteMemberAction { get; }
        public ActionTracker LoadStudyStepProgressAction { get; }
        public ActionTracker LoadSubTaskOccurrencesAction { get; }
        public ActionTracker LoadUserProfileAction { get; }
        public Thickness ReconsentBannerPadding => IsBannerRequired ? new Thickness(0, 200, 0, 20) : new Thickness(0, 170, 0, 20);
        public string ReconsentBody { get; set; } = "MyTasksReconsentBody".Localized();
        // Reconsent banner will be displayed regarles the user has task or not.
        public bool ReconsentIsVisible => IsReconsentState && !(ConsentCard?.IsVisible ?? false);
        public string ReconsentTitle { get; set; } = "MyTasksReconsentTitle".Localized();
        public List<StudySubTaskOccurrence> RemainingActivities => StudySubTaskOccurrences?.FindAll(obj => !new[] { "completed", "expired", "skipped" }.Contains(obj.Status)) ?? new List<StudySubTaskOccurrence>();
        public string RemainingActivitiesText
        {
            get
            {
                var remainingActivities = RemainingActivities.Count;
                if (ConsentCard != null && ConsentCard.IsVisible)
                {
                    remainingActivities++;
                }

                var _remainingActivitiesIsVisible = CurrentSiteMember != null && (RemainingActivities.Count() > 0 || CurrentSiteMember.ConsentStatus != "consent_initial" || ConsentCard != null);

                RemainingActivitiesIsVisible = _remainingActivitiesIsVisible;

                OnPropertyChanged(nameof(RemainingActivitiesIsVisible));

                var header = string.Empty;

                try
                {
                    var myTasksHeaderRemainingActivitiesText = "MyTasksHeaderRemainingActivitiesText".Localized();
                    header = string.Format(new PluralFormatProvider(), myTasksHeaderRemainingActivitiesText, remainingActivities);
                }
                catch (Exception ex)
                {

                }
                return header;
            }
        }
        public bool RemainingActivitiesIsVisible { get; set; } = false;
        public BaseCard StatusCard { get; private set; }
        public ObservableCollection<IMaterialCard> TaskCards
        {
            get => _TaskCards;
            set
            {
                if (value != _TaskCards)
                {
                    _TaskCards = value;
                    OnPropertyChanged(nameof(_TaskCards));
                    OnPropertyChanged(nameof(TaskCardsIsVisible));
                }
            }
        }

        private bool _displayPatientStudyProgress;
        public bool DisplayPatientStudyProgress
        {
            get => _displayPatientStudyProgress;
            set
            {
                if (value != _displayPatientStudyProgress)
                {
                    _displayPatientStudyProgress = value;
                    OnPropertyChanged(nameof(DisplayPatientStudyProgress));
                    OnPropertyChanged(nameof(WeekTitle));
                }
            }
        }

        public bool TaskCardsIsVisible => true;
        public string Title { get; set; }

        public async void ExecuteOpenListVCCommandAsync()
        {
            //await Navigation.Push<VideoConsultListViewModel>();
        }

        public ICommand GoToSubTask
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (IsTaskButtonProcessing) return;
                    IsTaskButtonProcessing = true;

                    if (!await IsInternetConnected())
                    {
                        IsTaskButtonProcessing = false;
                        return;
                    }

                    var item = e as IMaterialCard;
                    var userId = clinical6.User.Id;

                    if (IsDemoFlag)
                    {
                        var flow = (item.ButtonReference as FlowContainerV2Flow);
                        //var flowSummary = new SimpleFlowSummary { PermanentLink = flow.PermanentLink };
                        //var InitParam = new FlowProcessInitValues(flowSummary, flow.Name, userId ?? 0);

                        IsTaskButtonProcessing = false;

                        if (await IsInternetConnected())
                        {
                            //await Navigation.Push<FlowProcessViewModel, FlowProcessInitValues>(InitParam);
                        }
                    }
                    else
                    {
                        var type = item.ButtonReference.GetType();
                        switch (type.Name)
                        {
                            case "ConsentGrants":
                                var consentGrant = (item.ButtonReference as ConsentGrants);
                                if (consentGrant != null)
                                {
                                    try
                                    {
                                        consentGrant = await clinical6.Insert<ConsentGrants>(consentGrant);
                                        IsTaskButtonProcessing = false;
                                        if (consentGrant == null || consentGrant.Progress == "user_signed") return;
                                        //await Navigation.Push<RemoteConsentViewModel, ConsentGrants>(consentGrant);
                                    }
                                    catch (Clinical6ServerException ex)
                                    {
                                        //await Navigation.Push<RemoteConsentViewModel, ConsentGrants>(new ConsentGrants() { ErrorResponse = ex.ErrorResponse });
                                    }
                                    catch (Exception ex)
                                    {
                                        MainThread.BeginInvokeOnMainThread(async delegate
                                        {
                                            await Application.Current.MainPage.DisplayAlert("InvalidPhoneNumber".Localized(), ex.Message, "DialogOk".Localized());
                                        });
                                        return;
                                    }
                                }
                                break;
                            case "StudySubTaskOccurrence":
                                var occurrence = (item.ButtonReference as StudySubTaskOccurrence);
                                switch (occurrence.Taskable.Type)
                                {
                                    case "c6__flow_connections":
                                        var occurrenceFlow = ((FlowConnection)occurrence.Taskable).Flow;

                                        Flow flow = await clinical6.Get<Flow>(new Flow { Id = occurrenceFlow.Id });
                                        //var flowSummary = new SimpleFlowSummary { PermanentLink = flow.PermanentLink };
                                        //var InitParam = new FlowProcessInitValues(flowSummary, occurrence.SubTaskDefinition.Label, userId ?? 0);

                                        //if (occurrence.SubTaskDraft == null)
                                        //{
                                        //    try
                                        //    {
                                        //        var resultSubTaskDraft = await occurrence.SaveAsDraft();

                                        //        if (resultSubTaskDraft != null)
                                        //        {
                                        //            occurrence.SubTaskDraft = resultSubTaskDraft;
                                        //            InitParam.FlowDataGroup = occurrence.SubTaskDraft.TaskableProgress;
                                        //        }
                                        //    }
                                        //    catch (Exception ex)
                                        //    {
                                        //        Console.WriteLine(ex);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    InitParam.FlowDataGroup = occurrence.SubTaskDraft.TaskableProgress;
                                        //    InitParam.LoadPreviousAnswers = true;
                                        //}

                                        //InitParam.CallBackOnExitFlow += async delegate
                                        //{
                                        //    InitParam.CallBackOnFlowSubmited = null;
                                        //    InitParam.CallBackOnExitFlow = null;
                                        //    await LoadData();
                                        //};

                                        //InitParam.CallBackOnFlowSubmited += async (FlowProcess obj) =>
                                        //{
                                        //    occurrence.FlowDataGroup = obj.FlowDataGroup;
                                        //    occurrence.Status = "completed";

                                        //    try
                                        //    {
                                        //        var results = await clinical6.Update<StudySubTaskOccurrence>(occurrence);
                                        //    }
                                        //    catch (Exception ex)
                                        //    {
                                        //        Console.WriteLine(ex.Message);
                                        //    }

                                        //    InitParam.CallBackOnFlowSubmited = null;
                                        //    InitParam.CallBackOnExitFlow = null;

                                        //    await LoadData();
                                        //};

                                        //IsTaskButtonProcessing = false;

                                        //if (await IsInternetConnected())
                                        //{
                                        //    bool useName = Settings.GetBoleanProperty(Settings.UseNameForFlow);
                                        //    if (useName)
                                        //    {
                                        //        // This nulls out the subtask card name and so uses flow name instead. 
                                        //        // See FlowProcessViewModel.LoadFlow().
                                        //        InitParam.FlowTitle = "";
                                        //    }
                                        //    await Navigation.Push<FlowProcessViewModel, FlowProcessInitValues>(InitParam);
                                        //}

                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    IsTaskButtonProcessing = false;
                });
            }
        }
        public ICommand RefreshCommand => new Command(async () => await LoadData());

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xamarin.Forms.Clinical6.Core.ViewModels.MyTasksViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="userService">User service.</param>
        /// <param name="cacheService">Cache service.</param>
        public MyTasksViewModel(
            INavigationService navigationService,
            IUserService userService,
            ICacheService cacheService)
        : base(navigationService)
        {
            _userService = userService;
            _cacheService = cacheService;

            CurrentSiteMember = new SiteMember();

            LoadAutoRefreshAction = new ActionTracker(LoadAutoRefresh, null, OnErrorPrintToConsole);
            LoadUserProfileAction = new ActionTracker(LoadUserProfile, null, OnErrorPopNavigation);
            LoadSiteAction = new ActionTracker(LoadSite, null, OnErrorPopNavigation);
            LoadSiteMemberAction = new ActionTracker(LoadSiteMember, null, OnErrorPopNavigation);
            LoadStudyStepProgressAction = new ActionTracker(LoadStudyStepProgress, null, OnErrorPopNavigation);
            LoadSubTaskOccurrencesAction = new ActionTracker(LoadSubTaskOccurrences, null, OnErrorPopNavigation);

            TaskCards = new ObservableCollection<IMaterialCard>();
            StudySteps = new List<StudyStepProgress>();
            StudyStepTasks = new List<StudyTask>();
            StudySubTaskOccurrences = new List<StudySubTaskOccurrence>();
            //RemoteConsentCompletedViewModel.RemoteConsentCompletedEvent += RemoteConsentCompletedViewModel_RemoteConsentCompletedEvent;
        }

        /// <summary>
        /// Loads Auto Refresh
        /// </summary>
        public void InitAutoRefresh()
        {
            Application.Current.Dispatcher.StartTimer(TimeSpan.FromSeconds(20), () =>
            {
                if (clinical6.User != null && _autoRefreshEnable && !LoadAutoRefreshAction.IsBusy)
                {
                    LoadAutoRefreshAction.Run();
                }
                return _autoRefreshEnable; // True = Repeat again, False = Stop the timer
            });

            //Device.StartTimer(TimeSpan.FromSeconds(15), () =>
            //{
            //    if (clinical6.User != null && _autoRefreshEnable && !LoadAutoRefreshAction.IsBusy)
            //    {
            //        LoadAutoRefreshAction.Run();
            //    }

            //    return _autoRefreshEnable; // True = Repeat again, False = Stop the timer
            //});
        }

        /// <summary>
        /// Starts the refresh status
        /// </summary>
        /// <returns></returns>
        public async Task LoadAutoRefresh()
        {
            await LoadData(true);
        }

        /// <summary>
        /// Inits the implementation.
        /// </summary>
        /// <returns>The implementation.</returns>
        protected override async Task InitImplementation()
        {
            // for some reason need this on initial load
            RemainingActivitiesIsVisible = false;
            OnPropertyChanged(nameof(RemainingActivitiesIsVisible));

            if (Settings.GetBoleanProperty(Settings.SignatureComplete))
            {
                Settings.SetBoleanProperty(Settings.SignatureComplete, false);

                if (!await IsInternetConnected())
                    return;

                //await Navigation.Push<RemoteConsentCompletedViewModel>();
            }

            IsInitialLoading = true;
            OnPropertyChanged(nameof(IsInitialLoading));
            clinical6.AuthToken = await _cacheService.GetAuthToken();
            Title = "MyTasksTitle".Localized();
            IsInitialLoading = false;
            OnPropertyChanged(nameof(IsInitialLoading));
            await LoadData();
        }

        /// <summary>
        /// Formats the site address.
        /// </summary>
        /// <returns>The site address.</returns>
        /// <param name="site">Site.</param>
        private string FormatSiteAddress(Site site)
        {
            return Regex.Replace(string.Format("MyTasksStatusCardAddressFormat".Localized()
                , site.Name
                , site.Location.Street
                , site.Location.City
                , site.Location.State
                , site.Location.PostalCode
                , site.ContactName
                , site.PhoneNumber
                , site.Email)
                .Replace(",  ", " "), @"\s{2,}", Environment.NewLine);
        }

        /// <summary>
        /// Action to happen if user account is "study_complete", "withdrawn", "screening_failed", or "inactive"
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUserValid()
        {
            if (clinical6.User == null)
                return false;

            if (!await IsInternetConnected())
                return false;

            DocumentRoot<StatusModel> AccountStatus = await clinical6.User.GetRegistrationStatus();
            switch (AccountStatus.Data.Value.ToLower())
            {
                case "withdrawn":
                    //await Navigation.Push<WithdrawnAccountViewModel>();
                    return false;
                case "inactive":
                    //await Navigation.Push<DisabledAccountViewModel>();
                    return false;
                case "study_complete":
                case "screening_failed":
                    return false;
                default:
                    break;
            }

            return true;
        }


        /// <summary>
        /// Is the internet connected?
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsInternetConnected()
        {
            var access = Connectivity.NetworkAccess;
            if (access != NetworkAccess.Internet && access != NetworkAccess.ConstrainedInternet)
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    _autoRefreshEnable = false;
                    //await Navigation.StartLogin<AppInitViewModel>();
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads demo data
        /// </summary>
        /// <returns></returns>
        private async Task<bool> LoadDemoData()
        {
            var ImageArray = new List<string> { "pic_caught_up", "pic_consent_required", "pic_diary", "pic_survey", "pic_take_medication" };
            TaskCards = new ObservableCollection<IMaterialCard>();

            var options = new Options { Url = "api/v2/data_collection/containers/dashboard" };
            var genericService = new GenericJsonHttpService();
            try
            {
                var result = await (new GenericJsonHttpService()).Get<FlowContainerV2Root>(options);
                var OrderedFlowProcesses = result.Data.Attributes.FlowProcesses.OrderBy(o => o.Position).ToList();
                foreach (var flow in OrderedFlowProcesses)
                {
                    TaskCards.Add(new BaseCard
                    {
                        Id = flow.Name,
                        Label = flow.Name,
                        //Title = flow.Description,
                        ImageSource = ImageArray[flow.Name.Length % ImageArray.Count],
                        ButtonText = "StartText".Localized(),
                        ButtonIsVisible = true,
                        ButtonReference = flow
                    });
                }
                OnPropertyChanged(nameof(TaskCards));
            }
            catch (Exception ex)
            {
                return false;
            }
            OnPropertyChanged(nameof(TaskCardsIsVisible));
            return true;
        }

        /// <summary>
        /// Get Study Settings
        /// </summary>
        /// <returns></returns>
        private async Task<List<AppSettings>> GetStudySettings()
        {
            try
            {
                var ServiceSDK = new Clinical6BaseService();
                ServiceSDK.BaseUrl = EnvironmentConfig.ApiRoot;

                var service = new JsonApiHttpService
                {
                    BaseUrl = EnvironmentConfig.ApiRoot,
                    AuthToken = await new CacheService().GetAuthToken()
                };

                var options = new Options { Url = "/v3/public_settings" };

                var response = await ServiceSDK.Get<List<AppSettings>>(options);

                return response;
            }
            catch
            {

            }

            return new List<AppSettings>();
        }

        /// <summary>
        /// Load App Settings for Study Name
        /// </summary>
        /// <returns></returns>
        private async Task LoadAppSettings(bool forceUpdate = false)
        {
            appSettings = await _cacheService.GetStudySettings();

            if (forceUpdate)
            {
                appSettings = await GetStudySettings();
            }

            if (appSettings?.Count > 0)
            {
                var studyNameSettings = appSettings.Where(c => c.Id == "study_name");
                if (studyNameSettings?.Count() > 0)
                {
                    StudyName = studyNameSettings.FirstOrDefault().SettingsAttributes.Value?.ToString();
                }

                if (appSettings?.Count > 0)
                {
                    var studySettings = appSettings.Where(c => c.Id == "display_patient_study_progress");
                    if (studySettings?.Count() > 0)
                    {
                        try
                        {
                            DisplayPatientStudyProgress = System.Convert.ToBoolean(studySettings.FirstOrDefault().SettingsAttributes.Value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }

                OnPropertyChanged(nameof(DisplayPatientStudyProgress));
            }
            else
            {
                DisplayPatientStudyProgress = true;
            }
        }

        /// <summary>
        /// Loads the available strategies to be used for logic in showing the status or consent cards
        /// </summary>
        /// <returns></returns>
        private async Task LoadAvailableStrategies()
        {
            AvailableStrategies = await clinical6.GetChildren<List<ConsentAvailableStrategyModel>>(clinical6.User, new ConsentAvailableStrategyModel().Type);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>The data.</returns>
        private async Task LoadData(bool forceUpdate = false)
        {
            try
            {
                if (!await IsInternetConnected())
                    return;

                if (!forceUpdate)
                    IsRefreshing = true;

                if (await IsUserValid())
                {
                    await LoadUserProfileAction.Run();
                    await LoadSiteMemberAction.Run();
                    await LoadAvailableStrategies();

                    await LoadAppSettings(forceUpdate);
                    LoadRemoteConsent();

                    if (ConsentCard == null || forceUpdate)
                    {
                        await Task.WhenAll(
                            LoadStudyStepProgressAction.Run(),
                            IsDemoFlag ? LoadDemoData() : LoadSubTaskOccurrencesAction.Run()
                            );
                    }
                }

                ResetProperties();

                //await new NotificationService().LoadNotificationsAsync();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"MyTaskViewModel.LoadData Exception: {exc}");
            }
            finally
            {
                if (!forceUpdate)
                    IsRefreshing = false;
            }
        }

        /// <summary>
        /// Load remote consent card if strategies exist
        /// </summary>
        private void LoadRemoteConsent()
        {
            if (AvailableStrategies == null || !AvailableStrategies.Any())
            {
                if (ConsentCard != null && TaskCards != null && TaskCards.Count() > 0)
                {
                    TaskCards.Remove(ConsentCard);
                    ConsentCard = null;
                }
                return;
            }

            if (CurrentSiteMember?.LastConsentedAt == null)
            {
                //  return;
            }

            int? formId = 0;

            if (AvailableStrategies?.First().Forms?.Count > 0)
            {
                formId = AvailableStrategies.First().Forms.First().Id;
            }

            var consentGrant = new ConsentGrants
            {
                GrantedFor = new User { Id = clinical6.User.Id, Type = "mobile_users" },
                Strategy = new ConsentStrategyModel { Id = AvailableStrategies.First().Strategy.Id },
                Form = new ConsentForms { Id = formId },
                RequestType = "pma"
            };

            ConsentCard = new BaseCard()
            {
                // Verbiage depends on consent vs. reconsent status. Functionality is the same.
                Id = IsReconsentState ? "MyTasksStatusRemoteConsentLabelRe".Localized() : "MyTasksStatusRemoteConsentLabel".Localized(),
                Label = IsReconsentState ? "MyTasksStatusRemoteConsentLabelRe".Localized() : "MyTasksStatusRemoteConsentLabel".Localized(),
                ImageSource = "pic_consent_required",
                Title = IsReconsentState ? "MyTasksStatusRemoteConsentTitleRe".Localized() : "MyTasksStatusRemoteConsentTitle".Localized(),
                Body = string.Format(IsReconsentState ? "MyTasksStatusRemoteConsentBodyRe".Localized() : "MyTasksStatusRemoteConsentBody".Localized(), StudyName),
                IsVisible = true,
                ButtonIsVisible = true,
                ButtonText = "SignInformedConsentText".Localized(),
                ButtonReference = consentGrant
            };

            try
            {
                if (ConsentCard.IsVisible && (TaskCards.Count <= 0 || (StudySteps.Count == 0 && TaskCards[0].Label != ConsentCard.Label)))
                {
                    if (TaskCards?.Count > 0 && TaskCards[0].ButtonReference != null)
                    {
                        if (TaskCards[0].ButtonReference?.GetType().Name == "StudySubTaskOccurrence")
                        {

                        }
                        else
                        {
                            TaskCards.Clear();
                            TaskCards = new ObservableCollection<IMaterialCard>();
                        }
                    }
                    else
                    {
                        TaskCards.Clear();
                        TaskCards = new ObservableCollection<IMaterialCard>();
                    }

                    if (TaskCards.Where(c => c.Id == ConsentCard?.Id).Count() == 0)
                    {
                        TaskCards.Add(ConsentCard);
                        TaskCards.OrderBy(a => a.Priority);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TaskCards.Add(ConsentCard);
            }

            OnPropertyChanged(nameof(StatusCard));
            OnPropertyChanged(nameof(TaskCards));
            OnPropertyChanged(nameof(TaskCardsIsVisible));
            OnPropertyChanged(nameof(HeaderTitleText));
            OnPropertyChanged(nameof(RemainingActivitiesText));

            // This is needed for the reconsent banner to be updated.
            OnPropertyChanged(nameof(ReconsentIsVisible));
            OnPropertyChanged(nameof(IsBannerRequired));
            OnPropertyChanged(nameof(ReconsentBannerPadding));
            OnPropertyChanged(nameof(RemainingActivitiesIsVisible));
        }

        /// <summary>
        /// Loads the site.
        /// </summary>
        /// <returns>The site member.</returns>
        private async Task LoadSite()
        {
            if (CurrentSiteMember == null || CurrentSiteMember.Site == null || CurrentSiteMember.Site.Id == 0) return;
            CurrentSite = CurrentSiteMember.Site;
            var site = await clinical6.Get<Site>(CurrentSite);
            CurrentSite = site != null ? site : CurrentSite;
        }

        /// <summary>
        /// Loads the site member.
        /// </summary>
        /// <returns>The site member.</returns>
        private async Task LoadSiteMember()
        {
            var result = await clinical6.Get<SiteMember>(clinical6.User.SiteMember);
            CurrentSiteMember = result ?? CurrentSiteMember;
        }

        /// <summary>
        /// Loads study step progress.
        /// </summary>
        /// <returns>The data.</returns>
        private async Task LoadStudyStepProgress()
        {
            var result = await clinical6.GetChildren<List<StudyStepProgress>>(clinical6.User.SiteMember, new StudyStepProgress().Type);
            StudySteps = result ?? StudySteps;
            CurrentStudyStep = StudySteps.Find(x => x.LeftAt == null);
        }

        /// <summary>
        /// Loads the sub task occurrences.  This loads information into the
        /// cards displayed on the dashboard (MyTasksPage).  This call directly 
        /// affects the status card - thus the update.
        /// </summary>
        /// <returns>The sub task occurrences.</returns>
        private async Task LoadSubTaskOccurrences()
        {
            StudySubTaskOccurrences = await clinical6.GetChildren<List<StudySubTaskOccurrence>>(
                clinical6.User.SiteMember,
                new StudySubTaskOccurrence().Type,
                options: new Options
                {
                    UriQuery = new
                    {
                        filters = new
                        {
                            sub_task_definition = new
                            {
                                system_executed = "false",
                                site_user_executed = "false",
                                show_in_pma = "true"
                            }
                        }
                    }
                });

            if (RemainingActivities.Count > 0)
            {
                foreach (var task in RemainingActivities)
                {
                    var card = new BaseCard()
                    {
                        Id = task.SubTaskDefinition.Id?.ToString() ?? "",
                        Label = task.SubTaskDefinition.Label,
                        Title = task.SubTaskDefinition.Title,
                        Body = task.SubTaskDefinition.Body,
                        ImageSource = task.SubTaskDefinition.Image?.Url,
                        Priority = task.SubTaskDefinition.Id ?? 1,
                        Type = task.Type
                    };
                    if (task.Taskable != null)
                    {
                        card.ButtonReference = task;
                        card.ButtonIsVisible = true;
                        card.ButtonText = (task?.SubTaskDraft == null ? task.SubTaskDefinition.Button : task.SubTaskDefinition.ResumeButton) ?? "MyTasksStatusCardDefaultButtonText".Localized();
                    }

                    var tasks = TaskCards.Where(c => c.Id == card.Id);
                    // add cards only if they don't already exist
                    if (tasks.Count() == 0)
                    {
                        TaskCards.Add(card);
                        TaskCards.OrderBy(a => a.Priority);
                    }
                    else if (tasks.Count() == 1 && !tasks.First().Equals(card))
                    {
                        // Update card to have new information
                        TaskCards.Remove(tasks.First());
                        TaskCards.Add(card);
                        TaskCards.OrderBy(a => a.Priority);
                        OnPropertyChanged(nameof(TaskCards));
                    }
                }

                // remove cards that aren't remaining tasks (and are not consent/status cards)
                foreach (var task in TaskCards.Where(t => t.Type == new StudySubTaskOccurrence().Type))
                {
                    if (RemainingActivities.Where(c => c.SubTaskDefinition.Id?.ToString() == task.Id).Count() == 0)
                    {
                        TaskCards.Remove(task);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(0.3));

                OnPropertyChanged(nameof(TaskCards));
            }

            //clear remaining items in TaskCards when all activities are
            //complete
            if (RemainingActivities.Any() && TaskCards != null)
            {
                // we clear this list only if we dont have any custom card
                if (StatusCard != null)
                {
                    if ((RemainingActivities.Count <= 0 || (TaskCards?[0].Label != StatusCard.Label)))
                    {
                        OnPropertyChanged(nameof(TaskCards));
                    }
                    else
                    {
                        TaskCards?.Clear();
                    }
                }
            }

            OnPropertyChanged(nameof(TaskCardsIsVisible));

            if (CurrentSite == null || CurrentSite.SiteId == null)
            {
                CurrentSite = new Site();
                await LoadSiteAction.Run();
            }

            UpdateStatusCard();
        }

        /// <summary>
        /// Loads the profile task.
        /// </summary>
        /// <returns>The profile task.</returns>
        private async Task LoadUserProfile()
        {
            clinical6.User = await clinical6.GetProfile();
            CurrentSiteMember.Id = clinical6.User.SiteMember.Id;
            OnPropertyChanged(nameof(GreetingsText));

            var valueDate = clinical6.User.Profile.RandomizedAt;

            if (clinical6.User.Profile.ScreenedAt != null)
            {
                valueDate = clinical6.User.Profile.ScreenedAt;
            }

            OnPropertyChanged(nameof(WeekTitle));

            try
            {
                if (valueDate != null)
                {
                    int Weeks = 1;

                    try
                    {
                        var initialDate = CleanDatetime(DateTime.UtcNow.ToLocalTime());
                        var endDate = CleanDatetime(Convert.ToDateTime(valueDate).ToLocalTime());

                        var days = (DateTime.UtcNow.ToLocalTime() - Convert.ToDateTime(valueDate).ToLocalTime()).TotalDays;
                        days = (initialDate - endDate).TotalDays;
                        Weeks = (int)(days) / 7; // 0 based weeks

                        //  days = 0  1  2  3  4  5  6 // week 0
                        //  days = 7  8  9 10 11 12 13 // week 1

                        if (Settings.GetBoleanProperty(Settings.StudyWeeksStartAtOne))
                            Weeks++; // Makes weeks 1-based
                    }
                    catch
                    {
                        Weeks = 1;
                    }

                    WeekNumber = Weeks;
                }
                else
                {
                    WeekNumber = 1;
                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// On Error, print to console
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Task OnErrorPrintToConsole(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// On Error, pop navigation
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="arg">Argument.</param>
        private async Task OnErrorPopNavigation(Exception arg = null)
        {
            await Navigation.Pop();
        }

        /// <summary>
        /// Handles the completed event from a remote consent call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RemoteConsentCompletedViewModel_RemoteConsentCompletedEvent(object sender, EventArgs e)
        {
            await LoadData();
        }

        /// <summary>
        /// Resets several properties: GreetingsText, HeaderTitleText, 
        /// RemainingActivitiesIsVisible, RemainingActivitiesText,
        /// and HeaderTitleAccessibleText.
        /// </summary>
        private void ResetProperties()
        {
            OnPropertyChanged(nameof(GreetingsText));
            OnPropertyChanged(nameof(HeaderTitleText));
            OnPropertyChanged(nameof(ReconsentIsVisible));
            OnPropertyChanged(nameof(IsBannerRequired));
            OnPropertyChanged(nameof(ReconsentBannerPadding));
            OnPropertyChanged(nameof(RemainingActivitiesIsVisible));
            OnPropertyChanged(nameof(RemainingActivitiesText));
            OnPropertyChanged(nameof(HeaderTitleAccessibleText));
            OnPropertyChanged(nameof(DisplayPatientStudyProgress));
        }

        /// <summary>
        /// Updates the status card property.
        /// </summary>
        private void UpdateStatusCardProperty()
        {
            // Don't clear if it's the same status - but show status card if
            // there is no data
            try
            {
                if (StatusCard.IsVisible && (TaskCards.Count <= 0 || (StudySteps.Count == 0 && TaskCards[0].Id != StatusCard.Id)))
                {
                    TaskCards.Clear();
                    TaskCards.Add(StatusCard);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            OnPropertyChanged(nameof(StatusCard));
            OnPropertyChanged(nameof(TaskCards));
            OnPropertyChanged(nameof(TaskCardsIsVisible));
        }

        /// <summary>
        /// Updates the status card.
        /// </summary>
        private void UpdateStatusCard()
        {
            if (CurrentSiteMember == null) return;

            switch (CurrentSiteMember.ConsentStatus)
            {
                case "initial":
                case "consent_initial":
                    if (CurrentSiteMember.LastConsentedAt == null)
                    {
                        StatusCard = new BaseCard()
                        {
                            Id = "MyTasksStatusCardConsentInitialLabel".Localized(),
                            Label = "MyTasksStatusCardConsentInitialLabel".Localized(),
                            ImageSource = "pic_consent_required",
                            Title = "MyTasksStatusCardConsentInitialTitle".Localized(),
                            Body = "MyTasksStatusCardConsentInitialBody".Localized(),
                            AdditionalInfo = FormatSiteAddress(CurrentSite),
                            IsVisible = true,
                        };
                        UpdateStatusCardProperty();
                    }
                    break;
                case "in_progress":
                case "consent_in_progress":
                    if (CurrentSiteMember?.FirstConsentedAt == null && AvailableStrategies?.Count > 0)
                    {
                        int? formId = 0;

                        if (AvailableStrategies?.First().Forms?.Count > 0)
                        {
                            formId = AvailableStrategies.First().Forms.First().Id;
                        }
                        var consentGrant = new ConsentGrants
                        {
                            GrantedFor = new User { Id = clinical6.User.Id, Type = "mobile_users" },
                            Strategy = new ConsentStrategyModel { Id = AvailableStrategies.First().Strategy.Id },
                            Form = new ConsentForms { Id = formId },
                            RequestType = "pma"
                        };

                        StatusCard = new BaseCard()
                        {
                            // Verbiage depends on consent vs. reconsent status. Functionality is the same.
                            Id = IsReconsentState ? "MyTasksStatusRemoteConsentLabelRe".Localized() : "MyTasksStatusRemoteConsentLabel".Localized(),
                            Label = IsReconsentState ? "MyTasksStatusRemoteConsentLabelRe".Localized() : "MyTasksStatusRemoteConsentLabel".Localized(),
                            ImageSource = "pic_consent_required",
                            Title = IsReconsentState ? "MyTasksStatusRemoteConsentTitleRe".Localized() : "MyTasksStatusRemoteConsentTitle".Localized(),
                            Body = string.Format(IsReconsentState ? "MyTasksStatusRemoteConsentBodyRe".Localized() : "MyTasksStatusRemoteConsentBody".Localized(), StudyName),
                            IsVisible = true,
                            ButtonIsVisible = true,
                            ButtonText = "SignInformedConsentText".Localized(),
                            ButtonReference = consentGrant
                        };

                        UpdateStatusCardProperty();
                    }
                    else if (CurrentSiteMember.LastConsentedAt == null && CurrentSiteMember.FirstConsentedAt == null && !AvailableStrategies.Any())
                    {
                        StatusCard = new BaseCard()
                        {
                            Id = "MyTasksStatusCardRemoteConsentInProgressLabel".Localized(),
                            Label = "MyTasksStatusCardRemoteConsentInProgressLabel".Localized(),
                            ImageSource = "pic_consent_required",
                            Title = "MyTasksStatusCardRemoteConsentInProgressTitle".Localized(),
                            Body = string.Format("MyTasksStatusCardRemoteConsentInProgressBody".Localized(), StudyName),
                            IsVisible = true,

                        };
                        UpdateStatusCardProperty();
                    }
                    else if (CurrentSiteMember.LastConsentedAt == null)
                    {
                        // @TODO is this else if necessary?
                        StatusCard = new BaseCard()
                        {
                            Id = "MyTasksStatusCardConsentInProgressLabel".Localized(),
                            Label = "MyTasksStatusCardConsentInProgressLabel".Localized(),
                            ImageSource = "pic_consent_required",
                            Title = "MyTasksStatusCardConsentInProgressTitle".Localized(),
                            Body = "MyTasksStatusCardConsentInProgressBody".Localized(),
                            AdditionalInfo = FormatSiteAddress(CurrentSite),
                            IsVisible = true,

                        };
                        UpdateStatusCardProperty();
                    }
                    break;
                case "complete":
                case "consent_complete":
                    // No study steps means that only the All Caught Up card should ever be shown. If there are study steps (studySteps != 0),
                    // then there is the possibility that the current study step is not defined at this point and in that case the Waiting Step
                    // card should be shown.
                    int studySteps = CurrentSiteMember.ActiveStudyDefinitionVersion?.Definition?.DefinitionVersions?.Count ?? 0;

                    if (studySteps != 0 && RemainingActivities?.Count == 0 && CurrentStudyStep == null)
                    {
                        // Waiting Step
                        StatusCard = new BaseCard()
                        {
                            Id = "MyTasksStatusCardConsentCompleteUnassignedLabel".Localized(),
                            Label = "MyTasksStatusCardConsentCompleteUnassignedLabel".Localized(),
                            ImageSource = "pic_big_error",
                            Title = "MyTasksStatusCardConsentCompleteUnassignedTitle".Localized(),
                            Body = "MyTasksStatusCardConsentCompleteUnassignedBody".Localized(),
                            IsVisible = true,
                        };
                    }
                    else if (RemainingActivities.Count == 0)
                    {
                        // All Caught Up
                        StatusCard = new BaseCard()
                        {
                            Id = "MyTasksStatusCardConsentCompleteAssignedLabel".Localized(),
                            Label = "MyTasksStatusCardConsentCompleteAssignedLabel".Localized(),
                            ImageSource = "pic_caught_up",
                            Title = "MyTasksStatusCardConsentCompleteAssignedTitle".Localized(),
                            Body = "MyTasksStatusCardConsentCompleteAssignedBody".Localized(),
                            IsVisible = true,
                        };

                        TaskCards.Clear();
                        TaskCards.Add(StatusCard);
                        UpdateStatusCardProperty();
                    }
                    break;
                default:
                    StatusCard = new BaseCard()
                    {
                        IsVisible = false
                    };
                    break;
            }
        }
    }
}