using Xamarin.Forms.Clinical6.UI.Views;
using Xamarin.Forms.Clinical6.ViewModels;

namespace Xamarin.Forms.Clinical6.Views
{
    public partial class SubMenuPage : BaseContentPage<AppSubMenuViewModel>
    {
        public SubMenuPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            CustomBackButtonAction = () => HandleTapped(this, null);
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;

            list.SelectedItem = null;
        }

        private void HandleTapped(object sender, System.EventArgs e)
        {
            var navStack = Navigation?.NavigationStack;
            if (navStack?.Count == 2)
            {
                var dashboardMasterPage = MainService.HomePage.Value as DashboardMasterPage;
                if (dashboardMasterPage is DashboardMasterPage)
                {
                    dashboardMasterPage.IsPresented = !dashboardMasterPage.IsPresented;
                }
            }

            ViewModel.Navigation.Pop();
        }
    }
}