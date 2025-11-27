using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Views;
using Xamarin.Forms.Clinical6.Views;

namespace Xamarin.Forms.Clinical6.UI.Services
{
    public class NavigationService : INavigationService
    {
        private bool UseAnimations = true;
        public static readonly IDictionary<Type, Type> _viewModelMapping = new Dictionary<Type, Type>();

        public INavigation Navigation { get; set; }

        public static void RegisterMapping<TVM, TPage>()
            where TPage : Page, IViewModelPage<TVM>
            where TVM : BaseViewModel
        {
            _viewModelMapping[typeof(TVM)] = typeof(TPage);
        }

        public NavigationService(params object[] args) { }

        public NavigationService() { }

        #region INavigationService implementation

        public async Task InsertPrevious<TVM>() where TVM : BaseViewModel
        {
            var current = Navigation?.NavigationStack.LastOrDefault();
            if (current == null)
                return;

            await Navigate<TVM>(async page => Navigation.InsertPageBefore(page, current));
        }

        public async Task InsertPrevious<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>
        {
            var current = Navigation?.NavigationStack.LastOrDefault();
            if (current == null)
                return;

            await Navigate<TVM>(async page => Navigation.InsertPageBefore(page, current), vm => vm.InitParam = param);
        }

        public async Task Pop()
        {
            if (Navigation == null)
                return;
            await Navigation.PopAsync(UseAnimations);
        }

        public async Task PopModal()
        {
            if (Navigation == null)
                return;

            if (Navigation.ModalStack.Any())
            {
                await Navigation.PopModalAsync(UseAnimations);
            }
        }

        public async Task PopPrevious()
        {
            if (Navigation == null || Navigation.NavigationStack.Count < 2)
                return;

            var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2];
            Navigation.RemovePage(previousPage);
            await Task.Yield();
        }

        public async Task PopToRoot()
        {
            if (Navigation == null)
                return;
            await Navigation.PopToRootAsync(UseAnimations);
        }

        public async Task PopBelow()
        {
            if (Navigation == null)
                return;
            while (Navigation.NavigationStack.Count > 1)
            {
                Navigation.RemovePage(Navigation.NavigationStack.First());
            }
            await Task.Yield();
        }

        public async Task Push<TVM>() where TVM : BaseViewModel
        {
            if (Navigation == null)
                return;
            await Navigate<TVM>(page => Navigation.PushAsync(page, UseAnimations));
        }

        public Task NavigatePage<TVM, TParameter>(TParameter param, bool fromMenu = false) where TVM : BaseViewModel<TParameter>
        {
            var newPage = GetNewPage<TVM>();

            var vmPage = newPage as IViewModelPage<TVM>;
            if (vmPage != null)
            {
                vmPage.ViewModel.InitParam = param;
            }

            var app = Application.Current;

            // Fix: Don't blindly cast MainPage to NavigationPage. Check if it's a FlyoutPage first.
            FlyoutPage homePage = null;
            if (app.MainPage is FlyoutPage fp)
            {
                homePage = fp;
            }
            else if (app.MainPage is NavigationPage np && np.CurrentPage is FlyoutPage fp2)
            {
                homePage = fp2;
            }

            if (homePage != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (homePage.Detail is NavigationPage && fromMenu)
                        {
                            var nav = homePage.Detail as NavigationPage;

                            if (nav.CurrentPage is DashboardTabPage)
                            {
                                var tab = nav.CurrentPage as DashboardTabPage;
                                var navTab = tab.CurrentPage.Navigation;
                                navTab.PushAsync(newPage);
                                //tab.CurrentPage.TabIndex = 1;
                                tab.CurrentPage = tab.Children[1];
                                homePage.IsPresented = false;
                                return;
                            }
                        }

                        homePage.Detail = new NavigationPage(newPage);
                        homePage.Detail.Title = string.Empty;
                        homePage.IsPresented = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return;
                    }
                });
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Navigates the page.
        /// </summary>
        /// <returns>The page.</returns>
        /// <param name="fromMenu">If set to <c>true</c> from menu.</param>
        /// <typeparam name="TVM">The 1st type parameter.</typeparam>
        public Task NavigatePage<TVM>(bool fromMenu = false) where TVM : BaseViewModel
        {
            var newPage = GetNewPage<TVM>();

            var app = Application.Current;

            // Fix: Don't blindly cast MainPage to NavigationPage. Check if it's a FlyoutPage first.
            FlyoutPage homePage = null;
            if (app.MainPage is FlyoutPage fp)
            {
                homePage = fp;
            }
            else if (app.MainPage is NavigationPage np && np.CurrentPage is FlyoutPage fp2)
            {
                homePage = fp2;
            }

            if (homePage != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (homePage.Detail is NavigationPage && fromMenu)
                        {
                            var nav = homePage.Detail as NavigationPage;

                            if (nav.CurrentPage is DashboardTabPage)
                            {
                                var tab = nav.CurrentPage as DashboardTabPage;
                                var navTab = tab.CurrentPage.Navigation;
                                navTab.PushAsync(newPage);
                                //tab.CurrentPage.TabIndex = 1;
                                tab.CurrentPage = tab.Children[1];
                                homePage.IsPresented = false;
                                return;
                            }

                            //NOTE: If the current page is already pushed return
                            if (nav.CurrentPage.GetType() == newPage.GetType())
                                return;
                        }

                        homePage.Detail = new NavigationPage(newPage);
                        homePage.Detail.Title = string.Empty;
                        homePage.IsPresented = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                });
            }

            return Task.CompletedTask;
        }

        string viewmodelTitle;

        public async Task Push<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>
        {
            if (Navigation == null)
                return;

            //if (param is FlowProcessInitValues || param is CompletedFlowProcessViewModel.InitValues || param is Core.Models.FileUploadParams)
            //{
            //    if (param is FlowProcessInitValues)
            //    {
            //        viewmodelTitle = (param as FlowProcessInitValues).FlowTitle;
            //    }
            //    else if (param is CompletedFlowProcessViewModel.InitValues)
            //    {
            //        viewmodelTitle = (param as CompletedFlowProcessViewModel.InitValues).FlowTitle;
            //    }
            //    /* Legacy
            //    else if (param is Core.Models.FileUploadParams)
            //    {
            //        viewmodelTitle = "UploadFormsText".Localized();
            //    }
            //    else if (param is Core.ViewModels.CompletedFlowProcessViewModel.InitValues)
            //    {
            //        viewmodelTitle = "UploadFormsText".Localized();
            //    }
            //    */

            //    await Navigate<TVM>(NavigatetoCr, vm => vm.InitParam = param);
            //}
            //else if (param is string)
            //{
            //    await Navigate<TVM>(NavigatetoToPage, vm => vm.InitParam = param);
            //}
            //else
            //{
            //    await Navigate<TVM>(page => Navigation.PushAsync(page, UseAnimations), vm => vm.InitParam = param);
            //}
        }

        private async Task NavigatetoToPage(Page contentPageCr)
        {
            /* Legacy
            
            if (contentPageCr is FullScreenUploadPage)
            {
                if (Application.Current.Properties.ContainsKey("PreviousNavColor"))
                {
                    try
                    {
                        PreviouNavColor = (Color)(Application.Current.Properties["PreviousNavColor"]);

                        (contentPageCr as FullScreenUploadPage).NavBarColor = PreviouNavColor;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    }
                }
            }
            */
            await Navigation.PushAsync(contentPageCr, UseAnimations);
        }

        private async Task NavigatetoCr(Page contentPageCr)
        {
            //if (contentPageCr is BaseContentPage<FlowProcessViewModel>)
            //{
            //    (contentPageCr as BaseContentPage<FlowProcessViewModel>).NavBarColor = await GetColorCr(viewmodelTitle);
            //}
            //else if (contentPageCr is CompletedFlowProcessPage)
            //{
            //    (contentPageCr as CompletedFlowProcessPage).NavBarColor = await GetColorCr(viewmodelTitle);
            //}
            //else if (contentPageCr is FlowProcessSplashPage)
            //{
            //    (contentPageCr as FlowProcessSplashPage).NavBarColor = await GetColorCr(viewmodelTitle);
            //}
            /* Legacy
            else if (contentPageCr is CompleteUploadFlowProcessPage)
            {
                (contentPageCr as CompleteUploadFlowProcessPage).NavBarColor = await GetColorCr(viewmodelTitle);
            }
            else if (contentPageCr is FullScreenUploadPage)
            {
                (contentPageCr as FullScreenUploadPage).NavBarColor = await GetColorCr(viewmodelTitle);
            }
            else if (contentPageCr is UploadFlowProcessPage && PreviouNavColor != null)
            {
                if (Application.Current.Properties.ContainsKey("PreviousNavColor"))
                {
                    try
                    {
                        PreviouNavColor = (Color)(Application.Current.Properties["PreviousNavColor"]);

                        (contentPageCr as UploadFlowProcessPage).NavBarColor = PreviouNavColor;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    }
                }
            }
            */

            contentPageCr.Title = viewmodelTitle;

            await Navigation.PushAsync(contentPageCr, UseAnimations);
        }

        Color PreviouNavColor;

        public async Task<Color> GetColorCr(string FlowTitle)
        {
            string OrangeHex = "#ED6D49";
            string OrangeDarkHex = "#E9661E";
            string GreenHex = "#0F673D";
            string PurpleCrHex = "#B22D73";
            string DarkBlueRcHex = "#216588";
            string RedDarkCrHex = "#B42A2E";
            string BlueLightCrHex = "#59C2EE";
            string YellowDarkCrHex = "#D3971C";
            string PurpleDarkCrHex = "#822FA8";
            string GreenDarkCrHex = "#00A100";
            string PinkDarkCrHex = "#BD357D";
            string BlueDarkCrHex = "#216588";
            string BlackCrHex = "#515151";

            Color Orange = Color.FromHex(OrangeHex);
            Color OrangeDark = Color.FromHex(OrangeDarkHex);
            Color Blue = Color.FromHex("#189CE5");
            Color Green = Color.FromHex(GreenHex);
            Color PurpleCr = Color.FromHex(PurpleCrHex);
            Color DarkBlueCr = Color.FromHex(DarkBlueRcHex);
            Color RedDarkCr = Color.FromHex(RedDarkCrHex);
            Color BlueLightCr = Color.FromHex(BlueLightCrHex);
            Color YellowDarkCr = Color.FromHex(YellowDarkCrHex);
            Color PurpleDarkCr = Color.FromHex(PurpleDarkCrHex);
            Color GreenDarkCr = Color.FromHex(GreenDarkCrHex);
            Color PinkDarkCr = Color.FromHex(PinkDarkCrHex);
            Color BlueDarkCr = Color.FromHex(BlueDarkCrHex);
            Color BlackCr = Color.FromHex(BlackCrHex);

            var defaultColor = Blue;

            //Application.Current.Properties[Settings.PreviousNavColor] = defaultColor;
            Preferences.Set(Settings.PreviousNavColor, defaultColor.ToHex());
            //await Application.Current.SavePropertiesAsync();

            PreviouNavColor = defaultColor;

            return defaultColor;
        }

        public async Task PushModal<TVM>() where TVM : BaseViewModel
        {
            if (Navigation == null)
                return;
            await Navigate<TVM>(page => Navigation.PushModalAsync(page, UseAnimations));
        }

        public async Task PushModal<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>
        {
            if (Navigation == null)
                return;
            await Navigate<TVM>(page => Navigation.PushModalAsync(page, UseAnimations), vm => vm.InitParam = param);
        }

        public async Task StartLogin<TVM>() where TVM : BaseViewModel
        {
            try
            {
                var app = Application.Current;
                //if (!(app.MainPage is LoginNavigationPage))
                //{
                //    await Navigate<TVM>(async (page) => app.MainPage = new LoginNavigationPage(page));
                //}
                //else if (app.MainPage is AppNavigationPage)
                //{
                //    if ((app.MainPage as AppNavigationPage).CurrentPage is FlyoutPage)
                //    {
                //        await Navigate<TVM>(delegate (Page page)
                //        {
                //            ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).Detail = new LoginNavigationPage(page);
                //            ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).IsPresented = false;
                //            return null;
                //        });
                //    }
                //}
                //else
                //{
                //    await Push<TVM>();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Push<TVM>();
            }
        }

        public async Task StartLogin<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>
        {
            var app = Application.Current;
            //if (!(app.MainPage is LoginNavigationPage))
            //{
            //    await Navigate<TVM>(async page => app.MainPage = new LoginNavigationPage(page), vm => vm.InitParam = param);
            //}
            //else if ((app.MainPage is AppNavigationPage))
            //{
            //    await Navigate<TVM>(async delegate (Page page)
            //    {
            //        //vm.InitParam = param
            //        ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).Detail = new LoginNavigationPage(page);
            //        ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).IsPresented = false;
            //    });
            //}
            //else
            //{
            //    await Push<TVM>();
            //}
        }

        public async Task StartMainApp(Page dashboard = null)
        {
            var app = Application.Current;

            if (dashboard != null)
            {
                // Fix: Set the dashboard (FlyoutPage) directly as MainPage.
                // Do NOT wrap it in AppNavigationPage.
                app.MainPage = dashboard;
            }
            else if (app.MainPage is FlyoutPage flyoutPage)
            {
                flyoutPage.Detail = new NavigationPage(new DashboardTabPage());
                flyoutPage.IsPresented = false;
            }
            else if (app.MainPage is NavigationPage navPage && navPage.CurrentPage is FlyoutPage fp)
            {
                // Handle legacy case just in case
                fp.Detail = new NavigationPage(new DashboardTabPage());
                fp.IsPresented = false;
            }
            else
            {
                await PopToRoot();
            }
        }

        public async Task SwitchPage<TVM>() where TVM : BaseViewModel
        {
            await Navigate<TVM>(HandleSwitchPage);
        }

        async Task HandleSwitchPage(Page page)
        {
            var app = Application.Current;

            //if (app.MainPage is AppInitPage)
            //{
            //    app.MainPage = new AppNavigationPage(new DashboardMasterPage())
            //    {
            //        BarTextColor = Colors.White
            //    };

            //    if ((app.MainPage as AppNavigationPage).CurrentPage is FlyoutPage)
            //    {
            //        ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).Detail = new NavigationPage(page);
            //        ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).IsPresented = false;
            //    }
            //}
            //else if (!(app.MainPage is AppNavigationPage))
            //{
            //    await Navigation.PushAsync(page, UseAnimations);
            //    await PopPrevious();
            //}
            //else if ((app.MainPage is AppNavigationPage))
            //{
            //    if ((app.MainPage as AppNavigationPage).CurrentPage is FlyoutPage)
            //    {
            //        ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).Detail = new NavigationPage(page);
            //        ((app.MainPage as AppNavigationPage).CurrentPage as FlyoutPage).IsPresented = false;
            //    }
            //}
            //else
            //{
            //    await Navigation.PushAsync(page, UseAnimations);
            //}
        }

        public async Task SwitchPage<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>
        {
            await Push<TVM, TParameter>(param);
            await PopPrevious();
        }
        #endregion

        private async Task Navigate<TVM>(Func<Page, Task> nav, Action<TVM> setParam = null) where TVM : BaseViewModel
        {
            var newPage = GetNewPage<TVM>();

            var vmPage = newPage as IViewModelPage<TVM>;
            if (vmPage != null)
            {
                setParam?.Invoke(vmPage.ViewModel);
            }

            await nav(newPage);
        }

        public static Page GetNewPage<TVM>() where TVM : BaseViewModel
        {
            try
            {
                var pageType = GetPageType<TVM>();
                return Activator.CreateInstance(pageType) as Page;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        public static Type GetPageType<TVM>() where TVM : BaseViewModel
        {
            try
            {
                var pageType = _viewModelMapping[typeof(TVM)];
                var nameModel = typeof(TVM).Name;
                foreach (var item in _viewModelMapping)
                {
                    //Console.WriteLine(item.Key.Name);

                    if (nameModel.Replace("ViewModel", "RenderViewModel") == item.Key.Name)
                    {
                        pageType = _viewModelMapping[item.Key];
                        break;
                    }
                }
                return pageType;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task WithDisabledAnimation(Func<Task> NavigationTask)
        {
            UseAnimations = false;
            await NavigationTask();
            UseAnimations = true;
        }
    }
}