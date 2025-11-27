using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Views;

namespace Xamarin.Forms.Clinical6.Views
{
    public partial class AlertsPage : BaseContentPage<AlertsViewModel>
    {
        public AlertsPage()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var alertsViewModel = BindingContext as AlertsViewModel;
            if (alertsViewModel != null && !alertsViewModel.IsRefreshing)
                alertsViewModel.RefreshCommand.Execute(null);
        }
    }
}