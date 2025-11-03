using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Services;
using Xamarin.Forms.Clinical6.ViewModels;


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
        public event EventHandler<int> DashboardBadgeEvent;

        public DashboardTabPage()
        {
            InitializeComponent();
            Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);

            //MainService.Instance.SessionTimedOutEvent -= MainService_SessionTimedOutEvent;
            //MainService.Instance.SessionTimedOutEvent += MainService_SessionTimedOutEvent;

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

            var menuPage = NavigationService.GetNewPage<AppMenuViewModel>();
            var homePage = NavigationService.GetNewPage<MyTasksViewModel>();

            if(DeviceInfo.Platform == DevicePlatform.Android)
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
            else if(DeviceInfo.Platform == DevicePlatform.iOS)
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

        /*private void MainService_SessionTimedOutEvent(object sender, bool e)
        {
            Navigation.PopModalAsync().GetAwaiter();
            MenuOpenClose(false);
            Navigation.PopToRootAsync().GetAwaiter();
            var timeoutPage = NavigationService.GetPage<SessionTimeOutViewModel>();
            Navigation.PushAsync(timeoutPage).GetAwaiter();
        }*/

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

            MenuOpenClose(!menuIsOpen);
        }

        public void MenuOpenClose(bool isPresented)
        {
            var masterpage = MainService.HomePage?.Value as DashboardMasterPage;

            if (masterpage == null)
                return; // safely exit if masterpage is not ready

            // Attach/detach event safely
            if (masterpage != null)
            {
                masterpage.IsPresentedChanged -= HomePageIsPresentedChanged;
                masterpage.IsPresentedChanged += HomePageIsPresentedChanged;

                masterpage.IsPresented = isPresented;
            }

            menuIsOpen = isPresented;

            // Safely set CurrentPage
            if (Children != null && Children.Count > lastSelectedItemIndex && lastSelectedItemIndex >= 0)
            {
                var nextPage = Children[lastSelectedItemIndex];
                if (nextPage != null)
                    CurrentPage = nextPage;
            }
        }


        void HomePageIsPresentedChanged(object sender, EventArgs e)
        {
            var masterpage = MainService.HomePage.Value as DashboardMasterPage;

            if (masterpage is DashboardMasterPage)
            {
                FlyoutPage homePage = masterpage;
                menuIsOpen = homePage.IsPresented;
            }

            var menuPageType = NavigationService.GetPageType<AppMenuViewModel>();
            var appMenuViewModel = Children.FirstOrDefault(p => p.GetType() == menuPageType)?.BindingContext as AppMenuViewModel;
            if (appMenuViewModel != null)
            {
                appMenuViewModel.IsMenuPresented = menuIsOpen;
            }

            CurrentPage = Children[lastSelectedItemIndex];
        }
    }
}