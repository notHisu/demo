using System;
using System.Diagnostics;
using Xamarin.Forms.Clinical6.Core;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Services;
using Xamarin.Forms.Clinical6.ViewModels;

namespace Xamarin.Forms.Clinical6.UI.Views
{
    public class BaseContentPage<BVM> : BasePage, IViewModelPage<BVM> where BVM : BaseViewModel
    {
        private Color _navBarColor = (Color)AppHelpers.GetResource("DefaultNavBackground");

        public virtual Color NavBarColor
        {
            get => _navBarColor;
            set
            {
                _navBarColor = value;
                OnPropertyChanged();
            }
        }

        private Color _navBarTextColor = (Color)AppHelpers.GetResource("DefaultNavText");

        public Color NavBarTextColor
        {
            get => _navBarTextColor;
            set
            {
                _navBarTextColor = value;
                OnPropertyChanged();
            }
        }

        private string _NavTitle;
        public string NavTitle
        {
            get => _NavTitle;
            set
            {
                _NavTitle = value;
                OnPropertyChanged();
            }
        }

        private BVM _viewModel;

        public BVM ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }

        public BaseContentPage()
        {
            Debug.WriteLine("Current page: " + GetType().Name + " ViewModel: " + typeof(BVM));

            ViewModel = AppContainer.Current.Resolve<BVM>(); // Activator.CreateInstance<BVM>();

            FlowDirection = FlowDirection.LeftToRight;

            // Override the name used in the UI to navigate back to this page
            NavigationPage.SetBackButtonTitle(this, "NavActionBack".Localized());

            this.SetBinding(IsBusyProperty, $"{nameof(BaseViewModel.PageBusy)}.{nameof(BusyTracker.IsBusy)}", BindingMode.OneWay);

            if (ViewModel.Navigation is INavigationService nav)
            {
                nav.Navigation = Navigation;
            }

            MainService.CurrentNavigationService = ViewModel.Navigation;
        }

        public BaseContentPage(object[] args)
        {
            Debug.WriteLine("Current page: " + GetType().Name + " ViewModel: " + typeof(BVM));

            ViewModel = AppContainer.Current.Resolve<BVM>(); // Activator.CreateInstance<BVM>();

            FlowDirection = FlowDirection.LeftToRight;

            // Override the name used in the UI to navigate back to this page
            NavigationPage.SetBackButtonTitle(this, "NavActionBack".Localized());

            this.SetBinding(IsBusyProperty, $"{nameof(BaseViewModel.PageBusy)}.{nameof(BusyTracker.IsBusy)}", BindingMode.OneWay);

            if (ViewModel.Navigation is INavigationService nav)
            {
                nav.Navigation = Navigation;
            }

            MainService.CurrentNavigationService = ViewModel.Navigation;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.Init();

            if (MainService.Instance.TimeOutDisabled)
            {
                return;
            }

            if (MainService.Instance.CheckExpirtionToken() && AppContainer.Current.Resolve<IAppSpecificConfig>().VideoConsultSessionTimeOut)
            {
                Settings.SetProperty(Settings.SleepTime, DateTime.UtcNow.Ticks.ToString());

                //await ViewModel.Navigation.NavigatePage<SessionTimeOutViewModel>();
            }

            Settings.SetProperty(Settings.SleepTime, DateTime.UtcNow.Ticks.ToString());
        }
    }
}