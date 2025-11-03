using Xamarin.Forms.Clinical6.Core;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.UI.Views;
using Xamarin.Forms.Clinical6.ViewModels;

namespace Xamarin.Forms.Clinical6.Views
{
    public partial class MenuPage : BaseContentPage<AppMenuViewModel>
    {
        private bool _subscribed = false;

        public MenuPage()
        {
            Title = "MenuPageTitleText".Localized();
            IconImageSource = "gui_tab_more";
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            // Subscribe safely to ViewModel event if ViewModel is initialized
            if (ViewModel != null)
                ViewModel.RefreshElements += RefreshElements;
        }

        /// <summary>
        /// Safely refresh the menu UI elements
        /// </summary>
        private void RefreshElements()
        {
            if (imageLogo == null || ViewModel == null)
                return;

            imageLogo.Source = ViewModel.StudyLogo;
        }

        /// <summary>
        /// Update whether the menu is currently presented
        /// </summary>
        public void UpdatePresented(bool presented)
        {
            if (ViewModel != null)
                ViewModel.IsMenuPresented = presented;
        }

        /// <summary>
        /// Handle item tap and reset selection
        /// </summary>
        private void HandleItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView list)
                list.SelectedItem = null;
        }

        /// <summary>
        /// Called when the page appears. Subscribes to master page events safely.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel == null)
                return;

            // Safely retrieve DashboardMasterPage
            var masterpage = MainService.HomePage?.Value as DashboardMasterPage;

            if (masterpage != null && !_subscribed)
            {
                masterpage.IsPresentedChanged += OnIsPresentedChanged;
                _subscribed = true;
            }

            // Refresh UI elements safely
            RefreshElements();
        }

        /// <summary>
        /// Trigger data loading when the menu becomes visible
        /// </summary>
        private async void OnIsPresentedChanged(object sender, System.EventArgs e)
        {
            if (!(sender is DashboardMasterPage masterPage))
                return;

            if (!masterPage.IsPresented || ViewModel?.LoadDataAction == null)
                return;

            if (!ViewModel.LoadDataAction.IsBusy)
            {
                await ViewModel.LoadDataAction.Run();
            }
        }

        /// <summary>
        /// Cleanup event subscriptions to prevent memory leaks
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (_subscribed)
            {
                var masterpage = MainService.HomePage?.Value as DashboardMasterPage;
                if (masterpage != null)
                    masterpage.IsPresentedChanged -= OnIsPresentedChanged;

                _subscribed = false;
            }
        }
    }
}
