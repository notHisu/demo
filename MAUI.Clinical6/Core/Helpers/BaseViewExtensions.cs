using Clinical6SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Services;
using Xamarin.Forms.Clinical6.ViewModels;
using Xamarin.Forms.Clinical6.Views;

namespace Xamarin.Forms.Clinical6.Helpers
{
    public static class AppMenuExtensions
    {
        public static async void NavigateSubMenu(this BaseViewModel page, AppMenu currentMenu, List<AppMenu> menusList)
        {
            var menuParams = new AppSubMenuViewModel.InitValues();
            menuParams.IsSubMenu = true;
            menuParams.Menus = menusList;
            menuParams.ParentMenu = currentMenu;
            await page.Navigation.NavigatePage<AppSubMenuViewModel, AppSubMenuViewModel.InitValues>(menuParams, fromMenu: true);
        }

        private static async Task<bool> ValidInternetConnection(INavigationService navigation)
        {
            var access = Connectivity.NetworkAccess;
            if (access != NetworkAccess.Internet && access != NetworkAccess.ConstrainedInternet)
            {
                //await navigation.StartLogin<AppInitViewModel>();
                return false;
            }

            return true;
        }

        /// <summary>
        /// example 1. Action is 'Url' then ActionObject will be a URL.
        /// example 2. Action is 'dynamic_contents' then ActionObject is 'glossary_of_terms'
        /// example 3. Action is 'no_action' then ActionObject is 'null'
        /// example 4. Action is 'profile' then ActionObject is 'null'
        /// example 5. Action is 'subcategory' then ActionObject is 'null'
        /// </summary>
        public static async void Navigate(this BaseViewModel page, AppMenu currentMenu)
        {
            if (!await ValidInternetConnection(page.Navigation))
                return;

            switch (currentMenu.Action)
            {
                case "profile":
                case "my profile":
                case "myprofile":
                case "participant_profile":
                    //await NavigateTo<ProfileViewModel>(page.Navigation);
                    break;
                case "contact":
                    //await NavigateTo<ContactViewModel>(page.Navigation);
                    break;
                case "logout":
                    break;
                case "predefined_screen":
                    await GoPredifinedScreen(page.Navigation, currentMenu);
                    break;
                case "url":
                    {
                        var uri = new Uri(currentMenu.ActionObject.ToString());
                        if (await Launcher.CanOpenAsync(uri))
                            await Launcher.OpenAsync(uri);
                        break;
                    }
                case "videoconsults":
                case "video_consults":
                    //await NavigateTo<VideoConsultListViewModel>(page.Navigation);
                    break;
                case "video":
                    //await NavigateTo<VideoListViewModel>(page.Navigation);
                    break;
                //NOTE: App menu action item to directly navigate to specific generic html document
                case "generic_html_pages":
                    {
                        //var genericHtmlParam = new GenericHtmlInitParam(currentMenu.Title, null);
                        //await NavigateTo<GenericHtmlViewModel, GenericHtmlInitParam>(page.Navigation, genericHtmlParam);
                    }
                    break;
                case "dynamic_contents":
                case "dynamic_content_type":
                    {
                        await GoPredifinedScreen(page.Navigation, currentMenu);
                        break;
                    }
            }
        }

        private static async Task GoPredifinedScreen(INavigationService navService, AppMenu currentMenu)
        {
            if (!await ValidInternetConnection(navService))
                return;

            switch (currentMenu.ActionDetail.PermanentLink)
            {
                case "article":
                    //await NavigateTo<ArticlesViewModel>(navService);
                    break;
                case "participant_profile":
                    //await NavigateTo<ProfileViewModel>(navService);
                    break;
                case "article_resources":
                    //await NavigateTo<AboutTheStudyViewModel>(navService);
                    break;
                case "contact":
                    //await NavigateTo<ContactViewModel>(navService);
                    break;
                case "video_consults":
                case "videoconsults":
                    //await NavigateTo<VideoConsultListViewModel>(navService);
                    break;
                case "video":
                    //await NavigateTo<VideoListViewModel>(navService);
                    break;
                case "history":
                case "dashboard":
                case "activity_history":
                    //await NavigateTo<FlowSurveyHistoryViewModel>(navService);
                    break;
                case "User Guide":
                case "UserGuide":
                    //await NavigateTo<UserGuideViewModel>(navService);
                    break;
                case "appointments":
                case "appointment":
                    //await NavigateTo<AppointmentsViewModel>(navService);
                    break;
                case "logout":
                    break;
                case "glossary":
                case "faq":
                case "policy":
                    {
                        var glossaryParams = new AppSubMenuViewModel.InitValues();
                        glossaryParams.IsSubMenu = true;
                        glossaryParams.ParentMenu = currentMenu;
                        //await NavigateTo<ContentListViewModel, AppSubMenuViewModel.InitValues>(navService, glossaryParams);
                    }
                    break;
                //NOTE: App menu action item to directly navigate to predetermined list of generic html documents
                case "generic_html_page":
                    {
                        //var genericHtmlListParam = new GenericHtmlListInitParam(currentMenu.Title);
                        //await NavigateTo<GenericHtmlListViewModel, GenericHtmlListInitParam>(navService, genericHtmlListParam);
                    }
                    break;
                case "garmin":
                case "fitbit":
                    //await NavigateTo<ExternalOAuthViewModel, AppMenu>(navService, currentMenu);
                    break;
                //case "home":
                //default:
                //    {
                //        var masterpage = MainService.HomePage?.Value as DashboardMasterPage;
                //        if (masterpage != null)
                //        {
                //            masterpage.IsPresented = false;
                //            var detailNavigationPage = masterpage.Detail as NavigationPage;
                //            var dashboardTabPage = detailNavigationPage?.CurrentPage as DashboardTabPage;
                //            if (dashboardTabPage != null)
                //            {
                //                var homePage = dashboardTabPage.Children?.FirstOrDefault(p => p.GetType() == NavigationService.GetPageType<MyTasksViewModel>());
                //                if (homePage != null)
                //                {
                //                    dashboardTabPage.CurrentPage = homePage;
                //                }
                //            }
                //        }
                //    }
                //    break;
            }
        }

        private static async Task NavigateTo<TVM>(INavigationService navService) where TVM : BaseViewModel
        {
            if (navService?.Navigation?.NavigationStack?.Count > 1)
            {
                await navService?.Push<TVM>();
                return;
            }

            await navService?.NavigatePage<TVM>(fromMenu: true);
        }

        private static async Task NavigateTo<TVM, TParam>(INavigationService navService, TParam param) where TVM : BaseViewModel<TParam>
        {
            if (navService?.Navigation?.NavigationStack?.Count > 1)
            {
                await navService?.Push<TVM, TParam>(param);
                return;
            }

            await navService?.NavigatePage<TVM, TParam>(param, true);
        }
    }
}