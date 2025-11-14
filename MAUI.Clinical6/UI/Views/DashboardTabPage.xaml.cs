using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Services;
using Xamarin.Forms.Clinical6.ViewModels;
#if ANDROID
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using Google.Android.Material.BottomNavigation;
#endif


namespace Xamarin.Forms.Clinical6.Views
{
    public partial class DashboardTabPage : TabbedPage
    {
        public static readonly BindableProperty IsAndroidLeftNavBarItemVisibleProperty =
            BindableProperty.Create(nameof(IsAndroidLeftNavBarItemVisibleProperty), typeof(bool), typeof(DashboardTabPage), false);

        public bool IsAndroidLeftNavBarItemVisible
        {
            get { return (bool)GetValue(IsAndroidLeftNavBarItemVisibleProperty); }
            set { SetValue(IsAndroidLeftNavBarItemVisibleProperty, value); }
        }

        private bool menuIsOpen = false;
        private int lastSelectedItemIndex;
        private bool isHandlingMenuTab = false; // Guard flag to prevent recursive events
        private Page menuPage;
        private Page homePage;
        private bool isSubscribedToFlyout = false;
        public event EventHandler<int> DashboardBadgeEvent;

        public DashboardTabPage()
        {
            InitializeComponent();
            Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);

#if ANDROID
            // Fix bottom padding when ToolbarPlacement is Bottom
            // Remove the extra bottom padding that Android adds for bottom tabs
            Padding = new Thickness(0, 0, 0, 0);

            // Set this AFTER InitializeComponent
            Loaded += OnDashboardTabPageLoaded;
#endif

            var navigationService = new NavigationService();

            var alertPage = NavigationService.GetNewPage<AlertsViewModel>();
            ((AlertsViewModel)alertPage.BindingContext).BadgeEvent -= DashboardTabPageBadgeEvent;
            ((AlertsViewModel)alertPage.BindingContext).BadgeEvent += DashboardTabPageBadgeEvent;

            var alertNavigationPage = new NavigationPage(alertPage);
            alertNavigationPage.Title = "AlertsText".Localized();
            alertNavigationPage.IconImageSource = "gui_tab_alerts";
            alertNavigationPage.BarTextColor = (Color)AppHelpers.GetResource("DarkGrey");

            var toolbarItem = new ToolbarItem();
            toolbarItem.Text = "ArchiveText".Localized();
            toolbarItem.IconImageSource = "gui_tool_archive_2";
            toolbarItem.SetBinding(MenuItem.CommandProperty, new Binding("ArchiveListCommand"));
            alertPage.ToolbarItems.Add(toolbarItem);

            alertPage.Disappearing -= AlertPage_Disappearing;
            alertPage.Disappearing += AlertPage_Disappearing;

            alertPage.Appearing -= AlertPage_Appearing;
            alertPage.Appearing += AlertPage_Appearing;

            menuPage = NavigationService.GetNewPage<AppMenuViewModel>();
            homePage = NavigationService.GetNewPage<MyTasksViewModel>();

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                if (menuPage != null)
                {
                    menuPage.AutomationId = "MenuPageTab";
                    Children.Add(menuPage);
                }

                if (homePage != null)
                {
                    homePage.IconImageSource = "gui_tab_home";
                    homePage.AutomationId = "HomePageTab";
                    Children.Add(homePage);
                }

                if (!MainService.Instance.HideAlertsSection)
                {
                    alertPage.AutomationId = "AlertPageTab";
                    Children.Add(alertNavigationPage);
                }
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                if (!MainService.Instance.HideAlertsSection)
                {
                    alertPage.AutomationId = "AlertPageTab";
                    Children.Add(alertNavigationPage);
                }

                if (homePage != null)
                {
                    homePage.IconImageSource = "gui_tab_home";
                    homePage.AutomationId = "HomePageTab";
                    Children.Add(homePage);
                }

                if (menuPage != null)
                {
                    menuPage.AutomationId = "MenuPageTab";
                    Children.Add(menuPage);
                }
            }

            var indexPage = Children.IndexOf(homePage);

            CurrentPage = Children[indexPage];
            lastSelectedItemIndex = indexPage;

            CurrentPageChanged += (object sender, EventArgs e) =>
            {
                // Guard against recursive event firing
                if (isHandlingMenuTab)
                    return;

                ///adding this make sure alert tab updates on navigating to.
                if (Children.IndexOf(CurrentPage) == 0)
                {
                    var alertsViewModel = alertPage.BindingContext as AlertsViewModel;
                    if (alertsViewModel != null && !alertsViewModel.IsRefreshing)
                        alertsViewModel.RefreshCommand.Execute(null);
                }

                HandleCurrentPageChanged(sender, e);
            };
            NavigationPage.SetHasNavigationBar(this, false);
        }

#if ANDROID
        private void OnDashboardTabPageLoaded(object sender, EventArgs e)
        {
            // Force layout update to remove padding
            if (Handler?.PlatformView is ViewGroup viewGroup)
            {
                viewGroup.SetPadding(0, 0, 0, 0);
                viewGroup.SetClipToPadding(false);

                // Find and fix the BottomNavigationView padding
                FixBottomNavigationPadding(viewGroup);
            }
        }

        private void FixBottomNavigationPadding(ViewGroup viewGroup)
        {
            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                var child = viewGroup.GetChildAt(i);

                if (child is BottomNavigationView bottomNav)
                {
                    // Remove window insets listener that adds padding
                    bottomNav.SetOnApplyWindowInsetsListener(null);

                    // Set padding to zero
                    bottomNav.SetPadding(0, 0, 0, 0);

                    // Remove any bottom margin
                    if (bottomNav.LayoutParameters is ViewGroup.MarginLayoutParams marginParams)
                    {
                        marginParams.BottomMargin = 0;
                        bottomNav.LayoutParameters = marginParams;
                    }
                }
                else if (child is ViewGroup childGroup)
                {
                    // Recursively check child view groups
                    FixBottomNavigationPadding(childGroup);
                }
            }
        }
#endif

        private void AlertPage_Appearing(object sender, EventArgs e)
        {
            IsAndroidLeftNavBarItemVisible = false;
        }

        private void AlertPage_Disappearing(object sender, EventArgs e)
        {
            IsAndroidLeftNavBarItemVisible = true;
        }

        public void DashboardTabPageBadgeEvent(object sender, int count)
        {
            DashboardBadgeEvent?.Invoke(null, count);
        }

        void HandleCurrentPageChanged(object sender, EventArgs e)
        {
            var i = Children.IndexOf(CurrentPage);
            Debug.WriteLine("Page No:" + i);

            var menuPageType = NavigationService.GetPageType<AppMenuViewModel>();
            if (CurrentPage.GetType() != menuPageType)
            {
                lastSelectedItemIndex = i;
                return;
            }

            // Set the flag to prevent recursive event firing
            isHandlingMenuTab = true;

            // Get the masterpage
            var masterpage = MainService.HomePage?.Value as DashboardMasterPage;
            if (masterpage != null)
            {
                // Subscribe to IsPresentedChanged event if not already subscribed
                if (!isSubscribedToFlyout)
                {
                    masterpage.IsPresentedChanged -= OnFlyoutIsPresentedChanged;
                    masterpage.IsPresentedChanged += OnFlyoutIsPresentedChanged;
                    isSubscribedToFlyout = true;
                }

                // Sync with actual flyout state before toggling
                menuIsOpen = masterpage.IsPresented;

                // Toggle the flyout menu
                masterpage.IsPresented = !menuIsOpen;
            }

            // Use Dispatcher to switch back to the last selected tab
            // This ensures the UI update happens after the flyout state change
            Dispatcher.Dispatch(() =>
            {
                try
                {
                    if (Children != null && Children.Count > lastSelectedItemIndex && lastSelectedItemIndex >= 0)
                    {
                        var nextPage = Children[lastSelectedItemIndex];
                        if (nextPage != null)
                        {
                            CurrentPage = nextPage;
                        }
                    }
                }
                finally
                {
                    // Reset the flag AFTER the tab switch completes
                    isHandlingMenuTab = false;
                }
            });
        }

        private void OnFlyoutIsPresentedChanged(object sender, EventArgs e)
        {
            var masterpage = sender as DashboardMasterPage;
            if (masterpage != null)
            {
                // Keep menuIsOpen in sync with actual flyout state
                menuIsOpen = masterpage.IsPresented;
                Debug.WriteLine($"Flyout state changed: {menuIsOpen}");

                // Update menu view model
                var menuPageType = NavigationService.GetPageType<AppMenuViewModel>();
                var appMenuViewModel = Children.FirstOrDefault(p => p.GetType() == menuPageType)?.BindingContext as AppMenuViewModel;
                if (appMenuViewModel != null)
                {
                    appMenuViewModel.IsMenuPresented = menuIsOpen;
                }
            }
        }

        public void MenuOpenClose(bool isPresented)
        {
            var masterpage = MainService.HomePage?.Value as DashboardMasterPage;

            if (masterpage == null)
                return; // safely exit if masterpage is not ready

            // Subscribe to IsPresentedChanged event if not already subscribed
            if (!isSubscribedToFlyout)
            {
                masterpage.IsPresentedChanged -= OnFlyoutIsPresentedChanged;
                masterpage.IsPresentedChanged += OnFlyoutIsPresentedChanged;
                isSubscribedToFlyout = true;
            }

            masterpage.IsPresented = isPresented;
        }

        void HomePageIsPresentedChanged(object sender, EventArgs e)
        {
            var masterpage = MainService.HomePage?.Value as DashboardMasterPage;

            if (masterpage == null)
                return;

            menuIsOpen = masterpage.IsPresented;

            var menuPageType = NavigationService.GetPageType<AppMenuViewModel>();
            var appMenuViewModel = Children.FirstOrDefault(p => p.GetType() == menuPageType)?.BindingContext as AppMenuViewModel;
            if (appMenuViewModel != null)
            {
                appMenuViewModel.IsMenuPresented = menuIsOpen;
            }
        }
    }
}