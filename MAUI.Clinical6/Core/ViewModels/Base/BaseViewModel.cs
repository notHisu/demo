using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.ViewModels;

namespace Xamarin.Forms.Clinical6.Core.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        private readonly AsyncLock _initLock = new AsyncLock();
        private Command _CloseCommand;

        /// <summary>
        /// A high-level indicator that the page itself is busy doing work
        /// </summary>
        /// <remarks>
        /// To use this, override the property to point to one of the BusyTrackers
        /// in your view model.
        /// If there are multiple BusyTrackers and you want the page to appear
        /// busy if any one of them is busy, look at the MultiBusyTracker class.
        /// </remarks>        
        public virtual BusyTracker PageBusy { get; } = new BusyTracker();

        /// <summary>
        /// The abstract navigation object
        /// </summary>
        /// <remarks>
        /// This should be set by the parent page (or view model), but in some cases it may not be.
        /// </remarks>
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            // If the parent has not set the navigation service, initialize one with no nav capabilities
            get => _navigation ?? (_navigation = AppContainer.Current.Resolve<INavigationService>());
            set => _navigation = value;
        }

        /// <summary>
        /// If true, the view model is fully initialized
        /// </summary>
        private bool _initialized;

        public bool Initialized
        {
            get => _initialized;
            protected set => SetProperty(ref _initialized, value);
        }

        /// <summary>
        /// Stud yLogo
        /// </summary>
        public ImageSource StudyLogo
        {
            get
            {
                if (MainService.Instance.StudyLogo == null)
                {
                    return ImageSource.FromFile("pma_logo.png");
                }

                return MainService.Instance.StudyLogo;
            }
        }

        protected BaseViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        public BaseViewModel(object[] args)
        {
        }

        protected BaseViewModel()
        {
        }

        #region Lifecycle Methods

        /// <summary>
        /// Initialize the view model after the view has been loaded
        /// </summary>
        /// <remarks>
        /// Any long-running tasks should happen in this method.
        /// 
        /// Quick high-level setup and value initialization can safely happen in
        /// the constructor.
        /// </remarks>
        public async Task Init()
        {
            // Use a lock to prevent double initialization
            using (await _initLock.LockAsync())
            {
                if (Initialized) return;

                await InitImplementation();
                Initialized = true;
            }
        }

        /// <summary>
        /// If implemented in a derived class, refreshes the data in the ViewModel
        /// </summary>
        public virtual Task Refresh() => Task.FromResult(false);

        #endregion

        public Command OnCloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new Command(ExecutCloseCommand));
            }
        }

        private async void ExecutCloseCommand()
        {
            await _navigation.Pop();
        }

        public string AppVersion
        {
            get
            {
                //IDeviceInfoService _deviceInfoService = new DeviceInfoService();
                return "1.0.0";
            }
        }

        string _pageTitle = string.Empty;

        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                _pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));
            }
        }

        public ICommand OnPolicyCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ShowPoliciesAsync();
                });
            }
        }

        async Task ShowPoliciesAsync()
        {
            //var Params = new AboutViewModel.InitValues();
            //Params.PolicyType = AboutViewModel.PolicyEnum.All;
            //await Navigation.PushModal<AboutViewModel, AboutViewModel.InitValues>(Params);
        }

        protected abstract Task InitImplementation();
    }

    public abstract class BaseViewModel<TParam> : BaseViewModel
    {
        /// <summary>
        /// The parameter used to initialize this view model
        /// </summary>
        /// <remarks>
        /// This should be set before the view model is bound to a view, and
        /// can be referenced within the Init logic.
        /// </remarks>
        public TParam InitParam { get; set; }

        protected BaseViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        protected BaseViewModel(params object[] args) : base(args)
        {
        }

        protected BaseViewModel()
        {
        }
    }
}