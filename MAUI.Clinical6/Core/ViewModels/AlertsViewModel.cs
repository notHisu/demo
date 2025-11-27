using Akavache;
using Clinical6SDK;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;
using Clinical6SDK.Services;
using Clinical6SDK.Services.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Core.Services;

namespace Xamarin.Forms.Clinical6.Core.ViewModels
{
    public class AlertsViewModel : BaseViewModel
    {
        private readonly string _archiveStateContextMenuText = "ArchiveText".Localized();
        private readonly string _readStateContextMenuText = "ReadText".Localized();
        private readonly string unReadStateContextMenuText = "UnreadText".Localized();

        private string _readImageState = "gui_mail_read";
        private string _unreadImageState = "gui_mail_unread";

        private readonly ICacheService _cacheService;

        public string Title { get; set; }
        public ActionTracker LoadUserProfileAction { get; }
        public ActionTracker LoadNotificationData { get; }
        public ActionTracker MarkAllAction { get; }
        public Clinical6Instance Clinical6 { get; private set; } = new Clinical6Instance();
        public Profile CurrentUserProfile => Clinical6.User.Profile;

        public event EventHandler<int> BadgeEvent;
        public static string KeyCache = string.Empty;

        public static void UpdateCache()
        {
            KeyCache = "alerts" + DateTime.UtcNow.ToString("ssMM");
        }

        private bool _isInitialLoading;
        public bool IsInitialLoading
        {
            get { return _isInitialLoading; }
            private set
            {
                _isInitialLoading = value;
                OnPropertyChanged(nameof(IsInitialLoading));
            }
        }

        public ObservableCollection<Swipeable> _notifications;
        public ObservableCollection<Swipeable> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged(nameof(Notifications));
                TriggerBadgeEvent();
                Settings.SetProperty("AlertTime", DateTime.UtcNow.Ticks.ToString());
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private bool _canShowListView;
        public bool CanShowListView
        {
            get { return _canShowListView; }
            set
            {
                _canShowListView = value;
                OnPropertyChanged(nameof(CanShowListView));

                if (!CanShowListView)
                {
                    OnPropertyChanged(nameof(AreAlertsVisible));
                    OnPropertyChanged(nameof(AreAlertsNotVisible));
                }
            }
        }

        public bool AreAlertsVisible => Notifications != null && Notifications.Count > 0;
        public bool AreAlertsNotVisible => !IsInitialLoading && !AreAlertsVisible && !IsRefreshing;

        public string NoAlertsTitle => "NoAlertsTitle".Localized();
        public string NoAlertsDescription => "NoAlertsDescription".Localized();

        private Notification _selectedNotification;
        public Notification SelectedNotification
        {
            get { return _selectedNotification; }
            set
            {
                _selectedNotification = value;
                OnPropertyChanged(nameof(SelectedNotification));
            }
        }

        public ICommand SelectedItemCommand { get; set; }
        public ICommand ReadCommand { get; set; }
        public ICommand ArchiveCommand { get; set; }
        public ICommand ArchiveListCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xamarin.Forms.Clinical6.Core.ViewModels.MyTasksViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="userService">User service.</param>
        /// <param name="cacheService">Cache service.</param>
        public AlertsViewModel(INavigationService navigationService, ICacheService cacheService) : base(navigationService)
        {
            UpdateCache();
            _cacheService = cacheService;
            Clinical6 = new Clinical6Instance();

            Clinical6.BaseUrl = EnvironmentConfig.ApiRoot;

            LoadUserProfileAction = new ActionTracker(async () => Clinical6.User = await Clinical6.GetProfile(), null, OnError);
            LoadNotificationData = new ActionTracker(LoadNotifications, null, OnError);
            MarkAllAction = new ActionTracker(MarkAll, null, OnError);

            SelectedItemCommand = new Command<Swipeable>(async (swipeable) =>
            {
                await OnSelectedItemNotification(swipeable);
            });

            ReadCommand = new Command<Swipeable>(async (swipeable) =>
            {
                if (swipeable.IsNewInfoAvailable)
                    await OnReadNotification(swipeable);
                else
                    await OnUnreadNotification(swipeable);
            });

            ArchiveCommand = new Command<Swipeable>(async (swipeable) =>
            {
                await OnArchiveNotification(swipeable);
            });

            ArchiveListCommand = new Command(async () =>
            {
                CanShowListView = false;
                //await Navigation.Push<AlertsArchiveViewModel>();
            });

            RefreshCommand = new Command(async () => await LoadData());
        }

        /// <summary>
        /// Ons the error.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="arg">Argument.</param>
        private async Task OnError(Exception arg = null)
        {
            await Navigation.Pop();
        }

        /// <summary>
        /// Archive all.
        /// </summary>
        /// <returns>The all.</returns>
        public async Task MarkAll()
        {
            var service = new JsonApiHttpService();

            Console.WriteLine(service.AuthToken);

            var options = new Options { Url = string.Format("/v3/mobile_users/{0}/notification/deliveries/archive_all", Clinical6.User.Id) };
            //options.Add("useNewApi", "true");

            string request = @"{
                                    'data': {
                                        'attributes': {
                                          'status':'completed'
                                        }
                                    }
                                }";

            await service.Insert<object>(request, options);
            UpdateCache();
            await LoadNotifications();
        }

        private async Task<List<Notification>> LoadNotificationsCache(Action<Exception> retry = null)
        {
            try
            {
                if (!MainService.Instance.EnableCache)
                {
                    return await FetchNotifications();
                }
                var cache = Akavache.BlobCache.LocalMachine;
                List<Notification> results;

                var cachedPostsPromise = cache.GetAndFetchLatest(
                    KeyCache,
                    () => FetchNotifications(),
                    offset =>
                    {
                        TimeSpan elapsed = DateTimeOffset.Now - offset;
                        return elapsed > new TimeSpan(hours: 0, minutes: 0, seconds: 25);
                    });

                cachedPostsPromise.Subscribe(subscribedPosts =>
                {
                    Debug.WriteLine("Subscribed Posts ready");
                    results = subscribedPosts;
                });

                results = await cachedPostsPromise.FirstOrDefaultAsync();
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadNotificationsCache Exception: {ex}");
                return null;
            }
        }

        private async Task<List<Notification>> FetchNotifications(Action<Exception> retry = null)
        {
            var result = await Clinical6.GetChildren<List<Notification>>(Clinical6.User, new Notification().Type, new Options
            {
                UriQuery = new
                {
                    filters = new
                    {
                        status = "pending",
                    }
                }
            });

            var apiResult = result.Where(c => c.ChannelType == "api");

            if (apiResult != null)
            {
                result = apiResult.ToList();
            }

            return result;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>The data.</returns>
        private async Task LoadNotifications()
        {
            try
            {
                // Required
                if (Clinical6.User == null)
                {
                    Clinical6.User = await Clinical6.GetProfile();
                }

                var result = await LoadNotificationsCache();

                var sortedNotifications = result.OrderByDescending(o => o.CreatedAt).ToList();

                ProcessNotifications(sortedNotifications);

                OnPropertyChanged(nameof(AreAlertsVisible));
                OnPropertyChanged(nameof(AreAlertsNotVisible));

                Settings.SetProperty("AlertTime", DateTime.UtcNow.Ticks.ToString());
            }
            catch
            {
                await LogoutTask();
            }
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

        /// <summary>
        /// Invoke badge event to update badge count
        /// on alerts tab
        /// </summary>
        private void TriggerBadgeEvent()
        {
            if (Notifications == null)
                return;

            if (Notifications != null && Notifications.Any())
            {
                int unreadCount = Notifications.Where(x => !((Notification)x.Item).ReadAt.HasValue).Count();
                BadgeEvent?.Invoke(null, unreadCount);
            }
        }

        /// <summary>
        /// load sorted list of notifications
        /// </summary>
        /// <param name="sortedNotifications"></param>
        private void ProcessNotifications(IEnumerable<Notification> sortedNotifications)
        {
            var filteredNotifications = sortedNotifications?.Where(x => !x.ArchivedAt.HasValue);
            if (filteredNotifications == null)
                return;

            CanShowListView = filteredNotifications.Any();

            var notifications = new List<Swipeable>();

            filteredNotifications.ToList().ForEach(x =>
            {
                var swipeableRow = new Swipeable();
                swipeableRow.Item = x;
                swipeableRow.ExtraInfo = GetFormattedDate(x);
                swipeableRow.Title = x.Title;
                swipeableRow.Description = x.Message;

                var menuContextText = !x.ReadAt.HasValue ? _readStateContextMenuText : unReadStateContextMenuText;

                swipeableRow.IsNewInfoAvailable = !x.ReadAt.HasValue;
                swipeableRow.RightButtonImage = "gui_tool_archive";
                swipeableRow.RightButtonText = _archiveStateContextMenuText;
                swipeableRow.LeftButtonImage = !x.ReadAt.HasValue ? _readImageState : _unreadImageState;
                swipeableRow.LeftButtonText = menuContextText;

                notifications.Add(swipeableRow);
            });

            Notifications = new ObservableCollection<Swipeable>(notifications);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        private string GetFormattedDate(Notification notification)
        {
            var createdDate = notification.CreatedAt.Value.ToLocalTime();

            var cultureInfo = AppHelpers.GetCultureInfoSafely();

            if (createdDate.Date == DateTime.Today.Date)
                return createdDate.ToString("h:mm tt", cultureInfo);
            else
                return createdDate.ToString("MMM dd", cultureInfo);
        }

        /// <summary>
        /// Gets selected item from list
        /// </summary>
        /// <param name="swipeable"></param>
        /// <returns></returns>
        private async Task OnSelectedItemNotification(Swipeable swipeable)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var notification = (Notification)swipeable.Item;
                //await Navigation.Push<AlertDetailViewModel, Notification>(notification);
                await OnReadNotification(swipeable);
            }
            catch(Exception exc)
            {
                Console.WriteLine($"OnSelectedItemNotification Exception: {exc}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swipeable"></param>
        /// <returns></returns>
        private async Task OnArchiveNotification(Swipeable swipeable)
        {
            var notification = (Notification)swipeable.Item;

            notification.Archive = true;
            notification.Read = true;

            var isSuccess = await UpdateNotification(notification);

            if (isSuccess == false)
                return;

            UpdateCache();

            if (Notifications != null && Notifications.Any())
            {
                var notificationToRemove = Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First();
                Notifications.Remove(notificationToRemove);
                Notifications = new ObservableCollection<Swipeable>(Notifications);
                CanShowListView = Notifications.Any();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swipeable"></param>
        /// <returns></returns>
        private async Task OnReadNotification(Swipeable swipeable)
        {
            var notification = (Notification)swipeable.Item;

            notification.Read = true;

            var isSuccess = await UpdateNotification(notification);
            if (isSuccess == false)
                return;

            if (Notifications != null && Notifications.Any())
            {
                //update status for current notification
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().Item
                = notification;

                //update NewInfoAvailable for current swipeable
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().IsNewInfoAvailable
                    = false;

                //update LeftButtonText for current swipeable
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().LeftButtonText
                   = unReadStateContextMenuText;

                //update left menu context image
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().LeftButtonImage
                    = _unreadImageState;

                Notifications = new ObservableCollection<Swipeable>(Notifications);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swipeable"></param>
        /// <returns></returns>
        private async Task OnUnreadNotification(Swipeable swipeable)
        {
            var notification = (Notification)swipeable.Item;

            notification.Read = false;

            var isSuccess = await UpdateNotification(notification);
            if (isSuccess == false)
                return;

            if (Notifications != null && Notifications.Any())
            {
                //update status for current notification
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().Item
                = notification;

                //update NewInfoAvailable for current swipeable
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().IsNewInfoAvailable
                    = true;

                //update LeftButtonText for current swipeable
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().LeftButtonText
                    = _readStateContextMenuText;

                //update left menu context image
                Notifications.Where(x => ((Notification)x.Item).Id == notification.Id).First().LeftButtonImage
                    = _readImageState;

                Notifications = new ObservableCollection<Swipeable>(Notifications);
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>The data.</returns>
        private async Task LoadData()
        {
            IsRefreshing = true;

            await LoadNotificationData.Run();

            IsRefreshing = false;
        }

        /// <summary>
        /// Inits the implementation.
        /// </summary>
        /// <returns>The implementation.</returns>
        protected override async Task InitImplementation()
        {
            IsInitialLoading = true;
            OnPropertyChanged(nameof(IsInitialLoading));

            Clinical6.AuthToken = await _cacheService.GetAuthToken();

            await LoadNotificationData.Run();
            Title = "MyTasksTitle".Localized();

            IsInitialLoading = false;
            OnPropertyChanged(nameof(IsInitialLoading));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        private async Task<bool> UpdateNotification(Notification notification)
        {
            var options = new Options
            {
                Url = string.Format("/v3/notification/deliveries/{0}", notification.Id)
            };

            var result = await Clinical6.Update<Notification>(notification, options);

            var updatedNotifications = await Clinical6.GetChildren<List<Notification>>(CurrentUserProfile.User, new Notification().Type, new Options
            {
                UriQuery = new
                {
                    filters = new
                    {
                        status = "pending"
                    }
                }
            });
            var updatedNotification = updatedNotifications.ToList().First(x => x.Id == notification.Id);
            notification.ReadAt = updatedNotification.ReadAt;
            notification.ArchivedAt = updatedNotification.ArchivedAt;
            return true;
        }
    }
}