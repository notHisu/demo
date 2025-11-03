using System.Threading.Tasks;
using System;
using Xamarin.Forms.Clinical6.Core.ViewModels;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface INavigationService
    {
        /// <summary>
        /// Forms INavigation
        /// </summary>
        INavigation Navigation { get; set; }

        /// <summary>
        /// Insert, before the current page, a new page based off its view model
        /// </summary>
        Task InsertPrevious<TVM>() where TVM : BaseViewModel;

        /// <summary>
        /// Insert, before the current page, a new page based off its view model, using a parameter
        /// </summary>
        Task InsertPrevious<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>;

        /// <summary>
        /// Navigate to a page based on its view model
        /// </summary>
        Task Push<TVM>() where TVM : BaseViewModel;

        /// <summary>
        /// Navigate to a page based on its view model, using a parameter
        /// </summary>
        Task Push<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>;

        /// <summary>
        /// Navigate to a modal page based on its view model
        /// </summary>
        Task PushModal<TVM>() where TVM : BaseViewModel;

        /// <summary>
        /// Navigate to a modal page based on its view model, using a parameter
        /// </summary>
        Task PushModal<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>;

        /// <summary>
        /// Navigate back, if possible
        /// </summary>
        Task Pop();

        /// <summary>
        /// Navigate back, if possible
        /// </summary>
        Task PopModal();

        /// <summary>
        /// Attempt to pop off the page below this one
        /// </summary>
        Task PopPrevious();

        /// <summary>
        /// Navigate back to the root of the stack
        /// </summary>
        Task PopToRoot();

        /// <summary>
        /// Pops off all pages below the current one
        /// </summary>
        Task PopBelow();

        /// <summary>
        /// Begin a new login UI session, navigating to the specified view model
        /// </summary>
        Task StartLogin<TVM>() where TVM : BaseViewModel;

        /// <summary>
        /// Begin a new login UI session, navigating to the specified view model
        /// </summary>
        Task StartLogin<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>;

        /// <summary>
        /// Launch the main app UI
        /// </summary>
        Task StartMainApp(Page dashboard = null);

        /// <summary>
        /// Similar to Push, but drops the current page so the user can't go back
        /// </summary>
        Task SwitchPage<TVM>() where TVM : BaseViewModel;

        /// <summary>
        /// Similar to Push, but drops the current page so the user can't go back
        /// </summary>
        Task SwitchPage<TVM, TParameter>(TParameter param) where TVM : BaseViewModel<TParameter>;

        Task NavigatePage<TVM>(bool fromMenu = false) where TVM : BaseViewModel;

        Task NavigatePage<TVM, TParameter>(TParameter param, bool fromMenu = false) where TVM : BaseViewModel<TParameter>;

        Task WithDisabledAnimation(Func<Task> NavigationTask);
    }
}